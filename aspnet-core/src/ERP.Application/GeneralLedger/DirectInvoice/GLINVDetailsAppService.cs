

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.GeneralLedger.DirectInvoice.Dtos;
using ERP.GeneralLedger.SetupForms;

namespace ERP.GeneralLedger.DirectInvoice
{
	[AbpAuthorize(AppPermissions.Pages_GLINVDetails)]
    public class GLINVDetailsAppService : ERPAppServiceBase, IGLINVDetailsAppService
    {
		 private readonly IRepository<GLINVDetail> _glinvDetailRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;


        public GLINVDetailsAppService(IRepository<GLINVDetail> glinvDetailRepository, IRepository<ChartofControl, string> chartofControlRepository, IRepository<AccountSubLedger> accountSubLedgerRepository) 
		  {
			_glinvDetailRepository = glinvDetailRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;

        }

        public async Task<PagedResultDto<GLINVDetailDto>> GetGLINVDData(int detId)
        {

            var filteredGLINVDetails = _glinvDetailRepository.GetAll().Where(e => e.DetID == detId);
            var chartAcc = _chartofControlRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);
            var subAcc = _accountSubLedgerRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId);

            var glinvDetails = from ca in chartAcc
                              join d in filteredGLINVDetails on ca.Id equals d.AccountID
                              join sl in subAcc on new { X = d.AccountID, Y = (int)d.SubAccID } equals new { X = sl.AccountID, Y = sl.Id } into sb
                              from p in sb.DefaultIfEmpty()
                              select new GLINVDetailDto
                              {
                                DetID = d.DetID,
                                AccountID = d.AccountID,
                                AccountDesc = ca.AccountName,
                                SubAccID = d.SubAccID,
                                SubAccDesc = p.SubAccName != null ? p.SubAccName : "",
                                Narration = d.Narration,
                                Amount = d.Amount,
                                IsAuto = d.IsAuto,
                                Id = d.Id
                              };

            var totalCount = await glinvDetails.CountAsync();

            return new PagedResultDto<GLINVDetailDto>(
                totalCount,
                await glinvDetails.ToListAsync()
            );
        }

    }
}