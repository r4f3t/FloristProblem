using System.Collections.Generic;

namespace FlorisProblem
{

    public class Florist
    {
        public Florist()
        {
            OrderCount = 0;
        }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public int OrderCount { get; set; }
        //public List<Order> Orders { get; set; }
    }


}
