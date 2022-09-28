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
using ERP.AccountPayables.Importing.DirectInvoice.Dto;
using ERP.AccountPayables.Dtos;
using ERP.GeneralLedger.DirectInvoice;
using ERP.AccountPayables.Importing.DirectInvoice;

namespace ERP.AccountPayables.Importing.DirectInvoices
{
    public class ImportDirectInvoicesToExcelJob : BackgroundJob<ImportDirectInvoiceFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<GLINVHeader> _DirectInvoicesRepository;
        private readonly IDirectInvoicesListExcelDataReader _DirectInvoicesListExcelDataReader;
        private readonly IInvalidDirectInvoiceExporter _invalidDirectInvoicesExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportDirectInvoicesToExcelJob(
            IRepository<GLINVHeader> DirectInvoicesRepository,
            IDirectInvoicesListExcelDataReader DirectInvoicesListExcelDataReader,
            IInvalidDirectInvoiceExporter invalidDirectInvoicesExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _DirectInvoicesRepository = DirectInvoicesRepository;
            _DirectInvoicesListExcelDataReader = DirectInvoicesListExcelDataReader;
            _invalidDirectInvoicesExporter = invalidDirectInvoicesExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportDirectInvoiceFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var DirectInvoice = GetDirectInvoicesListFromExcelOrNull(args);
                if (DirectInvoice == null || !DirectInvoice.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateDirectInvoices(args, DirectInvoice);
            }
        }

        private List<ImportDirectInvoiceDto> GetDirectInvoicesListFromExcelOrNull(ImportDirectInvoiceFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _DirectInvoicesListExcelDataReader.GetDirectInvoiceFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateDirectInvoices(ImportDirectInvoiceFromExcelJobArgs args, List<ImportDirectInvoiceDto> DirectInvoices)
        {
            var invalidDirectInvoices = new List<ImportDirectInvoiceDto>();

            foreach (var ledger in DirectInvoices)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateDirectInvoicesAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidDirectInvoices.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidDirectInvoices.Add(ledger);
                    }
                }
                else
                {
                    invalidDirectInvoices.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportDirectInvoicesResultAsync(args, invalidDirectInvoices));
        }

        private async Task CreateDirectInvoicesAsync(ImportDirectInvoiceDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            //var DirectInvoices = _objectMapper.Map<GLINVHeader>(input);
            //DirectInvoices.TenantId = (int)tenantId;
            var DirectInvoice = await _DirectInvoicesRepository.FirstOrDefaultAsync(x => x.DocNo == input.DocNo && x.TypeID == "AP" && x.TenantId == tenantId);
            if (DirectInvoice != null)
            {
                DirectInvoice.CprID = input.CprID;
                DirectInvoice.CprNo = input.CprNo;
                DirectInvoice.CprDate = input.CprDate;
                try
                {
                    await _DirectInvoicesRepository.UpdateAsync(DirectInvoice);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
               
                //_objectMapper.Map(input, DirectInvoice);
            }
            
        }

        private async Task ProcessImportDirectInvoicesResultAsync(ImportDirectInvoiceFromExcelJobArgs args, List<ImportDirectInvoiceDto> invalidDirectInvoices)
        {
            if (invalidDirectInvoices.Any())
            {
                var file = _invalidDirectInvoicesExporter.ExportToFile(invalidDirectInvoices);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllDirectInvoiceSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportDirectInvoiceFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToDirectInvoicesList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
