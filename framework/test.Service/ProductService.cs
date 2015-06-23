using System;
using System.Collections.Generic;
using test.Model;
using System.Linq;
using test.Infrastructure;
using test.Model.Entities;
using test.Service.Contract;

namespace test.Service
{
	public class ProductService : IProductService
	{
		static readonly List<ProductInfo> _db = new List<ProductInfo> ();

        [Cache(KeyName = "GetProductList{userId}", Subscribe = new[] { "AddProduct", "SetUserRole{userId}" })]
		public List<ProductInfo> GetProductList (long userId, int startIndex, int endIndex, out int total)
		{
			total = _db.Count;
			var roles = AuthService._db [userId];

			return _db.Take (roles.Count).ToList ();
		}

		[Cache (Publish = "AddProduct")]
		public ProductInfo AddProduct (ProductInfo productInfo)
		{
			_db.Add (productInfo);

			return productInfo;
		}
	}
}

