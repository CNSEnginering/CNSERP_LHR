using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using ERP.Dto;

namespace ERP.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
