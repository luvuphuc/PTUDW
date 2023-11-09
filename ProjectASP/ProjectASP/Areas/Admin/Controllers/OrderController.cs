using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using ProjectASP.Library;
using UDW.Library;

namespace ProjectASP.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        OrdersDAO ordersDAO = new OrdersDAO();

        // GET: Admin/Order
        public ActionResult Index()
        {
            return View(ordersDAO.getList("Index"));
        }

        // GET: Admin/Order/Details/5
        public ActionResult Details(int? id)
        {
            List<Orders> ls = ordersDAO.getList("Index");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy đơn hàng");
                return RedirectToAction("Index");
            }
            Orders order = ordersDAO.getRow(id);
            if (order == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy đơn hàng");
            }
            return View(order);
        }

        // GET: Admin/Order/Create
        public ActionResult Create()
        {
            ViewBag.CatList = new SelectList(ordersDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(ordersDAO.getList("Index"), "Order", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orders orders)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho cac truong sau:
                //---Create At
                orders.CreateAt = DateTime.Now;
                //---Create By
                orders.CreateBy = Convert.ToInt32(Session["UserID"]);
                //update at
                orders.UpdateAt = DateTime.Now;
                //update by
                orders.UpdateBy = Convert.ToInt32(Session["UserID"]);

                ordersDAO.Insert(orders);
                //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Tạo mới đơn hàng thành công");

                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(ordersDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(ordersDAO.getList("Index"), "Order", "Name");
            return View(orders);

        }

        // GET: Admin/Order/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.CatList = new SelectList(ordersDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(ordersDAO.getList("Index"), "Order", "Name");
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Orders orders = ordersDAO.getRow(id);
            if (orders == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }

            return View(orders);
        }

        // POST: Admin/Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orders orders)
        {
            if (ModelState.IsValid)
            {
                //chinh sua 1 so thong tin
                //xu ly tu dong cho cac truong sau
                //update at
                orders.UpdateAt = DateTime.Now;
                //update by
                orders.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //cap nhat db
                TempData["message"] = new XMessage("success", "Cập nhật thông tin thành công");
                ordersDAO.Update(orders);
                return RedirectToAction("Index");
            }
            ViewBag.CatList = new SelectList(ordersDAO.getList("Index"), "Id", "Name");
            ViewBag.OrderList = new SelectList(ordersDAO.getList("Index"), "Order", "Name");
            return View(orders);
        }

        // GET: Admin/Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Trash");
            }
            Orders orders = ordersDAO.getRow(id);
            if (orders == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Trash");
            }
            return View(orders);
        }

        // POST: Admin/Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = ordersDAO.getRow(id);
            //tim thay mau tin => xoa
            ordersDAO.Delete(orders);
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            return RedirectToAction("Trash");
        }
        public ActionResult Status(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            Orders orders = ordersDAO.getRow(id);
            if (orders == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            orders.Status = (orders.Status == 1) ? 2 : 1;
            //cap nhat update at
            orders.UpdateAt = DateTime.Now;
            //cap nhat update by
            orders.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            ordersDAO.Update(orders);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Orders orders = ordersDAO.getRow(id);
            if (orders == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            orders.Status = 0;
            //cap nhat update at
            orders.UpdateAt = DateTime.Now;
            //cap nhat update by
            orders.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            ordersDAO.Update(orders);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");

        }
        public ActionResult Trash()
        {
            return View(ordersDAO.getList("Trash"));
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
            Orders orders = ordersDAO.getRow(id);
            if (orders == null)
            {
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai status =2
            orders.Status = 2;
            //cap nhat update at
            orders.UpdateAt = DateTime.Now;
            //cap nhat update by
            orders.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            ordersDAO.Update(orders);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Trash");
        }
    }
}
