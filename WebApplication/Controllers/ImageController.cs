using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;


namespace WebApplication.Controllers
{
    public class ImageController : ApiController
    {
        WebApplication.Models.Image[] products = new WebApplication.Models.Image[] 
        { 
            new WebApplication.Models.Image { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 }, 
            new WebApplication.Models.Image { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
            new WebApplication.Models.Image { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
        };

        public IEnumerable<WebApplication.Models.Image> GetAllImages()
        {
            return products;
        }

        public IHttpActionResult GetImage(int id)
        {
            var product = products.FirstOrDefault((p) => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
