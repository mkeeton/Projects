using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcedMemories.Models
{
  public class Gallery
  {

    public IList<CakeViewModel> Cakes { get; set; }
    public IList<SearchCategorySelection> SearchCategories { get; set; }
  }
}