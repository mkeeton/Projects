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

      public AdminController(IUnitOfWork unitOfWork)
      {
        _unitOfWork = unitOfWork;
      }

        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction("Cakes");
        }

        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Cakes()
        {

          return View(Mapper.Map<IList<IcedMemories.Domain.Models.Cake>, IList<Models.CakeViewModel>>(await _unitOfWork.CakeManager.GetCakesAsync()));
        }

        [HttpGet]
        public async Task<ActionResult> CakeDetails(Guid? id)
        {
          Models.CakeViewModel _cake;
          if (id.HasValue == true)
          {
            _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(await _unitOfWork.CakeManager.LoadAsync(id.Value));
          }
          else
          {
            _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(new IcedMemories.Domain.Models.Cake());
            _cake.DateAdded = System.DateTime.Now;
          }

          try
          {
            _cake.Categories = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection>>(await _unitOfWork.SearchCategoryManager.GetCategoriesAsync());
            IList<IcedMemories.Domain.Models.SearchCategoryOption> _selectedOptions = await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsForCakeAsync(_cake.Id);
            IList<IcedMemories.Domain.Models.SearchCategorySelection> _selections = await _unitOfWork.SearchCategorySelectionManager.GetCategorySelectionsForCakeAsync(_cake.Id);
            foreach (Models.SearchCategorySelection _category in _cake.Categories)
            {
              _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
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
              _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(await _unitOfWork.CakeManager.LoadAsync(id.Value));
            }
            else
            {
                _cake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(new IcedMemories.Domain.Models.Cake());
                _cake.DateAdded = System.DateTime.Now;
            }

          try
          {
            _cake.Categories = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection>>(await _unitOfWork.SearchCategoryManager.GetCategoriesAsync());
            IList<IcedMemories.Domain.Models.SearchCategoryOption> _selectedOptions = await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsForCakeAsync(_cake.Id);
            IList<IcedMemories.Domain.Models.SearchCategorySelection> _selections = await _unitOfWork.SearchCategorySelectionManager.GetCategorySelectionsForCakeAsync(_cake.Id);
          foreach(Models.SearchCategorySelection _category in _cake.Categories)
          {
            _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
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
          IcedMemories.Domain.Models.Cake _cake = await _unitOfWork.CakeManager.LoadAsync(model.Id);
          String _imagePath = "";
          String _imageExtension = "";
          if(_cake == null)
          {
            _cake = new Domain.Models.Cake();
            
          }
          if (_cake.Id == Guid.Empty)
          {
            _cake.DateAdded = System.DateTime.Now;
          }
          _cake.Title = model.Title;
          _cake.Description = model.Description;
          if (model.ImageUpload != null)
          {
            if (model.ImageUpload.FileName.LastIndexOf(".") > -1)
            { 
              try
              {
                _imageExtension = model.ImageUpload.FileName.Substring(model.ImageUpload.FileName.LastIndexOf("."));
                if ((_cake.ImageLink == null) || (_cake.ImageLink.Trim() == "") || (_cake.ImageLink.LastIndexOf(".")==-1))
                {
                  _imagePath = "/Images/Cakes/" + _cake.DateAdded.Year.ToString("0000") + "/" + _cake.DateAdded.Month.ToString("00") + "/" + Guid.NewGuid().ToString() + _imageExtension;
                }
                else
                {
                  _imagePath = _cake.ImageLink.Substring(0, _cake.ImageLink.LastIndexOf(".")) + _imageExtension;
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
              catch(Exception exUpload)
              {
              }
            }
          }
          else
          {
          }
          await _unitOfWork.CakeManager.SaveAsync(_cake);
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
                  await _unitOfWork.SearchCategorySelectionManager.SaveAsync(_newSelection);
                }
              }
              else
              {
                if(_sOption.SelectionId!=Guid.Empty)
                {
                  await _unitOfWork.SearchCategorySelectionManager.DeleteAsync(_sOption.SelectionId);
                }
              }
            }
          }
          Models.CakeViewModel _returnCake = Mapper.Map<IcedMemories.Domain.Models.Cake, Models.CakeViewModel>(_cake);
          try
          {
            _returnCake.Categories = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection>>(await _unitOfWork.SearchCategoryManager.GetCategoriesAsync());
            IList<IcedMemories.Domain.Models.SearchCategoryOption> _selectedOptions = await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsForCakeAsync(_cake.Id);
            IList<IcedMemories.Domain.Models.SearchCategorySelection> _selections = await _unitOfWork.SearchCategorySelectionManager.GetCategorySelectionsForCakeAsync(_cake.Id);
            foreach (Models.SearchCategorySelection _category in _returnCake.Categories)
            {
              _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
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
              IcedMemories.Domain.Models.Cake _cake = await _unitOfWork.CakeManager.LoadAsync(id.Value);
              if(_cake != null)
              {
                _unitOfWork.BeginWork();
                try
                {
                  try
                  { 
                    System.IO.File.Delete(Server.MapPath(_cake.ImageLink));
                  }
                  catch(Exception exImage)
                  { }
                  await _unitOfWork.SearchCategorySelectionManager.DeleteForCakeAsync(_cake.Id);
                  await _unitOfWork.CakeManager.DeleteAsync(_cake.Id);
                  _unitOfWork.CommitWork();
                }
                catch(Exception ex)
                { 
                  _unitOfWork.RollbackWork();
                }
                
                
              }
            }
            return RedirectToAction("Cakes");
        }

        [Authorize(Roles="Admin")]
        public async Task<ActionResult> Categories()
        {

          return View(Mapper.Map < IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection> > (await _unitOfWork.SearchCategoryManager.GetCategoriesAsync()));
        }

        [HttpGet]
        public async Task<ActionResult> CategoryDetails(Guid? id)
        {
          Models.SearchCategorySelection _category;
          if (id.HasValue)
          {
            _category = Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(await _unitOfWork.SearchCategoryManager.LoadAsync(id.Value));
          }
          else
          {
            _category = Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(new IcedMemories.Domain.Models.SearchCategory());
          }

          try
          {
            _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
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
            _category= Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(await _unitOfWork.SearchCategoryManager.LoadAsync(id.Value));
          }
          else
          {
            _category= Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(new IcedMemories.Domain.Models.SearchCategory());
          }
           
          try
          {
            _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
          }
          catch (Exception ex)
          {

          }
          return PartialView("CategoryDetailsPartial",_category);
        }

        [HttpPost]
        public async Task<ActionResult> CategoryDetails(Models.SearchCategorySelection model)
        {
          IcedMemories.Domain.Models.SearchCategory _category = await _unitOfWork.SearchCategoryManager.LoadAsync(model.Id);
          if (_category == null)
          {
            _category = new Domain.Models.SearchCategory();
          }
          _category.Name = model.Name;
          await _unitOfWork.SearchCategoryManager.SaveAsync(_category);
          foreach(Models.SearchCategoryOptionSelection _cOption in model.Options)
          {
            IcedMemories.Domain.Models.SearchCategoryOption _option = await _unitOfWork.SearchCategoryOptionManager.LoadAsync(_cOption.Id);
            if(_option==null)
            {
              _option = new IcedMemories.Domain.Models.SearchCategoryOption();  
            }
            _option.CategoryId = _category.Id;
            _option.Name = _cOption.Name;
            await _unitOfWork.SearchCategoryOptionManager.SaveAsync(_option);
          }
          Models.SearchCategorySelection _returnCategory = Mapper.Map<IcedMemories.Domain.Models.SearchCategory, Models.SearchCategorySelection>(_category);
          try
          { 
            _returnCategory.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsAsync(_returnCategory.Id));
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