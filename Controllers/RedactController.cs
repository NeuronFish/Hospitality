using Hospitality.Data;
using Hospitality.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            return View(Hostel.GetFloors(hostelId, Context));
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
			return Redirect("/Redact/Building");
        }
        [HttpGet]
        public IActionResult CreateRoom(int floorId)
        {
            return View(new Room() { FloorId = floorId, Index = Floor.LastIndex(floorId, Context) + 1 });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateRoom(Room room)
        {
            Context.Rooms.Add(room);
            Context.SaveChanges();
            return Redirect("/Redact/Building");
        }
        [HttpGet]
        public IActionResult Delete(int floorId)
        {
			int hostelId = Context.Floors.First(floor => floor.Id == floorId).HostelId;
			if (!EnsureData(floorId, hostelId))
				return NotFound();
			Hostel.Remove(floorId, Context);
            return Redirect("/Redact/Building");
        }
        [HttpGet]
        public IActionResult EditRoom(int roomId)
        {
            Room? room = Context.Rooms.FirstOrDefault(room => room.Id == roomId);
            if (room == null)
                return NotFound();
            return View(Context.Rooms.First(room => room.Id == roomId));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditRoom(Room room)
        {
            Context.Rooms.Entry(room).State = EntityState.Modified;
            Context.SaveChanges();
            return Redirect("/Redact/Building");
        }
    }
}
