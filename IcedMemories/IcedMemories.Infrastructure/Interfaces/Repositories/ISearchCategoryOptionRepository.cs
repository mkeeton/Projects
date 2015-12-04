using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Interfaces.Repositories
{
    public interface ISearchCategoryOptionRepository
    {
        Task<IList<SearchCategoryOption>> GetCategoryOptionsAsync(Guid categoryId);
        Task<IList<SearchCategoryOption>> GetCategoryOptionsForCakeAsync(Guid cakeId);
        Task<IList<SearchCategoryOption>> GetCategoryOptionsForCakeAndOptionAsync(Guid cakeId, Guid categoryOptionId);
        Task<SearchCategoryOption> LoadAsync(Guid categoryOptionId);
        Task SaveAsync(SearchCategoryOption categoryOption);
        Task CreateAsync(SearchCategoryOption categoryOption);
        Task UpdateAsync(SearchCategoryOption categoryOption);

    }
}
