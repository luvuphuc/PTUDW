﻿using MyClass.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyClass.DAO
{
    public class SuppliersDAO
    {
        private MyDBContext db = new MyDBContext();
        public List<Suppliers> getList()
        {
            return db.Suppliers.ToList();
        }
        // GET: Admin/Supplier

        //INDEX dua vao status = 1,2, còn status = 0 == thung rac
        public List<Suppliers> getList(string status = "All")
        {
            List<Suppliers> list = null;
            switch (status)
            {
                case "Index":
                    {
                        list = db.Suppliers.Where(m => m.Status != 0).ToList();
                        break;
                    }
                case "Trash":
                    {
                        list = db.Suppliers.Where(m => m.Status == 0).ToList();
                        break;
                    }
                default:
                    {
                        return db.Suppliers.ToList();
                    }
            }
            return list;
        }
        // Details
        public Suppliers getRow(int? id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                return db.Suppliers.Find(id);
            }
        }
        public Suppliers getRow(string slug)
        {
            return db.Suppliers
                .Where(m => m.Slug == slug && m.Status == 1)
                .FirstOrDefault();
        }

        // Create 
        public int Insert(Suppliers row)
        {
            db.Suppliers.Add(row);
            return db.SaveChanges();

        }
        //Update
        public int Update(Suppliers row)
        {
            db.Entry(row).State = EntityState.Modified;
            return db.SaveChanges();
        }

        //Delete
        public int Delete(Suppliers row)
        {
            db.Suppliers.Remove(row);
            return db.SaveChanges();
        }

    }
}

