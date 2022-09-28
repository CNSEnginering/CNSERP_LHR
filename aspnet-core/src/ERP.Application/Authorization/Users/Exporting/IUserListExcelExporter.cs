using System.Collections.Generic;
using ERP.Authorization.Users.Dto;
using ERP.Dto;

namespace ERP.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}