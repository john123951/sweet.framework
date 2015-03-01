using System;
using System.Linq;
using test.Model;
using System.Collections.Generic;
using test.Infrastructure;

namespace test.Service
{
	public class UserService : IUserService
	{
		static readonly List<UserInfo> _db = new List<UserInfo> ();

		[Cache (KeyName = "GetUserList", Subscribe = new []{ "InsertUser", "RemoveUser" })]
		public List<UserInfo> GetUserList ()
		{
			return _db;
		}

		[Cache (Publish = "InsertUser")]
		public bool InsertUser (UserInfo userInfo)
		{
			_db.Add (userInfo);
			return true;
		}

		[Cache (Publish = "RemoveUser")]
		public bool RemoveUser (long userId)
		{
			var model = _db.FirstOrDefault (x => x.Id == userId);
			if (model != null) {
				_db.Remove (model);
				return true;
			}
			return false;
		}
	}
}

