using System;

namespace test.UI.Model.Entities
{
    [Serializable]
    public class ProductInfo
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public long UserId { get; set; }
    }
}