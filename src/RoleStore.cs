using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.PostgreSQL
{
	/// <summary>
	/// Class that implements the key ASP.NET Identity role store interfaces.
	/// </summary>
	public class RoleStore : IRoleStore<IdentityRole>, IQueryableRoleStore<IdentityRole>
	{
		private RoleTable _roleTable;
		private SqlDatabase _database;

		/// <summary>
		/// Constructor that takes a PostgreSQLDatabase as argument.
		/// </summary>
		/// <param name="connection"></param>
		public RoleStore(IDbConnection connection)
		{
			_database = new SqlDatabase(connection);
			_roleTable = new RoleTable(_database);
		}

		public IQueryable<IdentityRole> Roles
		{
			get
			{
				var result = _roleTable.GetAllRoleNames() as System.Collections.Generic.List<IdentityRole>;
				return result.AsQueryable();
			}
		}

		public Task CreateAsync(IdentityRole role)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			_roleTable.Insert(role);

			return Task.FromResult<Object>(null);
		}

		public Task DeleteAsync(IdentityRole role)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			_roleTable.Delete(role.Id);

			return Task.FromResult<Object>(null);
		}

		public Task<IdentityRole> FindByIdAsync(String roleId)
		{
			IdentityRole result = _roleTable.GetRoleById(roleId) as IdentityRole;

			return Task.FromResult<IdentityRole>(result);
		}

		public Task<IdentityRole> FindByNameAsync(String roleName)
		{
			var role = _roleTable.GetRoleByName(roleName);
			IdentityRole result = role as IdentityRole;

			return Task.FromResult<IdentityRole>(result);
		}

		public Task UpdateAsync(IdentityRole role)
		{
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}

			_roleTable.Update(role);

			return Task.FromResult<Object>(null);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				if (_database != null)
				{
					_database.Dispose();
					_database = null;
				}
			}
		}
	}
}
