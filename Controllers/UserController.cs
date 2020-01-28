using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HirePros.Data;
using HirePros.Models;
using HirePros.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Web.MVC;

namespace HirePros.Controllers
{
    public class UserController : Controller
    {
        private readonly ServiceDbContext context;
        public UserController(ServiceDbContext dbContext)
        {
            context = dbContext;
        }


        public IActionResult Index(string Username = "User")// if there is no username it says welcome User
        {
            ViewBag.Username = Username;
            User user = context.Users.Single(u => u.Username == Username);
            return View(user);
        }

        public IActionResult Add()
        {
            AddUserViewModel addUserViewModel = new AddUserViewModel();
            return View(addUserViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddUserViewModel addUserViewModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    Username = addUserViewModel.Username,
                    Email = addUserViewModel.Email,
                    Password = addUserViewModel.Password
                };
                

                context.Users.Add(newUser);
                context.SaveChanges();

                return Redirect("Index?username=" + newUser.Username);
                //return Redirect("VieUserProf/"+newUser.ID);

            }

            return View(addUserViewModel);
        }

        public IActionResult Login()
        {
            AddUserViewModel addUserViewModel = new AddUserViewModel();
            return View(addUserViewModel);
        } 

        [HttpPost]
        public IActionResult Login(AddUserViewModel addUserViewModel)
        {

            User newUser = context.Users.Where(u => u.Username == addUserViewModel.Username & u.Password == addUserViewModel.Password).FirstOrDefault();
            
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;
            //using whree and not Single to account for if the login account enterd doesnt exist
            if (newUser == null)
            {
                ViewBag.error = "Invalid Login or password";
            }
            
            else
            {
                
                if (newUser.Username == "Admin" )
                {

                    //Create the identity for the user  
                    identity = new ClaimsIdentity(new[] 
                    {
                    new Claim(ClaimTypes.Name, newUser.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                     }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

                else
                {
                    //Create the identity for the user  
                    identity = new ClaimsIdentity(new[] 
                    {
                    new Claim(ClaimTypes.Name, newUser.Username),
                    new Claim(ClaimTypes.Role, "User")
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

                if (isAuthenticated)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (newUser.Username == "Admin")
                    {
                        return Redirect("Index?username=" + newUser.Username);
                    }
                    return Redirect("ViewUserProf?name=" + newUser.Username); 
                }
                return View();

               
                
            }

           //var xyz=User.Identity.Name;
           
            return View(addUserViewModel);
        }

        [HttpPost]
        public IActionResult UserLogout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("User/Login");
        }
        public IActionResult ViewUserProf(string name)
        {
            User newUser = context.Users.Single(u => u.Username==name);
            List<UserProf> listing = context.UserProfs.Include(listing => listing.Professional).Where(up => up.UserID == newUser.ID).ToList();
           // List<Professional> pList = context.Professionals.Where(p => p.ID = listing);
            ViewUserProfViewModel viewUserProfViewModel = new ViewUserProfViewModel
            {
                User = newUser,
                Listing = listing
            };

            return View(viewUserProfViewModel);
        }

        //Menu/AddPros/3
        public IActionResult AddUserPros(int id)
        {
           
            User user = context.Users.Where(u => u.ID == id).FirstOrDefault();
            List<Professional> profs = context.Professionals.ToList();
            AddUserProfViewModel addUserProfViewModel = new AddUserProfViewModel(user, profs);
            return View(addUserProfViewModel);
        }

        [HttpPost]
        public IActionResult AddUserPros(AddUserProfViewModel addUserProfViewModel)
        {
           
            if (ModelState.IsValid)
            {
                var professionalID = addUserProfViewModel.ProfessionalID;
                var userID = addUserProfViewModel.UserID;

                IList<UserProf> existingPro = context.UserProfs.Where(up => up.UserID == userID).Where(p => p.ProfessionalID == professionalID).ToList();

                if (existingPro.Count == 0)
                {
                    UserProf userList = new UserProf
                    {
                        ProfessionalID = professionalID,
                        UserID = userID

                    };
                    context.UserProfs.Add(userList);
                    context.SaveChanges();
                    return Redirect("/User/ViewUserProf/" + addUserProfViewModel.UserID);
                }


            }

            return View(addUserProfViewModel);
        }

 

    }
}