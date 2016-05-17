using sweet.framework.Infrastructure.Interfaces;
using System.Collections.Generic;
using test.UI.Model.ServiceDtos;

namespace test.UI.Service.Contract
{
    public interface IAuthService : IService
    {
        List<RoleDto> GetUserRole(long userId);

        bool SetUserRole(long userId, List<RoleDto> roles);
    }
}