using Microsoft.AspNetCore.Mvc.Rendering;
using HirePros.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.ViewModels
{
    public class AddProfessionalViewModel
    {
        [Required]
        [Display(Name = "Company/Professional Name")]
        public string Name { get; set; }

        [StringLength(30, MinimumLength = 5, ErrorMessage = "Enter between 5-30 characters")]
        public string Description { get; set; }

        [Required]
        public string Email { get; set; }

        public int ServicesID { get; set; }

        public List<SelectListItem> Services { get; set; }

        public AddProfessionalViewModel(IEnumerable<Services> services)
        {
            Services = new List<SelectListItem>();

            foreach (Services Serv in services)
            {
                Services.Add(new SelectListItem()
                {
                    Value = (Serv.ID).ToString(),
                    Text = Serv.ServiceName
                });
            }
        }

        public AddProfessionalViewModel()
        {

        }

    }

}

