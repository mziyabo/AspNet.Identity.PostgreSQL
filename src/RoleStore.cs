using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.PostgreSQL
{
    /// <summary>
    /// Class that implements the key ASP.NET Identity role store iterfaces.
    /// </summary>
    public class RoleStore<TRole> : IQueryableRoleStore<TRole>
            where TRole : IdentityRole
    {
        private RoleTable roleTable;
        public PostgreSQLDatabase Database { get; private set; }

        public IQueryable<TRole> Roles
        {
            get
            {
                var result = roleTable.GetAllRoleNames() as System.Collections.Generic.List<TRole>;
                return result.AsQueryable();
            }
        }

        /// <summary>
        /// Default constructor that initializes a new PostgreSQLDatabase instance using the Default Connection string.
        /// </summary>
        public RoleStore() :
            this(new PostgreSQLDatabase())
        {
        }

        /// <summary>
        /// Constructor that takes a PostgreSQLDatabase as argument.
        /// </summary>
        /// <param name="database"></param>
        public RoleStore(PostgreSQLDatabase database)
        {
            this.Database = database;
            this.roleTable = new RoleTable(database);
        }

        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            roleTable.Insert(role);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            roleTable.Delete(role.Id);

            return Task.FromResult<Object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            TRole result = roleTable.GetRoleById(roleId) as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            var role = roleTable.GetRoleByName(roleName);
            TRole result = role as TRole;

            return Task.FromResult<TRole>(result);
        }

        public Task UpdateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            roleTable.Update(role);

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
                if (Database != null)
                {
                    Database.Dispose();
                    Database = null;
                }
            }
        }
    }
}
