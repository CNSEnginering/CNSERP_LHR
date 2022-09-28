using Abp.Domain.Repositories;
using ERP.CommonServices;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.Adjustment;
using ERP.SupplyChain.Inventory.IC_Item;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class AdjustmentReportAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<ICADJHeader> _icadjHeaderRepository;
        private readonly IRepository<ICADJDetail> _icadjDetailRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<CompanyProfile, string> _companyProfileRepository;

        public AdjustmentReportAppService(IRepository<ICADJHeader> icadjHeaderRepository, IRepository<ICADJDetail> icadjDetailRepository, IRepository<ICLocation> icLocationRepository, IRepository<ICItem> ICItemRepository, IRepository<CompanyProfile, string> companyProfileRepository)
        {
            _icadjHeaderRepository = icadjHeaderRepository;
            _icadjDetailRepository = icadjDetailRepository;
            _icLocationRepository = icLocationRepository;
            _ICItemRepository = ICItemRepository;
            _companyProfileRepository = companyProfileRepository;
        }

        public List<AdjustmentReportDto> GetAdjustmentData(DateTime fromDate, DateTime toDate, int fromDoc, int toDoc, string fromLoc, string toLoc)
        {
            List<int> LocIds = new List<int>();
            if (fromLoc != "")
            {
                LocIds = fromLoc.Split(',').Select(int.Parse).ToList();
            }

            var tenantId = AbpSession.TenantId;
            var iccnsHeader = _icadjHeaderRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.DocDate.Value.Date >= fromDate.Date && d.DocDate.Value.Date <=toDate.Date).Where(d=>d.DocNo>=fromDoc && d.DocNo<=toDoc);
            var iccnsDetail = _icadjDetailRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var locations = _icLocationRepository.GetAll().Where(e => e.TenantId == tenantId &&
                            LocIds.Contains((int)e.LocID));
            var icItem = _ICItemRepository.GetAll().Where(e => e.TenantId == tenantId);
            var company = _companyProfileRepository.GetAll().Where(e => e.TenantId == tenantId);


            var adjustment = (from d in iccnsDetail
                           join h in iccnsHeader on d.DocNo equals h.DocNo
                            join l in locations on h.LocID equals l.LocID
                            join i in icItem on d.ItemID equals i.ItemId into it
                            from p in it.DefaultIfEmpty()
                            select new AdjustmentReportDto
                            {
                                LocID=h.LocID.Value,
                                LocDesc=l.LocName,
                                DocDate=h.DocDate.Value.Date,
                                DocNo=d.DocNo,
                                Narration=h.Narration!= null ? h.Narration:"",
                                OrderNo=h.OrdNo!= null ? h.OrdNo:"",
                                ItemCode=d.ItemID,
                                ItemDesc=p.Descp!= null ? p.Descp:"",
                                Unit=d.Unit,
                                Conver= Convert.ToDecimal(d.Conver),
                                Type=d.Type,
                                Qty=Convert.ToDecimal(d.Qty),
                                Rate= Convert.ToDecimal(d.Cost),
                                Amount= Convert.ToDecimal(d.Amount),
                                CreatedBy=h.CreatedBy,
                                CompanyName = company.SingleOrDefault().CompanyName,
                                CompanyAddress = company.SingleOrDefault().Address1,
                                CompanyPhone = company.SingleOrDefault().Phone,
                                StartDate=fromDate,
                                EndDate=toDate
                            }).ToList();

            string pjson = JsonConvert.SerializeObject(adjustment);
            return adjustment.ToList();
        }
    }
}
