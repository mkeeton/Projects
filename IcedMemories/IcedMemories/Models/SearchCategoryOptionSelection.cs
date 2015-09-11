using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IcedMemories.Models
{
  public class SearchCategoryOptionSelection
  {
    public Guid Id { get; set;}
    public String Name { get; set;}
    public bool Selected { get; set;}
    public Guid SelectionId { get; set;}
  }
}