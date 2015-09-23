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

    public class AdminController : Controller
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

        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Cakes");
        }

        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Cakes()
        {

          return View(Mapper.Map < IList<IcedMemories.Domain.Models.Cake>, IList<Models.CakeViewModel> > (await WorkManager.CakeManager.GetCakesAsync()));
        }

        [HttpGet]
        public async Task<ActionResult> CakeDetails(Guid? id)
        {
          Models.CakeViewModel _cake;
          if (id.HasValue == true)
          {
            _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(await WorkManager.CakeManager.LoadAsync(id.Value));
          }
          else
          {
            _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(new IcedMemories.Domain.Models.Cake());
            _cake.DateAdded = System.DateTime.Now;
          }

          try
          {
            _cake.Categories = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection>>(await WorkManager.SearchCategoryManager.GetCategoriesAsync());
            IList<IcedMemories.Domain.Models.SearchCategoryOption> _selectedOptions = await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsForCakeAsync(_cake.Id);
            IList<IcedMemories.Domain.Models.SearchCategorySelection> _selections = await WorkManager.SearchCategorySelectionManager.GetCategorySelectionsForCakeAsync(_cake.Id);
            foreach (Models.SearchCategorySelection _category in _cake.Categories)
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
          return View("CakeDetails",_cake);
        }

        [HttpGet]
        public async Task<ActionResult> CakeDetailsPartial(Guid? id)
        {
            Models.CakeViewModel _cake;
            if (id.HasValue == true)
            {
                _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(await WorkManager.CakeManager.LoadAsync(id.Value));
            }
            else
            {
                _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(new IcedMemories.Domain.Models.Cake());
                _cake.DateAdded = System.DateTime.Now;
            }

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
          String _imagePath = "";
          if(_cake == null)
          {
            _cake = new Domain.Models.Cake();
            _cake.DateAdded = System.DateTime.Now;
          }
          _cake.Title = model.Title;
          _cake.Description = model.Description;
          if (model.ImageUpload != null)
          {
            if(_cake.ImageLink=="")
            { 
              _imagePath = "/Images/Cakes/" + _cake.DateAdded.Year.ToString("0000") + "/" + _cake.DateAdded.Month.ToString("00") + "/" + Guid.NewGuid().ToString() + ".jpg";
            }
            else
            {
              _imagePath = _cake.ImageLink;
            }
            if (System.IO.Directory.Exists(Server.MapPath("/Images/Cakes/" + _cake.DateAdded.Year.ToString("0000"))) == false)
            {
              System.IO.Directory.CreateDirectory(Server.MapPath("/Images/Cakes/" + _cake.DateAdded.Year.ToString("0000")));
            }
            if (System.IO.Directory.Exists(Server.MapPath("/Images/Cakes/" + _cake.DateAdded.Year.ToString("0000") + "/" + _cake.DateAdded.Month.ToString("00"))) == false)
            {
              System.IO.Directory.CreateDirectory(Server.MapPath("/Images/Cakes/" + _cake.DateAdded.Year.ToString("0000") + "/" + _cake.DateAdded.Month.ToString("00")));
            }
            try
            {
              if(System.IO.File.Exists(Server.MapPath(_imagePath)))
              {
                System.IO.File.Delete(Server.MapPath(_imagePath));
              }
              model.ImageUpload.SaveAs(Server.MapPath(_imagePath));
              _cake.ImageLink = _imagePath;
            }
            catch(Exception ex)
            { 
            }
          }
          await WorkManager.CakeManager.SaveAsync(_cake);
          foreach(Models.SearchCategorySelection _sCat in model.Categories)
          {
            foreach(Models.SearchCategoryOptionSelection _sOption in _sCat.Options)
            {
              if(_sOption.Selected==true)
              {
                if(_sOption.SelectionId==Guid.Empty)
                {
                  IcedMemories.Domain.Models.SearchCategorySelection _newSelection = new Domain.Models.SearchCategorySelection();
                  _newSelection.CakeId = _cake.Id;
                  _newSelection.CategoryOptionId = _sOption.Id;
                  await WorkManager.SearchCategorySelectionManager.SaveAsync(_newSelection);
                }
              }
              else
              {
                if(_sOption.SelectionId!=Guid.Empty)
                {
                  await WorkManager.SearchCategorySelectionManager.DeleteAsync(_sOption.SelectionId);
                }
              }
            }
          }
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
          //return PartialView("CakeDetailsPartial", _returnCake);
          return RedirectToAction("Cakes");
        }

        public async Task<ActionResult> DeleteCake(Guid? id)
        {
            if(id.HasValue)
            {
              IcedMemories.Domain.Models.Cake _cake = await WorkManager.CakeManager.LoadAsync(id.Value);
              if(_cake != null)
              {
                WorkManager.BeginWork();
                try
                {
                  System.IO.File.Delete(Server.MapPath(_cake.ImageLink));
                  await WorkManager.SearchCategorySelectionManager.DeleteForCakeAsync(_cake.Id);
                  await WorkManager.CakeManager.DeleteAsync(_cake.Id);
                  WorkManager.CommitWork();
                }
                catch(Exception ex)
                { 
                  WorkManager.RollbackWork();
                }
                
                
              }
            }
            return RedirectToAction("Cakes");
        }

        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Categories()
        {

          return View(Mapper.Map < IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection> > (await WorkManager.SearchCategoryManager.GetCategoriesAsync()));
        }

        [HttpGet]
        public async Task<ActionResult> CategoryDetails(Guid? id)
        {
          Models.SearchCategorySelection _category;
          if (id.HasValue)
          {
            _category = Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(await WorkManager.SearchCategoryManager.LoadAsync(id.Value));
          }
          else
          {
            _category = Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(new IcedMemories.Domain.Models.SearchCategory());
          }

          try
          {
            _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
          }
          catch (Exception ex)
          {

          }
          return View("CategoryDetails", _category);
        }

        [HttpGet]
        public async Task<ActionResult> CategoryDetailsPartial(Guid? id)
        {
          Models.SearchCategorySelection _category;
          if(id.HasValue)
          {
            _category= Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(await WorkManager.SearchCategoryManager.LoadAsync(id.Value));
          }
          else
          {
            _category= Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(new IcedMemories.Domain.Models.SearchCategory());
          }
           
          try
          {
            _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
          }
          catch (Exception ex)
          {

          }
          return PartialView("CategoryDetailsPartial",_category);
        }

        [HttpPost]
        public async Task<ActionResult> CategoryDetails(Models.SearchCategorySelection model)
        {
          IcedMemories.Domain.Models.SearchCategory _category = await WorkManager.SearchCategoryManager.LoadAsync(model.Id);
          if (_category == null)
          {
            _category = new Domain.Models.SearchCategory();
          }
          _category.Name = model.Name;
          await WorkManager.SearchCategoryManager.SaveAsync(_category);
          foreach(Models.SearchCategoryOptionSelection _cOption in model.Options)
          {
            IcedMemories.Domain.Models.SearchCategoryOption _option = await WorkManager.SearchCategoryOptionManager.LoadAsync(_cOption.Id);
            if(_option==null)
            {
              _option = new IcedMemories.Domain.Models.SearchCategoryOption();  
            }
            _option.CategoryId = _category.Id;
            _option.Name = _cOption.Name;
            await WorkManager.SearchCategoryOptionManager.SaveAsync(_option);
          }
          Models.SearchCategorySelection _returnCategory = Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(_category);
          try
          { 
            _returnCategory.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await WorkManager.SearchCategoryOptionManager.GetCategoryOptionsAsync(_returnCategory.Id));
          }
          catch(Exception ex)
          {

          }
          return PartialView("CategoryDetailsPartial", _returnCategory);
        }

        public ActionResult Add()
        {
            return PartialView("CategoryOptionsPartial", new IcedMemories.Models.SearchCategoryOptionSelection());
        }
    }
}