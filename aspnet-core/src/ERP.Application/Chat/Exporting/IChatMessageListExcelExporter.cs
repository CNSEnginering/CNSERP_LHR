using System.Collections.Generic;
using ERP.Chat.Dto;
using ERP.Dto;

namespace ERP.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(List<ChatMessageExportDto> messages);
    }
}
