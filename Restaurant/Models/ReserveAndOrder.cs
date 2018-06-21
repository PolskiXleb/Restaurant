using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Restaurant.Models
{
    public class ReserveAndOrder
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Shedule> Shedules { get; set; }
    }
}