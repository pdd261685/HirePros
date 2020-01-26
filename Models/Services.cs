using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.Models
{
    //to create a new persistence class that contains service that pofessionals cater to
    public class Services
    {
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDesc { get; set; }
        public double Rate { get; set; }

        public string Rating { get; set; }

        public IList<Professional> Professionals { get; set; }// to refernce to all professionals that cater to specific service

        public Services()
        {

        }
    }
}

