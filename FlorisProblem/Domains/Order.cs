using FlorisProblem.Domains;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace FlorisProblem
{

    public class Order
    {
        public Order()
        {
            CloserFlorists = new List<FloristDistance>();
        }
        public int Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Florist Florist { get; set; }
        public List<FloristDistance> CloserFlorists { get; set; }
        public List<FloristDistance> GetCloserFlorists()
        {
            return CloserFlorists.OrderBy(x=>x.Distance).ToList();
        }
        public double GetCloserFloristDistance()
        {
            return CloserFlorists.OrderBy(x => x.Distance).Take(1).Single().Distance;
        }


    }


}
