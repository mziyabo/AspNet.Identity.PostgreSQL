using System;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.PostgreSQL
{
	public class IdentityUserManager : UserManager<IdentityUser, Guid>
	{
		public IdentityUserManager(IUserStore<IdentityUser, Guid> store)
				: base(store)
		{
		}
	}
}
