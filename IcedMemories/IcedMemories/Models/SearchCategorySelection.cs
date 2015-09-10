using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcedMemories.Models
{
  public class SearchCategorySelection
  {
    Guid Id { get; set;}
    String Name { get; set;}
    IList<SearchCategoryOptionSelection> Options { get; set;}
  }
}