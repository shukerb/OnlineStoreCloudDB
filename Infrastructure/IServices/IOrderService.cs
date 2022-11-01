using DataLayer.DTO;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IServices
{
    public interface IOrderService
    {
        Task<Order> GetById(string Id);
        Task<List<Order>> GetAll();
        Task Delete(string orderId);
        Task<Order> Add(OrderDTO order);
        Task<Order> Update(Order order);

    }
}
