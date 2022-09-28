using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Threading;
using Abp.UI;
using ERP.Notifications;
using ERP.Storage;
using ERP.GeneralLedger.SetupForms.ControlDetails.Dto;
using Abp.Domain.Repositories;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms.Importing.ControlDetail.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.ControlDetail
{
    public class ImportControlDetailToExcelJob : BackgroundJob<ImportControlDetailFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ERP.GeneralLedger.SetupForms.ControlDetail> _ControlDetailRepository;
        private readonly IControlDetailListExcelDataReader _ControlDetailListExcelDataReader;
        private readonly IInvalidControlDetailExporter _invalidControlDetailExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportControlDetailToExcelJob(
            IRepository<SetupForms.ControlDetail> ControlDetailRepository,
            IControlDetailListExcelDataReader ControlDetailListExcelDataReader,
            IInvalidControlDetailExporter invalidControlDetailExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _ControlDetailRepository = ControlDetailRepository;
            _ControlDetailListExcelDataReader = ControlDetailListExcelDataReader;
            _invalidControlDetailExporter = invalidControlDetailExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportControlDetailFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var ControlDetail = GetControlDetailListFromExcelOrNull(args);
                if (ControlDetail == null || !ControlDetail.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateControlDetail(args, ControlDetail);
            }
        }

        private List<ImportControlDetailDto> GetControlDetailListFromExcelOrNull(ImportControlDetailFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _ControlDetailListExcelDataReader.GetControlDetailFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateControlDetail(ImportControlDetailFromExcelJobArgs args, List<ImportControlDetailDto> ControlDetails)
        {
            var invalidControlDetails = new List<ImportControlDetailDto>();

            foreach (var ledger in ControlDetails)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateControlDetailAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidControlDetails.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidControlDetails.Add(ledger);
                    }
                }
                else
                {
                    invalidControlDetails.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportControlDetailResultAsync(args, invalidControlDetails));
        }

        private async Task CreateControlDetailAsync(ImportControlDetailDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var ControlDetails = _objectMapper.Map<SetupForms.ControlDetail>(input);
            ControlDetails.TenantId = (int)tenantId;
            var controldetail = await _ControlDetailRepository.FirstOrDefaultAsync(x => x.Seg1ID == ControlDetails.Seg1ID && x.TenantId == tenantId);
            if (controldetail == null)
            {
                await _ControlDetailRepository.InsertAsync(ControlDetails);
            }
            else
            {
                input.Id = controldetail.Id;
                _objectMapper.Map(input, controldetail);
            }



        }

        private async Task ProcessImportControlDetailResultAsync(ImportControlDetailFromExcelJobArgs args, List<ImportControlDetailDto> invalidControlDetails)
        {
            if (invalidControlDetails.Any())
            {
                var file = _invalidControlDetailExporter.ExportToFile(invalidControlDetails);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllControlDetailSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportControlDetailFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToControlDetailsList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
