﻿using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory;
using ERP.SupplyChain.Inventory.IC_Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Reports.SupplyChain.Inventory
{
    public class GatePassReportAppService : ERPReportAppServiceBase , IGatePassReportAppService
    {
        private IRepository<GatePass> _gatePassRepository;
        private IRepository<GatePassDetail> _gatePassDetailRepository;
        private IRepository<ChartofControl, string> _accountRepository;
        private IRepository<AccountSubLedger> _accountSubRepository;
        private IRepository<ICItem> _itemRepository;
        public GatePassReportAppService(IRepository<GatePass> gatePassRepository,
          IRepository<GatePassDetail> gatePassDetailRepository,
          IRepository<ChartofControl, string> accountRepository,
          IRepository<AccountSubLedger> accountSubRepository,
          IRepository<ICItem> itemRepository)
        {
            _gatePassRepository = gatePassRepository;
            _gatePassDetailRepository = gatePassDetailRepository;
            _accountRepository = accountRepository;
            _accountSubRepository = accountSubRepository;
            _itemRepository = itemRepository;
        }
        public List<GatePassReport> GetData(int tenantId,
            int fromDoc, int toDoc,int typeId)
        {
            tenantId = tenantId > 0 ? tenantId : (int)AbpSession.TenantId;
            var data = from a in _gatePassRepository.GetAll().Where(a => a.TenantId == tenantId)
                       join
                       b in _gatePassDetailRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.TenantId, B = a.Id } equals new { A = b.TenantId, B = b.DetID }
                       join
                       c in _accountRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = a.AccountID, B = a.TenantId } equals new { A = c.Id, B = c.TenantId }
                       join
                       //d in _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId)
                       //on new { A = Convert.ToInt32(a.PartyID), B = a.TenantId } equals new { A = d.Id, B = d.TenantId }
                       //join
                       //d1 in _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId)
                       //on new { A = a.AccountID, B = a.TenantId } equals new { A = d1.AccountID, B = d1.TenantId }
                       //join
                       e in _itemRepository.GetAll().Where(a => a.TenantId == tenantId)
                       on new { A = b.ItemID, B = b.TenantId } equals new { A = e.ItemId, B = e.TenantId }
                       where (a.DocNo >= fromDoc && a.DocNo <= toDoc && a.TenantId == tenantId
                       && a.TypeID == typeId
                      )
                       select new GatePassReport()
                       {
                           PartyAddress = _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId).Count() > 0 ?
                           _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId).FirstOrDefault().Address1 + "" +
                           _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId).FirstOrDefault().Address2 : "",
                           PartyName = _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId).Count() > 0 ?
                           _accountSubRepository.GetAll().Where(a => a.TenantId == tenantId).FirstOrDefault().SubAccName : "",
                           Unit = b.Unit,
                           ItemId = b.ItemID,
                           Descp = e.Descp,
                           Qty = b.Qty,
                           Remarks = b.Comments,
                           DocNo = a.DocNo,
                           DriverName = a.DriverName,
                           VehicleNo = a.VehicleNo,
                           DocDate = a.DocDate.ToString(),
                           Type = a.TypeID == 1 ? "INWARD SLIP" : "OUTWARD SLIP",
                           Id = a.Id,
                           GPtype = a.GPType == 1 ? "Returnable" : "Non Returnable",
                           Narration = a.Narration
                       };
            return data.ToList();
        }
    }

   
}
