using System;
using System.Collections.Generic;
using System.IO;
using CicekSepetiAlgoritma.Models;
using CicekSepetiAlgoritma.ENUMS;
using Newtonsoft.Json;
using System.Linq;
namespace CicekSepetiAlgoritma
{
    class Program
    {
        static void Main(string[] args)
        {
            string BayiJson = File.ReadAllText("/Users/hamzaolak/Documents/Ar-ge Projeleri/C#/CSharpBlankSolution/CicekSepetiAlgoritma/JSON/Bayi.json");
            List<Bayi> BayiList = JsonConvert.DeserializeObject<List<Bayi>>(BayiJson);


            string SiparisJson = File.ReadAllText("/Users/hamzaolak/Documents/Ar-ge Projeleri/C#/CSharpBlankSolution/CicekSepetiAlgoritma/JSON/Siparis.json");
            List<Siparis> SiparisList = JsonConvert.DeserializeObject<List<Siparis>>(SiparisJson);

            List<SiparisMinUzaklik> SiparisMinUzaklikList = new List<SiparisMinUzaklik>();
            int KC = 0, YC = 0, MC = 0;
            double TotalDistance = 0;

            foreach (Siparis siparis in SiparisList)
            {
                List<SiparisBayiUzaklik> SiparisBayiUzaklikList = new List<SiparisBayiUzaklik>();
                double minUzaklık = double.MaxValue;
                Bayi bayiG = null;

                foreach (Bayi bayi in BayiList)
                {
                    SiparisBayiUzaklik siparisBayiUzaklik = new SiparisBayiUzaklik();
                    siparisBayiUzaklik.Bayi = bayi;
                    siparisBayiUzaklik.SiparisKodu = siparis.SiparisKod;
                    siparisBayiUzaklik.Uzaklik = Math.Sqrt(Math.Pow(bayi.Latitude - siparis.Latitude, 2) + Math.Pow(bayi.Longitude - siparis.Longitude, 2));
                    SiparisBayiUzaklikList.Add(siparisBayiUzaklik);

                    if (minUzaklık > siparisBayiUzaklik.Uzaklik)
                    {
                        minUzaklık = siparisBayiUzaklik.Uzaklik;
                        bayiG = bayi;
;                    }
                }

                SiparisMinUzaklik siparisMinUzaklik = new SiparisMinUzaklik();
                siparisMinUzaklik.SiparisKod = siparis.SiparisKod;
                siparisMinUzaklik.Bayi = bayiG;
                siparisMinUzaklik.MinUzaklik = minUzaklık;
                siparisMinUzaklik.siparisBayiUzaklikList = SiparisBayiUzaklikList;
                SiparisMinUzaklikList.Add(siparisMinUzaklik);
                Console.WriteLine("SiparisKod: "+ siparisMinUzaklik.SiparisKod + " " + siparisMinUzaklik.Bayi.BayiKod + " " 
                    + siparisMinUzaklik.Bayi.BayiAdi + " Min Uzaklık:" + siparisMinUzaklik.MinUzaklik);

                if(siparisMinUzaklik.Bayi.BayiKod == (int)BayiMaxMin.KIRMIZI)
                {
                    KC++;
                }else if (siparisMinUzaklik.Bayi.BayiKod == (int)BayiMaxMin.YESIL)
                {
                    YC++; 
                }else if (siparisMinUzaklik.Bayi.BayiKod == (int)BayiMaxMin.MAVI)
                {
                    MC++;
                }
                TotalDistance += siparisMinUzaklik.MinUzaklik;
            }

            PrintCountersAndDistance(SiparisMinUzaklikList);

            //KC = 0;
            //YC = 0; 
            //MC = 0;
            TotalDistance = 0;

            foreach (Bayi bayi in BayiList)
            {
                if (isBayiUygun(bayi, KC, YC, MC))
                {
                    continue;
                }
                if ((bayi.MinSip > KC && bayi.BayiKod == (int)BayiMaxMin.KIRMIZI) ||
                    (bayi.MinSip > YC && bayi.BayiKod == (int)BayiMaxMin.YESIL) ||
                    (bayi.MinSip > MC && bayi.BayiKod == (int)BayiMaxMin.MAVI))
                {

                    SiparisMinUzaklikList = SiparisMinUzaklikList.OrderBy(x => x.getDistanceBetweenBayiAndMin(bayi.BayiKod)).ToList();
                    foreach (SiparisMinUzaklik siparisMinUzaklik in SiparisMinUzaklikList)
                    {
                        if (siparisMinUzaklik.Bayi.BayiKod == bayi.BayiKod)
                        {
                            continue;
                        }
                        if ((siparisMinUzaklik.Bayi.BayiKod == (int)BayiMaxMin.KIRMIZI
                            && siparisMinUzaklik.Bayi.MinSip < KC ) ||
                                (siparisMinUzaklik.Bayi.BayiKod == (int)BayiMaxMin.YESIL
                            && siparisMinUzaklik.Bayi.MinSip < YC ) ||
                                (siparisMinUzaklik.Bayi.BayiKod == (int)BayiMaxMin.MAVI
                            && siparisMinUzaklik.Bayi.MinSip < MC ))
                        {

                            switch (siparisMinUzaklik.Bayi.BayiKod)
                            {
                                case (int)BayiMaxMin.KIRMIZI:
                                    KC--;
                                    break;
                                case (int)BayiMaxMin.YESIL:
                                    YC--;
                                    break;
                                case (int)BayiMaxMin.MAVI:
                                    MC--;
                                    break;
                                default:
                                    break;
                            }

                            siparisMinUzaklik.Bayi = bayi;
                            siparisMinUzaklik.MinUzaklik = siparisMinUzaklik.siparisBayiUzaklikList.Where(x => x.Bayi.BayiKod == bayi.BayiKod).First().Uzaklik;

                            switch (bayi.BayiKod)
                            {
                                case (int)BayiMaxMin.KIRMIZI:
                                    KC++;
                                    break;
                                case (int)BayiMaxMin.YESIL:
                                    YC++;
                                    break;
                                case (int)BayiMaxMin.MAVI:
                                    MC++;
                                    break;
                                default:
                                    break;
                            }

                            if(isBayiUygun(bayi, KC, YC, MC))
                            {
                                break;
                            }
                        }


                    }
                }
            

                if((bayi.MaxSip < KC && bayi.BayiKod == (int)BayiMaxMin.KIRMIZI) ||
                    (bayi.MaxSip < YC && bayi.BayiKod == (int)BayiMaxMin.YESIL) ||
                    (bayi.MaxSip < MC && bayi.BayiKod == (int)BayiMaxMin.MAVI))
                {
                    List<SiparisBayiUzaklik> siparisBayiUzaklikList = new List<SiparisBayiUzaklik>();
                    foreach(SiparisMinUzaklik siparisMinUzaklik in SiparisMinUzaklikList)
                    {
                        if(siparisMinUzaklik.Bayi.BayiKod != bayi.BayiKod)
                        {
                            continue;
                        }

                        foreach(SiparisBayiUzaklik siparisBayiUzaklik in siparisMinUzaklik.siparisBayiUzaklikList)
                        {
                            if(siparisBayiUzaklik.Bayi.BayiKod == siparisMinUzaklik.Bayi.BayiKod)
                            {
                                continue;
                            }
                            SiparisBayiUzaklik diff = new SiparisBayiUzaklik();
                            diff.Bayi = siparisBayiUzaklik.Bayi;
                            diff.SiparisKodu = siparisBayiUzaklik.SiparisKodu;
                            diff.Uzaklik = Math.Abs(siparisMinUzaklik.MinUzaklik - siparisBayiUzaklik.Uzaklik);
                            siparisBayiUzaklikList.Add(diff);
                            
                        }
                    }
                    siparisBayiUzaklikList = siparisBayiUzaklikList.OrderBy(x => x.Uzaklik).ToList();

                    foreach (SiparisBayiUzaklik x in siparisBayiUzaklikList)
                    {
                        if((x.Bayi.BayiKod == (int)BayiMaxMin.KIRMIZI
                            && x.Bayi.MaxSip > KC) ||
                                (x.Bayi.BayiKod == (int)BayiMaxMin.YESIL
                            && x.Bayi.MaxSip > YC) ||
                                (x.Bayi.BayiKod == (int)BayiMaxMin.MAVI
                            && x.Bayi.MaxSip > MC))
                        {
                            if(SiparisMinUzaklikList.Where(y => y.SiparisKod == x.SiparisKodu).First().Bayi.BayiKod != bayi.BayiKod)
                            {
                                continue;
                            }

                            switch (bayi.BayiKod)
                            {
                                case (int)BayiMaxMin.KIRMIZI:
                                    KC--;
                                    break;
                                case (int)BayiMaxMin.YESIL:
                                    YC--;
                                    break;
                                case (int)BayiMaxMin.MAVI:
                                    MC--;
                                    break;
                                default:
                                    break;
                            }

                            SiparisMinUzaklikList = ChangeItem(x.SiparisKodu, x.Bayi, SiparisMinUzaklikList);

                            switch (x.Bayi.BayiKod)
                            {
                                case (int)BayiMaxMin.KIRMIZI:
                                    KC++;
                                    break;
                                case (int)BayiMaxMin.YESIL:
                                    YC++;
                                    break;
                                case (int)BayiMaxMin.MAVI:
                                    MC++;
                                    break;
                                default:
                                    break;
                            }

                            if (isBayiUygun(bayi, KC, YC, MC))
                            {
                                break;
                            }

                        }
                    }
                }
                PrintCountersAndDistance(SiparisMinUzaklikList);
            }
            PrintCountersAndDistance(SiparisMinUzaklikList);



            Console.ReadLine();
        }

        static List<SiparisMinUzaklik> ChangeItem(int siparisNo,Bayi bayi, List<SiparisMinUzaklik> SiparisMinUzaklikList)
        {
            foreach (SiparisMinUzaklik x in SiparisMinUzaklikList)
            {
                if(x.SiparisKod == siparisNo)
                {
                    x.Bayi = bayi;
                    x.MinUzaklik = x.siparisBayiUzaklikList.Where(y => y.Bayi.BayiKod == bayi.BayiKod).First().Uzaklik;
                    break;
                }
            }
            return SiparisMinUzaklikList;
        }

        static void PrintCountersAndDistance(List<SiparisMinUzaklik> SiparisMinUzaklikList)
        {
            int KC = 0, YC = 0, MC = 0;
            double TotalDistance = 0;
            foreach (SiparisMinUzaklik x in SiparisMinUzaklikList)
            {
                TotalDistance += x.MinUzaklik;
                switch (x.Bayi.BayiKod)
                {
                    case (int)BayiMaxMin.KIRMIZI:
                        KC++;
                        break;
                    case (int)BayiMaxMin.YESIL:
                        YC++;
                        break;
                    case (int)BayiMaxMin.MAVI:
                        MC++;
                        break;
                    default:
                        break;
                }
                Console.WriteLine(x.Bayi.BayiAdi);
            }

            Console.WriteLine("---------------");
            Console.WriteLine("Kırmızı: " + KC + " Yeşil: " + YC + " Mavi: " + MC);
            Console.WriteLine("Total Distance 1: " + TotalDistance);
            Console.WriteLine("---------------");
        }

        static bool isBayiUygun(Bayi bayi,int KC,int YC,int MC)
        {
            if (bayi.BayiKod == (int)BayiMaxMin.KIRMIZI)
            {
                if (bayi.MinSip <= KC && bayi.MaxSip >= KC)
                {
                    return true;
                }
            }
            else if (bayi.BayiKod == (int)BayiMaxMin.YESIL)
            {
                if (bayi.MinSip <= YC && bayi.MaxSip >= YC)
                {
                    return true;
                }
            }
            else if (bayi.BayiKod == (int)BayiMaxMin.MAVI)
            {
                if (bayi.MinSip <= MC && bayi.MaxSip >= MC)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
