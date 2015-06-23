using sweet.framework.Infrastructure.Attr;
using System.Collections.Generic;
using System.Linq;
using test.UI.Model.Entities;
using test.UI.Service.Contract;

namespace test.UI.Service
{
    public class ProductService : IProductService
    {
        private static readonly List<ProductInfo> _db = new List<ProductInfo>();

        [Cache(KeyName = "GetProductList{userId}", Subscribe = new[] { "AddProduct{userId}", "SetUserRole{userId}" })]
        public List<ProductInfo> GetProductList(long userId, int startIndex, int endIndex, out int total)
        {
            total = _db.Count;

            return _db.Where(x => x.UserId == userId).ToList();
        }

        [Cache(Publish = "AddProduct{productInfo.UserId}")]
        public ProductInfo AddProduct(ProductInfo productInfo)
        {
            _db.Add(productInfo);

            return productInfo;
        }
    }
}