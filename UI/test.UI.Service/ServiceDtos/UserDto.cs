using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* =======================================================================
* 创建时间：2016/5/17 11:40:49
* 作者：sweet
* Framework: 4.5
* ========================================================================
*/

namespace test.UI.Service.ServiceDtos
{
    [Serializable]
    public class UserDto
    {
        public long Id { get; set; }

        public string UserName { get; set; }
    }
}