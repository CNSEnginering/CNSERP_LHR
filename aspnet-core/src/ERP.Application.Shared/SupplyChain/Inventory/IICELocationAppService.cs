using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.SupplyChain.Inventory.Dtos;
using ERP.Dto;


namespace ERP.SupplyChain.Inventory
{
    public interface IICELocationAppService : IApplicationService 
    {
        Task<PagedResultDto<GetICELocationForViewDto>> GetAll(GetAllICELocationInput input);

        Task<GetICELocationForViewDto> GetICELocationForView(int id);

		Task<GetICELocationForEditOutput> GetICELocationForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditICELocationDto input);

		Task Delete(int Id);

		
    }
}