using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using ERP.CommonServices;
using ERP.Reports.SupplyChain.Inventory.Dtos;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.Consumption;
using ERP.SupplyChain.Inventory.IC_Item;
using ERP.SupplyChain.Inventory.Opening;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class ConsumptionReportAppService : ERPReportAppServiceBase
    {
        private readonly IRepository<ICCNSHeader> _icCNSHeaderRepository;
        private readonly IRepository<CostCenter> _ccRepository;
        private readonly IRepository<ICCNSDetail> _icCNSDetailRepository;
        private readonly IRepository<ICLocation> _icLocationRepository;
        private readonly IRepository<ICItem> _ICItemRepository;
        private readonly IRepository<CompanyProfile, string> _companyProfileRepository;

        public ConsumptionReportAppService(IRepository<ICCNSHeader> icCNSHeaderRepository,
            IRepository<ICCNSDetail> icCNSDetailRepository, IRepository<ICLocation> icLocationRepository,
            IRepository<ICItem> ICItemRepository, IRepository<CompanyProfile, string> companyProfileRepository,
            IRepository<CostCenter> ccRepository)
        {
            _icCNSHeaderRepository = icCNSHeaderRepository;
            _icCNSDetailRepository = icCNSDetailRepository;
            _icLocationRepository = icLocationRepository;
            _ICItemRepository = ICItemRepository;
            _companyProfileRepository = companyProfileRepository;
            _ccRepository = ccRepository;
        }

        public List<ConsumptionReportDto> GetConsumptionData(DateTime fromDate, DateTime toDate, int fromDoc, int toDoc, string fromLoc, string toLoc, string ccId,string fromItem,string toItem)
        {
            List<int> LocIds = new List<int>();
            if (fromLoc != "")
            {
                LocIds = fromLoc.Split(',').Select(int.Parse).ToList();
            }
            ccId = ccId == "" ? ccId = "0" : ccId.Trim();
            var tenantId = AbpSession.TenantId;
            IQueryable<ICCNSHeader> iccnsHeader = null;
            IQueryable<CostCenter> costCenter = null;
            //if (ccId != "0")
            //    iccnsHeader = _icCNSHeaderRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.DocDate.Date >= fromDate.Date && d.DocDate.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc && d.CCID == ccId);
            //else
                iccnsHeader = _icCNSHeaderRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.DocDate.Date >= fromDate.Date && d.DocDate.Date <= toDate.Date).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);


            var iccnsDetail = _icCNSDetailRepository.GetAll().Where(e => e.TenantId == tenantId).Where(d => d.DocNo >= fromDoc && d.DocNo <= toDoc);
            var locations = _icLocationRepository.GetAll().Where(e => e.TenantId == tenantId &&
                            LocIds.Contains((int)e.LocID));
            //var icItem = _ICItemRepository.GetAll().Where(e => e.TenantId == tenantId && e.ItemId.CompareTo(fromItem)>= 0 && e.ItemId.CompareTo(toItem)<= 0 );
            var icItem = _ICItemRepository.GetAll().Where(e => e.TenantId == tenantId);
            var company = _companyProfileRepository.GetAll().Where(e => e.TenantId == tenantId);
            //if (ccId != "0")
            //    costCenter = _ccRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.CCID == ccId);
            //else
                costCenter = _ccRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var consumption = (from d in iccnsDetail
                               join h in iccnsHeader on d.DocNo equals h.DocNo
                               //join c in costCenter on h.CCID equals c.CCID
                               join l in locations on h.LocID equals l.LocID
                               join i in icItem on d.ItemID equals i.ItemId into it
                               from p in it.DefaultIfEmpty()

                               select new ConsumptionReportDto
                               {
                                   LocID = h.LocID,
                                   CCID ="", //c.CCID,
                                   CCName ="", //c.CCName,
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
                                   Rate = Convert.ToDecimal(d.Cost),
                                   Amount = Convert.ToDecimal(d.Amount),
                                   CreatedBy = h.CreatedBy,
                                   CompanyName = company.SingleOrDefault().CompanyName,
                                   CompanyAddress = company.SingleOrDefault().Address1.ToString() + "-" + company.SingleOrDefault().Address2.ToString(),
                                   CompanyPhone = company.SingleOrDefault().Phone,
                                   StartDate = fromDate,
                                   EndDate = toDate
                               }).ToList();

            string pjson = JsonConvert.SerializeObject(consumption);
            return consumption.ToList();
        }
    }
}
