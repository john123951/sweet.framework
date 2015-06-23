using System.Collections.Generic;
using test.Infrastructure.Interfaces;
using test.Model.Entities;

namespace test.Service.Contract
{
    public interface IUserService : IMainService
    {
        List<UserInfo> GetUserList();

        bool InsertUser(UserInfo userInfo);

        bool RemoveUser(long userId);
    }
}