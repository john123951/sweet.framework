using sweet.framework.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.UI.Respository.Entities;

namespace test.UI.Respository.Contract
{
    public interface IProductRepository : IRepository<ProductEntity, long>
    {
    }
}