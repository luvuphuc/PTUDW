using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectASP.App_Start
{
    public static class SessionConfig
    {
        //1. Lưu session cho User
        public static void SetUser(Users user)
        {
            //lưu vào session
            HttpContext.Current.Session["user"] = user;
        }

        //2. Lấy session cho User
        public static Users GetUser()
        {
            //Lấy vào session
            return (Users)HttpContext.Current.Session["user"];
        }
    }
}