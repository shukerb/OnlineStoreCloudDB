using DataLayer.DTO;
using Infrastructure.IServices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace OnlineStoreCloudDB.Controllers
{
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Function("GetUsers")]
        public async Task<HttpResponseData> GetUsers(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users")]
                HttpRequestData requestData)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(await _userService.GetAll(), HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Something wrong happened.", HttpStatusCode.BadRequest);
            }
            
            return response;
        }

        [Function("GetUser")]
        public async Task<HttpResponseData> GetUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "users/{userId}")]
                HttpRequestData requestData,
            string userId)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(await _userService.GetById(userId),HttpStatusCode.OK);
            }
            catch
            {
                await response.WriteAsJsonAsync("Something wrong happened.", HttpStatusCode.BadRequest);
            }
            
            return response;
        }

        [Function("DeleteUser")]
        public async Task<HttpResponseData> DeleteUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "DELETE", Route = "users/{userId}")]
                HttpRequestData requestData,
            string userId)
        {
            HttpResponseData response = requestData.CreateResponse();
            try
            {
                await _userService.DeleteUser(userId);
                await response.WriteAsJsonAsync("User is successfully removed.", HttpStatusCode.Accepted);
            }
            catch
            {
                await response.WriteAsJsonAsync("User was not deleted.", HttpStatusCode.BadRequest);
            }
            return response;
        }

        [Function("AddUser")]
        public async Task<HttpResponseData> AddUser(
            [HttpTrigger(AuthorizationLevel.Anonymous,"POST", Route = "users")]
                HttpRequestData requestData)
        {
            string requestBody = await new StreamReader(requestData.Body).ReadToEndAsync();

            HttpResponseData response = requestData.CreateResponse();
            try
            {

                UserDTO userDTO = JsonConvert.DeserializeObject<UserDTO>(requestBody);
                await response.WriteAsJsonAsync(await _userService.AddUser(userDTO), HttpStatusCode.Created);
            }
            catch
            {
                await response.WriteAsJsonAsync("User was not created.", HttpStatusCode.BadRequest);
            }
            return response;
        }
    }
}
