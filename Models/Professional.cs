using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.Models
{
    public class Professional
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string Email { get; set; }

        public Services Services { get; set; }// navigation property


        public int ServicesID { get; set; }//foreign key to refer to the Service IDs

        public List<UserProf> UserProf { get; set; }// intermediary classes for many-tomany realtions with User class

        public Professional()
        {

        }

    }
}

