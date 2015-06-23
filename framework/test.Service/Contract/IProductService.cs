using System.Collections.Generic;
using test.Infrastructure.Interfaces;
using test.Model.Entities;

namespace test.Service.Contract
{
    public interface IProductService : IMainService
    {
        List<ProductInfo> GetProductList(long userId, int startIndex, int endIndex, out int total);

        ProductInfo AddProduct(ProductInfo productInfo);
    }
}