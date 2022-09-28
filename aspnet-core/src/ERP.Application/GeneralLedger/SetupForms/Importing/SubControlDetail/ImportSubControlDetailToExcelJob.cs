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
using ERP.GeneralLedger.SetupForms.Importing.SUbControlDetail;
using ERP.GeneralLedger.SetupForms.Importing.SubControlDetail.Dto;
using ERP.GeneralLedger.SetupForms.SubControlDetails.Dto;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ERP.GeneralLedger.SetupForms.Importing.SubControlDetail
{
    public class ImportSubControlDetailToExcelJob : BackgroundJob<ImportSubControlDetailFromExcelJobArgs>, ITransientDependency
    {
        private readonly IRepository<SetupForms.SubControlDetail> _SubControlDetailRepository;
        private readonly ISubControlDetailListExcelDataReader _SubControlDetailListExcelDataReader;
        private readonly IInvalidSubControlDetailExporter _invalidSubControlDetailExporter;
        private readonly IAppNotifier _appNotifier;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ILocalizationSource _localizationSource;
        private readonly IObjectMapper _objectMapper;

        public UserManager UserManager { get; set; }

        public ImportSubControlDetailToExcelJob(
            IRepository<SetupForms.SubControlDetail> SubControlDetailRepository,
            ISubControlDetailListExcelDataReader SubControlDetailListExcelDataReader,
            IInvalidSubControlDetailExporter invalidSubControlDetailExporter,
            IAppNotifier appNotifier,
            IBinaryObjectManager binaryObjectManager,
            ILocalizationManager localizationManager,
            IObjectMapper objectMapper)
        {
            _SubControlDetailRepository = SubControlDetailRepository;
            _SubControlDetailListExcelDataReader = SubControlDetailListExcelDataReader;
            _invalidSubControlDetailExporter = invalidSubControlDetailExporter;
            _appNotifier = appNotifier;
            _binaryObjectManager = binaryObjectManager;
            _objectMapper = objectMapper;
            _localizationSource = localizationManager.GetSource(ERPConsts.LocalizationSourceName);
        }

        [UnitOfWork]
        public override void Execute(ImportSubControlDetailFromExcelJobArgs args)
        {
            using (CurrentUnitOfWork.SetTenantId(args.TenantId))
            {
                var SubControlDetail = GetControlDetailListFromExcelOrNull(args);
                if (SubControlDetail == null || !SubControlDetail.Any())
                {
                    SendInvalidExcelNotification(args);
                    return;
                }

                CreateSubControlDetail(args, SubControlDetail);
            }
        }

        private List<ImportSubControlDetailDto> GetControlDetailListFromExcelOrNull(ImportSubControlDetailFromExcelJobArgs args)
        {
            try
            {
                var file = AsyncHelper.RunSync(() => _binaryObjectManager.GetOrNullAsync(args.BinaryObjectId));
                return _SubControlDetailListExcelDataReader.GetSubControlDetailFromExcel(file.Bytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void CreateSubControlDetail(ImportSubControlDetailFromExcelJobArgs args, List<ImportSubControlDetailDto> SubControlDetails)
        {
            var invalidSubControlDetails = new List<ImportSubControlDetailDto>();

            foreach (var ledger in SubControlDetails)
            {
                if (ledger.CanBeImported())
                {
                    try
                    {
                        AsyncHelper.RunSync(() => CreateSubControlDetailAsync(ledger));
                    }
                    catch (UserFriendlyException exception)
                    {
                        ledger.Exception = exception.Message;
                        invalidSubControlDetails.Add(ledger);
                    }
                    catch (Exception exception)
                    {
                        ledger.Exception = exception.ToString();
                        invalidSubControlDetails.Add(ledger);
                    }
                }
                else
                {
                    invalidSubControlDetails.Add(ledger);
                }
            }

            AsyncHelper.RunSync(() => ProcessImportSubControlDetailResultAsync(args, invalidSubControlDetails));



        }

        private async Task CreateSubControlDetailAsync(ImportSubControlDetailDto input)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();
            input.TenantId = (int)tenantId;

            //var SubControlDetails = _objectMapper.Map<SetupForms.SubControlDetail>(input);
            //SubControlDetails.TenantId = (int)tenantId;
            //var Subcontroldetail = await _SubControlDetailRepository.FirstOrDefaultAsync(x => x.Seg2ID == SubControlDetails.Seg2ID && x.TenantId == tenantId);
            //if (Subcontroldetail == null)
            //{
            //    var id = await _SubControlDetailRepository.InsertAndGetIdAsync(SubControlDetails);
            //    Insert_Seg3(SubControlDetails.Seg2ID, id);
            //}
            //else
            //{
            //    input.Id = Subcontroldetail.Id;

            //    //var AccSubControlDetail = await _SubControlDetailRepository.FirstOrDefaultAsync(x =>  x.Seg1ID == ControlDetails.Seg1ID && x.TenantId == tenantId);
            //    _objectMapper.Map(input, Subcontroldetail);
            //}


            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("SP_Insert_GLSeg2", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tenantId", tenantId);
                cmd.Parameters.AddWithValue("@Seg2ID", input.Seg2ID);
                cmd.Parameters.AddWithValue("@SegName", input.SegmentName);
                cn.Open();
                await cmd.ExecuteNonQueryAsync();
                //   // cn.Close();
            }


        }

        private void Insert_Seg3(string seg2Id, int id)
        {
            var tenantId = CurrentUnitOfWork.GetTenantId();
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            using (SqlConnection cn = new SqlConnection(str))
            {
                SqlCommand cmd = new SqlCommand("SP_Insert_GLSeg3", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tenantId", tenantId);
                cmd.Parameters.AddWithValue("@Seg2ID", seg2Id);
                cmd.Parameters.AddWithValue("@Id", id);
                cn.Open();
                cmd.ExecuteNonQuery();
             //   // cn.Close();
            }

        }

        private async Task ProcessImportSubControlDetailResultAsync(ImportSubControlDetailFromExcelJobArgs args, List<ImportSubControlDetailDto> invalidSubControlDetails)
        {
            if (invalidSubControlDetails.Any())
            {
                var file = _invalidSubControlDetailExporter.ExportToFile(invalidSubControlDetails);
                await _appNotifier.SomeUsersCouldntBeImported(args.User, file.FileToken, file.FileType, file.FileName);
            }
            else
            {
                await _appNotifier.SendMessageAsync(
                    args.User,
                    _localizationSource.GetString("AllSubControlDetailSuccessfullyImportedFromExcel"),
                    Abp.Notifications.NotificationSeverity.Success);
            }
        }

        private void SendInvalidExcelNotification(ImportSubControlDetailFromExcelJobArgs args)
        {
            _appNotifier.SendMessageAsync(
                args.User,
                _localizationSource.GetString("FileCantBeConvertedToSubControlDetailsList"),
                Abp.Notifications.NotificationSeverity.Warn);
        }
    }
}
