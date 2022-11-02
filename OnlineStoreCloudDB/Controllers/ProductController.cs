using Infrastructure.IServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DTO;
using Newtonsoft.Json;
using System.IO;
using HttpMultipartParser;
using DataLayer.Models;

namespace OnlineStoreCloudDB.Controllers
{
    public class ProductController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Function("GetProducts")]
        public async Task<HttpResponseData> GetProducts(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products")]
                HttpRequestData requestData)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(await _productService.GetAll(), HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Something wrong happened.", HttpStatusCode.BadRequest);
            }

            return response;
        }

        [Function("GetProduct")]
        public async Task<HttpResponseData> GetProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products/{productId}")]
                HttpRequestData requestData,
            string productId)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(await _productService.GetById(productId), HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Something wrong happened.", HttpStatusCode.BadRequest);
            }

            return response;
        }

        [Function("DeleteProduct")]
        public async Task<HttpResponseData> DeleteProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "products/{productId}")]
                HttpRequestData requestData,
            string productId)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await _productService.Delete(productId);
                await response.WriteAsJsonAsync("Product is successfully removed.", HttpStatusCode.Accepted);
            }
            catch
            {
                await response.WriteAsJsonAsync("Product was not deleted.", HttpStatusCode.BadRequest);
            }
            return response;
        }

        [Function("AddProduct")]
        public async Task<HttpResponseData> AddProduct(
            [HttpTrigger(AuthorizationLevel.Anonymous,"POST", Route = "products")]
                HttpRequestData requestData)
        {
            string requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();
            HttpResponseData response = requestData.CreateResponse();
            try
            {

                ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
                await response.WriteAsJsonAsync(await _productService.Add(productDTO), HttpStatusCode.Created);
            }
            catch
            {
                await response.WriteAsJsonAsync("Product was not created.", HttpStatusCode.BadRequest);
            }
            return response;
        }

        [Function("AddProductImage")]
        public async Task<HttpResponseData> AddProductImage(
            [HttpTrigger(AuthorizationLevel.Anonymous,"POST", Route = "Products/image/{productId}")]
                HttpRequestData requestData,
            string productId)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                MultipartFormDataParser requestBodyData = MultipartFormDataParser.Parse(requestData.Body);
                FilePart imageFile = requestBodyData.Files[0];

                Product product = await _productService.AddImage(productId,imageFile);
                await response.WriteAsJsonAsync($"Image is successfully added", HttpStatusCode.Created);
            }
            catch
            {
                await response.WriteAsJsonAsync("Product image was not added.", HttpStatusCode.BadRequest);
            }
            return response;
        }
    }
}
