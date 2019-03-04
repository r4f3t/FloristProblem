using System;
using System.Collections.Generic;
using System.Linq;

namespace CicekSepetiAlgoritma.Models
{
    public class SiparisMinUzaklik
    {
        public int SiparisKod { get; set; }
        public Bayi Bayi { get; set; }
        public double MinUzaklik { get; set; }
        public List<SiparisBayiUzaklik> siparisBayiUzaklikList { get; set; }

        public Double getDistanceBetweenBayiAndMin(int BayiKodu)
        {
            var bayiDistance = siparisBayiUzaklikList.Where(x => x.Bayi.BayiKod == BayiKodu).First();
            return Math.Abs(MinUzaklik - bayiDistance.Uzaklik);
        }
    }
}
