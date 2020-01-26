using HirePros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.ViewModels

{
    public class ViewUserProfViewModel
    {
        public User User { get; set; }
        public IList<UserProf> Listing { get; set; }

    }
}

