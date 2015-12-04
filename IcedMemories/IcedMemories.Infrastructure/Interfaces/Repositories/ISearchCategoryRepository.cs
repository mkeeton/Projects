using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Interfaces.Repositories
{
    public interface ISearchCategoryRepository
    {
        Task<IList<SearchCategory>> GetCategoriesAsync();
        Task<SearchCategory> LoadAsync(Guid categoryId);
        Task SaveAsync(SearchCategory category);
        Task CreateAsync(SearchCategory category);
        Task UpdateAsync(SearchCategory category);

    }
}
