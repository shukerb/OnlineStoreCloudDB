using DataLayer.DTO;
using DataLayer.Models;
using HttpMultipartParser;
using Infrastructure.IRepositories;
using Infrastructure.IServices;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IOnlineStoreRead<Product> _onlineStoreRead;
        private readonly IOnlineStoreWrite<Product> _onlineStoreWrite;
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient blobContainerClient;

        public ProductService(IOnlineStoreRead<Product> onlineStoreRead, IOnlineStoreWrite<Product> onlineStoreWrite)
        {
            blobServiceClient = new BlobServiceClient(Environment.GetEnvironmentVariable("BlobCreds:ConnectionString", EnvironmentVariableTarget.Process));
            blobContainerClient = blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("BlobCreds:ContainerName", EnvironmentVariableTarget.Process));
            _onlineStoreRead = onlineStoreRead;
            _onlineStoreWrite = onlineStoreWrite;

        }

        public async Task<Product> Add(ProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException("Product can not be empty.");
            }
            if (await GetByName(product.Name) != null)
            {
                throw new ArgumentException($"Product with the same name {product.Name} already exists.");
            }
            if (product.Amount < 0)
            {
                throw new ArgumentException("invalid product amount.");
            }
            Product p = new Product(
                Guid.NewGuid(),
                product.Name,
                NullOrEmptyStringChecker(product.Description),
                product.Amount
                );
            return await _onlineStoreWrite.Add(p);
        }

        public async Task<Product> AddImage(string id, FilePart image)
        {
            //make sure product exists
            var product = await GetById(id);

            if (!(image.ContentType is "image/png" or "image/jpeg") )
                throw new ArgumentException("Only images of [png, jpeg] type can be added.");

            BlobClient blobClient = blobContainerClient.GetBlobClient(image.Name);
            await blobClient.UploadAsync(image.Data);

            ProductImage productImage = new ProductImage(
                blobClient.Uri.AbsoluteUri,
                image.Name
                );
            //add image to the product
            product.Images.Add(productImage);

            return await _onlineStoreWrite.Update(product);
        }

        public async Task Delete(string id)
        {
            Product product = await GetById(NullOrEmptyStringChecker(id));
            await _onlineStoreWrite.Delete(product);
        }

        public async Task<List<Product>> GetAll()
        {
            return await _onlineStoreRead.GetAll().ToListAsync();
        }

        public async Task<Product> GetById(string id)
        {
            Guid ProductId;
            Guid.TryParse(NullOrEmptyStringChecker(id), out ProductId);

            if (!Guid.TryParse(NullOrEmptyStringChecker(id), out ProductId))
            {
                throw new ArgumentException("Invalid product ID was provided.");
            }

            var result = await _onlineStoreRead.GetAll().FirstOrDefaultAsync(product => product.Id == ProductId);

            if (result == null)
            {
                throw new ArgumentException($"Product with ID:{id}, does not exist.");
            }

            return result;
        }

        public async Task<Product> GetByName(string name)
        {
            NullOrEmptyStringChecker(name);
            var result = await _onlineStoreRead.GetAll().FirstOrDefaultAsync(product => product.Name == name);
            return result;
        }

        public async Task<Product> Update(Product product)
        {
            Product p = await GetById(product.Id.ToString());
            p = product;
            await _onlineStoreWrite.Update(p);
            return p;
        }
        private string NullOrEmptyStringChecker(string stringToCheck)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentNullException("Please fill all the Product information.");
            }
            return stringToCheck;
        }
    }
}
