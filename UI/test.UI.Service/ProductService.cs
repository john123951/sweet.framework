using sweet.framework.Infrastructure.Attr;
using sweet.framework.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using test.UI.Model.ServiceDtos;
using test.UI.Respository.Contract;
using test.UI.Respository.Entities;
using test.UI.Service.Contract;

namespace test.UI.Service
{
    public class ProductService : IProductService
    {
        private IRepository<ProductEntity, long> _productRepository;

        public ProductService(IRepository<ProductEntity, long> productRepository)
        {
            _productRepository = productRepository;
        }

        [Cache(KeyName = "GetProductList{userId}", Subscribe = new[] { "AddProduct{userId}", "SetUserRole{userId}" })]
        public List<ProductDto> GetProductList(long userId, int startIndex, int endIndex, out int total)
        {
            var query = _productRepository.LoadEntities(x => x.UserId == userId);

            total = query.Count();
            var list = query.ToList().Select(x => new ProductDto { Id = x.Id, Name = x.Name, Price = x.Price, UserId = x.UserId }).ToList();

            return list;
        }

        [Cache(Publish = "AddProduct{productInfo.UserId}")] //No Cache ,Only Publish
        public ProductDto AddProduct(ProductDto productInfo)
        {
            var entity = new ProductEntity { Name = productInfo.Name, Price = productInfo.Price, UserId = productInfo.UserId };

            bool insertSuccess = _productRepository.Insert(entity);

            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                UserId = entity.UserId
            };
        }
    }
}