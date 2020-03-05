using Microsoft.AspNetCore.Mvc;
using Senparc.Scf.AreaBase.Admin.Filters;
using Senparc.Scf.Core.Models.VD;
using Senparc.Scf.Core.WorkContext;
using Senparc.Scf.Mvc.UI;
using Senparc.Scf.XscfBase;
using System.Collections.Generic;

namespace Senparc.Scf.AreaBase.Admin//  Senparc.Areas.Admin
{

    public interface IAdminPageModelBase : IPageModelBase
    {
        AdminWorkContext AdminWorkContext { get; set; }

        List<IXscfRegister> XscfRegisterList { get; }

        IActionResult RenderError(string message);
    }

    //暂时取消权限验证
    //[ServiceFilter(typeof(AuthenticationAsyncPageFilterAttribute))]
    [AdminAuthorize("AdminOnly")]
    public class AdminPageModelBase : PageModelBase, IAdminPageModelBase
    {
        /// <summary>
        /// 存储相关用户信息
        /// </summary>
        public virtual AdminWorkContext AdminWorkContext { get; set; }

        /// <summary>
        /// 所有 XscfRegister 列表（包括还未注册的）
        /// </summary>
        public virtual List<IXscfRegister> XscfRegisterList => Senparc.Scf.XscfBase.Register.RegisterList;


        public virtual IActionResult RenderError(string message)
        {
            //保留原有的controller和action信息
            //ViewData["FakeControllerName"] = RouteData.Values["controller"] as string;
            //ViewData["FakeActionName"] = RouteData.Values["action"] as string;

            return Page();//TODO：设定一个特定的错误页面

            //return View("Error", new Error_ExceptionVD
            //{
            //    //HandleErrorInfo = new HandleErrorInfo(new Exception(message), Url.RequestContext.RouteData.GetRequiredString("controller"), Url.RequestContext.RouteData.GetRequiredString("action"))
            //});
        }
    }
}
