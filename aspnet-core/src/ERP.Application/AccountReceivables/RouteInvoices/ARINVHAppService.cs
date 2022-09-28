

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.AccountReceivables.RouteInvoices.Exporting;
using ERP.AccountReceivables.RouteInvoices.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Sales.OERoutes;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Sales.SalesReference;

namespace ERP.AccountReceivables.RouteInvoices
{
	[AbpAuthorize(AppPermissions.Pages_ARINVH)]
    public class ARINVHAppService : ERPAppServiceBase, IARINVHAppService
    {
        private readonly IRepository<ARINVH> _arinvhRepository;
		private readonly IARINVHExcelExporter _arinvhExcelExporter;
        private readonly IRepository<ARINVD> _arinvdRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<GLLocation> _glLocationRepository;
        private readonly IRepository<OERoutes> _oeRoutesRepository;
        private readonly IRepository<TransactionType> _transactionTypeRepository;
        private readonly IRepository<SalesReference> _salesReferenceRepository;



        public ARINVHAppService(IRepository<ARINVH> arinvhRepository,
            IARINVHExcelExporter arinvhExcelExporter,
            IRepository<ARINVD> arinvdRepository,
            IRepository<User, long> userRepository,
            IRepository<GLLocation> glLocationRepository,
            IRepository<OERoutes> oeRoutesRepository,
            IRepository<TransactionType> transactionTypeRepository,
            IRepository<SalesReference> salesReferenceRepository

            )
        {
			_arinvhRepository = arinvhRepository;
			_arinvhExcelExporter = arinvhExcelExporter;
            _arinvdRepository = arinvdRepository;
            _userRepository = userRepository;
            _glLocationRepository = glLocationRepository;
            _oeRoutesRepository = oeRoutesRepository;
            _transactionTypeRepository = transactionTypeRepository;
            _salesReferenceRepository = salesReferenceRepository;

        }

        public async Task<PagedResultDto<GetARINVHForViewDto>> GetAll(GetAllARINVHInput input)
         {

            var filteredARINVH = _arinvhRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SaleTypeID.Contains(input.Filter) || e.PaymentOption.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.PostedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinInvDateFilter != null, e => e.InvDate >= input.MinInvDateFilter)
                        .WhereIf(input.MaxInvDateFilter != null, e => e.InvDate <= input.MaxInvDateFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinRoutIDFilter != null, e => e.RoutID >= input.MinRoutIDFilter)
                        .WhereIf(input.MaxRoutIDFilter != null, e => e.RoutID <= input.MaxRoutIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.RefNoFilter), e => e.RefNo.ToLower() == input.RefNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaleTypeIDFilter), e => e.SaleTypeID.ToLower() == input.SaleTypeIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentOptionFilter), e => e.PaymentOption.ToLower() == input.PaymentOptionFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration.ToLower() == input.NarrationFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID.ToLower() == input.BankIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter), e => e.ChequeNo.ToLower() == input.ChequeNoFilter.ToLower().Trim())
                        .WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                        .WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter), e => e.PostedBy.ToLower() == input.PostedByFilter.ToLower().Trim())
                        .WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
                        .WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter);

			var pagedAndFilteredARINVH = filteredARINVH
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var arinvh = from o in pagedAndFilteredARINVH
                         select new GetARINVHForViewDto() {
							ARINVH = new ARINVHDto
							{
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                InvDate = o.InvDate,
                                LocID = o.LocID,
                                RoutID = o.RoutID,
                                RefNo = o.RefNo,
                                SaleTypeID = o.SaleTypeID,
                                PaymentOption = o.PaymentOption,
                                Narration = o.Narration,
                                BankID = o.BankID,
                                AccountID = o.AccountID,
                                ConfigID = o.ConfigID,
                                ChequeNo = o.ChequeNo,
                                LinkDetID = o.LinkDetID,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Posted = o.Posted,
                                PostedBy = o.PostedBy,
                                PostedDate = o.PostedDate,
                                Id = o.Id
							}
						};

            var totalCount = await filteredARINVH.CountAsync();

            return new PagedResultDto<GetARINVHForViewDto>(
                totalCount,
                await arinvh.ToListAsync()
            );
         }
		 
		 public async Task<GetARINVHForViewDto> GetARINVHForView(int id)
         {
            var arinvh = await _arinvhRepository.GetAsync(id);

            var output = new GetARINVHForViewDto { ARINVH = ObjectMapper.Map<ARINVHDto>(arinvh) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ARINVH_Edit)]
		 public async Task<CreateOrEditARINVHDto> GetARINVHForEdit(EntityDto input)
         {
            var arinvh = await _arinvhRepository.FirstOrDefaultAsync(input.Id);

            var output = ObjectMapper.Map<CreateOrEditARINVHDto>(arinvh);

            //Location Description 
            output.LocDesc = _glLocationRepository.GetAll().Where(o => o.LocId == output.LocID).Count() > 0 ?
              _glLocationRepository.GetAll().Where(o => o.LocId == output.LocID).FirstOrDefault().LocDesc : "";

            //Route Description
            output.RoutDesc = _oeRoutesRepository.GetAll().Where(o => o.RoutID == output.RoutID).Count() > 0 ?
                _oeRoutesRepository.GetAll().Where(o => o.RoutID == output.RoutID).FirstOrDefault().RoutDesc : "";

            //SaleType Description
            output.SaleTypeDesc = _transactionTypeRepository.GetAll().Where(o => o.TypeId == output.SaleTypeID).Count() > 0 ?
                _transactionTypeRepository.GetAll().Where(o => o.TypeId == output.SaleTypeID).FirstOrDefault().Description : "";

            //Reference Description
            output.RefDesc = _salesReferenceRepository.GetAll().Where(o => o.RefID == output.RefNo).Count() > 0 ?
                _salesReferenceRepository.GetAll().Where(o => o.RefID == output.RefNo).FirstOrDefault().RefName : "";

            var detailInvoices = _arinvdRepository.GetAll().Where(x => x.DetID == input.Id && x.TenantId == (int)AbpSession.TenantId);
            output.ARINVDetailDto = ObjectMapper.Map<List<ARINVDDto>>(detailInvoices);
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditARINVHDto input)
         {
            if(input.Id == null)
            {
				await Create(input);
			}
			else
            {
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ARINVH_Create)]
		 protected virtual async Task Create(CreateOrEditARINVHDto input)
         {
            var arinvh = ObjectMapper.Map<ARINVH>(input);

			
			if (AbpSession.TenantId != null)
			{
				arinvh.TenantId = (int) AbpSession.TenantId;
                arinvh.CreateDate = DateTime.Now;
                arinvh.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            }
            var getId = await _arinvhRepository.InsertAndGetIdAsync(arinvh);

            if (input.ARINVDetailDto != null)
            {
                foreach (var data in input.ARINVDetailDto)
                {
                    var InvoiceDetail = ObjectMapper.Map<ARINVD>(data);
                    if (AbpSession.TenantId != null)
                    {
                        InvoiceDetail.TenantId = (int)AbpSession.TenantId;
                        //InvoiceDetail.CreatedDate = DateTime.Now;
                        //InvoiceDetail.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    }
                    InvoiceDetail.DetID = getId;
                    //InvoiceDetail.DocNo = input.DocNo;
                    await _arinvdRepository.InsertAsync(InvoiceDetail);
                }
            }
		

            //await _arinvhRepository.InsertAsync(arinvh);
         }

        [AbpAuthorize(AppPermissions.Pages_ARINVH_Edit)]
        protected virtual async Task Update(CreateOrEditARINVHDto input)
        {
            var arinvh = await _arinvhRepository.FirstOrDefaultAsync((int)input.Id);
            arinvh.AudtDate = DateTime.Now;
            arinvh.AudtUser = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;

            ObjectMapper.Map(input, arinvh);

            var InvoiceDetail = await _arinvdRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId && x.DetID == input.Id).ToListAsync();
            if (InvoiceDetail != null)
            {
                foreach (var item in InvoiceDetail)
                {
                    await _arinvdRepository.DeleteAsync(item);
                }
            }

            if (input.ARINVDetailDto != null)
            {
                foreach (var data in input.ARINVDetailDto)
                {
                    var InvoiceDetail1 = ObjectMapper.Map<ARINVD>(data);
                    if (AbpSession.TenantId != null)
                    {
                        InvoiceDetail1.TenantId = (int)AbpSession.TenantId;
                        //InvoiceDetail1.CreatedDate = DateTime.Now;
                        //InvoiceDetail1.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
                    }
                    InvoiceDetail1.DetID = arinvh.Id;
                    await _arinvdRepository.InsertAsync(InvoiceDetail1);
                }
            }
        }

		 [AbpAuthorize(AppPermissions.Pages_ARINVH_Delete)]
         public async Task Delete(EntityDto input)
         {
            var InvoiceDetail = await _arinvdRepository.GetAll().Where(x=>x.TenantId == AbpSession.TenantId && x.DetID == input.Id).ToListAsync();
            if(InvoiceDetail != null)
            {
                foreach (var item in InvoiceDetail)
                {
                    await _arinvdRepository.DeleteAsync(item);
                }
            }
            await _arinvhRepository.DeleteAsync(input.Id);
         }

        public int GetDocId()
        {
            var result = _arinvhRepository.GetAll().DefaultIfEmpty().Max(o => o.DocNo);
            return result = result + 1;
        }

        public List<ARINVDDto> GetPostedInvoices(int routID, DateTime invDate)
        {
            string str = ConfigurationManager.AppSettings["ConnectionString"];
            var result = new List<ARINVDDto>();
            using (SqlConnection cn = new SqlConnection(str))
            {
                using (SqlCommand cmd = new SqlCommand("GetrouteDetail", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@SaleTypeID", saleTypeID);
                    cmd.Parameters.AddWithValue("@RoutID", routID);
                    cmd.Parameters.AddWithValue("@InvDate", invDate);
                    cmd.Parameters.AddWithValue("@TenantID", AbpSession.TenantId);
                    cn.Open();
                    using (SqlDataReader dataReader = cmd.ExecuteReader())
                    {

                        while (dataReader.Read())
                        {

                            result.Add(new ARINVDDto
                            {
                                AccountID = Convert.ToString(dataReader["SalesCtrlAcc"]),
                                AccountName = Convert.ToString(dataReader["AccountName"]),
                                SubAccID = Convert.ToInt32(dataReader["CustID"]),
                                SubAccName = Convert.ToString(dataReader["SubAccName"]),
                                DocNo = Convert.ToInt32(dataReader["DocNo"]),
                                InvNumber = Convert.ToString(dataReader["COKEINVOICE"]),
                                InvAmount = Convert.ToDouble(dataReader["INVAMOUNT"]),
                                //TaxAmount = Convert.ToDouble(dataReader["TAXAMOUNT"]),
                                RecpAmount = Convert.ToDouble(dataReader["RECEIPTAMT"]),
                                //ChequeNo = Convert.ToString(dataReader["CHEQUENO"])


                                //Date = dataReader["InvDate"].ToString(),
                                //Amount = Convert.ToDouble(dataReader["Amount"]),
                                //AlreadyPaid = Convert.ToDouble(dataReader["AlreadyPaid"]),
                                //Pending = Convert.ToDouble(dataReader["Pending"])
                            });
                        }
                    }
                }
            }
            return result;
        }
        public async Task<FileDto> GetARINVHToExcel(GetAllARINVHForExcelInput input)
         {

            var filteredARINVH = _arinvhRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.SaleTypeID.Contains(input.Filter) || e.PaymentOption.Contains(input.Filter) || e.Narration.Contains(input.Filter) || e.BankID.Contains(input.Filter) || e.AccountID.Contains(input.Filter) || e.ChequeNo.Contains(input.Filter) || e.AudtUser.Contains(input.Filter) || e.CreatedBy.Contains(input.Filter) || e.PostedBy.Contains(input.Filter))
                        .WhereIf(input.MinDocNoFilter != null, e => e.DocNo >= input.MinDocNoFilter)
                        .WhereIf(input.MaxDocNoFilter != null, e => e.DocNo <= input.MaxDocNoFilter)
                        .WhereIf(input.MinDocDateFilter != null, e => e.DocDate >= input.MinDocDateFilter)
                        .WhereIf(input.MaxDocDateFilter != null, e => e.DocDate <= input.MaxDocDateFilter)
                        .WhereIf(input.MinInvDateFilter != null, e => e.InvDate >= input.MinInvDateFilter)
                        .WhereIf(input.MaxInvDateFilter != null, e => e.InvDate <= input.MaxInvDateFilter)
                        .WhereIf(input.MinLocIDFilter != null, e => e.LocID >= input.MinLocIDFilter)
                        .WhereIf(input.MaxLocIDFilter != null, e => e.LocID <= input.MaxLocIDFilter)
                        .WhereIf(input.MinRoutIDFilter != null, e => e.RoutID >= input.MinRoutIDFilter)
                        .WhereIf(input.MaxRoutIDFilter != null, e => e.RoutID <= input.MaxRoutIDFilter)
                        //.WhereIf(!string.IsNullOrWhiteSpace(input.RefNoFilter), e => e.RefNo.ToLower() == input.RefNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SaleTypeIDFilter), e => e.SaleTypeID.ToLower() == input.SaleTypeIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PaymentOptionFilter), e => e.PaymentOption.ToLower() == input.PaymentOptionFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NarrationFilter), e => e.Narration.ToLower() == input.NarrationFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BankIDFilter), e => e.BankID.ToLower() == input.BankIDFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AccountIDFilter), e => e.AccountID.ToLower() == input.AccountIDFilter.ToLower().Trim())
                        .WhereIf(input.MinConfigIDFilter != null, e => e.ConfigID >= input.MinConfigIDFilter)
                        .WhereIf(input.MaxConfigIDFilter != null, e => e.ConfigID <= input.MaxConfigIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChequeNoFilter), e => e.ChequeNo.ToLower() == input.ChequeNoFilter.ToLower().Trim())
                        .WhereIf(input.MinLinkDetIDFilter != null, e => e.LinkDetID >= input.MinLinkDetIDFilter)
                        .WhereIf(input.MaxLinkDetIDFilter != null, e => e.LinkDetID <= input.MaxLinkDetIDFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AudtUserFilter), e => e.AudtUser.ToLower() == input.AudtUserFilter.ToLower().Trim())
                        .WhereIf(input.MinAudtDateFilter != null, e => e.AudtDate >= input.MinAudtDateFilter)
                        .WhereIf(input.MaxAudtDateFilter != null, e => e.AudtDate <= input.MaxAudtDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedByFilter), e => e.CreatedBy.ToLower() == input.CreatedByFilter.ToLower().Trim())
                        .WhereIf(input.MinCreateDateFilter != null, e => e.CreateDate >= input.MinCreateDateFilter)
                        .WhereIf(input.MaxCreateDateFilter != null, e => e.CreateDate <= input.MaxCreateDateFilter)
                        .WhereIf(input.PostedFilter > -1, e => Convert.ToInt32(e.Posted) == input.PostedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PostedByFilter), e => e.PostedBy.ToLower() == input.PostedByFilter.ToLower().Trim())
                        .WhereIf(input.MinPostedDateFilter != null, e => e.PostedDate >= input.MinPostedDateFilter)
                        .WhereIf(input.MaxPostedDateFilter != null, e => e.PostedDate <= input.MaxPostedDateFilter);

			var query = (from o in filteredARINVH
                         select new GetARINVHForViewDto() { 
							ARINVH = new ARINVHDto
							{
                                DocNo = o.DocNo,
                                DocDate = o.DocDate,
                                InvDate = o.InvDate,
                                LocID = o.LocID,
                                RoutID = o.RoutID,
                                RefNo = o.RefNo,
                                SaleTypeID = o.SaleTypeID,
                                PaymentOption = o.PaymentOption,
                                Narration = o.Narration,
                                BankID = o.BankID,
                                AccountID = o.AccountID,
                                ConfigID = o.ConfigID,
                                ChequeNo = o.ChequeNo,
                                LinkDetID = o.LinkDetID,
                                AudtUser = o.AudtUser,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreateDate = o.CreateDate,
                                Posted = o.Posted,
                                PostedBy = o.PostedBy,
                                PostedDate = o.PostedDate,
                                Id = o.Id
							}
						 });


            var arinvhListDtos = await query.ToListAsync();

            return _arinvhExcelExporter.ExportToFile(arinvhListDtos);
         }


    }
}