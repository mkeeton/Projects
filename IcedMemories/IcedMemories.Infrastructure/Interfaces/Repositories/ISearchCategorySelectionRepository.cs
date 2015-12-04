using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Interfaces.Repositories
{
    public interface ISearchCategorySelectionRepository
    {
        Task<IList<SearchCategorySelection>> GetCategorySelectionsAsync();
        Task<IList<SearchCategorySelection>> GetCategorySelectionsForCakeAsync(Guid cakeId);
        Task<SearchCategorySelection> LoadAsync(Guid categorySelectionId);
        Task SaveAsync(SearchCategorySelection categorySelection);
        Task CreateAsync(SearchCategorySelection categorySelection);
        Task UpdateAsync(SearchCategorySelection categorySelection);
        Task DeleteAsync(Guid categorySelectionId);
        Task DeleteForCakeAsync(Guid cakeId);
    }
}
