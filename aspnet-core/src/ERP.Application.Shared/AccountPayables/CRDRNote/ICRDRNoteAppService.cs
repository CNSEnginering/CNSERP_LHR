using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ERP.AccountPayables.CRDRNote.Dtos;

namespace ERP.AccountPayables.CRDRNote
{
    public interface ICRDRNoteAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCRDRNoteForViewDto>> GetAll(GetAllCRDRNoteInput input);

        Task<GetCRDRNoteForViewDto> GetCRDRNoteForView(int id);

		Task<GetCRDRNoteForEditOutput> GetCRDRNoteForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCRDRNoteDto input);

        Task Delete(EntityDto input);

    }
}