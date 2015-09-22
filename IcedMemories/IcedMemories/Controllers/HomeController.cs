using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using IcedMemories.Infrastructure;
using IcedMemories.Infrastructure.Interfaces;
using AutoMapper;

namespace IcedMemories.Controllers
{
  public class HomeController : Controller
  {

    private IUnitOfWork _unitOfWork;

    public IUnitOfWork WorkManager
    {
      get
      {
        return _unitOfWork ?? HttpContext.GetOwinContext().GetUserManager<IUnitOfWork>();
      }
      set
      {
        _unitOfWork = value;
      }
    }

    public ActionResult Index()
    {
      return View();
    }

    [System.Web.Mvc.Route("~/About")]
    public ActionResult About()
    {
      return View();
    }

    [System.Web.Mvc.Route("~/Contact")]
    public ActionResult Contact()
    {
      ViewBag.Message = "Iced Memories";

      return View();
    }

    [System.Web.Mvc.Route("~/Gallery")]
    public async Task<ActionResult> Gallery(Models.Gallery _gallery)
    {
      ViewBag.Message = "Gallery";
      if(_gallery==null)
      {
        _gallery = new Models.Gallery();
      }
      _gallery.Cakes = Mapper.Map<IList<IcedMemories.Domain.Models.Cake>, IList<Models.CakeViewModel>>(await WorkManager.CakeManager.GetCakesAsync());
      return View(_gallery);
    }
  }
}