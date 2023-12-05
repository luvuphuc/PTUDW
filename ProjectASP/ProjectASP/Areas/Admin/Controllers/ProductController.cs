﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyClass.DAO;
using MyClass.Model;
using ProjectASP.Areas.Admin.Code;
using ProjectASP.Library;
using UDW.Library;

namespace ProjectASP.Areas.Admin.Controllers
{
    [Role]
    public class ProductController : Controller
    {
        ProductsDAO productsDAO = new ProductsDAO();
        CategoriesDAO categoriesDAO = new CategoriesDAO();
        SuppliersDAO suppliersDAO = new SuppliersDAO();
        // GET: Admin/Product
        public ActionResult Index()
        {
            var products = productsDAO.getList("Index");
            foreach (var i in products)
            {
                Suppliers supplier = suppliersDAO.getRow(i.SupplierId);
                if (supplier != null)
                {
                    ViewBag.Name = supplier.Name;
                }
            }
            return View(products);
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            var ls = suppliersDAO.getList();
            {
                foreach (var i in ls)
                {
                    if (i.Id == products.Supplier)
                    {
                        ViewBag.Name = i.Name;
                    }
                }
            }
            return View(products);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");   //categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name"); //supplier
            return View();
        }

        // POST: Admin/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong cho mot so truong
                //---Create At
                products.CreateAt = DateTime.Now;
                //---Create By
                products.CreateBy = Convert.ToInt32(Session["UserID"]);

                //slug
                products.Slug = XString.Str_Slug(products.Name);
                //update at
                products.UpdateAt = DateTime.Now;
                //update by
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly hinh anh
                var img = Request.Files["img"];//lay thong tin file
                if (img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                //them moi mau tin vao db
                productsDAO.Insert(products);

                //thong bao trang thai thanh cong
                TempData["message"] = new XMessage("success", "Thêm mới sản phẩm thành công");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");   //categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name"); //supplier
            return View(products);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Không tìm thấy sản phẩm");
                return RedirectToAction("Index");
            }
            ViewBag.ListCatID = new SelectList(categoriesDAO.getList("Index"), "Id", "Name");   //categories
            ViewBag.ListSupID = new SelectList(suppliersDAO.getList("Index"), "Id", "Name"); //supplier
            return View(products);
        }

        // POST: Admin/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Products products)
        {
            if (ModelState.IsValid)
            {
                //xu ly tu dong mot so truong cu the
                //slug
                products.Slug = XString.Str_Slug(products.Name);
                //update at
                products.UpdateAt = DateTime.Now;
                //update by
                products.UpdateBy = Convert.ToInt32(Session["UserID"]);
                //xu ly hinh anh
                // Kiểm tra và xóa file hình ảnh cũ
                string imageName = products.Image;

                // Kiểm tra và xóa file hình ảnh
                if (!string.IsNullOrEmpty(imageName))
                {
                    string slug = XString.Str_Slug(products.Name);
                    string fileExtension = Path.GetExtension(imageName); // Lấy phần mở rộng của tệp tin
                    string imagePath = Path.Combine(Server.MapPath("~/Public/img/supplier/"), slug + fileExtension);

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
                products.Image = null;
                // lay thong tin file
                //xu ly cho phan upload hình ảnh
                var img = Request.Files["img"];//lay thong tin file
                if (img != null && img.ContentLength != 0)
                {
                    string[] FileExtentions = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
                    //kiem tra tap tin co hay khong
                    if (FileExtentions.Contains(img.FileName.Substring(img.FileName.LastIndexOf("."))))//lay phan mo rong cua tap tin
                    {
                        string slug = products.Slug;
                        //ten file = Slug + phan mo rong cua tap tin
                        string imgName = slug + img.FileName.Substring(img.FileName.LastIndexOf("."));
                        products.Image = imgName;
                        //upload hinh
                        string PathDir = "~/Public/img/product/";
                        string PathFile = Path.Combine(Server.MapPath(PathDir), imgName);
                        img.SaveAs(PathFile);
                    }
                }//ket thuc phan upload hinh anh
                 //hien thi thong bao thanh cong
                TempData["message"] = new XMessage("success", "Cập nhật sản phẩm thành công");
                //cap nhat mau tin
                productsDAO.Update(products);
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // GET: Admin/Product/Delete/5
        // GET: Admin/Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Truy van mau tin theo Id
            Products products = productsDAO.getRow(id);

            if (productsDAO.Delete(products) == 1)
            {
                //duong dan den anh can xoa
                string PathDir = "~/Public/img/product/";
                //cap nhat thi phai xoa file cu
                if (products.Image != null)
                {
                    string DelPath = Path.Combine(Server.MapPath(PathDir), products.Image);
                    System.IO.File.Delete(DelPath);
                }
            }
            //Thong bao thanh cong
            TempData["message"] = new XMessage("success", "Xóa danh mục thành công");
            //O lai trang thung rac
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
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Cập nhật trạng thái thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            products.Status = (products.Status == 1) ? 2 : 1;
            //cap nhat update at
            products.UpdateAt = DateTime.Now;
            //cap nhat update by
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            productsDAO.Update(products);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Cập nhật trạng thái thành công");
            //tro ve trang Index
            return RedirectToAction("Index");
        }

        // POST: Admin/Product/DelTrash
        public ActionResult DelTrash(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Xóa mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai
            products.Status = 0;
            //cap nhat update at
            products.UpdateAt = DateTime.Now;
            //cap nhat update by
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            productsDAO.Update(products);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Xóa mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Index");

        }
        public ActionResult Trash()
        {
            return View(productsDAO.getList("Trash"));
        }

        // POST: Admin/Supplier/Undo
        public ActionResult Undo(int? id)
        {
            if (id == null)
            {
                //hien thi thong bao
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            Products products = productsDAO.getRow(id);
            if (products == null)
            {
                TempData["message"] = new XMessage("danger", "Phục hồi mẩu tin thất bại");
                return RedirectToAction("Index");
            }
            //cap nhat trang thai status =2
            products.Status = 2;
            //cap nhat update at
            products.UpdateAt = DateTime.Now;
            //cap nhat update by
            products.UpdateBy = Convert.ToInt32(Session["UserID"]);
            //update db
            productsDAO.Update(products);
            //hien thi thong bao
            TempData["message"] = new XMessage("success", "Phục hồi mẩu tin thành công");
            //tro ve trang Index
            return RedirectToAction("Trash");
        }
    }
}