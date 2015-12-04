using System.Data.Entity;

namespace ISSolutions.Infrastructure.Repositories.EntityFramework
{
    public class ISSolutionsDbContext : DbContext
    {
        public ISSolutionsDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<ISSolutions.Domain.Models.SessionState.Session> Sessions { get; set; }
        public DbSet<ISSolutions.Domain.Models.Users.User> Users { get; set; }
        public DbSet<ISSolutions.Domain.Models.Users.UserLogin> UserLogins { get; set; }
    }

    public class ISSolutionsDbContextInitialiser : DropCreateDatabaseIfModelChanges<ISSolutionsDbContext>
    {
        protected override void Seed(ISSolutionsDbContext context)
        {


            base.Seed(context);
        }
    }
}
