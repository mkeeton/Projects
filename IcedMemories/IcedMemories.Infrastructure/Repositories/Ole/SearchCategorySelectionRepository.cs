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

namespace IcedMemories.Infrastructure.Repositories.Ole
{
    public class SearchCategorySelectionRepository : ISearchCategorySelectionRepository
    {
        private readonly IDbContext CurrentContext;

        public SearchCategorySelectionRepository(IDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("connectionString");

            this.CurrentContext = context;
        }

        public void Dispose()
        {

        }

        public virtual Task<IList<SearchCategorySelection>> GetCategorySelectionsAsync()
        {

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    return (IList<SearchCategorySelection>)connection.Query<SearchCategorySelection>("select * FROM app_SearchCategorySelections", new { }).ToList();
            });
        }

        public virtual Task<IList<SearchCategorySelection>> GetCategorySelectionsForCakeAsync(Guid cakeId)
        {

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    return (IList<SearchCategorySelection>)connection.Query<SearchCategorySelection>("select * FROM app_SearchCategorySelections WHERE CakeId=@CakeId", new { CakeId = cakeId }).ToList();
            });
        }

        public virtual Task<SearchCategorySelection> LoadAsync(Guid categorySelectionId)
        {
            if (categorySelectionId == Guid.Empty)
                throw new ArgumentNullException("categorySelectionId");

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    return connection.Query<SearchCategorySelection>("select * from app_SearchCategorySelections where Id = @Id", new { Id = categorySelectionId }).SingleOrDefault();
            });
        }

        public Task SaveAsync(SearchCategorySelection categorySelection)
        {
            if (categorySelection.Id == Guid.Empty)
            {
                return CreateAsync(categorySelection);
            }
            else
            {
                return UpdateAsync(categorySelection);
            }
        }

        public Task CreateAsync(SearchCategorySelection categorySelection)
        {
            if (categorySelection == null)
                throw new ArgumentNullException("categorySelection");

            return Task.Factory.StartNew(() =>
            {
                categorySelection.Id = Guid.NewGuid();
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    connection.Execute("insert into app_SearchCategorySelections(Id, CakeId, CategoryOptionId) values(@Id, @CakeId, @CategoryOptionId)", new {Id = categorySelection.Id, cakeId = categorySelection.CakeId, CategoryOptionId = categorySelection.CategoryOptionId});
            });
        }

        public Task UpdateAsync(SearchCategorySelection categorySelection)
        {
            if (categorySelection == null)
                throw new ArgumentNullException("categorySelection");

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                  connection.Execute("update app_SearchCategorySelections SET CakeId=@CakeId, CategoryOptionId=@CategoryOptionId where Id = @Id", new { cakeId = categorySelection.CakeId, CategoryOptionId = categorySelection.CategoryOptionId, Id = categorySelection.Id });
            });
        }

        public Task DeleteAsync(Guid categorySelectionId)
        {
            if (categorySelectionId == Guid.Empty)
                throw new ArgumentNullException("categorySelectionId");


            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    connection.Execute("delete from app_SearchCategorySelections WHERE Id=@Id", new { Id = categorySelectionId });
            });
        }

        public Task DeleteForCakeAsync(Guid cakeId)
        {
          if (cakeId == Guid.Empty)
            throw new ArgumentNullException("cakeId");


          return Task.Factory.StartNew(() =>
          {
            using (IDbConnection connection = CurrentContext.OpenConnection())
              connection.Execute("delete from app_SearchCategorySelections WHERE CakeId=@CakeId", new { CakeId = cakeId });
          });
        }
    }
}
