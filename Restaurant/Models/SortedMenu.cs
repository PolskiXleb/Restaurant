using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Restaurant.Models;

namespace Restaurant.Models
{
    public class SortedMenu
    {
        public List<Dish> Dishes { get; set; }

        public DishCategory Category { get; set; }
    }
}