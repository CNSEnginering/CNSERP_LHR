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
using ERP.SupplyChain.Inventory.ItemPrice.Dto;
using ERP.SupplyChain.Inventory.ItemPrice.Importing.Dto;
using Abp.Runtime.Session;

namespace ERP.SupplyChain.Inventory.ItemPrice.Importing
{
    public class ImportItemPriceToExcelJob : BackgroundJob<ImportItemPriceFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ItemPricing> _ItemPriceRepository;
        private readonly IItemPriceListExcelDataReader _ItemPriceListExcelDataReader;
        private readonly IInvalidItemPriceExporter _invalidItemPriceExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<User, long> _userRepository;



private readonly IAbpSession _abpSession;
        public ImportItemPriceToExcelJob(
            IRepository<ItemPricing> ItemPriceRepository,
            IItemPriceListExcelDataReader ItemPriceListExcelDataReader,
            IInvalidItemPriceExporter invalidItemPriceExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper, IRepository<User, long> userRepository, IAbpSession abpSession)
        {
            _ItemPriceRepository = ItemPriceRepository;
            _ItemPriceListExcelDataReader = ItemPriceListExcelDataReader;
            _invalidItemPriceExporter = invalidItemPriceExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
            _userRepository = userRepository;
            _abpSession = abpSession;
        }

        [UnitOfWork]
        public override void Execute(ImportItemPriceFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var ItemPrice = GetItemPriceListFromExcelOrNull(args);
                if (ItemPrice == null || !ItemPrice.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateItemPrice(args, ItemPrice);
            }
        }

        private List<ImportItemPriceDto> GetItemPriceListFromExcelOrNull(ImportItemPriceFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _ItemPriceListExcelDataReader.GetItemPriceFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateItemPrice(ImportItemPriceFromExcelJobArgs args, List<ImportItemPriceDto> ItemPrices)
        {
            var invalidItemPrices = new List<ImportItemPriceDto>();

            foreach (var ledger in ItemPrices)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateItemPriceAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidItemPrices.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidItemPrices.Add(ledger);
                    }
                }
                else
                {
                    invalidItemPrices.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportItemPriceResultAsync(args, invalidItemPrices));
        }

        private async Task CreateItemPriceAsync(ImportItemPriceDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();
          //  long userid = (long)_abpSession.GetUserId();


            input.TenantId = (int)tenantId;

            var ItemPrices = _objectMapper.Map<ItemPricing>(input);
            ItemPrices.TenantId = (int)tenantId;
            var ItemPrice = await _ItemPriceRepository.FirstOrDefaultAsync(x => x.Id == ItemPrices.Id && x.TenantId == tenantId);
            if (ItemPrice == null)
            {
               if (input.CreateDate == null)
                {
                    ItemPrices.CreateDate = DateTime.Now;
                }

                if (string.IsNullOrWhiteSpace(input.CreatedBy))
                {
                    ItemPrices.CreatedBy = "admin";// _userRepository.GetAsync(userid).Result.UserName;
                }
                if (input.AudtDate == null)
                {
                    ItemPrices.AudtDate = DateTime.Now;
                }

                if (string.IsNullOrWhiteSpace(input.AudtUser))
                {
                    ItemPrices.AudtUser = "Admin"; // _userRepository.GetAsync(userid).Result.UserName;
                }

                await _ItemPriceRepository.InsertAsync(ItemPrices);
            }
            else
            {
                input.Id = ItemPrice.Id;
                _objectMapper.Map(input, ItemPrice);
            }



        }

        private async Task ProcessImportItemPriceResultAsync(ImportItemPriceFromExcelJobArgs args, List<ImportItemPriceDto> invalidItemPrices)
        {
            if (invalidItemPrices.Any())
            {
                var file = _invalidItemPriceExporter.ExportToFile(invalidItemPrices);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllItemPriceSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportItemPriceFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToItemPricesList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
