using DataLayer.DTO;
using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IServices
{
    public interface IReviewService
    {
        Task<Review> Add(ReviewDTO review);
        Task<List<Review>> GetAll();
        Task<Review> Delete(int id);
    }
}
