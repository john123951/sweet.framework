using System;
using test.Model;
using System.Collections.Generic;
using test.Infrastructure;

namespace test.Service
{
	public interface IUserService : IMainService
	{
		List<UserInfo> GetUserList ();

		bool InsertUser (UserInfo userInfo);

		bool RemoveUser (long userId);
	}
}

