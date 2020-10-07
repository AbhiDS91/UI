using DataTransfer.Prepros.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DataTransfer.Prepros.Controllers
{
    public class PropertiesController : Controller
    {
        // GET: Properties
        public ActionResult Index()
        {
            return View(this.GetExcelRecords());
        }

        [Route("Properties/{code?}/Code")]
        public ActionResult Code(string code)
        {
            if (String.IsNullOrEmpty(code) || code == "Code")
            {
                return RedirectToAction("Index");
            }
            
            return View(new PropertyImages()
            {
                PropertyCode = code,
                Images = this.GetExcelRecords().Where(x => x.PropertyCode == code && !string.IsNullOrEmpty(x.ImageID)).ToList()
            });
        }

        [Route("Properties/{code?}/Detail")]
        public ActionResult Detail(string code)
        {
            if (String.IsNullOrEmpty(code) || code == "Code")
            {
                return RedirectToAction("Index");
            }

            var PropertyImagesList = this.GetExcelRecords().Where(x => x.PropertyCode == code && !string.IsNullOrEmpty(x.ImageID)).ToList();


            return View(new PropertyImages()
            {
                PropertyCode = code,
                Images = PropertyImagesList.OrderByDescending(o => o.RankingScore).ToList()
            });
        }

        #region >> READ DATA FROM EXCEL

        private List<Properties> GetExcelRecords()
        {
            String filePath = Server.MapPath(Url.Content("~/Content/properties-data.xlsx"));
            List<Properties> _properties = new List<Properties>();
            using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read()) //Each row of the file
                    {
                        if (reader.Depth == 0) continue;
                        _properties.Add(new Properties
                        {
                            ImageID = reader.GetValue(0) == null ? "" : reader.GetValue(0).ToString(),
                            PropertyCode = reader.GetValue(1) == null ? "" : reader.GetValue(1).ToString(),
                            CategoryName = reader.GetValue(2) == null ? "" : reader.GetValue(2).ToString(),
                            RankingScore = reader.GetValue(3) == null ? 0 : Convert.ToDecimal(reader.GetValue(3))
                        });
                    }
                }
            }
            return _properties;
        }

        #endregion

    }
}
