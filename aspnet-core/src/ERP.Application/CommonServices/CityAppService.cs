using Abp.Domain.Repositories;
using ERP.CommonServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace ERP.CommonServices
{
    public class CityAppService : ERPAppServiceBase, ICityAppService
    {
        private readonly IRepository<City> _cityRepository;

        public CityAppService(IRepository<City> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<PagedResultDto<GetCityForViewDto>> GetAll()
        {
            var allCities = _cityRepository.GetAll();

            var cities = from c in allCities
                         select new GetCityForViewDto()
                         {
                             City = new CityDto
                             {
                                 CityID = c.CityID,
                                 Name = c.Name,
                                 ProvinceID = c.ProvinceID,
                                 CountryID = c.CountryID,
                                 preFix = c.preFix,
                                 Id = c.Id
                             }
                         };

            var totalCount = await allCities.CountAsync();

            return new PagedResultDto<GetCityForViewDto>(
                totalCount,
                await cities.ToListAsync()
            );



        }

    }
}
