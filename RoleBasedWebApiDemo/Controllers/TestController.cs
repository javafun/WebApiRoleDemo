using RoleBasedWebApiDemo.Infrastructure.Utilities;
using RoleBasedWebApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace RoleBasedWebApiDemo.Controllers
{
    public class TestController : Controller
    {
        public ActionResult GetProducts()
        {
            var productsUrl = Url.RouteUrl("DefaultApi", new
            {
                httproute = "",
                controller =
                    "products"
            }, "http");
            using (var client = new HttpClient())
            {
                var token = RSAClass.Encrypt("john");
                client.DefaultRequestHeaders.Add("Authorization-Token", token);
                var products = client
                    .GetAsync(productsUrl)
                    .Result
                    .Content
                    .ReadAsAsync<IEnumerable<Product>>()
                    .Result;
                return Json(products, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult PostProduct()
        {
            var productsUrl = Url.RouteUrl("DefaultApi", new
            {
                httproute = "",
                controller =
                    "products"
            }, "http");
            using (var client = new HttpClient())
            {
                var token = RSAClass.Encrypt("john");
                client.DefaultRequestHeaders.Add("Authorization-Token", token);
                var product = new Product
                {
                    Id = 1,
                    Name = "test product"
                };
                var result = client
                    .PostAsync<Product>(productsUrl, product, new JsonMediaTypeFormatter())
                    .Result;
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("Sorry you are not authorized to perform this operation");
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
