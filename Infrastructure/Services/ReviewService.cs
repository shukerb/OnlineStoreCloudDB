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
        private readonly IOnlineStoreRead<Review> _onlineStoreRead;
        private readonly IOnlineStoreWrite<Review> _onlineStoreWrite;
        private readonly IProductService _productService;

        public ReviewService(IOnlineStoreRead<Review> onlineStore, IOnlineStoreWrite<Review> onlineStoreWrite, IProductService productService)
        {
            _productService = productService;
            _onlineStoreRead = onlineStore;
            _onlineStoreWrite = onlineStoreWrite;
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
            return await _onlineStoreWrite.Add(r);
        }

        public async Task<List<Review>> GetAll()
        {
            return await _onlineStoreRead.GetAll().ToListAsync();
        }
    }
}
