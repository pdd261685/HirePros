using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HirePros.Data;
using HirePros.Models;
using HirePros.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HirePros.Controllers
{
    public class ProfessionalController : Controller
    {
        private readonly ServiceDbContext context;// private so only accessed by this class ; this is a way to reference the objects
        //refernce used to interface with the database
        public ProfessionalController(ServiceDbContext dbContext)//overload to reference the private field context
        {
            context = dbContext;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserName") == "Admin")
            {
                IList<Professional> professionals = context.Professionals.Include(p => p.Services).ToList();
                //accessed by context as Professionals ia a dbset that holds on to Professional class objects and turn that dbset into a list


                return View(professionals);
            }

            return Redirect("User/Index?username="+ HttpContext.Session.GetString("UserName"));
            //return Redirect("Home");
            
        }

        public IActionResult Add()
        {
            //Viemodel will take the list of categories and create a select list with the below
            if (HttpContext.Session.GetString("UserName") == "Admin")
            {
                AddProfessionalViewModel addProfessionalViewModel = new AddProfessionalViewModel(context.Services.ToList());
                ViewBag.Title = "Professionals";
                return View(addProfessionalViewModel);
            }
            return Redirect("/User/Index?username=" + HttpContext.Session.GetString("UserName"));
        }

        [HttpPost]
        public IActionResult Add(AddProfessionalViewModel addProfessionalViewModels)
        {
            if (ModelState.IsValid)
            {
                Services newService = context.Services.Single(s => s.ID == addProfessionalViewModels.ServicesID);

                Professional newProf = new Professional
                {

                    Name = addProfessionalViewModels.Name,
                    Description = addProfessionalViewModels.Description,
                    Email = addProfessionalViewModels.Email,
                    Services = newService
                };

                context.Professionals.Add(newProf);
                context.SaveChanges();

                return Redirect("/Professional");
            }

            return View(addProfessionalViewModels);
        }


        //to list all professionals catering to a particular service
        public IActionResult Service(int id)
        {
            if (id == 0)//If id is not specified
            {
                return Redirect("/Home");

            }


            Services theServ = context.Services.Include(prof => prof.Professionals).Single(prof => prof.ID == id);
            //to refernce all the professionals from the other side of the relationship, services side

            ViewBag.Title = "Professionals provinding " + theServ.ServiceName + " :";

            return View("Index", theServ.Professionals);


        }
    }
}