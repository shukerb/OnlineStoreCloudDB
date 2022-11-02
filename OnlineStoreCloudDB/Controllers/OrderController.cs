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
using DataLayer.Models;

namespace OnlineStoreCloudDB.Controllers
{
    public class OrderController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Function("GetOrders")]
        public async Task<HttpResponseData> GetOrders(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orders")]
                HttpRequestData requestData)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(await _orderService.GetAll(), HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Something wrong happened.", HttpStatusCode.BadRequest);
            }

            return response;
        }

        [Function("GetOrder")]
        public async Task<HttpResponseData> GetOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orders/{orderId}")]
                HttpRequestData requestData,
            string orderId)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(await _orderService.GetById(orderId), HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Something wrong happened.", HttpStatusCode.BadRequest);
            }

            return response;
        }

        [Function("DeleteOrder")]
        public async Task<HttpResponseData> DeleteOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "orders/{orderId}}")]
                HttpRequestData requestData,
            string orderId)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await _orderService.Delete(orderId);
                await response.WriteAsJsonAsync("Order is deleted.", HttpStatusCode.Accepted);
            }
            catch
            {
                await response.WriteAsJsonAsync("Order was not deleted.", HttpStatusCode.BadRequest);
            }
            return response;
        }

        [Function("CreateOrder")]
        public async Task<HttpResponseData> CreateOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous,"POST", Route = "orders")]
                HttpRequestData requestData)
        {
            string requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();

            HttpResponseData response = requestData.CreateResponse();
            try
            {
                OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
                await response.WriteAsJsonAsync(await _orderService.Add(orderDTO), HttpStatusCode.Created);
            }
            catch
            {
                await response.WriteAsJsonAsync("Order was not created.", HttpStatusCode.BadRequest);
            }
            return response;
        }

        [Function("EditOrder")]
        public async Task<HttpResponseData> EditOrder(
            [HttpTrigger(AuthorizationLevel.Anonymous,"POST", Route = "orders/order/{orderId}")]
                HttpRequestData requestData,
            string orderId)
        {
            string requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();

            HttpResponseData response = requestData.CreateResponse();
            try
            {
                Order order = JsonConvert.DeserializeObject<Order>(requestBody);
                await response.WriteAsJsonAsync(await _orderService.Update(order), HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Order was not edited.", HttpStatusCode.BadRequest);
            }
            return response;
        }

    }
}
