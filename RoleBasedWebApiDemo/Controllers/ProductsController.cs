using RoleBasedWebApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

using System.Web.Security;

namespace RoleBasedWebApiDemo.Controllers
{
    public class ProductsController : ApiController
    {
        // GET /api/products => only users having the Users role can call this
        [Authorize(Roles = "Users")]
        public HttpResponseMessage Get()
        {
            var products = Enumerable.Range(1, 5).Select(x => new Product
            {
                Id = x,
                Name = "product " + x
            });
            return Request.CreateResponse(HttpStatusCode.OK, products);
        }

        [Authorize(Roles="Admin")]
        public void Post(Product product)
        {

        }

    }
}
