using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMapUI.Models
{
    public class FloristOrderVM
    {
        public List<Florist> Florists { get; set; }
        public List<Order> Orders { get; set; }
    }
}