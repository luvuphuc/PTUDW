using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class ProductsDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Products> getList()
        {
            return db.Products.ToList();
        }
        // GET: Admin/Supplier

        //INDEX dua vao status = 1,2, còn status = 0 == thung rac
        public List<Products> getList(string status = "All")
        {
            List<Products> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Products.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Products.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        return db.Products.ToList();
                    }
            }
            return list;
        }
        // Details
        public Products getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Products.Find(id);
            }
        }


        // Create 
        public int Insert(Products row)
        {
            db.Products.Add(row);
            return db.SaveChanges();

        }
        //Update
        public int Update(Products row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Delete
        public int Delete(Products row)
        {
            db.Products.Remove(row);
            return db.SaveChanges();
        }
    }
}
