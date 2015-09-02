using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IcedMemories.Data.Interfaces;
using IcedMemories.Data.DbContext;

namespace IcedMemories.Infrastructure.Repositories
{
  public class DbContext
  {

    public static IDbContext Create()
    {
      return new DapperDbContext();
    }
  }
}
