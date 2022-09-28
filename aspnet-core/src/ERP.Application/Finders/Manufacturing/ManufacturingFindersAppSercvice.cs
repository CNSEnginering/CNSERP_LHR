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
   public class ManufacturingFindersAppSercvice : ERPAppServiceBase
    {
        private readonly IRepository<MFTOOLTY> _mfToolRepository;

        public ManufacturingFindersAppSercvice(IRepository<MFTOOLTY> mfToolRepository)
        {
            _mfToolRepository = mfToolRepository;
        }
        
       
        public async Task<LookupDto<ToolTypeFindersDto>> GetAllToolTypelookupTable(GetAllForLookupTableInput input)
        {
            var query = _mfToolRepository.GetAll().Where(o => o.TenantId == AbpSession.TenantId);
           
            var lookupTableDto = from o in query
                                 select new ToolTypeFindersDto
                                 {
                                     Id = o.Id.ToString(),
                                     TypeId=o.TOOLTYID,
                                     DisplayName = o.TOOLTYDESC
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
