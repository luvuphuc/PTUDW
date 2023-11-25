
using MyClass.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
namespace ProjectASP.Controllers
{
    public class ModuleController : Controller
    {
        ///////////////////////////////////////////////////////////////////////////
        MenusDAO menusDAO = new MenusDAO();

        ///////////////////////////////////////////////////////////////////////////
        // GET: Mainmenu
        public ActionResult MainMenu()
        {
            List<Menus> list = menusDAO.getListByParentId(0, "MainMenu");
            return View("MainMenu", list);
        }
    }
}