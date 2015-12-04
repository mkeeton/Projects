using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Interfaces.Repositories
{
    public interface IRoleRepository<T> : IRoleStore<Role, Guid>
    {
    }
}
