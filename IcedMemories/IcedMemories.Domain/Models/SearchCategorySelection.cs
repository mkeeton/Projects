using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcedMemories.Domain.Models
{
  public class SearchCategorySelection
  {
    public Guid Id { get; set; }
    public Guid CakeId {  get; set;}
    public Guid CategoryOptionId {  get; set;}
  }
}
