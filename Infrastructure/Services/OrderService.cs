using DataLayer.DTO;
using DataLayer.Models;
using Infrastructure.IRepositories;
using Infrastructure.IServices;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOnlineStoreRead<Order> _onlineStoreRead;
        private readonly IOnlineStoreWrite<Order> _onlineStoreWrite;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public OrderService(IOnlineStoreRead<Order> onlineStoreRead, IOnlineStoreWrite<Order> onlineStoreWrite, IUserService userService, IProductService productService)
        {
            _productService = productService;
            _userService = userService;
            _onlineStoreRead = onlineStoreRead;
            _onlineStoreWrite = onlineStoreWrite;
        }

        public async Task<Order> Add(OrderDTO order)
        {
            await _userService.GetById(order.UserId);

            foreach (Product product in order.Products)
            {
                await _productService.GetById(product.Id.ToString());
            }

            if (order == null)
            {
                throw new ArgumentNullException("Order can not be empty.");
            }

            Order o = new Order(
                Guid.NewGuid(),
                Guid.Parse(order.UserId),
                order.Products,
                DateTime.Now,
                false
                );
            
            return await _onlineStoreWrite.Add(o);
        }

        public async Task Delete(string orderId)
        {
            Order order = await GetById(NullOrEmptyStringChecker(orderId));
            await _onlineStoreWrite.Delete(order);
        }

        public async Task<List<Order>> GetAll()
        {
            return await _onlineStoreRead.GetAll().ToListAsync();
        }

        public async Task<Order> GetById(string Id)
        {
            Guid OrderId;
            Guid.TryParse(NullOrEmptyStringChecker(Id), out OrderId);

            if (!Guid.TryParse(NullOrEmptyStringChecker(Id), out OrderId))
            {
                throw new ArgumentException("Invalid order ID was provided.");
            }

            var result = await _onlineStoreRead.GetAll().FirstOrDefaultAsync(order => order.Id == OrderId);

            if (result == null)
            {
                throw new ArgumentException($"Order with ID:{Id}, does not exist.");
            }

            return result;
        }

        public async Task<Order> Update(Order order)
        {
            Order o = await GetById(order.Id.ToString());
            if (order.ShippingDate > order.CreationDate)
            {
                order.IsShipped = true;
            }
            o = order;
            await _onlineStoreWrite.Update(o);
            return o;
        }
        private string NullOrEmptyStringChecker(string stringToCheck)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentNullException("Please fill all the Order information.");
            }
            return stringToCheck;
        }
    }
}
