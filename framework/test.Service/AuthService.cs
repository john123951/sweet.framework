using System;
using System.Collections.Generic;
using test.Model;
using test.Infrastructure;

namespace test.Service
{
	public class AuthService : IAuthService
	{
		public static readonly Dictionary<long , List<RoleInfo>> _db = new Dictionary<long, List<RoleInfo>> ();

		[Cache (KeyName = "GetUserRole{0}", Subscribe = new []{ "SetUserRole{0}" })]
		public List<RoleInfo> GetUserRole (long userId)
		{
			if (false == _db.ContainsKey (userId)) {
				_db.Add (userId, new List<RoleInfo> ());
			}

			return _db [userId];
		}

		[Cache (Publish = "SetUserRole{0}")]
		public bool SetUserRole (long userId, List<RoleInfo> roles)
		{
			_db [userId] = roles;

			return true;
		}
	}
}

