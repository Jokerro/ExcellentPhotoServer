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
        [AllowAnonymous]
        public ActionResult ProcessImage()
        {
            return View();
        }
        public class ImageAnalyzer
        {
            private string fileName;
            private double colorMark;
            private double overalMark;
            private const string PROCESSING_MODULE = "ExcellentPhoto\\ExcellentPhoto\\Release\\ExcellentPhoto.exe";
            private const string COLOR_ANALYZE = " color ";
            private const string FACE_ANALYZE = " face ";
            private const string OVERAL_ANALYZE = " all ";
            private string processingRequests;
            
            public void AddParameter(string analyzeParameter)
            {
                processingRequests += analyzeParameter;
            }
            public void OverallAnalyze(string imagePath)
            {
                ClearParametrs();
                AddParameter(OVERAL_ANALYZE);
                AddParameter(imagePath);
                StartAnalyze();
            }
            private void ClearParametrs()
            {
                processingRequests = "";
            }
            private void StartAnalyze()
            {
                try
                {
                    System.Diagnostics.Process analyze = System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + PROCESSING_MODULE, processingRequests);
                    ClearParametrs();
                    analyze.WaitForExit();
                    analyze.Close();
                }
                catch (Exception e)
                {
                  
                }
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ImageProcessor(IEnumerable<HttpPostedFileBase> fileUpload)
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
                    ImageAnalyzer imageAnalyzer = new ImageAnalyzer();
                    imageAnalyzer.OverallAnalyze(path + filename);
                }
            }

            return RedirectToAction("Index");
        }

    }
}