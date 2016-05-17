using sweet.framework.Infrastructure.Attr;
using System.Collections.Generic;
using System.Linq;
using test.UI.Model.ServiceDtos;
using test.UI.Service.Contract;

namespace test.UI.Service
{
    public class UserService : IUserService
    {
        private static readonly List<UserDto> _db = new List<UserDto>();

        [Cache(KeyName = "GetUserList", Subscribe = new[] { "InsertUser", "RemoveUser" })]
        public List<UserDto> GetUserList()
        {
            return _db;
        }

        [Cache(Publish = "InsertUser")]
        public bool InsertUser(UserDto userInfo)
        {
            _db.Add(userInfo);
            return true;
        }

        [Cache(Publish = "RemoveUser")]
        public bool RemoveUser(long userId)
        {
            var model = _db.FirstOrDefault(x => x.Id == userId);
            if (model != null)
            {
                _db.Remove(model);
                return true;
            }
            return false;
        }
    }
}