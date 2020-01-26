using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public IList<UserProf> UserProf { get; set; } = new List<UserProf>();
        //to store the relationship between Users shortlisting various professionals

        public User()
        {

        }
    }
}
