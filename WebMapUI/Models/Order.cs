using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMapUI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Renk { get; set; }
    }
}