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
        private static WebApplication.Models.ImageAnalyzerHandler imageAnalyzer = new WebApplication.Models.ImageAnalyzerHandler();
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
        [AllowAnonymous]
        public ActionResult ProcessImage()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult AnalyzingResult()
        {
            ExcellentPhotoLibrary.Image image = imageAnalyzer.GetImage();
            ViewBag.FaceAnalyzeRating = image.GetFaceRating();
            ViewBag.ColorAnalyzeRating = image.GetColorRating();
            ViewBag.OverallAnalyzeRating = image.GetOverallRating();
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult ImageProcessor(IEnumerable<HttpPostedFileBase> fileUpload, String analysisType)
        {
            foreach (var file in fileUpload)
            {
                if (file == null) continue;
                string path = AppDomain.CurrentDomain.BaseDirectory + "UploadedFiles\\";
                string filename = System.IO.Path.GetFileName(file.FileName);
                if (filename != null)
                {
                    file.SaveAs(System.IO.Path.Combine(path, filename));
                    string savedFilePath = file.FileName;
                    imageAnalyzer.SetAnalyzedImage(path + filename);
                    if (analysisType.Equals("colorAnalysis"))
                    {
                        imageAnalyzer.ColorAnalysis();
                    }
                    else if (analysisType.Equals("faceAnalysis"))
                    {
                        imageAnalyzer.FaceAnalysis();
                    }
                    else
                    {
                        imageAnalyzer.OverallAnalysis();
                    }
                }
            }
            return RedirectToAction("AnalyzingResult");
        }

    }
}