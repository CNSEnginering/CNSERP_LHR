using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ERP.Finders.Dtos;
using ERP.Manufacturing.SetupForms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Finders.Manufacturing
{
    public class MFFindersAppService : ERPAppServiceBase
    {
        private readonly IRepository<MFTOOLTY> _mfToolRepository;
        private readonly IRepository<MFRESMAS> _mfResMasResRepository;
        private readonly IRepository<MFTOOL> _mfToolResRepository;
        public MFFindersAppService(IRepository<MFTOOL> mfToolResRepository,
            IRepository<MFRESMAS> mfResMasResRepository, IRepository<MFTOOLTY> mfToolRepository)
        {
            _mfToolRepository = mfToolRepository;
            _mfResMasResRepository = mfResMasResRepository;
            _mfToolResRepository = mfToolResRepository;
        }
        [HttpGet]
        public async Task<LookupDto<ToolTypeFindersDto>> GetAllToolTypelookupTable(GetAllForLookupTableInput input)
        {
            var record = new LookupDto<ToolTypeFindersDto>();
            if (input.Target=="Resources")
            {
                record=await GetResourceLookupTable(input);
            }
            else if (input.Target=="Tools")
            {
                record= await GetToolLookupTable(input);
            }
            else
            {
                record= await GetToolTypeLookupTable(input);
            }
            return record;
        }
        private async Task<LookupDto<ToolTypeFindersDto>> GetResourceLookupTable(GetAllForLookupTableInput input)
        {
            var query = _mfResMasResRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new ToolTypeFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.RESDESC,
                                     UnitCost=o.UNITCOST.ToString(),
                                     UOM=o.UNIT
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<ToolTypeFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<ToolTypeFindersDto>> GetToolLookupTable(GetAllForLookupTableInput input)
        {
            var query = _mfToolResRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new ToolTypeFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     DisplayName = o.TOOLDESC
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<ToolTypeFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }
        private async Task<LookupDto<ToolTypeFindersDto>> GetToolTypeLookupTable(GetAllForLookupTableInput input)
        {
            var query = _mfToolRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);

            var lookupTableDto = from o in query
                                 select new ToolTypeFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     TypeId = o.TOOLTYID,
                                     DisplayName = o.TOOLTYDESC,
                                     UnitCost = o.UNITCOST.ToString(),
                                     UOM = o.UNIT
                                 };
            lookupTableDto = input.Sorting == "id DESC" ? lookupTableDto.OrderByDescending(o => o.Id) : lookupTableDto.OrderBy(o => o.Id);
            var pageData = lookupTableDto.PageBy(input);
            var lookupTableDtoList = await pageData.ToListAsync();
            var getData = new LookupDto<ToolTypeFindersDto>(
                query.Count(),
                lookupTableDtoList
            );

            return getData;
        }

    }
}
