using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ProcessImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> fileUpload)
        {
            foreach (var file in fileUpload)
            {
                if (file == null) continue;
                string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles/";
                string filename = System.IO.Path.GetFileName(file.FileName);
                if (filename != null)
                {
                    file.SaveAs(System.IO.Path.Combine(path, filename));
                }
            }

            return RedirectToAction("Index");
        }

    }
}