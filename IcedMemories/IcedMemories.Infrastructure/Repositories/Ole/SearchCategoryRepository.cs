﻿using System;
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
    public class SearchCategoryRepository : ISearchCategoryRepository
    {
        private readonly IDbContext CurrentContext;

        public SearchCategoryRepository(IDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("connectionString");

            this.CurrentContext = context;
        }

        public void Dispose()
        {

        }

        public virtual Task<IList<SearchCategory>> GetCategoriesAsync()
        {

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    return (IList<SearchCategory>)connection.Query<SearchCategory>("select * FROM app_SearchCategories ORDER BY Name", new { }).ToList();
            });
        }

        public virtual Task<SearchCategory> LoadAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
              return Task.Factory.StartNew(() =>
              {
                return new SearchCategory();
              });
            }

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    return connection.Query<SearchCategory>("select * from app_SearchCategories where Id = @Id", new { Id = categoryId }).SingleOrDefault();
            });
        }

        public Task SaveAsync(SearchCategory category)
        {
            if (category.Id == Guid.Empty)
            {
                return CreateAsync(category);
            }
            else
            {
                return UpdateAsync(category);
            }
        }

        public Task CreateAsync(SearchCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            return Task.Factory.StartNew(() =>
            {
                category.Id = Guid.NewGuid();
                using (IDbConnection connection = CurrentContext.OpenConnection())
                    connection.Execute("insert into app_SearchCategories(Id, Name) values(@Id, @Name)", new{Id = category.Id, Name = category.Name});
            });
        }

        public Task UpdateAsync(SearchCategory category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            return Task.Factory.StartNew(() =>
            {
                using (IDbConnection connection = CurrentContext.OpenConnection())
                  connection.Execute("update app_SearchCategories SET Name=@Name where Id = @Id", new { Name = category.Name, Id = category.Id });
            });
        }
    }
}
