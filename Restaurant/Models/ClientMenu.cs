using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class ClientMenu
    {
        public IEnumerable<DishCategory> Categories { get; set; }

        public IEnumerable<Dish> Dishes { get; set; }
    }
}