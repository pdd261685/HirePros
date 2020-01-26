using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HirePros.Data;
using HirePros.Models;
using HirePros.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HirePros.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ServiceDbContext context;

        public ServiceController(ServiceDbContext dbContext)
        {
            context = dbContext;
        }

        //Get Controller
        public IActionResult Index()
        {
            
            List<Services> servs = context.Services.ToList();
            return View(servs);
            
            //return Redirect("User/Index?username=" + HttpContext.Session.GetString("UserName"));
        }

        //Get Controller
        public IActionResult Add()
        {
            if (HttpContext.Session.GetString("UserName") == "Admin")
            {
                AddServiceViewModel addServiceViewModel = new AddServiceViewModel();
                return View(addServiceViewModel);
            }
            return Redirect("/User/Index?username=" + HttpContext.Session.GetString("UserName"));


        }

        //Post Controller

        [HttpPost]
        public IActionResult Add(AddServiceViewModel addServiceViewModel)
        {
            if (ModelState.IsValid)
            {
                Services newService = new Services
                {
                    ServiceName = addServiceViewModel.ServiceName,
                    ServiceDesc = addServiceViewModel.ServiceDesc
                };

                context.Services.Add(newService);
                context.SaveChanges();

                return Redirect("/Service");
            }

            return View(addServiceViewModel);
        }
    }
}