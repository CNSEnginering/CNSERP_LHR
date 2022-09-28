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
using ERP.GeneralLedger.SetupForms.Importing.GroupCategory.Dto;
using ERP.GeneralLedger.SetupForms.GroupCategories.Dto;

namespace ERP.GeneralLedger.SetupForms.Importing.GroupCategory
{
    public class ImportGroupCategoryToExcelJob : BackgroundJob<ImportGroupCategoryFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<ERP.GeneralLedger.SetupForms.GroupCategory> _GroupCategoryRepository;
        private readonly IGroupCategoryListExcelDataReader _GroupCategoryListExcelDataReader;
        private readonly IInvalidGroupCategoryExporter _invalidGroupCategoryExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportGroupCategoryToExcelJob(
            IRepository<SetupForms.GroupCategory> GroupCategoryRepository,
            IGroupCategoryListExcelDataReader GroupCategoryListExcelDataReader,
            IInvalidGroupCategoryExporter invalidGroupCategoryExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _GroupCategoryRepository = GroupCategoryRepository;
            _GroupCategoryListExcelDataReader = GroupCategoryListExcelDataReader;
            _invalidGroupCategoryExporter = invalidGroupCategoryExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportGroupCategoryFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var GroupCategory = GetGroupCategoryListFromExcelOrNull(args);
                if (GroupCategory == null || !GroupCategory.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateGroupCategory(args, GroupCategory);
            }
        }

        private List<ImportGroupCategoryDto> GetGroupCategoryListFromExcelOrNull(ImportGroupCategoryFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _GroupCategoryListExcelDataReader.GetGroupCategoryFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateGroupCategory(ImportGroupCategoryFromExcelJobArgs args, List<ImportGroupCategoryDto> GroupCategorys)
        {
            var invalidGroupCategorys = new List<ImportGroupCategoryDto>();

            foreach (var ledger in GroupCategorys)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateGroupCategoryAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidGroupCategorys.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidGroupCategorys.Add(ledger);
                    }
                }
                else
                {
                    invalidGroupCategorys.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportGroupCategoryResultAsync(args, invalidGroupCategorys));
        }

        private async Task CreateGroupCategoryAsync(ImportGroupCategoryDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();

            
            input.TenantId = (int)tenantId;

            var GroupCategorys = _objectMapper.Map<SetupForms.GroupCategory>(input);
            GroupCategorys.TenantId = (int)tenantId;
            //var GroupCategory = await _GroupCategoryRepository.FirstOrDefaultAsync(x => x.Seg1ID == GroupCategorys.Seg1ID && x.TenantId == tenantId);
            //if (GroupCategory == null)
            //{
            //    await _GroupCategoryRepository.InsertAsync(GroupCategorys);
            //}
            //else
            //{
            //    input.Id = GroupCategory.Id;
            //    _objectMapper.Map(input, GroupCategory);
            //}



        }

        private async Task ProcessImportGroupCategoryResultAsync(ImportGroupCategoryFromExcelJobArgs args, List<ImportGroupCategoryDto> invalidGroupCategorys)
        {
            if (invalidGroupCategorys.Any())
            {
                var file = _invalidGroupCategoryExporter.ExportToFile(invalidGroupCategorys);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllGroupCategorySuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportGroupCategoryFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToGroupCategorysList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
