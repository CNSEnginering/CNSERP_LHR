using Abp.Notifications;
using ERP.Dto;

namespace ERP.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        public UserNotificationState? State { get; set; }
    }
}