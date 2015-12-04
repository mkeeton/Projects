using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Data.Interfaces;
using IcedMemories.Infrastructure.Interfaces.Repositories;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        void BeginWork();
        void CommitWork();
        void RollbackWork();
        IUserRepository<User> UserManager { get; set;}
        IRoleRepository<Role> RoleManager { get; set; }
        ICakeRepository CakeManager { get; set; }
        ISearchCategoryRepository SearchCategoryManager { get; set; }
        ISearchCategoryOptionRepository SearchCategoryOptionManager { get; set; }
        ISearchCategorySelectionRepository SearchCategorySelectionManager { get; set; }
    }
}
