using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Domain.Models;

namespace IcedMemories.Infrastructure.Interfaces.Repositories
{
    public interface ICakeRepository
    {
        Task<IList<Cake>> GetCakesAsync();
        Task<IList<Cake>> GetCakesAsync(System.Collections.Generic.IList<IcedMemories.Domain.Models.SearchCategoryOption> _categoryOptions);
        Task<Cake> LoadAsync(Guid cakeId);
        Task SaveAsync(Cake cake);
        Task CreateAsync(Cake cake);
        Task UpdateAsync(Cake cake);
        Task DeleteAsync(Guid cakeId);
    }
}
