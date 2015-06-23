using System.Collections.Generic;
using sweet.framework.Infrastructure.Interfaces;
using test.UI.Model.Entities;

namespace test.UI.Service.Contract
{
    public interface IProductService : IMainService
    {
        List<ProductInfo> GetProductList(long userId, int startIndex, int endIndex, out int total);

        ProductInfo AddProduct(ProductInfo productInfo);
    }
}