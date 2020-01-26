using Microsoft.AspNetCore.Mvc.Rendering;
using HirePros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.ViewModels
{
    public class AddUserProfViewModel
    {

        public User User { get; set; }
        public List<SelectListItem> Professionals { get; set; }

        public int UserID { get; set; }
        public int ProfessionalID { get; set; }

        public AddUserProfViewModel() { }

        public AddUserProfViewModel(User user, IEnumerable<Professional> profs)
        {
            Professionals = new List<SelectListItem>();

            foreach (var pro in profs)
            {
                Professionals.Add(new SelectListItem
                {
                    Value = pro.ID.ToString(),
                    Text = pro.Name
                });
            }
            User = user;
        }


    }
}

