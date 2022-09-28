using Abp.AspNetCore.Mvc.Authorization;
using ERP.Storage;
using ERP.Web.Controllers.InventoryController;

namespace ERP.Web.Controllers
{
    [AbpMvcAuthorize]
    public class IcItemController : IcItemControllerBase
    {
        public IcItemController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}