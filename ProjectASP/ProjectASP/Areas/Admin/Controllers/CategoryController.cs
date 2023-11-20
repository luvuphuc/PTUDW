using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.Model;
using MyClass.DAO;
using UDW.Library;
using ProjectASP.Library;

namespace ProjectASP.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        LinksDAO linksDAO = new LinksDAO();
        // GET: Admin/Category

        //INDEX
        public ActionResult Index()
        {
            var categories = categoriesDAO.getList("Index");
            foreach (var i in categories)
            {
                Categories category = categoriesDAO.getRow(i.ParentId);
                if (category != null)
                {
                    ViewBag.Name = category.Name;
                }
            }
            return View(categoriesDAO.getList("Index"));
        }


        // GET: Admin/Category/Details
        public ActionResult Details(int? id)
        {
            List<Categories> ls = categoriesDAO.getList("Index");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy loại hàng");
            }
            if(categories.ParentId == 0)
            {
                ViewBag.Name = categories.Name;
            }
            else
            {
                foreach (var i in ls)
                {
                    if (i.Id == categories.ParentId)
                    {
                        ViewBag.Name = i.Name;
                    }
                }
            }
            
            return View(categories);
        }

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"),"Id","Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong sau:
                //---Create At
                categories.CreateAt = DateTime.Now;
                //---Create By
                categories.CreateBy = Convert.ToInt32(Session["UserID"]);

                //slug
                categories.Slug = XString.Str_Slug(categories.Name);

                //parent id
                if(categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }

                // xu ly order
                if(categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }

                //update at
                categories.UpdateAt = DateTime.Now;
                //update by
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);

                categoriesDAO.Insert(categories);
                //xu ly cho muc Topics
                if (categoriesDAO.Insert(categories) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableId = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success","Tạo mới loại sản phẩm thành công");

                return RedirectToAction("Index");
            }

            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);

        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            
            return View(categories);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Categories categories)
        {
            if (ModelState.IsValid)
            {
                //chinh sua 1 so thong tin
                //xu ly tu dong cho cac truong sau:
                //slug
                categories.Slug = XString.Str_Slug(categories.Name);

                //parent id
                if (categories.ParentId == null)
                {
                    categories.ParentId = 0;
                }

                // xu ly order
                if (categories.Order == null)
                {
                    categories.Order = 1;
                }
                else
                {
                    categories.Order += 1;
                }

                //update at
                categories.UpdateAt = DateTime.Now;
                //update by
                categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //cap nhat db
                TempData["message"] = new XMessage("success", "Cập nhật thông tin thành công");
                categoriesDAO.Update(categories);
                //xu ly cho muc Topics
                if (categoriesDAO.Insert(categories) == 1)//khi them du lieu thanh cong
                {
                    Links links = new Links();
                    links.Slug = categories.Slug;
                    links.TableId = categories.Id;
                    links.Type = "category";
                    linksDAO.Insert(links);
                }
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(categoriesDAO.getList("Index"), "Order", "Name");
            return View(categories);
        }

        // GET: Admin/Category/Delete/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Trash");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Trash");
            }
            return View(categories);
        }

        // POST: Admin/Category/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Categories categories = categoriesDAO.getRow(id);
            //tim thay mau tin => xoa
            categoriesDAO.Delete(categories);
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            return RedirectToAction("Trash");
        }

        // POST: Admin/Category/Status
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            categories.Status = (categories.Status == 1) ? 2 : 1;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        // POST: Admin/Category/DelTrash
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            categories.Status = 0;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");

        }
        public ActionResult Trash()
        {
            return View(categoriesDAO.getList("Trash"));
        }

        // POST: Admin/Category/Undo
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Categories categories = categoriesDAO.getRow(id);
            if (categories == null)
            {
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai status =2
            categories.Status = 2;
            //cap nhat update at
            categories.UpdateAt = DateTime.Now;
            //cap nhat update by
            categories.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            categoriesDAO.Update(categories);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Trash");
        }
    }
}
