using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo_Authentication
{
    public class LoginAuthorizeAttribute : AuthorizeAttribute
    {
        /*public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //判断是否跳过授权过滤器
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            //TODO：授权校验

        }*/

    }
    [AttributeUsage(AttributeTargets.Property)]
    public class TestAttribute : Attribute
    {
        private bool canOperate;

        public bool CanOperate { get { return canOperate; } }
        public TestAttribute(bool flag)
        {
            canOperate = flag;
        }
    }
}
