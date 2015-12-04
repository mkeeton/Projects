using System;
using System.Collections.Generic;
using ISSolutions.Domain.Interfaces.Repositories;
using ISSolutions.Domain.Models.SessionState;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Profiling;

namespace ISSolutions.Infrastructure.Repositories.EntityFramework
{
    public class SessionRepository : ISessionRepository
    {
        public IQueryable<Session> Sessions
        {
            get
            {
                using (var db = new ISSolutionsDbContext())
                    return db.Sessions;
            }
        }

        public async Task<Session> FindAsync(Guid ID)
        {
            using (var db = new ISSolutionsDbContext())
                return await db.Sessions.FindAsync(ID);
        }

        public async Task AddAsync(Session session)
        {
            using (var db = new ISSolutionsDbContext())
            {
                db.Sessions.Add(session);
                await db.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(Guid ID)
        {
            using (var db = new ISSolutionsDbContext())
            {
                var session = await db.Sessions.FindAsync(ID);
                db.Sessions.Remove(session);
                await db.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Session session)
        {
            using (var db = new ISSolutionsDbContext())
            {
                db.Entry(session).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Session>> GetSessions(Session.SearchParameters Params)
        {
            using (var db = new ISSolutionsDbContext())
            {
                // Only access Tasks created by current user
                var tasks = db.Sessions.Where(t => t.ID == Params.ID);

                //if (!string.IsNullOrEmpty(q) && q != "undefined")
                //{
                //    tasks = tasks.Where(t => t.Description.Contains(q));
                //}

                //if (0 < offset)
                //{
                //    tasks = tasks.Skip(offset);
                //}

                //if (limit.HasValue)
                //{
                //    tasks = tasks.Take(limit.Value);
                //}

                using (MiniProfiler.Current.Step("Running query on DB"))
                    return await Sessions.ToListAsync();
            }
        }
    }
}
