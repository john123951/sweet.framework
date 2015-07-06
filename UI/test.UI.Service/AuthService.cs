using System.Collections.Generic;
using sweet.framework.Infrastructure.Attr;
using test.UI.Model.Entities;
using test.UI.Service.Contract;

namespace test.UI.Service
{
	public class AuthService : IAuthService
	{
		public static readonly Dictionary<long , List<RoleInfo>> _db = new Dictionary<long, List<RoleInfo>> ();

        [Cache(KeyName = "GetUserRole{userId}", Subscribe = new[] { "SetUserRole{userId}" })]
		public List<RoleInfo> GetUserRole (long userId)
		{
			if (false == _db.ContainsKey (userId)) {
				_db.Add (userId, new List<RoleInfo> ());
			}

			return _db [userId];
		}

        [Cache(Publish = "SetUserRole{userId}")]
		public bool SetUserRole (long userId, List<RoleInfo> roles)
		{
			_db [userId] = roles;

			return true;
		}
	}
}

