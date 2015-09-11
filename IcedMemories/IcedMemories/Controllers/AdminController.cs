using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using IcedMemories.Infrastructure;
using AutoMapper;

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

          return View(Mapper.Map < IList<IcedMemories.Domain.Models.Cake>, IList<Models.CakeViewModel> > (await WorkManager.CakeManager.GetCakesAsync()));
        }

        [HttpGet]
        public async Task<ActionResult> CakeDetails(Guid id)
        {
          Models.CakeViewModel _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(await WorkManager.CakeManager.LoadAsync(id));
          try
          { 
          _cake.Categories = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection>>(await WorkManager.SearchCategoryManager.GetCategoriesAsync());
          IList<IcedMemories.Domain.Models.SearchCategoryOption> _selectedOptions = await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsForCakeAsync(_cake.Id);
          IList<IcedMemories.Domain.Models.SearchCategorySelection> _selections = await WorkManager.SearchCategorySelectionManager.GetCategorySelectionsForCakeAsync(_cake.Id);
          foreach(Models.SearchCategorySelection _category in _cake.Categories)
          {
            _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
            foreach(IcedMemories.Domain.Models.SearchCategorySelection _selection in _selections)
            {
              foreach(Models.SearchCategoryOptionSelection _option in _category.Options)
              {
                if(_option.Id == _selection.CategoryOptionId)
                {
                  _option.SelectionId = _selection.Id;
                  _option.Selected = true;
                  break;
                }
              }
            }
          }
          }
          catch(Exception ex)
          {

          }
          return PartialView("CakeDetailsPartial", _cake);
        }

        [HttpPost]
        public async Task<ActionResult> CakeDetails(Models.CakeViewModel model)
        {
          IcedMemories.Domain.Models.Cake _cake = await WorkManager.CakeManager.LoadAsync(model.Id);
          if(_cake == null)
          {
            _cake = new Domain.Models.Cake();
          }
          _cake.Title = model.Title;
          _cake.Description = model.Description;
          if (model.ImageUpload != null)
          {
            String _imagePath = "/Images/Cakes/" + Guid.NewGuid().ToString() + ".jpg";
            model.ImageUpload.SaveAs(Server.MapPath(_imagePath));
            _cake.ImageLink = _imagePath;
          }
          await WorkManager.CakeManager.SaveAsync(_cake);
          Models.CakeViewModel _returnCake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(_cake);
          try
          {
            _returnCake.Categories = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection>>(await WorkManager.SearchCategoryManager.GetCategoriesAsync());
            IList<IcedMemories.Domain.Models.SearchCategoryOption> _selectedOptions = await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsForCakeAsync(_cake.Id);
            IList<IcedMemories.Domain.Models.SearchCategorySelection> _selections = await WorkManager.SearchCategorySelectionManager.GetCategorySelectionsForCakeAsync(_cake.Id);
            foreach (Models.SearchCategorySelection _category in _returnCake.Categories)
            {
              _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
              foreach (IcedMemories.Domain.Models.SearchCategorySelection _selection in _selections)
              {
                foreach (Models.SearchCategoryOptionSelection _option in _category.Options)
                {
                  if (_option.Id == _selection.CategoryOptionId)
                  {
                    _option.SelectionId = _selection.Id;
                    _option.Selected = true;
                    break;
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {

          }
          return PartialView("CakeDetailsPartial", _returnCake);
        }
    }
}