using System.Linq;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    public class WeddingController : Controller {
        private WeddingPlannerContext _context;
    
        public WeddingController(WeddingPlannerContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Index() {
            // Current User
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));
            ViewBag.User = CurrentUser;            
            // Display weddings
            List<Wedding> AllWeddings = _context.Weddings.Include(guests => guests.Guests).ThenInclude(guests => guests.User).OrderBy(wedding => wedding.Date).ToList();
            ViewBag.AllWeddings = AllWeddings;     
            return View("Dashboard");
        }

        [HttpGet]
        [Route("wedding/{id}")]
        public IActionResult WeddingId(int id) {
        Wedding CurrentWedding = _context.Weddings.Where(WeddingId => WeddingId.WeddingId == id).Include(guests => guests.Guests).ThenInclude(guest => guest.User).SingleOrDefault();
        ViewBag.CurrentWedding = CurrentWedding;
            return View("Wedding");
        }


        [HttpGet]
        [Route("new-wedding")]
        public IActionResult NewWedding()
        {
            ViewBag.Errors = TempData["Errors"];
            return View("NewWedding");
        }

        [HttpPost]
        [Route("add-wedding")]
        public IActionResult AddWedding(Wedding model){
            List<string> allErrors = new List <string>();
            User CurrentUser = _context.Users.SingleOrDefault(person => person.UserId == (int)HttpContext.Session.GetInt32("CurrUserId"));

            // Input into db
            if (ModelState.IsValid) {
                Wedding newWedding = new Wedding {
                    Bride = model.Bride,
                    Groom = model.Groom,
                    Date = model.Date,
                    Address = model.Address,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    UserId = CurrentUser.UserId
                };
            _context.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Index");
            }
            // If there are validation errors:
            foreach (var i in ModelState.Values) {
                if (i.Errors.Count > 0) {
                    allErrors.Add(i.Errors[0].ErrorMessage.ToString());
                }
            }
            TempData["Errors"] = allErrors;
            return RedirectToAction("NewWedding");
        }

        [HttpGet]
        [Route("rsvp/{id}")]
        public IActionResult RSVP(int id) {
            int CurrentUser = (int)HttpContext.Session.GetInt32("CurrUserId");
            
            Guest AddRSVP = new Guest {
                WeddingId = id,
                UserId = CurrentUser
            };
            _context.Add(AddRSVP);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }    

        [HttpGet]
        [Route("unrsvp/{id}")]
        public IActionResult unRSVP(int id) {
            int CurrentUser = (int)HttpContext.Session.GetInt32("CurrUserId");
            Guest UnRsvp = _context.Guests.Where(user => user.UserId == CurrentUser).Where(wedding => wedding.WeddingId == id).SingleOrDefault();
            _context.Remove(UnRsvp);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }    

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id) {
            // Remove all RSVP Users
            List<Guest> RemoveGuests = _context.Guests.Where(wedding => wedding.WeddingId == id).ToList();
            foreach (var Rsvp in RemoveGuests) {
                _context.Remove(Rsvp);
            }
            _context.SaveChanges();

            // Then remove wedding
            int CurrentUser = (int)HttpContext.Session.GetInt32("CurrUserId");
            Wedding RemoveWedding = _context.Weddings.Where(user => user.UserId == CurrentUser).Where(wedding => wedding.WeddingId == id).SingleOrDefault();
            _context.Remove(RemoveWedding);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }    

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "LoginReg");        
    }
    }
}
