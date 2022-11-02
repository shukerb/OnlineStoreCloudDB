using DataLayer.DTO;
using Infrastructure.IServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreCloudDB.Controllers
{
    public class ReviewController
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Function("AddReview")]
        public async Task<HttpResponseData> AddReview(
            [HttpTrigger(AuthorizationLevel.Anonymous,"POST", Route = "reviews")]
                HttpRequestData requestData)
        {
            string requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();
            HttpResponseData response = requestData.CreateResponse();
            try
            {

                ReviewDTO reviewDTO = JsonConvert.DeserializeObject<ReviewDTO>(requestBody);
                await response.WriteAsJsonAsync(await _reviewService.Add(reviewDTO), HttpStatusCode.Created);
            }
            catch
            {
                await response.WriteAsJsonAsync("Review was not created.", HttpStatusCode.BadRequest);
            }
            return response;
        }
        [Function("GetReviews")]
        public async Task<HttpResponseData> GetReviews(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "reviews")]
                HttpRequestData requestData)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(await _reviewService.GetAll(), HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Something wrong happened.", HttpStatusCode.BadRequest);
            }

            return response;
        }
    }
}
