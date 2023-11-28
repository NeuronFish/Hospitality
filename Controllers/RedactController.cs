using Hospitality.Data;
using Hospitality.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospitality.Controllers
{
    [Authorize(Roles = "Manager")]
    public class RedactController : Controller
    {
        private HospitalityContext Context;

        public RedactController(HospitalityContext context)
        {
            Context = context;
		}
        [HttpGet]
		public IActionResult Building()
        {
			int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            return View(Hostel.GetData(hostelId, Context));
        }
        private bool EnsureData(int floorId, int hostelId)
        {
			if (Context.Floors.FirstOrDefault(floor => floor.Id == floorId) == null)
				return false;
			if (hostelId != Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId)
				return false;
            return true;
		}
        [HttpGet]
        public IActionResult AddFloor()
        {
			int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
			Hostel.AddFloor(hostelId, Context);
			return View("Building", Hostel.GetData(hostelId, Context));
        }
        [HttpGet]
        public IActionResult AddRoom(int floorId)
        {
            int hostelId = Context.Floors.First(floor => floor.Id == floorId).HostelId;
			if (!EnsureData(floorId, hostelId))
                return NotFound();
			Floor.AddRoom(floorId, Context);
			return View("Building", Hostel.GetData(hostelId, Context));
		}
        [HttpGet]
        public IActionResult Delete(int floorId)
        {
			int hostelId = Context.Floors.First(floor => floor.Id == floorId).HostelId;
			if (!EnsureData(floorId, hostelId))
				return NotFound();
			Hostel.Remove(floorId, Context);
            return View("Building", Hostel.GetData(hostelId, Context));
		}
        [HttpGet]
        public IActionResult EditRoom(int roomId)
        {
            Models.Room? room = Context.Rooms.FirstOrDefault(room => room.Id == roomId);
            if (room == null)
                return NotFound();
            return View(Context.Rooms.First(room => room.Id == roomId));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoom(int id, string name)
        {
            Room.ChangeName(id, name, Context);
            int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            return View("Building", Hostel.GetData(hostelId, Context));
        }
    }
}
