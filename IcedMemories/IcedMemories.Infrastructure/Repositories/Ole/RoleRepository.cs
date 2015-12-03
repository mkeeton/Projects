using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using Microsoft.AspNet.Identity;
using IcedMemories.Data.Interfaces;
using IcedMemories.Domain.Models;
using IcedMemories.Infrastructure.Interfaces.Repositories;

namespace IcedMemories.Infrastructure.Repositories.Ole
{
    public class RoleRepository<T> : IRoleRepository<T>
    {

        private readonly IDbContext CurrentContext;

        public RoleRepository(IDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("connectionString");

            this.CurrentContext = context;
        }

        public void Dispose()
        {

        }

        public Task CreateAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            return Task.Factory.StartNew(() =>
            {
                role.Id = Guid.NewGuid();
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    connection.Execute("insert into auth_Users(Id, UserName, PasswordHash, SecurityStamp, Email, EmailConfirmed, FirstName, LastName) values(@Id, @userName, @passwordHash, @securityStamp, @email, @emailConfirmed, @FirstName, @LastName)", role);
            });
        }

        public Task DeleteAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    connection.Execute("delete from auth_Users where Id = @Id", new { role.Id });
            });
        }

        public Task<Role> FindByIdAsync(Guid roleId)
        {
            if (roleId == Guid.Empty)
                throw new ArgumentNullException("roleId");

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    return connection.Query<Role>("select * from auth_Users where Id = @Id", new { Id = roleId }).SingleOrDefault();
            });
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentNullException("roleName");

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    return connection.Query<Role>("select * from auth_Users where lower(UserName) = lower(@userName)", new { roleName }).SingleOrDefault();
            });
        }

        public Task UpdateAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    connection.Execute("update auth_Users set UserName = @userName, PasswordHash = @passwordHash, SecurityStamp = @securityStamp, FirstName=@FirstName, LastName=@LastName where Id = @Id", role);
            });
        }

    }
}
