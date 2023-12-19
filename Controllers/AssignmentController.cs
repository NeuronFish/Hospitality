using Hospitality.Data;
using Hospitality.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospitality.Controllers
{
    [Authorize]
    public class AssignmentController : Controller
    {
        private HospitalityContext Context;

        public AssignmentController(HospitalityContext context)
        {
            Context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            AssignmentViewModel model = new AssignmentViewModel();
            model.SetData(hostelId, Context);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AssignmentViewModel model)
        {
            if (model.SelectedPersonnel == "None")
                model.SelectedPersonnel = null;
            Context.Assignments.Add(new Assignment()
            {
                RoomId = model.SelectedRoom,
                UserId = model.SelectedPersonnel,
                Description = model.Description
            });
            Context.SaveChanges();
            return Redirect("~/Menu/Index");
        }
    }
}
