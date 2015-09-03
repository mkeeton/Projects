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
    private CakeRepository _cakeRepository;

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

  }
}
