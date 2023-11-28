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
        public IActionResult Index()
        {
            int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            return View(Hostel.GetData(hostelId, Context));
        }
    }
}
