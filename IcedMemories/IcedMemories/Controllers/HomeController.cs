using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IcedMemories.Controllers
{
  public class HomeController : Controller
  {
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
  }
}