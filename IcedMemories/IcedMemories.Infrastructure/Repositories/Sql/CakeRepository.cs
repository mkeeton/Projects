using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using IcedMemories.Data.Interfaces;
using IcedMemories.Domain.Models;
using IcedMemories.Infrastructure.Interfaces.Repositories;

namespace IcedMemories.Infrastructure.Repositories.Sql
{
  public class CakeRepository : ICakeRepository
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

    public virtual Task<IList<Cake>> GetCakesAsync(System.Collections.Generic.IList<IcedMemories.Domain.Models.SearchCategoryOption> _categoryOptions)
    {
      IEnumerable<IcedMemories.Domain.Models.SearchCategoryOption> _sortedEnum = _categoryOptions.OrderBy(f => f.CategoryId);
      IList<IcedMemories.Domain.Models.SearchCategoryOption> _sortedList = _sortedEnum.ToList();
      StringBuilder _sql = new StringBuilder("app_Cakes C");
      string _sqlSection = "";
      Guid _catId = new Guid();
      int _catIndex = 0;
      foreach (IcedMemories.Domain.Models.SearchCategoryOption _option in _categoryOptions)
      {
        if (_option.CategoryId != _catId)
        {
          _catId = _option.CategoryId;
          _catIndex += 1;
          if (_sqlSection != "")
          {
            _sqlSection += "))";
            _sql.Insert(0, "(");
            _sql.Append(_sqlSection);
          }
          _sqlSection = String.Format(" INNER JOIN app_SearchCategorySelections SCS{0} ON SCS{0}.CakeId=C.Id AND SCS{0}.CategoryOptionId IN ('{1}'", _catIndex, _option.Id);
        }
        else
        {
          _sqlSection += String.Format(",'{0}'", _option.Id);
        }
      }
      if (_sqlSection != "")
      {
        _sqlSection += "))";
        _sql.Insert(0, "(");
        _sql.Append(_sqlSection);
      }
      _sql.Insert(0, "select DISTINCT C.* FROM ");
      _sql.Append(" ORDER BY C.DateAdded DESC");
      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return (IList<Cake>)connection.Query<Cake>(_sql.ToString(), new { }).ToList();
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

    public virtual Task DeleteAsync(Guid cakeId)
    {
      if (cakeId == Guid.Empty)
      {
        throw new ArgumentNullException("cakeId");
      }
      else
      {

        return Task.Factory.StartNew(() =>
        {
          using (IDbConnection connection = CurrentContext.OpenConnection())
            connection.Execute("Delete from app_Cakes where Id = @Id", new { Id = cakeId });
        });
      }
    }

  }
}
