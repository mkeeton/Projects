using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISSolutions.Domain.Models.SessionState
{
    public class Session
    {

        public Guid ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public String ClientIP { get; set; }


        public class SearchParameters
        {
            public Guid ID { get; set; }
            public string IPAddress { get; set; }
            public DateTime? CreatedDate { get; set; }

            public SearchParameters()
            {
                ID = new Guid();
                IPAddress = "";
                CreatedDate = null;
            }
        }
    }
}
