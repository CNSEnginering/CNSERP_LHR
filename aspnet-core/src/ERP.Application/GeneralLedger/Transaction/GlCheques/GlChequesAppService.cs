

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.GeneralLedger.Transaction.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using ERP.Authorization.Users;
using ERP.GeneralLedger.SetupForms;
using ERP.SupplyChain.Inventory;
using ERP.CommonServices;
using Abp.Collections.Extensions;
using ERP.GeneralLedger.Transaction.BankReconcile.Exporting;

namespace ERP.GeneralLedger.Transaction
{
    [AbpAuthorize(AppPermissions.Pages_GlCheques)]
    public class GlChequesAppService : ERPAppServiceBase, IGlChequesAppService
    {
        private readonly IRepository<GlCheque> _glChequeRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ChartofControl, string> _chartofControlRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedgerRepository;
        private readonly IRepository<GLLocation> _locRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<AccountSubLedger> _accountSubLedger;
        private readonly IGlChequesExcelExporter _glChequesExcelExporter;
        public GlChequesAppService(IRepository<GlCheque> glChequeRepository,
            IRepository<User, long> userRepository,
            IRepository<ChartofControl, string> chartofControlRepository,
            IRepository<AccountSubLedger> accountSubLedgerRepository,
            IRepository<GLLocation> locRepository,
            IRepository<Bank> bankRepository,
            IGlChequesExcelExporter glChequesExcelExporter,
            IRepository<AccountSubLedger> accountSubLedger)
        {
            _glChequeRepository = glChequeRepository;
            _userRepository = userRepository;
            _chartofControlRepository = chartofControlRepository;
            _accountSubLedgerRepository = accountSubLedgerRepository;
            _locRepository = locRepository;
            _bankRepository = bankRepository;
            _glChequesExcelExporter = glChequesExcelExporter;
            _accountSubLedger = accountSubLedger;
        }

        public async Task<PagedResultDto<GetGlChequeForViewDto>> GetAll(GetAllGlChequesInput input)
        {
            IQueryable<GlCheque> filteredGlCheques;
            if (input.Filter != null)
            {
                if (input.Filter.ToLower() == "issued" || input.Filter.ToLower() == "received")
                {
                    if (Convert.ToInt32(input.ChequeStatusFilter) == 0)
                    {
                        filteredGlCheques =
                   input.Filter.ToLower() == "issued" ?
                    _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                             .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.TypeID == input.TypeIDFilter) :
                            _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                             .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.TypeID == input.TypeIDFilter);
                    }
                    else
                    {
                        filteredGlCheques =
                        input.Filter.ToLower() == "issued" ?
                         _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                  .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.TypeID == input.TypeIDFilter
                                  && o.ChequeStatus == Convert.ToInt32(input.ChequeStatusFilter)
                                  ) :
                                 _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                  .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.TypeID == input.TypeIDFilter
                                  && o.ChequeStatus == Convert.ToInt32(input.ChequeStatusFilter)
                                  );
                    }
                }
                else
                {
                    if (Convert.ToInt32(input.ChequeStatusFilter) == 0)
                    {
                        filteredGlCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                  .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.DocID.ToString() == input.Filter
                                  );
                    }
                    else
                    {
                        filteredGlCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                     .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.DocID.ToString() == input.Filter
                                     &&
                                     o.ChequeStatus == Convert.ToInt32(input.ChequeStatusFilter)
                                     );

                    }
                }
            }
            else
            {

                filteredGlCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                    .WhereIf(Convert.ToInt32(input.ChequeStatusFilter) != 0, e => e.ChequeStatus == Convert.ToInt32(input.ChequeStatusFilter))
                .WhereIf(input.TypeIDFilter != null, e => e.TypeID == input.TypeIDFilter);

                //if (Convert.ToInt32(input.ChequeStatusFilter) == 0)
                //{
                //    filteredGlCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
                //}
                //else
                //{
                //    filteredGlCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.ChequeStatus == Convert.ToInt32(input.ChequeStatusFilter));
                //}                 
            }



            var pagedAndFilteredGlCheques = filteredGlCheques
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input);

            var glCheques = from o in pagedAndFilteredGlCheques
                            join
                            a in _accountSubLedger.GetAll()
                            on new { A = Convert.ToInt32(o.PartyID), B = o.TenantId, C = o.AccountID } equals new { A = a.Id, B = a.TenantId, C = a.AccountID }
                            into accJoin
                            from acc in accJoin.DefaultIfEmpty()
                            join
                            c in _bankRepository.GetAll()
                            on new { A = o.BankID, B = o.TenantId } equals new { A = c.BANKID, B = c.TenantId }
                            into bankJoin
                            from bank in bankJoin.DefaultIfEmpty()
                            where (o.TenantId == AbpSession.TenantId)
                            select new GetGlChequeForViewDto()
                            {
                                GlCheque = new GlChequeDto
                                {

                                    TenantID = o.TenantId,
                                    DocID = o.DocID,
                                    TypeID = o.TypeID,
                                    EntryDate = o.EntryDate.Value.Year.ToString() + "-" + o.EntryDate.Value.Month.ToString() + "-" + o.EntryDate.Value.Day.ToString(),
                                    ChequeDate = o.ChequeDate.Value.Year.ToString() + "-" + o.ChequeDate.Value.Month.ToString() + "-" + o.ChequeDate.Value.Day.ToString(),
                                    ChequeNo = o.ChequeNo,
                                    ChequeAmt = o.ChequeAmt,
                                    PartyName = acc.SubAccName,
                                    BankDesc = bank.BANKNAME,
                                    ChequeStatus = (o.ChequeStatus == 1) ? "Collected" : (o.ChequeStatus == 2) ? "Issued"
                                                   : (o.ChequeStatus == 3) ? "Deposited" : (o.ChequeStatus == 4) ? "Cleared"
                                                   : (o.ChequeStatus == 5) ? "Cancelled" : (o.ChequeStatus == 6) ? "Holded"
                                                   : (o.ChequeStatus == 7) ? "Bounced" : "",
                                    //ChequeStatus = o.ChequeStatus,
                                    PartyBank = o.PartyBank,
                                    ChequeRef = o.ChequeRef,
                                    Remarks = o.Remarks,
                                    LocationID = o.LocationID,
                                    AccountID = o.AccountID,
                                    PartyID = o.PartyID,
                                    BankID = o.BankID,
                                    // Posted = o.Posted,
                                    AUDTUSER = o.AUDTUSER,
                                    AudtDate = o.AudtDate,
                                    CreatedBy = o.CreatedBy,
                                    CreatedDate = o.CreatedDate,
                                    Id = o.Id,
                                    Posted=o.Posted
                                }
                            };

            var totalCount = await filteredGlCheques.CountAsync();

            return new PagedResultDto<GetGlChequeForViewDto>(
                totalCount,
                await glCheques.ToListAsync()
            );
        }

        public async Task<GetGlChequeForViewDto> GetGlChequeForView(int id)
        {
            var glCheque = await _glChequeRepository.GetAsync(id);

            var output = new GetGlChequeForViewDto { GlCheque = ObjectMapper.Map<GlChequeDto>(glCheque) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_GlCheques_Edit)]
        public async Task<GetGlChequeForEditOutput> GetGlChequeForEdit(EntityDto input)
        {
            var glCheque = await _glChequeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetGlChequeForEditOutput { GlCheque = ObjectMapper.Map<CreateOrEditGlChequeDto>(glCheque) };
            output.GlCheque.LocDesc = _locRepository.GetAll().Where(o => o.LocId == output.GlCheque.LocationID && o.TenantId == AbpSession.TenantId).Count() > 0 ?
                _locRepository.GetAll().Where(o => o.LocId == output.GlCheque.LocationID && o.TenantId == AbpSession.TenantId).FirstOrDefault().LocDesc :
                "";
            output.GlCheque.AccountName = _chartofControlRepository.GetAll().Where(o => o.Id == output.GlCheque.AccountID && o.TenantId == AbpSession.TenantId).Count() > 0 ?
               _chartofControlRepository.GetAll().Where(o => o.Id == output.GlCheque.AccountID && o.TenantId == AbpSession.TenantId).FirstOrDefault().AccountName :
               "";
            output.GlCheque.PartyName = _accountSubLedgerRepository.GetAll().Where(o => o.AccountID == output.GlCheque.AccountID
            && o.Id == output.GlCheque.PartyID && o.TenantId == AbpSession.TenantId
            ).Count() > 0 ?
              _accountSubLedgerRepository.GetAll().Where(o => o.AccountID == output.GlCheque.AccountID
            && o.Id == output.GlCheque.PartyID && o.TenantId == AbpSession.TenantId).FirstOrDefault().SubAccName :
              "";
            output.GlCheque.BankName = _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BANKID == output.GlCheque.BankID).Count() > 0 ?
                _bankRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId && o.BANKID == output.GlCheque.BankID).FirstOrDefault().BANKNAME : "";
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditGlChequeDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_GlCheques_Create)]
        protected virtual async Task Create(CreateOrEditGlChequeDto input)
        {
            var glCheque = ObjectMapper.Map<GlCheque>(input);
            glCheque.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            glCheque.CreatedBy = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            glCheque.AudtDate = DateTime.Now;
            glCheque.CreatedDate = DateTime.Now;
            if (input.ChequeStatus == "4" || input.ChequeStatus == "3")
            { 
                glCheque.StatusDate = input.StatusDate;
            }
            else
            {
                glCheque.StatusDate = null;
            }

            if (AbpSession.TenantId != null)
            {
                glCheque.TenantId = (int)AbpSession.TenantId;
            }


            await _glChequeRepository.InsertAsync(glCheque);
        }

        [AbpAuthorize(AppPermissions.Pages_GlCheques_Edit)]
        protected virtual async Task Update(CreateOrEditGlChequeDto input)
        {
            var glCheque = await _glChequeRepository.FirstOrDefaultAsync((int)input.Id);
            input.AUDTUSER = _userRepository.GetAll().Where(o => o.Id == AbpSession.UserId).SingleOrDefault().UserName;
            input.AudtDate = DateTime.Now;
            input.CreatedDate = glCheque.CreatedDate;
            if (input.ChequeStatus == "4" || input.ChequeStatus == "3")
            {
                glCheque.StatusDate = input.StatusDate;
            }
            else
            {
                glCheque.StatusDate = null;
            }

            if (AbpSession.TenantId != null)
            {
                input.TenantID = Convert.ToInt32(AbpSession.TenantId);
            }
            ObjectMapper.Map(input, glCheque);
        }

        [AbpAuthorize(AppPermissions.Pages_GlCheques_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _glChequeRepository.DeleteAsync(input.Id);
        }

        public int GetDocId(string type)
        {
            var result = _glChequeRepository.GetAll().Where(k => k.TypeID == Convert.ToInt32(type)).DefaultIfEmpty().Max(o => o.DocID);
            return Convert.ToInt32(result) + 1;
        }


        public async Task<FileDto> GetDataToExcel(GetAllGlChequesInput input)
        {

            IQueryable<GlCheque> filteredGlCheques;
            if (input.Filter != null)
            {
                if (input.Filter.ToLower() == "issued" || input.Filter.ToLower() == "received")
                {
                    filteredGlCheques =
                    input.Filter.ToLower() == "issued" ?
                     _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.TypeID == 0) :
                             _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                              .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.TypeID == 1);
                }
                else
                {
                    filteredGlCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId)
                                  .WhereIf(!string.IsNullOrEmpty(input.Filter), o => o.DocID.ToString() == input.Filter
                                  );
                }
            }
            else
            {
                filteredGlCheques = _glChequeRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
            }

            var query = from o in filteredGlCheques
                        select new GetGlChequeForViewDto()
                        {
                            GlCheque = new GlChequeDto
                            {

                                TenantID = o.TenantId,
                                DocID = o.DocID,
                                TypeID = o.TypeID,
                                EntryDate = o.EntryDate.Value.Year.ToString() + "-" + o.EntryDate.Value.Month.ToString() + "-" + o.EntryDate.Value.Day.ToString(),
                                ChequeDate = o.ChequeDate.Value.Year.ToString() + "-" + o.ChequeDate.Value.Month.ToString() + "-" + o.ChequeDate.Value.Day.ToString(),
                                ChequeNo = o.ChequeNo,
                                ChequeAmt = o.ChequeAmt,
                                //ChequeStatus = o.ChequeStatus,
                                PartyBank = o.PartyBank,
                                ChequeRef = o.ChequeRef,
                                Remarks = o.Remarks,
                                LocationID = o.LocationID,
                                AccountID = o.AccountID,
                                PartyID = o.PartyID,
                                BankID = o.BankID,
                                // Posted = o.Posted,
                                AUDTUSER = o.AUDTUSER,
                                AudtDate = o.AudtDate,
                                CreatedBy = o.CreatedBy,
                                CreatedDate = o.CreatedDate,
                                Id = o.Id
                            }
                        };



            var bankReconcileListDtos = await query.ToListAsync();

            return _glChequesExcelExporter.ExportToFile(bankReconcileListDtos);
        }
    }
}