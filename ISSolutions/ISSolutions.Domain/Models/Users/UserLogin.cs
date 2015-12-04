using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSolutions.Domain.Models.Users
{
    public class UserLogin
    {

        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public String IPAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ClosedDate { get; set; }
    }
}
