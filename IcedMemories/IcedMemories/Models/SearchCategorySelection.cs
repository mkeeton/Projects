using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcedMemories.Models
{
  public class SearchCategorySelection
  {
    public Guid Id { get; set;}
    public String Name { get; set;}
    public IList<SearchCategoryOptionSelection> Options { get; set;}
  }
}