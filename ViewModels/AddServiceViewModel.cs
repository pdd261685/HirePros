using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.ViewModels

{
    public class AddServiceViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Service Name")]

        public string ServiceName { get; set; }

        public string ServiceDesc { get; set; }

        public double Rate { get; set; }

        //public string Rating { get; set; }

        public AddServiceViewModel()
        {

        }

    }
}
