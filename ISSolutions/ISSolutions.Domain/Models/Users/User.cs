using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSolutions.Domain.Models.Users
{
    public class User
    {

        public Guid ID { get; set;}
        public String Username { get; set; }
        public String EmailAddress { get; set; }
        public String Password { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
