using Abp.AspNetCore.Mvc.Authorization;
using ERP.Authorization;
using ERP.Storage;
using Abp.BackgroundJobs;

namespace ERP.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
            // to do
        }

        
    }
}