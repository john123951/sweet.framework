using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2016/5/17 11:40:31
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace test.UI.Model.ServiceDtos
{
    [Serializable]
    public class ProductDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public long UserId { get; set; }
    }
}