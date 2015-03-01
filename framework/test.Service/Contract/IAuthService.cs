using System;
using test.Model;
using System.Collections.Generic;
using test.Infrastructure;

namespace test.Service
{
	public interface IAuthService : IMainService
	{
		List<RoleInfo> GetUserRole (long userId);

		bool SetUserRole (long userId, List<RoleInfo> roles);
	}
}

