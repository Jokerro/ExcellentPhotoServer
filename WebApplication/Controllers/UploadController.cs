using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net.Http.Formatting;

namespace WebApplication.Controllers
{
    public class UploadController : ApiController
    {
        private static WebApplication.Models.ImageAnalyzerHandler imageAnalyzer = new WebApplication.Models.ImageAnalyzerHandler();
        public async Task<HttpResponseMessage> PostFormData()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);            
            
            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                
                foreach (MultipartFileData file in provider.FileData)
                {
                    int fileNameLength = file.Headers.ContentDisposition.FileName.Length - 2;
                    root += "//";
                    root += file.Headers.ContentDisposition.FileName.Substring(1, fileNameLength);
                    if (File.Exists(root))
                    {
                        File.Delete(root);
                    }
                    File.Move(file.LocalFileName, root);
                }
                imageAnalyzer.SetAnalyzedImage(root);
                imageAnalyzer.OverallAnalysis();
                return Request.CreateResponse(HttpStatusCode.OK, imageAnalyzer.GetImage().GetOverallRating());
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

    }
}
