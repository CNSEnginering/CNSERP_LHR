using Abp.Domain.Repositories;
using ERP.CommonServices;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.Opening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class OpeningReportAppService: ERPReportAppServiceBase
    {
        private readonly IRepository<ICOPNHeader> _icOPNHeaderRepository;
        private readonly IRepository<ICOPNDetail> _icOPNDetailRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<CompanyProfile, string> _companyProfileRepository;

        public OpeningReportAppService(IRepository<ICOPNHeader> icOPNHeaderRepository, IRepository<ICOPNDetail> icopnDetailRepository, IRepository<ICLocation> icLocationRepository, IRepository<ICItem> ICItemRepository, IRepository<CompanyProfile, string> companyProfileRepository)
        {
            _icOPNHeaderRepository = icOPNHeaderRepository;
            _icOPNDetailRepository = icopnDetailRepository;
            _icLocationRepository = icLocationRepository;
            _ICItemRepository = ICItemRepository;
            _companyProfileRepository = companyProfileRepository;
        }

        public List<OpeningReportDto> GetOpeningData(DateTime fromDate, DateTime toDate, int fromDoc, int toDoc, string fromLoc, string toLoc)
        {
            List<int> LocIds = new List<int>();
            if (fromLoc != "")
            {
                LocIds = fromLoc.Split(',').Select(int.Parse).ToList();
            }

            var tenantId = AbpSession.TenantId;
            var icopnHeader = _icOPNHeaderRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.DocDate.Date >= fromDate.Date && d.DocDate.Date<=toDate.Date).Where(d=>d.DocNo>=fromDoc && d.DocNo<=toDoc);
            var icopnDetail = _icOPNDetailRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var locations = _icLocationRepository.GetAll().Where(e => e.TenantId == tenantId && 
                            LocIds.Contains((int)e.LocID));
            var icItem = _ICItemRepository.GetAll().Where(e => e.TenantId == tenantId);
            var company = _companyProfileRepository.GetAll().Where(e => e.TenantId == tenantId);


            var opening = (from d in icopnDetail
                            join h in icopnHeader on d.DocNo equals h.DocNo
                            join l in locations on h.LocID equals l.LocID
                            join i in icItem on d.ItemID equals i.ItemId into it
                            from p in it.DefaultIfEmpty()
                            select new OpeningReportDto
                            {
                                LocID = h.LocID,
                                LocDesc = l.LocName,
                                DocDate = h.DocDate.Date,
                                DocNo = d.DocNo,
                                Narration = h.Narration != null ? h.Narration : "",
                                OrderNo = h.OrdNo != null ? h.OrdNo : "",
                                ItemCode = d.ItemID,
                                ItemDesc = p.Descp != null ? p.Descp : "",
                                Unit = d.Unit,
                                Conver = Convert.ToDecimal(d.Conver),
                                Qty = Convert.ToDecimal(d.Qty),
                                Rate = Convert.ToDecimal(d.Rate),
                                Amount = Convert.ToDecimal(d.Amount),
                                CreatedBy = h.CreatedBy,
                                CompanyName = company.SingleOrDefault().CompanyName,
                                CompanyAddress = company.SingleOrDefault().Address1,
                                CompanyPhone = company.SingleOrDefault().Phone,
                                StartDate=fromDate,
                                EndDate=toDate
                            }).ToList();

            return opening.ToList();
        }
    }
}
