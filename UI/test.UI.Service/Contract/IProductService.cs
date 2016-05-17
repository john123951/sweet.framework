using sweet.framework.Infrastructure.Interfaces;
using System.Collections.Generic;
using test.UI.Model.ServiceDtos;

namespace test.UI.Service.Contract
{
    public interface IProductService : IService
    {
        List<ProductDto> GetProductList(long userId, int startIndex, int endIndex, out int total);

        ProductDto AddProduct(ProductDto productInfo);
    }
}