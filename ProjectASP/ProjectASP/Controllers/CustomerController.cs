using MyClass.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UDW.Library;

namespace ProjectASP.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection field)
        {
            UsersDAO usersDAO = new UsersDAO();

            String username = field["username"];
            String password = XString.ToMD5(field["password"]);
            //so sanh thong tin nguoi dung
            Users row_user = usersDAO.getRow(username, "customer");
            String strErr = "";
            if (row_user == null)
            {
                strErr = "Tên đăng nhập không tồn tại";
                ViewBag.Error = "<span class='text-danger'>" + strErr + "</div";
                return View("Login");
            }
            else
            {
                return RedirectToAction("Index", "Site");
            }
            
        }

        //////////////////////////////////////////////////////////////////////////
        // GET: Khachhang DangKy
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection field)
        {
            UsersDAO usersDAO=new UsersDAO();
            String fullname = field["fullname"];
            String email = field["email"];
            String phone = field["phone"];
            String username = field["username"];
            String password = field["password"];
            Users row_user = usersDAO.getRow(username);
            String strErr = "";
            if(row_user  == null)
            {
                //xu ly tu dong cho 1 so truong
                row_user.Status = 1;
                row_user.Role = "customer";
                row_user.CreateAt = DateTime.Now;
                // xu ly cac truong nhap vao
                row_user.Phone = phone;
                row_user.Email = email;
                row_user.Phone = phone;
                row_user.UserName = username;
                row_user.Password = password;
                usersDAO.Insert(row_user);
                return RedirectToAction("Login", "Customer");
            }
            else
            {
                strErr = "Tên đăng nhập đã tồn tại";
                ViewBag.Error = "<span class='text-danger'>" + strErr + "</div";
                return View("Register");
            }
        }
    }
}