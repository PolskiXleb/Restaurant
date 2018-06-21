using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class AdminController : Controller
    {
        private RestaurantEntities db = new RestaurantEntities();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Shedules()
        {
            var shedules = db.Shedules.Include(s => s.Table);
            return View(shedules.ToList());
        }

        public ActionResult CreateTable()
        {
            
            db.Tables.Add(new Table { IsEmpty = true });
            db.SaveChanges();

            return RedirectToAction("Tables");

        }
                

        public ActionResult Tables()
        {
            return View(db.Tables);
        }

        public ActionResult Orders()
        {
            var orders = db.Orders.Include(o => o.Table);
            return View(orders.ToList());
        }

        public ActionResult DetailsOrder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Dishes()
        {
            
            var dishes = db.Dishes.Include(d => d.DishCategory);
            return View(dishes.ToList());
        }


        public ActionResult CreateDish()
        {
            ViewBag.CategoryId = new SelectList(db.DishCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDish([Bind(Include = "Id,Name,Cost,CategoryId,Descriprion,ImagePath")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Dishes.Add(dish);
                db.SaveChanges();
                return RedirectToAction("Dishes");
            }

            ViewBag.CategoryId = new SelectList(db.DishCategories, "Id", "Name", dish.CategoryId);
            return View(dish);
        }

        public ActionResult EditDish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dishes.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.DishCategories, "Id", "Name", dish.CategoryId);
            return View(dish);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDish([Bind(Include = "Id,Name,Cost,CategoryId,Descriprion,ImagePath")] Dish dish)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dish).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Dishes");
            }
            ViewBag.CategoryId = new SelectList(db.DishCategories, "Id", "Name", dish.CategoryId);
            return View(dish);
        }

        public ActionResult DeleteDish(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dish dish = db.Dishes.Find(id);
            if (dish == null)
            {
                return HttpNotFound();
            }

            int count = dish.OrderContents.Count;

            if ( count > 0)
            {
                ViewBag.Error = "Блюдо используется в заказе. Удаление невозможно.";
            }

            return View(dish);
        }

        [HttpPost, ActionName("DeleteDish")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDishConfirmed(int id)
        {
            Dish dish = db.Dishes.Find(id);
            db.Dishes.Remove(dish);
            db.SaveChanges();
            return RedirectToAction("Dishes");
        }

        public ActionResult Categories()
        {
            return View(db.DishCategories.ToList());
        }


        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory([Bind(Include = "Id,Name")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                db.DishCategories.Add(dishCategory);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }

            return View(dishCategory);
        }

        public ActionResult EditCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = db.DishCategories.Find(id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }
            return View(dishCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory([Bind(Include = "Id,Name")] DishCategory dishCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dishCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(dishCategory);
        }

        public ActionResult DeleteCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DishCategory dishCategory = db.DishCategories.Find(id);
            if (dishCategory == null)
            {
                return HttpNotFound();
            }

            int count = dishCategory.Dishes.Count;

            if (count > 0)
            {
                ViewBag.Error = "Категория используется для блюда. Удаление невозможно.";
            }

            return View(dishCategory);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DishCategory dishCategory = db.DishCategories.Find(id);
            db.DishCategories.Remove(dishCategory);
            db.SaveChanges();
            return RedirectToAction("Categories");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}