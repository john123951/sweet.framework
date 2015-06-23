using System;

namespace test.Model.Entities
{
	[Serializable]
	public class ProductInfo
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }
	}
}

