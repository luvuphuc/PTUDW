using ProjectASP.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectASP.Areas.Admin.Code
{
    public class Role: AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filtercontext)
        {
            var user = SessionHelper.GetSession();
            if (user == null)
            {
                // Điều hướng về trang đăng nhập
                filtercontext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        controller = "Login",
                        action = "Index",
                        area = "Admin"
                    }));

                return;
            }
            return;
        }
    }
}