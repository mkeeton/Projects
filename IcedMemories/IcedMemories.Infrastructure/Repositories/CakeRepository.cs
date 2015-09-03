using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using IcedMemories.Data.Interfaces;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Repositories
{
  public class CakeRepository
  {
    private readonly IDbContext CurrentContext;

    public CakeRepository(IDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public CakeRepository()
    {
      this.CurrentContext = DbContext.Create();
    }

    public void Dispose()
    {

    }

    public virtual Task<IList<Cake>> GetCakesAsync()
    {

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return (IList<Cake>)connection.Query<Cake>("select * FROM app_Cakes", new {}).ToList();
      });
    }

  }
}
