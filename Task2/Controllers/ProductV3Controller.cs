using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Task2.Controllers.Filters;
using Task2.Models;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Task2.Controllers
{
    public class ProductV3Controller : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        public static class WebApiConfig
        {
            public static void Register(HttpConfiguration config)
            {
                config.Filters.Add(new ValidateModelAttribute());

                // ...
            }
        }
        //Version 3
        //[Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/v3/products")]
        public IEnumerable<Product> GetAllProductsFromRepository()
        {
            return repository.GetAll();

        }
        //Route constraints let you restrict how the parameters in the route template are matched. 
        //The general syntax is "{parameter:constraint}".
        //Constraints on URL parameters

        //We can even restrict the template placeholder to the type of parameter it can have. 
        //For example, we can restrict that the request will be only served if the id is greater than 2.
        //Otherwise the request will not work. For this, we will apply multiple constraints in the same request:


        //Type of the parameter id must be an integer.
        //id should be greater than 2.
        //http://localhost:9000/api/v3/products/1 404 error code
        [HttpGet]
        [Route("api/v3/products/{id:int:min(2)}", Name = "getProductByIdv3")]

        public Product retrieveProductfromRepository(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return item;
        }


        [HttpGet]
        [Route("api/v3/products", Name = "getProductByCategoryv3")]
        //http://localhost:9000/api/v3/products?category=
        //http://localhost:9000/api/v3/products?category=Groceries

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }
         

        [HttpPost]
        [Route("api/v3/products")]
        [ValidateModel] 
        public HttpResponseMessage PostProduct(  Product item)   
        {
            if (ModelState.IsValid)
            {
                item = repository.Add(item);
                var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

                // Generate a link to the new product and set the Location header in the response.

                string uri = Url.Link("getProductByIdv3", new { id = item.Id });
                response.Headers.Location = new Uri(uri);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }







        [HttpPut]
        [Route("api/v3/products/{id:int}")]
        public HttpResponseMessage PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!ModelState.IsValid)
            {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            repository.Update(product);

            var response = Request.CreateResponse<Product>(HttpStatusCode.OK, product);

            string uri = Url.Link("getProductByIdv3", new { id = product.Id });
            response.Headers.Location = new Uri(uri);
            return response; ;
        }   

        [HttpDelete]
        [Route("api/v3/products/{id:int}")]
        public HttpResponseMessage DeleteProduct(int id)
        {
            try
            {
                repository.Remove(id);

                var response = Request.CreateResponse<String>(HttpStatusCode.OK, "Delete Successful");

                string uri = Url.Link("getProductByIdv3", new { id = id });
                response.Headers.Location = new Uri(uri);
                return response; ;
            }
            catch {

                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Delete Unsuccessful");
            }

        }
    }
}
