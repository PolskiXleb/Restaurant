using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class ClientController : Controller
    {
        RestaurantEntities db = new RestaurantEntities();

        // GET: Client
        public ActionResult Menu(int? categoryId)
        {
            ClientMenu cm = new ClientMenu();
            cm.Categories = db.DishCategories;
            IEnumerable<Dish> dishes = db.Dishes;
            if (categoryId != null)
                cm.Dishes = dishes.Where(s => s.CategoryId == categoryId);
            else
                cm.Dishes = dishes;

            ViewBag.SelectedCategoryId = categoryId ?? 0;

            return View(cm);
        }

        public ActionResult PickDate()
        {
            ViewBag.Today = DateTime.UtcNow;

            return View();
        }

        [HttpGet]
        public ActionResult Reserve(DateTime date)
        {
            IEnumerable<Table> tables = db.Tables;

            DateTime newDate = new DateTime(date.Year, date.Month, date.Day);
            if (date != null)
            {
                foreach (Table t in tables)
                {

                    if (db.Shedules.Any(s => s.TableId == t.Id && s.Date == newDate && s.Accepted == false))
                    {
                        t.IsEmpty = false;
                    }
                    else
                    {
                        t.IsEmpty = true;
                    }

                }
            }

            if (date != null)
            {
                ViewBag.Date = date.ToUniversalTime();
                return View(db.Tables);
            }
            else
            {
                return RedirectToAction("PickDate");
            }
        }

        [HttpPost]
        public ActionResult Reserve(int table, DateTime date, string clientName)
        {
            Shedule shedule = new Shedule();

            shedule.ClientName = clientName;
            shedule.TableId = table;
            shedule.Date = date.Date;
            shedule.Accepted = false;

            db.Shedules.Add(shedule);
            db.SaveChanges();

            ViewBag.Date = date.ToUniversalTime();

            return View(db.Tables);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}