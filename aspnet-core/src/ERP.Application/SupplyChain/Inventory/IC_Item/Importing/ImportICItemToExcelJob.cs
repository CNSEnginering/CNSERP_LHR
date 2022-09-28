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
using ERP.SupplyChain.Inventory.IC_Item.Importing.Dto;
using ERP.SupplyChain.Inventory.IC_Item.Dto;

namespace ERP.SupplyChain.Inventory.IC_Item.Importing
{
    public class ImportICItemToExcelJob : BackgroundJob<ImportICItemFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IICItemListExcelDataReader _ICItemListExcelDataReader;
        private readonly IInvalidICItemExporter _invalidICItemExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportICItemToExcelJob(
            IRepository<ICItem> ICItemRepository,
            IICItemListExcelDataReader ICItemListExcelDataReader,
            IInvalidICItemExporter invalidICItemExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _ICItemRepository = ICItemRepository;
            _ICItemListExcelDataReader = ICItemListExcelDataReader;
            _invalidICItemExporter = invalidICItemExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportICItemFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var ICItem = GetICItemListFromExcelOrNull(args);
                if (ICItem == null || !ICItem.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateICItem(args, ICItem);
            }
        }

        private List<ImportICItemDto> GetICItemListFromExcelOrNull(ImportICItemFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _ICItemListExcelDataReader.GetICItemFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateICItem(ImportICItemFromExcelJobArgs args, List<ImportICItemDto> ICItems)
        {
            var invalidICItems = new List<ImportICItemDto>();

            foreach (var ledger in ICItems)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateICItemAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidICItems.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidICItems.Add(ledger);
                    }
                }
                else
                {
                    invalidICItems.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportICItemResultAsync(args, invalidICItems));
        }

        private async Task CreateICItemAsync(ImportICItemDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var ICItems = _objectMapper.Map<ICItem>(input);
            ICItems.TenantId = (int)tenantId;
            var ICItem = await _ICItemRepository.FirstOrDefaultAsync(x => x.ItemId == ICItems.ItemId && x.TenantId == tenantId);
            if (ICItem == null)
            {
                await _ICItemRepository.InsertAsync(ICItems);
            }
            else
            {
                input.Id = ICItem.Id;
                _objectMapper.Map(input, ICItem);
            }



        }

        private async Task ProcessImportICItemResultAsync(ImportICItemFromExcelJobArgs args, List<ImportICItemDto> invalidICItems)
        {
            if (invalidICItems.Any())
            {
                var file = _invalidICItemExporter.ExportToFile(invalidICItems);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllICItemSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportICItemFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToICItemsList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
