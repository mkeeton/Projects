using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using IcedMemories.Infrastructure;

namespace IcedMemories.Controllers
{

    public class AdminController : Controller
    {

      private UnitOfWork _unitOfWork;

        public UnitOfWork WorkManager
        {
          get
          {
            return _unitOfWork ?? HttpContext.GetOwinContext().GetUserManager<UnitOfWork>();
          }
          set
          {
            _unitOfWork = value;
          }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Cakes()
        {
          return View(await WorkManager.CakeManager.GetCakesAsync());
        }
    }
}