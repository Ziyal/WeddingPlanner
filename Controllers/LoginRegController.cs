using System;
using System.Collections.Generic;
using System.Linq;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WeddingPlanner.Controllers
{
    public class LoginRegController : Controller {
        private WeddingPlannerContext _context;
    
        public LoginRegController(WeddingPlannerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Errors = TempData["Errors"];
            return View("LoginReg");
        }

        [HttpPost]
        [Route("login_user")]
        public IActionResult Login(User model)  {
            List<string> allErrors = new List <string>();

            if(ModelState.IsValid) {
                User user = _context.Users.SingleOrDefault(person => person.Email == model.Email);
                System.Console.WriteLine(user);
                // Check if user exists
                if (user != null && model.Password != null) {
                
                    var Hasher = new PasswordHasher<User>();
                    // Check for correct password
                    if (0 != Hasher.VerifyHashedPassword(user, user.Password, model.Password)) {
                        HttpContext.Session.SetInt32("CurrUserId", user.UserId);
                        return RedirectToAction("Success");
                    }
                }
            }
            allErrors.Add("Incorrect email and/or password");
            TempData["Errors"] = allErrors;
            return RedirectToAction("Login");
        }


        [HttpPost]
        [Route("register_user")]
        public IActionResult RegisterUser(RegisterUser model)  {
            List<string> allErrors = new List <string>();
            System.Console.WriteLine(model);

            if(ModelState.IsValid) {
                User CheckUser = _context.Users.SingleOrDefault(person => person.Email == model.Email);
                System.Console.WriteLine(CheckUser);
                
                if (CheckUser != null) {
                    allErrors.Add("Email already in use");
                    TempData["Errors"] = allErrors;
                    return RedirectToAction("Index");
                }

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User newUser = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                
                _context.Add(newUser);
                _context.SaveChanges();
                // Grab user id
                User user = _context.Users.SingleOrDefault(person => person.Email == model.Email);
                HttpContext.Session.SetInt32("CurrUserId", user.UserId);
                return RedirectToAction("Success");
            }
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;
            return RedirectToAction("Index", model);
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            return RedirectToAction("Index", "Wedding");
        }        

    }
}
