using sweet.framework.Infrastructure.Interfaces;
using System.Collections.Generic;
using test.UI.Model.ServiceDtos;

namespace test.UI.Service.Contract
{
    public interface IUserService : IService
    {
        List<UserDto> GetUserList();

        bool InsertUser(UserDto userInfo);

        bool RemoveUser(long userId);
    }
}