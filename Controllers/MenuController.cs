using Hospitality.Data;
using Hospitality.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hospitality.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private HospitalityContext Context;
        
        public MenuController(HospitalityContext context)
        {
            Context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            MenuViewModel model = new MenuViewModel();
            model.SetData(hostelId, Context);
            return View(model);
        }
		public IActionResult Complete(int assingId)
        {
			int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            if (!Hostel.RemoveAssignment(assingId, hostelId, Context))
                return NotFound();
			return Redirect("~/Menu");
		}
        [HttpGet]
        public IActionResult Rooms()
        {
            int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            return View(Hostel.GetRooms(hostelId, Context));
        }
        [HttpGet]
        public IActionResult Register(int roomId)
        {
            return View(new Guest() { RoomId = roomId });
        }
        [HttpPost]
        public IActionResult Register(Guest guest)
        {
            Context.Guests.Add(guest);
            Context.SaveChanges();
            return View("Register", new Guest() { RoomId = guest.RoomId });
        }
        public IActionResult SetFree(int roomId)
        {
            Guest.SetFreeRoom(roomId, Context);
            return Redirect("~/Menu/Rooms");
        }
        [HttpGet]
        public IActionResult Guests()
        {
            int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            return View(Hostel.GetGuests(hostelId, Context));
        }
    }
}
