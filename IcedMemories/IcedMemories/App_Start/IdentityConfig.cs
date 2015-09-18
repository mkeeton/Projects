using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Security.Claims;
using IcedMemories.Data.Interfaces;
using IcedMemories.Domain.Models;
using IcedMemories.Infrastructure;
using IcedMemories.Infrastructure.Interfaces;

namespace IcedMemories
{
  // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

  public class ApplicationUserManager : UserManager<User, System.Guid>
  {
    public ApplicationUserManager(IUserStore<User, System.Guid> store)
      : base(store)
    {
    }

    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user)
    {
      // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
      var userIdentity = await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
      // Add custom user claims here
      return userIdentity;
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {
      var manager = new ApplicationUserManager(context.Get<IUnitOfWork>().UserManager);
      // Configure validation logic for usernames
      manager.UserValidator = new UserValidator<User, System.Guid>(manager)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };
      // Configure validation logic for passwords
      manager.PasswordValidator = new PasswordValidator
      {
        RequiredLength = 6,
        RequireNonLetterOrDigit = false,
        RequireDigit = false,
        RequireLowercase = false,
        RequireUppercase = false,
      };
      // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
      // You can write your own provider and plug in here.
      manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<User, System.Guid>
      {
        MessageFormat = "Your security code is: {0}"
      });
      manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<User, System.Guid>
      {
        Subject = "Security Code",
        BodyFormat = "Your security code is: {0}"
      });
      manager.EmailService = new EmailService();
      manager.SmsService = new SmsService();
      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider = new DataProtectorTokenProvider<User, System.Guid>(dataProtectionProvider.Create("ASP.NET Identity"));
      }
      return manager;
    }
  }

  public class ApplicationRoleManager : RoleManager<Role, System.Guid>
  {
    public ApplicationRoleManager(IRoleStore<Role, System.Guid> store)
      : base(store)
    {
    }

    //public async Task<ClaimsIdentity> GenerateUserIdentityAsync(User user)
    //{
    //  // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
    //  var userIdentity = await CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
    //  // Add custom user claims here
    //  return userIdentity;
    //}

    public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
    {
      var manager = new ApplicationRoleManager(context.Get<IUnitOfWork>().RoleManager);
      // Configure validation logic for usernames
      return manager;
    }
  }

  public class EmailService : IIdentityMessageService
  {
    public Task SendAsync(IdentityMessage message)
    {
      // Plug in your email service here to send an email.
      return Task.FromResult(0);
    }
  }

  public class SmsService : IIdentityMessageService
  {
    public Task SendAsync(IdentityMessage message)
    {
      // Plug in your sms service here to send a text message.
      return Task.FromResult(0);
    }
  }
}
