using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Data.Interfaces;
using IcedMemories.Domain.Models;
using IcedMemories.Infrastructure.Interfaces;
using IcedMemories.Infrastructure.Interfaces.Repositories;
using IcedMemories.Infrastructure.Repositories.Sql;

namespace IcedMemories.Infrastructure
{
  public class UnitOfWork : IDisposable, IUnitOfWork
  {

    public static IUnitOfWork Create(IDbContext context)
    {
      return new UnitOfWork(context);
    }


    public UnitOfWork(IDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException("connectionString");

      this.DbContext = context;
    }

    //public UnitOfWork()
    //{
    //  this._dbContext = DbContext.Create();
    //}

    public void Dispose()
    {

    }

    public IDbContext DbContext
    {
        get;set;
    }

    public void BeginWork()
    {
        DbContext.BeginTransaction();
    }

    public void CommitWork()
    {
        DbContext.CommitTransaction();
    }

    public void RollbackWork()
    {
        DbContext.RollbackTransaction();
    }

    public IUserRepository<User> UserManager
    {
        get;set;
    }

    public IRoleRepository<Role> RoleManager
    {
      get;
      set;
    }

    public ICakeRepository CakeManager
    {
      get;
      set;
    }

    public ISearchCategoryRepository SearchCategoryManager
    {
      get;
      set;
    }

    public ISearchCategoryOptionRepository SearchCategoryOptionManager
    {
      get;
      set;
    }

    public ISearchCategorySelectionRepository SearchCategorySelectionManager
    {
      get;
      set;
    }

  }
}
