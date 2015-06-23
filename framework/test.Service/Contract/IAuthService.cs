using System.Collections.Generic;
using test.Infrastructure.Interfaces;
using test.Model.Entities;

namespace test.Service.Contract
{
	public interface IAuthService : IMainService
	{
		List<RoleInfo> GetUserRole (long userId);

		bool SetUserRole (long userId, List<RoleInfo> roles);
	}
}

