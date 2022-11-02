using DataLayer.DTO;
using DataLayer.Models;
using HttpMultipartParser;

namespace Infrastructure.IServices
{
    public interface IProductService
    {
        Task<Product> GetById(string id);
        Task<Product> GetByName(string name);
        Task<List<Product>> GetAll();
        Task<Product> Update (Product product);
        Task<Product> Add(ProductDTO product);
        Task Delete(string id);
        Task<Product> AddImage(string id, FilePart image);
        
    }
}
