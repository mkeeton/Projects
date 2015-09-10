using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Data.Interfaces;
using IcedMemories.Domain.Models;
using IcedMemories.Infrastructure.Repositories;

namespace IcedMemories.Infrastructure
{
  public class UnitOfWork : IDisposable
  {
    private IDbContext _dbContext;
    private UserRepository<User> _userRepository;
    private RoleRepository<Role> _roleRepository;
    private CakeRepository _cakeRepository;
    private SearchCategoryRepository _categoryRepository;
    private SearchCategoryOptionRepository _categoryOptionRepository;
    private SearchCategorySelectionRepository _categoryOptionSelectionRepository;

    public static UnitOfWork Create()
    {
      return new UnitOfWork();
    }

    public IDbContext DbContext
    {
      get
      {
        return _dbContext;
      }
      set
      {
        _dbContext = value;
      }
    }

    public UnitOfWork()
    {
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

    public UserRepository<User> UserManager
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

    public RoleRepository<Role> RoleManager
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

    public CakeRepository CakeManager
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

    public SearchCategoryRepository SearchCategoryManager
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

    public SearchCategoryOptionRepository SearchCategoryOptionManager
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

    public SearchCategorySelectionRepository SearchCategorySelectionManager
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
