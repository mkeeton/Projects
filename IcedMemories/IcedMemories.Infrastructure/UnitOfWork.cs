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
    private IDbContext _dbContext;
    private IUserRepository<User> _userRepository;
    private IRoleRepository<Role> _roleRepository;
    private ICakeRepository _cakeRepository;
    private ISearchCategoryRepository _categoryRepository;
    private ISearchCategoryOptionRepository _categoryOptionRepository;
    private ISearchCategorySelectionRepository _categoryOptionSelectionRepository;

    public static IUnitOfWork Create(IDbContext context)
    {
      return new UnitOfWork(context);
    }


    public UnitOfWork(IDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException("connectionString");

      this._dbContext = context;
    }

    //public UnitOfWork()
    //{
    //  this._dbContext = DbContext.Create();
    //}

    public void Dispose()
    {

    }

    public void BeginWork()
    {
      _dbContext.BeginTransaction();
    }

    public void CommitWork()
    {
      _dbContext.CommitTransaction();
    }

    public void RollbackWork()
    {
      _dbContext.RollbackTransaction();
    }

    public IUserRepository<User> UserManager
    {
      get
      {
        if (_userRepository == null)
        {
          _userRepository = new UserRepository<User>(_dbContext);
        }
        return _userRepository;
      }
      private set
      {
        _userRepository = value;
      }
    }

    public IRoleRepository<Role> RoleManager
    {
      get
      {
        if (_roleRepository == null)
        {
          _roleRepository = new RoleRepository<Role>(_dbContext);
        }
        return _roleRepository;
      }
      private set
      {
        _roleRepository = value;
      }
    }

    public ICakeRepository CakeManager
    {
      get
      {
        if (_cakeRepository == null)
        {
          _cakeRepository = new CakeRepository(_dbContext);
        }
        return _cakeRepository;
      }
      private set
      {
        _cakeRepository = value;
      }
    }

    public ISearchCategoryRepository SearchCategoryManager
    {
      get
      {
        if (_categoryRepository == null)
        {
          _categoryRepository = new SearchCategoryRepository(_dbContext);
        }
        return _categoryRepository;
      }
      private set
      {
        _categoryRepository = value;
      }
    }

    public ISearchCategoryOptionRepository SearchCategoryOptionManager
    {
      get
      {
        if (_categoryOptionRepository == null)
        {
          _categoryOptionRepository = new SearchCategoryOptionRepository(_dbContext);
        }
        return _categoryOptionRepository;
      }
      private set
      {
        _categoryOptionRepository = value;
      }
    }

    public ISearchCategorySelectionRepository SearchCategorySelectionManager
    {
      get
      {
        if (_categoryOptionSelectionRepository == null)
        {
          _categoryOptionSelectionRepository = new SearchCategorySelectionRepository(_dbContext);
        }
        return _categoryOptionSelectionRepository;
      }
      private set
      {
        _categoryOptionSelectionRepository = value;
      }
    }

  }
}
