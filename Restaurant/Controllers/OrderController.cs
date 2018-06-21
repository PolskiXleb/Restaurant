using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class OrderController : Controller
    {
        RestaurantEntities db = new RestaurantEntities();
        // GET: Order
        public ActionResult ViewCreate()
        {
            return RedirectToAction("Create");
        }

        [HttpGet]
        public ActionResult Create()
        {
            IEnumerable<Table> tables = db.Tables;

            foreach (Table t in tables)
            {
                try
                {
                    if (db.Shedules.Any(s => s.TableId == t.Id && s.Date == DateTime.Today && s.Accepted == false))
                    {
                        t.IsEmpty = false;
                    }
                }
                catch { }
                
            }

            return View(tables);
        }

        [HttpPost]
        public ActionResult Create(string name, int table)
        {
            Order order = new Order();
            order.TableId = table;
            order.Time = DateTime.Now.TimeOfDay;
            order.Summary = 0;
            order.ClientName = name;
            order.Ongoing = true;
            db.Orders.Add(order);
            db.Tables.First(s => s.Id == table).IsEmpty = false;

            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Content(int orderId)
        {
          

            return View(db.Orders.First(s => s.Id == orderId));
        }

        public ActionResult List()
        {            
            ReserveAndOrder rao = new ReserveAndOrder();
            IEnumerable<Order> orders = db.Orders.Where(s => s.Ongoing == true);
            IEnumerable<Shedule> reserve = db.Shedules.Where(s => s.Accepted == false && s.Date == DateTime.Today);

            rao.Orders = orders;
            rao.Shedules = reserve;

            return View(rao);
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
            shedule.Date = date;
            shedule.Accepted = false;

            db.Shedules.Add(shedule);
            db.SaveChanges();

            ViewBag.Date = date.ToUniversalTime();

            return View(db.Tables);
        }

        public ActionResult AcceptReservation(int sheduleId)
        {
            Shedule shedule = db.Shedules.First(s => s.Id == sheduleId);
            Order order = new Order();
            Table table = db.Tables.First(s => s.Id == shedule.TableId);

            shedule.Accepted = true;
            table.IsEmpty = false;

            order.ClientName = shedule.ClientName;
            order.Table = shedule.Table;
            order.Time = DateTime.Now.TimeOfDay;
            order.Summary = 0;
            order.Ongoing = true;


            db.Orders.Add(order);
            db.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Menu(int orderId)
        {
            MenuOrder mo = new MenuOrder();
            ViewBag.OrderId = orderId;



            List<SortedMenu> sortedMenus = new List<SortedMenu>();

            foreach (DishCategory category in db.DishCategories)
            {
                SortedMenu sm = new SortedMenu();
                sm.Category = category;
                sm.Dishes = db.Dishes.Where(s => s.CategoryId == category.Id).ToList();

                sortedMenus.Add(sm);

            }


           mo.SortedMenues = sortedMenus;

            try
            {
                mo.OrderContents = db.OrderContents.Where(s => s.OrderId == orderId);
            }
            catch { }

            mo.Order = db.Orders.First(s => s.Id == orderId);

            try
            {
                mo.Order.Summary = mo.OrderContents.Select(s => s.Dish.Cost).Sum();
            }
            catch { }

            return View(mo);
        }
        
        public JsonResult GetOrderInfo(int orderId, int dishId)
        {
            OrderContent oc = new OrderContent();

            oc.DishId = dishId;
            oc.OrderId = orderId;

            db.OrderContents.Add(oc);
            db.SaveChanges();

            IEnumerable<Dish> orderContent = db.OrderContents.Where(s => s.OrderId == orderId).Select(d => d.Dish);
            Order order = db.Orders.First(s => s.Id == orderId);
            try
            {
                order.Summary = orderContent.Select(s => s.Cost).Sum();
                db.SaveChanges();
            }
            catch { }
            var orderInfo = new
            {
                DishName = db.Dishes.First(s => s.Id == dishId).Name,
                DishCost = db.Dishes.First(s => s.Id == dishId).Cost,
                Summary = order.Summary
            };

            return Json(orderInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Menu(int orderId, int dishId)
        {
            MenuOrder mo = new MenuOrder();
            OrderContent oc = new OrderContent();

            List<SortedMenu> sortedMenus = new List<SortedMenu>();

            foreach (DishCategory category in db.DishCategories)
            {
                SortedMenu sm = new SortedMenu();
                sm.Category = category;
                sm.Dishes = db.Dishes.Where(s => s.CategoryId == category.Id).ToList();

                sortedMenus.Add(sm);

            }
            mo.SortedMenues = sortedMenus;

            ViewBag.OrderId = orderId;
            oc.DishId = dishId;
            oc.OrderId = orderId;

            mo.OrderContents = db.OrderContents.Where(s => s.OrderId == orderId);
            mo.Order = db.Orders.First(s => s.Id == orderId);
            mo.Order.Summary = mo.OrderContents.Select(s => s.Dish.Cost).Sum();

            db.OrderContents.Add(oc);
            db.SaveChanges();

            return View(mo);
        }

        public ActionResult Close(int orderId)
        {
            Order order = db.Orders.First(s => s.Id == orderId);

            return View(order);
        }        


        public ActionResult Closing(int orderId)
        {
            Order order = db.Orders.First(s => s.Id == orderId);
            order.Ongoing = false;
            order.Table.IsEmpty = true;

            db.SaveChanges();

            return View(order);
        }
    }
}