using System;
using test.Model;
using System.Collections.Generic;
using test.Infrastructure;

namespace test.Service
{
	public interface IProductService : IMainService
	{
		List<ProductInfo> GetProductList (long userId, int startIndex, int endIndex, out int total);

		ProductInfo AddProduct (ProductInfo productInfo);
	}
}

