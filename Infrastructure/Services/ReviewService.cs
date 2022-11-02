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
    public class ReviewService : IReviewService
    {
        private readonly IOnlineStore<Review> _onlineStore;
        private readonly IProductService _productService;

        public ReviewService(IOnlineStore<Review> onlineStore, IProductService productService)
        {
            _productService = productService;
            _onlineStore = onlineStore;
        }
        public async Task<Review> Add(ReviewDTO review)
        {
            await _productService.GetById(review.ProductID.ToString());
            if (review == null)
                throw new ArgumentNullException("Review Can not be empty");

            if (review.Rating < 0 )
                throw new ArgumentOutOfRangeException("Invalid rating");

            Review r = new Review(
                Guid.NewGuid(),
                Guid.Parse(review.ProductID),
                review.Rating,
                review.Description
                );
            return await _onlineStore.Add(r);
        }

        public async Task<List<Review>> GetAll()
        {
            return await _onlineStore.GetAll().ToListAsync();
        }
    }
}
