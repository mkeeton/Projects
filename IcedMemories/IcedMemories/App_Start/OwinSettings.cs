using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.AspNet.Identity.Owin;
using IcedMemories.Data.Interfaces;
using IcedMemories.Infrastructure;
using IcedMemories.Infrastructure.Interfaces;


namespace IcedMemories.App_Start
{
  public class OwinSettings : IDisposable
  {

    public static OwinSettings Create()
    {
      HttpContext.Current.GetOwinContext().Get<IDbContext>().ConnectionString = WebConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString;
      HttpContext.Current.GetOwinContext().Get<IUnitOfWork>().DbContext = HttpContext.Current.GetOwinContext().Get<IDbContext>();
      return new OwinSettings();
    }

    public void Dispose()
    {

    }
  }
}