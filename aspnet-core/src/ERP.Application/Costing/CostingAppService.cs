using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ERP.SupplyChain.Inventory;
using System.Linq;

namespace ERP.Costing
{
    public class CostingAppService : ERPAppServiceBase
    {
        private readonly IRepository<ICLEDG> _icledgRepository;
        private readonly IRepository<ICSetup> _icsetupRepository;
        public CostingAppService(
            IRepository<ICLEDG> icledgRepository,
            IRepository<ICSetup> icsetupRepository
            )
        {
            _icledgRepository = icledgRepository;
            _icsetupRepository = icsetupRepository;
        }

        public double getCosting(DateTime docDate, string itemId, int locId, int docType, int docId)
        {
            var icSetup = _icsetupRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            double? costing = 0;
            if (icSetup.Count() > 0)
            {
                if (icSetup.FirstOrDefault().CostingMethod == 1)
                    costing = MovingCost(docDate, itemId, locId, docType, docId);
                else if (icSetup.FirstOrDefault().CostingMethod == 2)
                    costing = FifoCost(docDate, itemId, locId, docType, docId);
                else if (icSetup.FirstOrDefault().CostingMethod == 3)
                    costing = LifoCost(docDate, itemId, locId, docType, docId);
            }
            return Math.Round(Convert.ToDouble(costing), 2);
        }

        private double MovingCost(DateTime docDate, string itemId, int locId, int DocType, int DocId)
        {
            var result = _icledgRepository.GetAll().Where(o => o.DocDate <= docDate && o.ItemID == itemId && o.LocID == locId && !(o.DocType == DocType && o.DocNo >= DocId))
               .GroupBy(x => true)
               .Select(x => new
               {
                   Qty = x.Sum(y => y.Qty * y.Conver),
                   Amount = x.Sum(y => y.Amount),
               });
            if (result.Count() > 0)
                return result.FirstOrDefault().Qty == 0 ? 0 : Convert.ToDouble(result.FirstOrDefault().Amount / result.FirstOrDefault().Qty);
            else
                return 0;
        }
        private double? FifoCost(DateTime docDate, string itemId, int locId, int DocType, int DocId)
        {
            double? iBal = 0, TotalRec = 0, fifoCost = 0;
            List<int> docTypeArr = new List<int>() { 1, 3 };
            var itemBalance = _icledgRepository.GetAll().Where(o => o.DocDate <= docDate && o.ItemID == itemId && o.LocID == locId && !(o.DocType == DocType && o.DocNo >= DocId))
               .GroupBy(x => true)
               .Select(x => new
               {
                   balance = x.Sum(y => y.Qty * y.Conver)
               });
            var qtySum = _icledgRepository.GetAll().Where(o => o.DocDate <= docDate && o.ItemID == itemId && o.LocID == locId
                         && (docTypeArr.Contains(o.DocType) || (o.DocType == 6 && o.Qty > 0)))
               .GroupBy(x => true)
               .Select(x => new
               {
                   qty = x.Sum(y => y.Qty),
               });
            if (qtySum.Count() > 0)
                TotalRec = qtySum.FirstOrDefault().qty;

            if (TotalRec <= 0)
                iBal = TotalRec - (itemBalance.Count() > 0 ? itemBalance.FirstOrDefault().balance : 0);

            var result = _icledgRepository.GetAll().Where(o => o.DocDate <= docDate && o.ItemID == itemId && o.LocID == locId
                        && (docTypeArr.Contains(o.DocType) || (o.DocType == 6 && o.Qty > 0))).OrderBy(o => o.DocDate);

            foreach (var row in result)
            {
                iBal = iBal - (row.Qty * row.Conver);
                if (iBal < 0)
                    fifoCost = row.Rate / row.Conver;
            }

            return iBal;
        }

        private double? LifoCost(DateTime docDate, string itemId, int locId, int DocType, int DocId)
        {
            double? iBal = 0, TotalRec = 0, lifoCost = 0;
            List<int> docTypeArr = new List<int>() { 1, 3 };
            var itemBalance = _icledgRepository.GetAll().Where(o => o.DocDate <= docDate && o.ItemID == itemId && o.LocID == locId && !(o.DocType == DocType && o.DocNo >= DocId))
               .GroupBy(x => true)
               .Select(x => new
               {
                   balance = x.Sum(y => y.Qty * y.Conver)
               });
            var qtySum = _icledgRepository.GetAll().Where(o => o.DocDate <= docDate && o.ItemID == itemId && o.LocID == locId
                         && (docTypeArr.Contains(o.DocType) || (o.DocType == 6 && o.Qty > 0)))
               .GroupBy(x => true)
               .Select(x => new
               {
                   qty = x.Sum(y => y.Qty),
               });
            if (qtySum.Count() > 0)
                TotalRec = qtySum.FirstOrDefault().qty;

            if (TotalRec <= 0)
                iBal = TotalRec - (itemBalance.Count() > 0 ? itemBalance.FirstOrDefault().balance : 0);

            var result = _icledgRepository.GetAll().Where(o => o.DocDate <= docDate && o.ItemID == itemId && o.LocID == locId
                        && (docTypeArr.Contains(o.DocType) || (o.DocType == 6 && o.Qty > 0))).OrderByDescending(o => o.DocDate);

            foreach (var row in result)
            {
                iBal = iBal - (row.Qty * row.Conver);
                if (iBal < 0)
                    lifoCost = row.Rate / row.Conver;
            }

            return iBal;
        }
    }
}
