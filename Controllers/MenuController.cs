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
    }
}
