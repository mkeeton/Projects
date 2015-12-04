using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Interfaces.Repositories
{
    public interface IUserRepository<T> : IUserStore<User, Guid>, IUserLoginStore<User, Guid>, IUserPasswordStore<User, Guid>, IUserSecurityStampStore<User, Guid>, IUserEmailStore<User, Guid>, IUserRoleStore<User, Guid>
    {

    }
}
