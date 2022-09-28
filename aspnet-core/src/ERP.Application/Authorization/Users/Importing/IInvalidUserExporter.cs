using System.Collections.Generic;
using ERP.Authorization.Users.Importing.Dto;
using ERP.Dto;

namespace ERP.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
