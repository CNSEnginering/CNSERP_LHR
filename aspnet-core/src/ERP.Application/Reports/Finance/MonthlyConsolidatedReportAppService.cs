using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using ERP.Reports.Finance.Dto;

namespace ERP.Reports.Finance
{
    public class MonthlyConsolidatedReportAppService : ERPReportAppServiceBase, IMonthlyConsolidatedReportAppService
    {
        private readonly IRepository<MonthlyConsolidated> _monthlyConsolidatedRepository;
        public MonthlyConsolidatedReportAppService(IRepository<MonthlyConsolidated> monthlyConsolidatedRepository)
        {
            _monthlyConsolidatedRepository = monthlyConsolidatedRepository;
        }

        public List<MonthlyConsolidatedRpt> GetConsolidatedLedgers(DateTime fromDate, DateTime toDate, string fromAccount, string toAccount, string status, int fromLocId, int toLocId, int? curRate)
        {
            IQueryable<MonthlyConsolidatedRpt> data;
            if (status == "Approved")
            {
                data = from a in _monthlyConsolidatedRepository.GetAll()
                      .Where(o => o.TenantId == AbpSession.TenantId && o.Posted == false && o.Approved == true
                      && o.DocDate.Date >= fromDate.Date && o.DocDate.Date <= toDate.Date
                      && o.AccountId.CompareTo(fromAccount) >= 0
                      && o.AccountId.CompareTo(toAccount) <= 0
                      && o.LocId >= fromLocId && o.LocId <= toLocId
                      )
                       select new MonthlyConsolidatedRpt()
                       {
                           AccountID = a.AccountId,
                           AccountName = a.AccountName,
                           Jan = a.Jan / Convert.ToDouble(curRate),
                           JanDate = "Jan-" + GetYear(fromDate, toDate, 1),
                           Feb = a.Feb / Convert.ToDouble(curRate),
                           FebDate = "Feb-" + GetYear(fromDate, toDate, 2),
                           Mar = a.Mar / Convert.ToDouble(curRate),
                           MarDate = "Mar-" + GetYear(fromDate, toDate, 3),
                           Apr = a.Apr / Convert.ToDouble(curRate),
                           AprDate = "Apr-" + GetYear(fromDate, toDate, 4),
                           May = a.May / Convert.ToDouble(curRate),
                           MayDate = "May-" + GetYear(fromDate, toDate, 5),
                           Jun = a.Jun / Convert.ToDouble(curRate),
                           JunDate = "Jun-" + GetYear(fromDate, toDate, 6),
                           Jul = a.Jul / Convert.ToDouble(curRate),
                           JulDate = "Jul-" + GetYear(fromDate, toDate, 7),
                           Aug = a.Aug / Convert.ToDouble(curRate),
                           AugDate = "Aug-" + GetYear(fromDate, toDate, 8),
                           Sep = a.Sep / Convert.ToDouble(curRate),
                           SepDate = "Sep-" + GetYear(fromDate, toDate, 9),
                           Oct = a.Oct / Convert.ToDouble(curRate),
                           OctDate = "Oct-" + GetYear(fromDate, toDate, 10),
                           Nov = a.Nov / Convert.ToDouble(curRate),
                           NovDate = "Nov-" + GetYear(fromDate, toDate, 11),
                           Dec = a.Dec / Convert.ToDouble(curRate),
                           DecDate = "Dec-" + GetYear(fromDate, toDate, 12)
                       };
            }
            else if (status == "Posted")
            {
                data = from a in _monthlyConsolidatedRepository.GetAll()
                     .Where(o => o.TenantId == AbpSession.TenantId && o.Posted == true && o.Approved == false
                     && o.DocDate.Date >= fromDate.Date && o.DocDate.Date <= toDate.Date
                     && o.AccountId.CompareTo(fromAccount) >= 0
                     && o.AccountId.CompareTo(toAccount) <= 0
                     && o.LocId >= fromLocId && o.LocId <= toLocId
                     )
                       select new MonthlyConsolidatedRpt()
                       {
                           AccountID = a.AccountId,
                           AccountName = a.AccountName,
                           Jan = a.Jan / Convert.ToDouble(curRate),
                           JanDate = "Jan-" + GetYear(fromDate, toDate, 1),
                           Feb = a.Feb / Convert.ToDouble(curRate),
                           FebDate = "Feb-" + GetYear(fromDate, toDate, 2),
                           Mar = a.Mar / Convert.ToDouble(curRate),
                           MarDate = "Mar-" + GetYear(fromDate, toDate, 3),
                           Apr = a.Apr / Convert.ToDouble(curRate),
                           AprDate = "Apr-" + GetYear(fromDate, toDate, 4),
                           May = a.May / Convert.ToDouble(curRate),
                           MayDate = "May-" + GetYear(fromDate, toDate, 5),
                           Jun = a.Jun / Convert.ToDouble(curRate),
                           JunDate = "Jun-" + GetYear(fromDate, toDate, 6),
                           Jul = a.Jul / Convert.ToDouble(curRate),
                           JulDate = "Jul-" + GetYear(fromDate, toDate, 7),
                           Aug = a.Aug / Convert.ToDouble(curRate),
                           AugDate = "Aug-" + GetYear(fromDate, toDate, 8),
                           Sep = a.Sep / Convert.ToDouble(curRate),
                           SepDate = "Sep-" + GetYear(fromDate, toDate, 9),
                           Oct = a.Oct / Convert.ToDouble(curRate),
                           OctDate = "Oct-" + GetYear(fromDate, toDate, 10),
                           Nov = a.Nov / Convert.ToDouble(curRate),
                           NovDate = "Nov-" + GetYear(fromDate, toDate, 11),
                           Dec = a.Dec / Convert.ToDouble(curRate),
                           DecDate = "Dec-" + GetYear(fromDate, toDate, 12)
                       };
            }
            else
            {
                data = from a in _monthlyConsolidatedRepository.GetAll()
                     .Where(o => o.TenantId == AbpSession.TenantId && o.Posted == true && o.Approved == true
                     && o.DocDate.Date >= fromDate.Date && o.DocDate.Date <= toDate.Date
                     && o.AccountId.CompareTo(fromAccount) >= 0
                     && o.AccountId.CompareTo(toAccount) <= 0
                     && o.LocId >= fromLocId && o.LocId <= toLocId
                     )
                       select new MonthlyConsolidatedRpt()
                       {
                           AccountID = a.AccountId,
                           AccountName = a.AccountName,
                           Jan = a.Jan / Convert.ToDouble(curRate),
                           JanDate = "Jan-" + GetYear(fromDate, toDate, 1),
                           Feb = a.Feb / Convert.ToDouble(curRate),
                           FebDate = "Feb-" + GetYear(fromDate, toDate, 2),
                           Mar = a.Mar / Convert.ToDouble(curRate),
                           MarDate = "Mar-" + GetYear(fromDate, toDate, 3),
                           Apr = a.Apr / Convert.ToDouble(curRate),
                           AprDate = "Apr-" + GetYear(fromDate, toDate, 4),
                           May = a.May / Convert.ToDouble(curRate),
                           MayDate = "May-" + GetYear(fromDate, toDate, 5),
                           Jun = a.Jun / Convert.ToDouble(curRate),
                           JunDate = "Jun-" + GetYear(fromDate, toDate, 6),
                           Jul = a.Jul / Convert.ToDouble(curRate),
                           JulDate = "Jul-" + GetYear(fromDate, toDate, 7),
                           Aug = a.Aug / Convert.ToDouble(curRate),
                           AugDate = "Aug-" + GetYear(fromDate, toDate, 8),
                           Sep = a.Sep / Convert.ToDouble(curRate),
                           SepDate = "Sep-" + GetYear(fromDate, toDate, 9),
                           Oct = a.Oct / Convert.ToDouble(curRate),
                           OctDate = "Oct-" + GetYear(fromDate, toDate, 10),
                           Nov = a.Nov / Convert.ToDouble(curRate),
                           NovDate = "Nov-" + GetYear(fromDate, toDate, 11),
                           Dec = a.Dec / Convert.ToDouble(curRate),
                           DecDate = "Dec-" + GetYear(fromDate, toDate, 12)
                       };
            }

            string jsonp = JsonConvert.SerializeObject(data);
            return data.ToList();
        }
        public string GetYear(DateTime from, DateTime thru, int month)
        {
            var data = "";
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddMonths(1))
            {
                data = (month == day.Month) ? day.ToString("yy") : "";
                if (data != "")
                    break;
            }

            return data;
        }

        public string GetCol(int tenantId, DateTime fromDate, DateTime toDate, string fromAccount, string toAccount, int month)
        {
            var data = "";
            while (fromDate < toDate)
            {
                if (fromDate.Month == month)
                    data = fromDate.ToString("yy");
            }
            //var data = _monthlyConsolidatedRepository.GetAll()
            //            .Where(o => o.TenantId == tenantId
            //           && o.DocDate.Date >= fromDate.Date && o.DocDate.Date <= toDate.Date
            //           && o.AccountId.CompareTo(fromAccount) >= 0
            //           && o.AccountId.CompareTo(toAccount) <= 0
            //           && o.DocDate.Date.Month == month
            //           ).Select(x => new { res = x.DocDate.ToString("yy") }).Count() > 0
            //           ?
            //           _monthlyConsolidatedRepository.GetAll()
            //            .Where(o => o.TenantId == tenantId
            //           && o.DocDate.Date >= fromDate.Date && o.DocDate.Date <= toDate.Date
            //           && o.AccountId.CompareTo(fromAccount) >= 0
            //           && o.AccountId.CompareTo(toAccount) <= 0
            //           && o.DocDate.Date.Month == month
            //           ).Select(x => new { res = x.DocDate.ToString("yy") }).FirstOrDefault().res.ToString() : "";
            return data;
        }


    }
}
