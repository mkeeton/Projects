using System;
using System.Collections.Generic;
using ISSolutions.Domain.Models.SessionState;
using System.Linq;
using System.Threading.Tasks;

namespace ISSolutions.Domain.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        IQueryable<Session> Sessions { get; }

        Task<Session> FindAsync(Guid ID);

        Task AddAsync(Session session);

        Task RemoveAsync(Guid ID);

        Task UpdateAsync(Session session);

        Task<IEnumerable<Session>> GetSessions(Session.SearchParameters Params);
    }
}
