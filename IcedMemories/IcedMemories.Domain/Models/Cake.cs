using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcedMemories.Domain.Models
{
  public class Cake
  {
    public Guid Id { get; set;}
    public DateTime DateAdded {get; set;}
    public String Title { get; set;}
    public String Description { get; set;}
    public String ImageLink { get; set; }
    
  }
}
