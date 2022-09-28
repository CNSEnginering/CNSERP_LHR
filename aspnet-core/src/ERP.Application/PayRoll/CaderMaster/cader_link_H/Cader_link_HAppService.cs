using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using ERP.PayRoll.CaderMaster.cader_link_H.Dtos;
using ERP.Dto;
using Abp.Application.Services.Dto;
using ERP.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using ERP.Storage;
using ERP.Authorization.Users;
using ERP.PayRoll.CaderMaster.cader_link_D;
using System.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.SqlClient;

namespace ERP.PayRoll.CaderMaster.cader_link_H
{
    [AbpAuthorize(AppPermissions.Pages_Cader_link_H)]
    public class Cader_link_HAppService : ERPAppServiceBase, ICader_link_HAppService
    {
        private readonly IRepository<Cader_link_H> _cader_link_HRepository;
        private readonly IRepository<Cader_link_D> _cader_link_DRepository;
        private readonly IRepository<User,long> _userrepository;
        private readonly IRepository<Cader.Cader> _caderRepository;

        public Cader_link_HAppService(IRepository<Cader_link_H> cader_link_HRepository, IRepository<Cader.Cader> caderRepository, IRepository<Cader_link_D> cader_link_DRepository, IRepository<User,long> userrepository)
        {
            _cader_link_HRepository = cader_link_HRepository;
            _userrepository = userrepository;
            _cader_link_DRepository = cader_link_DRepository;
            _caderRepository = caderRepository;

        }

        public async Task<PagedResultDto<GetCader_link_HForViewDto>> GetAll(GetAllCader_link_HInput input)
        {

            var filteredCader_link_H = _cader_link_HRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Createdby.Contains(input.Filter) || e.Audit_by.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CreatedbyFilter), e => e.Createdby == input.CreatedbyFilter)
                        .WhereIf(input.MinCreatedDateFilter != null, e => e.CreatedDate >= input.MinCreatedDateFilter)
                        .WhereIf(input.MaxCreatedDateFilter != null, e => e.CreatedDate <= input.MaxCreatedDateFilter)
                        .WhereIf(input.MinAuditdateFilter != null, e => e.Auditdate >= input.MinAuditdateFilter)
                        .WhereIf(input.MaxAuditdateFilter != null, e => e.Auditdate <= input.MaxAuditdateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Audit_byFilter), e => e.Audit_by == input.Audit_byFilter);

            var pagedAndFilteredCader_link_H = filteredCader_link_H
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cader_link_H = from o in pagedAndFilteredCader_link_H
                               select new
                               {

                                   o.Createdby,
                                   o.CreatedDate,
                                   o.Auditdate,
                                   o.Audit_by,
                                   Id = o.Id
                               };

            var totalCount = await filteredCader_link_H.CountAsync();

            var dbList = await cader_link_H.ToListAsync();
            var results = new List<GetCader_link_HForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCader_link_HForViewDto()
                {
                    Cader_link_H = new Cader_link_HDto
                    {

                        Createdby = o.Createdby,
                        CreatedDate = o.CreatedDate,
                        Auditdate = o.Auditdate,
                        Audit_by = o.Audit_by,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCader_link_HForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetCader_link_HForViewDto> GetCader_link_HForView(int id)
        {
            var cader_link_H = await _cader_link_HRepository.GetAsync(id);

            var output = new GetCader_link_HForViewDto { Cader_link_H = ObjectMapper.Map<Cader_link_HDto>(cader_link_H) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Cader_link_H_Edit)]
        public async Task<GetCader_link_HForEditOutput> GetCader_link_HForEdit(EntityDto input)
        {
            var cader_link_H = await _cader_link_HRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCader_link_HForEditOutput { Cader_link_H = ObjectMapper.Map<CreateOrEditCader_link_HDto>(cader_link_H) };

            return output;
        }
        public List<SelectListItem> GetCaderList()
        {
            try
            {
                var TenantId = (Int32)AbpSession.TenantId;
                var List = _caderRepository.GetAll().Where(c => c.TenantId == TenantId).Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.CADER_NAME
                }).ToList();
                return List;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public List<SelectListItem> GetPayTypeList()
        {
            try
            {
                string str = ConfigurationManager.AppSettings["ConnectionStringHRM"];
                List<SelectListItem> list = new List<SelectListItem>();
                using (SqlConnection cn = new SqlConnection(str))
                {
                    SqlCommand cm = new SqlCommand("select * from PayType p where p.TenantId = " + AbpSession.TenantId, cn);
                    cn.Open();
                    using (SqlDataReader rdr = cm.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            SelectListItem item = new SelectListItem()
                            {
                                Text = rdr["Name"].ToString(),
                            };
                            list.Add(item);
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task CreateOrEdit(CreateOrEditCader_link_HDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Cader_link_H_Create)]
        protected virtual async Task Create(CreateOrEditCader_link_HDto input)
        {
            try
            {
                var cader_link_H = ObjectMapper.Map<Cader_link_H>(input);

                if (AbpSession.TenantId != null)
                {
                    cader_link_H.TenantId = (int)AbpSession.TenantId;
                    cader_link_H.Createdby = _userrepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId).FirstOrDefault().UserName;
                    cader_link_H.CreatedDate = DateTime.Now;
                }

                var caderM = await _cader_link_HRepository.InsertAndGetIdAsync(cader_link_H);

                if (input.CaderDetail != null && input.CaderDetail.Count > 0)
                {
                    foreach (var item in input.CaderDetail)
                    {

                        var caderDetail = ObjectMapper.Map<Cader_link_D>(item);
                        if (AbpSession.TenantId != null)
                        {
                            caderDetail.TenantId = (int)AbpSession.TenantId;
                        }
                        caderDetail.DetId = caderM;
                        await _cader_link_DRepository.InsertAsync(caderDetail);
                    }
                }
            }
            catch (Exception ex)
            {

               // throw;
            }
           

        }

        [AbpAuthorize(AppPermissions.Pages_Cader_link_H_Edit)]
        protected virtual async Task Update(CreateOrEditCader_link_HDto input)
        {
            var cader_link_H = await _cader_link_HRepository.FirstOrDefaultAsync((int)input.Id);
            cader_link_H.Audit_by = _userrepository.GetAll().Where(c => c.TenantId == AbpSession.TenantId).FirstOrDefault().UserName;
            cader_link_H.Auditdate = DateTime.Now;
            ObjectMapper.Map(input, cader_link_H);

            if (input.CaderDetail != null && input.CaderDetail.Count > 0)
            {
               await _cader_link_DRepository.DeleteAsync(c => c.DetId == input.Id);

                foreach (var item in input.CaderDetail)
                {
                   
                    var caderDetail = ObjectMapper.Map<Cader_link_D>(item);
                    if (AbpSession.TenantId != null)
                    {
                        caderDetail.TenantId = (int)AbpSession.TenantId;
                    }
                    caderDetail.DetId = input.Id.Value;
                    await _cader_link_DRepository.InsertAsync(caderDetail);
                }
            }

        }

        [AbpAuthorize(AppPermissions.Pages_Cader_link_H_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cader_link_HRepository.DeleteAsync(input.Id);
        }

    }
}