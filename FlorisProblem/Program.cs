using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlorisProblem.Domains;
using OfficeOpenXml;

namespace FlorisProblem
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Data Path First=");
            //C:\Users\Rafet\Downloads\siparis ve bayi koordinatları(1).xlsx
            string dataPath = Console.ReadLine(); ;
            dataPath = @"C:\Users\Rafet\Downloads\data.xlsx";
            FileStream streamTemp = File.Open(dataPath, FileMode.Open, FileAccess.Read);
            using (var package = new ExcelPackage(streamTemp))
            {
                List<Florist> florists = new List<Florist>();
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet[2];
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    var florist = new Florist();
                    florist.Name = workSheet.Cells[rowIterator, 1].Value != null ? workSheet.Cells[rowIterator, 1].Value.ToString() : string.Empty;
                    florist.Latitude = workSheet.Cells[rowIterator, 2].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 2].Value.ToString().Replace(".",",")) : 0;
                    florist.Longitude = workSheet.Cells[rowIterator, 3].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 3].Value.ToString().Replace(".",",")) : 0;
                    Console.WriteLine(florist.Name+" İçin Aralık Giriniz 0-99999:");
                    string entry = Console.ReadLine();
                    florist.MinVal =Convert.ToInt32(entry.Split('-')[0]);
                    florist.MaxVal = Convert.ToInt32(entry.Split('-')[1]);
                    florists.Add(florist);
                    // ListExcel.Items.Add($"{urun.Kod1}<---> {urun.Kod2}");
                }
                List<Order> orders = new List<Order>();
                workSheet = currentSheet[1];
                noOfCol = workSheet.Dimension.End.Column;
                noOfRow = workSheet.Dimension.End.Row;
                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                {
                    var order = new Order();
                    order.Id = workSheet.Cells[rowIterator, 1].Value != null ? Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value.ToString()) : 0;
                    order.Latitude = workSheet.Cells[rowIterator, 2].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 2].Value.ToString().Replace(".", ",")) : 0;
                    order.Longitude = workSheet.Cells[rowIterator, 3].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 3].Value.ToString().Replace(".", ",")) : 0;
                    FillClosest(order,florists);
                    orders.Add(order);
                    // ListExcel.Items.Add($"{urun.Kod1}<---> {urun.Kod2}");
                }
                int kirmizi = 0, yesil = 0, mavi = 0;
                foreach (var item in orders)
                {
                    Console.WriteLine("Sip No="+item.Id+" Florist="+item.Florist.Name+" Distance="+item.GetCloserFloristDistance());
                    if (item.Florist.Name=="Kırmızı ")
                    {
                        kirmizi++;
                    }
                    else if (item.Florist.Name == "Yeşil")
                    {
                        yesil++;
                    }
                    else
                    {
                        mavi++;
                    }
                }
                Console.WriteLine("Kırmızı="+kirmizi+" Yeşil="+yesil+" Mavi="+mavi );
            }
            Console.ReadLine();
        }

        public static Florist FillClosest(Order order,List<Florist> florists)
        {
            foreach (var item in florists)
            {
                var dist = GetDistance(order.Latitude, order.Longitude, item.Latitude, item.Longitude);
                order.CloserFlorists.Add(new FloristDistance() {Florist=item,Distance=dist });
            }
            order.CloserFlorists=order.CloserFlorists.OrderBy(x=>x.Distance).ToList();
            foreach (var item in order.CloserFlorists)
            {
                if (item.Florist.OrderCount<item.Florist.MaxVal)
                {
                    order.Florist = item.Florist;
                    florists.Where(x => x == item.Florist).Single().OrderCount++;
                    break;
                }
            }
            //order.Florist = order.CloserFlorists.Take(1).Single().Florist;
            return new Florist();
        }
        
        public static double GetDistance(double y1,double x1,double y2,double x2)
        {
            return Math.Sqrt(Math.Pow(y2 - y1, 2)+ Math.Pow(x2-x1, 2));
        }
       
    }
}
