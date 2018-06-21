using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Restaurant.Models;

namespace Restaurant.Models
{
    public class MenuOrder
    {
        public IEnumerable<SortedMenu> SortedMenues { get; set; }

        public IEnumerable<OrderContent> OrderContents { get; set; }

        public Order Order { get; set; }

    }
}