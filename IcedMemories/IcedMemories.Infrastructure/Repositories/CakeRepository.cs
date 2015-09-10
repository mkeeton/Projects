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

    public virtual Task<Cake> LoadAsync(Guid cakeId)
    {
      if (cakeId == Guid.Empty)
        throw new ArgumentNullException("cakeId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<Cake>("select * from app_Cakes where Id = @Id", new { Id = cakeId }).SingleOrDefault();
      });
    }

    public Task SaveAsync(Cake cake)
    {
      if(cake.Id==Guid.Empty)
      {
        return CreateAsync(cake);
      }
      else
      {
        return UpdateAsync(cake);
      }
    }

    public Task CreateAsync(Cake cake)
    {
      if (cake == null)
        throw new ArgumentNullException("cake");

      return Task.Factory.StartNew(() =>
      {
        cake.Id = Guid.NewGuid();
        cake.DateAdded = System.DateTime.Now;
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("insert into app_Cakes(Id, DateAdded, Title, Description, ImageLink) values(@Id, @DateAdded, @Title, @Description, @ImageLink)", cake);
      });
    }

    public Task UpdateAsync(Cake cake)
    {
      if (cake == null)
        throw new ArgumentNullException("cake");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("update app_Cakes SET Title=@Title, Description=@Description, ImageLink=@ImageLink where Id = @Id", cake);
      });
    }

  }
}
