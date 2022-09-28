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
using Abp.Domain.Repositories;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms.Importing.GroupCodes.Dto;
using ERP.GeneralLedger.SetupForms.GroupCodes.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCodes
{
    public class ImportGroupCodesToExcelJob : BackgroundJob<ImportGroupCodesFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ERP.GeneralLedger.SetupForms.GroupCode> _GroupCodesRepository;
        private readonly IGroupCodesListExcelDataReader _GroupCodesListExcelDataReader;
        private readonly IInvalidGroupCodesExporter _invalidGroupCodesExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportGroupCodesToExcelJob(
            IRepository<SetupForms.GroupCode> GroupCodesRepository,
            IGroupCodesListExcelDataReader GroupCodesListExcelDataReader,
            IInvalidGroupCodesExporter invalidGroupCodesExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _GroupCodesRepository = GroupCodesRepository;
            _GroupCodesListExcelDataReader = GroupCodesListExcelDataReader;
            _invalidGroupCodesExporter = invalidGroupCodesExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportGroupCodesFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var GroupCodes = GetGroupCodesListFromExcelOrNull(args);
                if (GroupCodes == null || !GroupCodes.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateGroupCodes(args, GroupCodes);
            }
        }

        private List<ImportGroupCodesDto> GetGroupCodesListFromExcelOrNull(ImportGroupCodesFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _GroupCodesListExcelDataReader.GetGroupCodesFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateGroupCodes(ImportGroupCodesFromExcelJobArgs args, List<ImportGroupCodesDto> GroupCodess)
        {
            var invalidGroupCodess = new List<ImportGroupCodesDto>();

            foreach (var ledger in GroupCodess)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateGroupCodesAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidGroupCodess.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidGroupCodess.Add(ledger);
                    }
                }
                else
                {
                    invalidGroupCodess.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportGroupCodesResultAsync(args, invalidGroupCodess));
        }

        private async Task CreateGroupCodesAsync(ImportGroupCodesDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var GroupCodess = _objectMapper.Map<SetupForms.GroupCode>(input);
            GroupCodess.TenantId = (int)tenantId;
           // var GroupCodes = await _GroupCodesRepository.FirstOrDefaultAsync(x => x.Seg1ID == GroupCodess.Seg1ID && x.TenantId == tenantId);
            //if (GroupCodes == null)
            //{
            //    await _GroupCodesRepository.InsertAsync(GroupCodess);
            //}
            //else
            //{
            //    input.Id = GroupCodes.Id;
            //    _objectMapper.Map(input, GroupCodes);
            //}



        }

        private async Task ProcessImportGroupCodesResultAsync(ImportGroupCodesFromExcelJobArgs args, List<ImportGroupCodesDto> invalidGroupCodess)
        {
            if (invalidGroupCodess.Any())
            {
                var file = _invalidGroupCodesExporter.ExportToFile(invalidGroupCodess);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllGroupCodesSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportGroupCodesFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToGroupCodessList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
