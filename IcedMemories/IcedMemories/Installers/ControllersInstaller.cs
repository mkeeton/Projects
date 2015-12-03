using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web;
using System.Web.Configuration;

namespace IcedMemories.Installers
{
  public class ControllersInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      container.Register(Classes.FromThisAssembly()
          .BasedOn<IController>()
          .LifestyleTransient());
      container.Register(
          Component.For<IcedMemories.Data.Interfaces.IDbContext>()
              .UsingFactoryMethod(_ => IcedMemories.Data.DbContext.DapperAccessDbContext.Create(WebConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString)).LifestylePerWebRequest()
      );

      container.Register(
                Component.For<IcedMemories.Infrastructure.Interfaces.Repositories.ICakeRepository>()
                .ImplementedBy<IcedMemories.Infrastructure.Repositories.Ole.CakeRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IcedMemories.Infrastructure.Interfaces.Repositories.IRoleRepository<IcedMemories.Domain.Models.Role>>()
                .ImplementedBy<IcedMemories.Infrastructure.Repositories.Ole.RoleRepository<IcedMemories.Domain.Models.Role>>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IcedMemories.Infrastructure.Interfaces.Repositories.ISearchCategoryOptionRepository>()
                .ImplementedBy<IcedMemories.Infrastructure.Repositories.Ole.SearchCategoryOptionRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IcedMemories.Infrastructure.Interfaces.Repositories.ISearchCategoryRepository>()
                .ImplementedBy<IcedMemories.Infrastructure.Repositories.Ole.SearchCategoryRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IcedMemories.Infrastructure.Interfaces.Repositories.ISearchCategorySelectionRepository>()
                .ImplementedBy<IcedMemories.Infrastructure.Repositories.Ole.SearchCategorySelectionRepository>()
                .LifeStyle.PerWebRequest
      );

      container.Register(
                Component.For<IcedMemories.Infrastructure.Interfaces.Repositories.IUserRepository<IcedMemories.Domain.Models.User>>()
                .ImplementedBy<IcedMemories.Infrastructure.Repositories.Ole.UserRepository<IcedMemories.Domain.Models.User>>()
                .LifeStyle.PerWebRequest
        // .UsingFactoryMethod(k => IcedMemories.Infrastructure.Repositories.Ole.UserRepository<System.Guid>.Create(k.Resolve<IcedMemories.Data.Interfaces.IDbContext>())).LifestylePerWebRequest()
      );

      container.Register(
                Component.For<IcedMemories.Infrastructure.Interfaces.IUnitOfWork>()
                .ImplementedBy<IcedMemories.Infrastructure.UnitOfWorkOle>()
                .LifeStyle.PerWebRequest
              //.UsingFactoryMethod(k => IcedMemories.Infrastructure.UnitOfWorkOle.Create(k.Resolve<IcedMemories.Data.Interfaces.IDbContext>())).LifestylePerWebRequest()
      );

      container.Register(Component.For<ApplicationUserManager>().UsingFactoryMethod((kernel, creationContext) =>
      {
        var userManager = new ApplicationUserManager(kernel.Resolve<IcedMemories.Infrastructure.Interfaces.Repositories.IUserRepository<System.Guid>>());
        userManager.UserValidator = new Microsoft.AspNet.Identity.UserValidator<IcedMemories.Domain.Models.User,System.Guid>(userManager) { AllowOnlyAlphanumericUserNames = false };
        return userManager;
      }).LifestylePerWebRequest());

      container.Register(Component.For<ApplicationRoleManager>().UsingFactoryMethod((kernel, creationContext) =>
      {
        var roleManager = new ApplicationRoleManager(kernel.Resolve<IcedMemories.Infrastructure.Interfaces.Repositories.IRoleRepository<System.Guid>>());
        return roleManager;
      }).LifestylePerWebRequest());

      container.Register(Component.For()
        .UsingFactoryMethod((kernel, creationContext) =>
        HttpContext.Current.GetOwinContext().Authentication)
        .LifestylePerWebRequest());
    }

  }
}