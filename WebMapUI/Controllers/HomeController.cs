using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using WebMapUI.Models;

namespace WebMapUI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult GetDataSet()
        {

            return View();
        }
       
        [HttpPost]
        public ActionResult MapView(HttpPostedFileBase filePath)
        {
            List<Florist> florists = new List<Florist>();
            List<Order> orders = new List<Order>();
            if (filePath.ContentLength > 0)
            {
                var fileName = Path.GetFullPath(filePath.FileName);
                FileStream streamTemp = filePath.InputStream as FileStream; //new FileStream(filena, FileMode.Open, FileAccess.Read);
                using (var package = new ExcelPackage(filePath.InputStream))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet[2];
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var florist = new Florist();
                        florist.Name = workSheet.Cells[rowIterator, 1].Value != null ? workSheet.Cells[rowIterator, 1].Value.ToString() : string.Empty;
                        florist.Latitude = workSheet.Cells[rowIterator, 2].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 2].Value.ToString().Replace(".", ",")) : 0;
                        florist.Longitude = workSheet.Cells[rowIterator, 3].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 3].Value.ToString().Replace(".", ",")) : 0;
                        florists.Add(florist);
                        // ListExcel.Items.Add($"{urun.Kod1}<---> {urun.Kod2}");
                    }
                    workSheet = currentSheet[1];
                    noOfCol = workSheet.Dimension.End.Column;
                    noOfRow = workSheet.Dimension.End.Row;
                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        var order = new Order();
                        order.Id = workSheet.Cells[rowIterator, 1].Value != null ? Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value.ToString()) : 0;
                        order.Latitude = workSheet.Cells[rowIterator, 2].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 2].Value.ToString().Replace(".", ",")) : 0;
                        order.Longitude = workSheet.Cells[rowIterator, 3].Value != null ? Convert.ToDouble(workSheet.Cells[rowIterator, 3].Value.ToString().Replace(".", ",")) : 0;
                        order.Renk = workSheet.Cells[rowIterator, 4].Value != null ? workSheet.Cells[rowIterator, 4].Value.ToString() : "";
                        orders.Add(order);
                        // ListExcel.Items.Add($"{urun.Kod1}<---> {urun.Kod2}");
                    }
                }
            }
            return View(new FloristOrderVM() { Florists = florists, Orders = orders });
        }


    }
}