using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.Models

{
    //Intermediary/Joint class between User class and Professional class, creates joint table in database
    public class UserProf
    {
        public int UserID { get; set; }
        public User User { get; set; }

        public int ProfessionalID { get; set; }
        public Professional Professional { get; set; }

        //the 2 IDs above are pair of IDs that will facilitate the joint behavior
    }
}
