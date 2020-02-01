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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using System.Web.MVC;

namespace HirePros.Controllers
{
    public class UserController : Controller
    {
        private readonly ServiceDbContext context;

        //Identity & authentication
        private static (ClaimsIdentity,bool) isUserAutheticated(User newUser)
        {
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;
          
            if (newUser != null)
            {

                if (newUser.Username == "Admin")
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
            }    

            return (identity,isAuthenticated);
 
        }
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

                bool isAuthenticated;
                ClaimsIdentity identity;
                (identity, isAuthenticated) = isUserAutheticated(newUser);
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


                //var xyz=User.Identity.Name;

                
                return Redirect("Index?username=" + newUser.Username);
               

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

            User newUser = context.Users.Where(u => u.Username == addUserViewModel.Username).FirstOrDefault();
                
            //using where and not Single to account for if the login account enterd doesnt exist
            if (newUser == null)
            {
                ViewBag.error = " User account does not exist ";

            }
            else if (newUser.Password!= addUserViewModel.Password)
            {
                ViewBag.error = "Invalid Password";
            }

            else
            {
                bool isAuthenticated;
                ClaimsIdentity identity;
                (identity, isAuthenticated) = isUserAutheticated(newUser);
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
            }

          //var xyz=User.Identity.Name;
           
            return View(addUserViewModel);
        }

        //[HttpPost]
        
        public IActionResult UserLogout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/User/Login");
        }

        [Authorize(Roles ="User")]
        
        public IActionResult ViewUserProf(string name)
        {
      
                User newUser = context.Users.Single(u => u.Username == name);
                List<UserProf> listing = context.UserProfs.Include(l => l.Professional).Where(up => up.UserID == newUser.ID).ToList();
                List<Professional> proList = new List<Professional>();
                foreach (var pr in listing)
                {
                    proList.Add(context.Professionals.Include(s => s.Services).Where(p => p.ID == pr.ProfessionalID).FirstOrDefault());
                }

                // List<Professional> pList = context.Professionals.Where(p => p.ID = listing);
                ViewUserProfViewModel viewUserProfViewModel = new ViewUserProfViewModel
                {
                    User = newUser,
                    Listing = listing
                };

      
                return View(proList);
            
            
        }

        //Menu/AddPros/3
        [Authorize(Roles = "User")]
        public IActionResult AddUserPros(string name, string error="")
        {

             User user = context.Users.Single(u => u.Username == name);
             List<Professional> profs = context.Professionals.ToList();

             AddUserProfViewModel addUserProfViewModel = new AddUserProfViewModel(user, profs);

             ViewBag.error = error;
             return View(addUserProfViewModel);
            
            //ViewBag.error = "Unauthorised Access";
            //return View();
        }

        [HttpPost]
        public IActionResult AddUserPros(AddUserProfViewModel addUserProfViewModel)
        {
            User user = context.Users.Single(u => u.Username == User.Identity.Name);

            if (ModelState.IsValid)
            {
                var professionalID = addUserProfViewModel.ProfessionalID;
                var userID = user.ID;

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
                    return Redirect("/User/ViewUserProf?name=" + user.Username);
                }
                else
                {
                    string err = "Already added, Please choose another Pro";
                    return Redirect("/User/AddUserPros?name=" + user.Username+"&error="+err);
                }
                

            }

            return Redirect("/User/AddUserPros?name="+user.Username);
        }

        public IActionResult MessagePro(string name, int pro)
        {
            User user = context.Users.Single(u => u.Username == name);
            UserProf userProf = context.UserProfs.Single(u => u.UserID == user.ID & u.ProfessionalID==pro);
            return View(userProf);
        }

        [HttpPost]
        public IActionResult MessagePro(UserProf userProf)
        {

            UserProf UserMsg= context.UserProfs.Single(up => up.UserID == userProf.UserID & up.ProfessionalID == userProf.ProfessionalID);

            UserMsg.UserMessage = userProf.UserMessage;

                context.SaveChanges();
                return Redirect("/User/ViewUserProf?name=" + User.Identity.Name);
            }
    }
}