using System.Collections.Generic;
using sweet.framework.Infrastructure.Interfaces;
using test.UI.Model.Entities;

namespace test.UI.Service.Contract
{
	public interface IAuthService : IMainService
	{
		List<RoleInfo> GetUserRole (long userId);

		bool SetUserRole (long userId, List<RoleInfo> roles);
	}
}

