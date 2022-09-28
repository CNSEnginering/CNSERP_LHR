using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.SetupForms.Exporting;
using ERP.GeneralLedger.SetupForms.Dtos;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRD;
using ERP.GeneralLedger.Transaction.VoucherEntry.GLTRH;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;

namespace ERP.GeneralLedger.SetupForms
{
	//[AbpAuthorize(AppPermissions.SetupForms_AccountSubLedgers)]
    public class VendorActivityAppService : ERPAppServiceBase, IVendorActivityAppService
    {
		 private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
		 private readonly IAccountSubLedgersExcelExporter _accountSubLedgersExcelExporter;
		 
	


        private readonly IRepository<GLTRDetail> _gltrDetailRepository;
        private readonly IRepository<GLTRHeader> _gltrHeaderRepository;



        public VendorActivityAppService( IRepository<GLTRDetail> gltrDetailRepository,  IRepository<GLTRHeader> gltrHeaderRepository) 
		  {
            _gltrHeaderRepository = gltrHeaderRepository;
            _gltrDetailRepository = gltrDetailRepository;
          }

        public  ListResultDto<GetVenderActivityForViewDto> GetVendorActivityForView(GetVendorActivityInputs inputs)
        {
           
            var query = (from o in _gltrHeaderRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId)
                         join o1 in _gltrDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId) on o.Id equals o1.DetID
                         into j1
                         from s1 in j1.DefaultIfEmpty()
                         where s1.SubAccID == inputs.SubAccID && s1.AccountID == inputs.AccountID 
                         //&& DateTime.Parse(o.DocDate.ToLongDateString()) < DateTime.Parse(DateTime.Now.ToLongDateString())
                        select new GetVenderActivityForViewDto
                         {
                             VenderActivity = new VenderActivityDto
                             {
                                 DocDate = o.DocDate,
                                 Type = o.BookID + '-' + o.ConfigID,
                                 DocNo = o.DocNo,
                                 Narration = s1.Narration,
                                 Debit = s1.Amount > 0 ? (double)s1.Amount : 0,
                                 Credit = s1.Amount < 0 ? (double)s1.Amount : 0,
                                 RunningTotal = 0
                             },
                             LastPayment = _gltrDetailRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).OrderByDescending(x => x.Id).Where(x => x.SubAccID == inputs.SubAccID && x.AccountID == inputs.AccountID).Select(s => s.Amount).FirstOrDefault()
                         }).ToList();



            var Outstandbal = query.Where(x => DateTime.Parse(x.VenderActivity.DocDate.ToLongDateString()) <= DateTime.Parse(DateTime.Now.ToLongDateString())).Select(x =>  (x.VenderActivity.Debit - Math.Abs(x.VenderActivity.Credit)));

            var OpeningBal = query.Where(x => DateTime.Parse(x.VenderActivity.DocDate.ToLongDateString()) < DateTime.Parse(DateTime.Now.ToLongDateString())).Select(x => (x.VenderActivity.Debit - Math.Abs(x.VenderActivity.Credit)));

            //from o in _gltrHeaderRepository.GetAll()
            //                  join o1 in _gltrDetailRepository.GetAll() on o.Id equals o1.DetID
            //                  into j1
            //                  from s1 in j1.DefaultIfEmpty()
            //                  where s1.SubAccID == inputs.SubAccID && s1.AccountID == inputs.AccountID && DateTime.Parse(o.DocDate.ToLongDateString()) < DateTime.Parse(DateTime.Now.ToLongDateString())
            //                  select new GetVenderActivityForViewDto
            //                  {
            //                      OpeningBalance = (decimal)j1.Sum(s1 => (double)s1.Amount)
            //                  };


            //    var final = query.Select(x=> x.OpeningBalance = Outstandbal.Select(o => o.OpeningBalance).Sum());

            decimal balance = 0;


            int i = 0;

            foreach (var item in query)
            {
                if (i == 0)
                {
                    balance += (decimal)OpeningBal.Sum() + (decimal)item.VenderActivity.Debit - (decimal)Math.Abs(item.VenderActivity.Credit);
                    item.VenderActivity.RunningTotal = (double)balance;
                }
                else
                {
                    balance +=  (decimal)item.VenderActivity.Debit - (decimal)Math.Abs(item.VenderActivity.Credit);
                    item.VenderActivity.RunningTotal = (double)balance;
                }


                item.OpeningBalance += (decimal)OpeningBal.Sum();
                item.OutstandingBalance += (decimal)Outstandbal.Sum();

                i++;
            }


            return new ListResultDto<GetVenderActivityForViewDto>(
                 query
                );
        }
    }
}