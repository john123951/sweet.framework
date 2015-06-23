using System.Collections.Generic;
using sweet.framework.Infrastructure.Interfaces;
using test.UI.Model.Entities;

namespace test.UI.Service.Contract
{
    public interface IUserService : IService
    {
        List<UserInfo> GetUserList();

        bool InsertUser(UserInfo userInfo);

        bool RemoveUser(long userId);
    }
}