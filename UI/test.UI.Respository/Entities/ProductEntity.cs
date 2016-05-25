using sweet.framework.Infrastructure.Model;
using System;

namespace test.UI.Respository.Entities
{
    public class ProductEntity : PrimaryEntity
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public long UserId { get; set; }
    }
}