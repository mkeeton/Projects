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
  public class SearchCategoryOptionRepository
  {
      private readonly IDbContext CurrentContext;

    public SearchCategoryOptionRepository(IDbContext context)
    {
      if (context == null)
        throw new ArgumentNullException("connectionString");

      this.CurrentContext = context;
    }

    public SearchCategoryOptionRepository()
    {
      this.CurrentContext = DbContext.Create();
    }

    public void Dispose()
    {

    }

    public virtual Task<IList<SearchCategoryOption>> GetCategoryOptionsAsync(Guid categoryId)
    {

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return (IList<SearchCategoryOption>)connection.Query<SearchCategoryOption>("select * FROM app_SearchCategoryOptions WHERE CategoryId=@CategoryId ORDER BY Name", new { Categoryid=categoryId}).ToList();
      });
    }

    public virtual Task<IList<SearchCategoryOption>> GetCategoryOptionsForCakeAsync(Guid cakeId)
    {

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return (IList<SearchCategoryOption>)connection.Query<SearchCategoryOption>("select SCO.* FROM app_SearchCategoryOptions SCO INNER JOIN app_SearchCategorySelections SCS ON SCS.CategoryOptionId=SCO.Id WHERE SCS.CakeId=@CakeId", new { CakeId=cakeId}).ToList();
      });
    }

    public virtual Task<IList<SearchCategoryOption>> GetCategoryOptionsForCakeAndOptionAsync(Guid cakeId, Guid categoryOptionId)
    {

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return (IList<SearchCategoryOption>)connection.Query<SearchCategoryOption>("select SCO.* FROM app_SearchCategoryOptions SCO INNER JOIN app_SearchCategorySelections SCS ON SCS.CategoryOptionId=SCO.Id WHERE SCS.CakeId=@CakeId AND SCO.Id=@CategoryOptionId", new { CakeId = cakeId, CategoryOptionId=categoryOptionId }).ToList();
      });
    }

    public virtual Task<SearchCategoryOption> LoadAsync(Guid categoryOptionId)
    {
      if (categoryOptionId == Guid.Empty)
        throw new ArgumentNullException("categoryOptionId");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          return connection.Query<SearchCategoryOption>("select * from app_SearchCategoryOptions where Id = @Id", new { Id = categoryOptionId }).SingleOrDefault();
      });
    }

    public Task SaveAsync(SearchCategoryOption categoryOption)
    {
      if (categoryOption.Id == Guid.Empty)
      {
        return CreateAsync(categoryOption);
      }
      else
      {
        return UpdateAsync(categoryOption);
      }
    }

    public Task CreateAsync(SearchCategoryOption categoryOption)
    {
      if (categoryOption == null)
        throw new ArgumentNullException("categoryOption");

      return Task.Factory.StartNew(() =>
      {
        categoryOption.Id = Guid.NewGuid();
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("insert into app_SearchCategoryOptions(Id, CategoryId, Name) values(@Id, @CategoryId, @Name)", categoryOption);
      });
    }

    public Task UpdateAsync(SearchCategoryOption categoryOption)
    {
      if (categoryOption == null)
        throw new ArgumentNullException("categoryOption");

      return Task.Factory.StartNew(() =>
      {
        using (IDbConnection connection = CurrentContext.OpenConnection())
          connection.Execute("update app_SearchCategoryOptions SET CategoryId=@CategoryId, Name=@Name where Id = @Id", categoryOption);
      });
    }
  }
}
