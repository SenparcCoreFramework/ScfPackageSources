using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Senparc.Scf.Core;
using Senparc.Scf.Core.Models;
using Senparc.Scf.Core.WorkContext.Provider;
using Senparc.Scf.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Senparc.Scf.AreaBase.Admin.Filters
{
    public class AuthenticationResultFilterAttribute : IAsyncPageFilter, IFilterMetadata
    {
        private IServiceProvider _serviceProvider;
        public AuthenticationResultFilterAttribute(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            await ValidatePermissionAsync(_serviceProvider, context, next);
        }

        /// <summary>
        /// 验证权限得方法
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public virtual async Task ValidatePermissionAsync(IServiceProvider serviceProvider, PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            bool canAccessResource = false;
            bool isAjax = false;
            isAjax = IsAjax(context);
            CustomerResourceFilterAttribute attributeCodes = context.HandlerMethod.MethodInfo
                .GetCustomAttributes(typeof(CustomerResourceFilterAttribute), false)
                .OfType<CustomerResourceFilterAttribute>()
                .FirstOrDefault();
            IEnumerable<string> resourceCodes = attributeCodes?.ResourceCodes.ToList() ?? new List<string>() { "*" };//当前方法的资源Code
            if (resourceCodes.Any(_ => "*".Equals(_)))
            {
                await next();
            }
            else
            {
                string url = string.Join(string.Empty, context.RouteData.Values.Values.Reverse());
                if (!url.StartsWith("/"))
                {
                    url = string.Concat("/", url); // /Admin/AdminUserInfo/Index
                }
                canAccessResource = await serviceProvider.GetService<SysPermissionService>().HasPermissionAsync(resourceCodes, url, isAjax);// await Task.FromResult(true);//TODO...
                if (canAccessResource)
                {
                    await next();
                }
                else
                {
                    string path = context.HttpContext.Request.Path.Value;
                    IActionResult actionResult = null;
                    if (isAjax)
                    {
                        actionResult = new OkObjectResult(new AjaxReturnModel<string>(path) { Msg = "您没有权限访问", Success = false }) { StatusCode = (int)System.Net.HttpStatusCode.Forbidden };
                    }

                    context.Result = actionResult ?? new RedirectResult("/Admin/Forbidden?url=" + System.Web.HttpUtility.UrlEncode(path));
                }
            }
        }

        /// <summary>
        /// 是否是Ajax请求
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool IsAjax(PageHandlerExecutingContext context)
        {
            bool isAjax = false;
            if (context.HttpContext.Request.Headers.TryGetValue("x-requested-with", out Microsoft.Extensions.Primitives.StringValues strings))
            {
                if (strings.Contains("XMLHttpRequest"))
                {
                    isAjax = true;
                }
            }
            return isAjax;
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
