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

    public HomeController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
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
      if(_gallery.SearchCategories==null)
      {
        _gallery.SearchCategories = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategory>, IList<Models.SearchCategorySelection>>(await _unitOfWork.SearchCategoryManager.GetCategoriesAsync());
        foreach (Models.SearchCategorySelection _category in _gallery.SearchCategories)
        {
          _category.Options = Mapper.Map<IList<IcedMemories.Domain.Models.SearchCategoryOption>, IList<Models.SearchCategoryOptionSelection>>(await _unitOfWork.SearchCategoryOptionManager.GetCategoryOptionsAsync(_category.Id));
        }
      }
      System.Collections.Generic.List<IcedMemories.Domain.Models.SearchCategoryOption> _searchCategoryOptions = new List<IcedMemories.Domain.Models.SearchCategoryOption>();
      IcedMemories.Domain.Models.SearchCategoryOption _searchOption;
      foreach(Models.SearchCategorySelection _selectedCategory in _gallery.SearchCategories)
      {
        foreach(Models.SearchCategoryOptionSelection _selectedOption in _selectedCategory.Options)
        {
          if(_selectedOption.Selected == true)
          {
            _searchOption = new Domain.Models.SearchCategoryOption();
            _searchOption.Id = _selectedOption.Id;
            _searchOption.CategoryId = _selectedCategory.Id;
            _searchOption.Name = _selectedOption.Name;
            _searchCategoryOptions.Add(_searchOption);
          }
        }
      }
      _gallery.Cakes = Mapper.Map<IList<IcedMemories.Domain.Models.Cake>, IList<Models.CakeViewModel>>(await _unitOfWork.CakeManager.GetCakesAsync(_searchCategoryOptions));
      return View(_gallery);
    }
  }
}