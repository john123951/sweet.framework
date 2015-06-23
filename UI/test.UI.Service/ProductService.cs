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

        [Cache(KeyName = "GetProductList{userId}", Subscribe = new[] { "AddProduct", "SetUserRole{userId}" })]
        public List<ProductInfo> GetProductList(long userId, int startIndex, int endIndex, out int total)
        {
            total = _db.Count;
            var roles = AuthService._db[userId];

            return _db.Take(roles.Count).ToList();
        }

        [Cache(Publish = "AddProduct")]
        public ProductInfo AddProduct(ProductInfo productInfo)
        {
            _db.Add(productInfo);

            return productInfo;
        }
    }
}