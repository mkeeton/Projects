﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IcedMemories.Domain.Models
{
  public class SearchCategoryOption
  {
    public Guid Id { get; set; }
    public Guid CategoryId {  get; set;}
    public string Name { get; set; }

  }
}
