using Hospitality.Data;
using Hospitality.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospitality.Controllers
{
    [Authorize(Roles = "Manager")]
    public class RegisterController : Controller
    {
        private UserManager<UserIdent> UserManager;
        private HospitalityContext Context;

        public RegisterController(UserManager<UserIdent> userManager, HospitalityContext context)
        {
            UserManager = userManager;
            Context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RegisterViewModel model)
        {
            int hostelId = Context.Users.Find(this.User.FindFirst(ClaimTypes.NameIdentifier).Value).PlaceId;
            UserIdent user = new UserIdent()
            {
                Name = model.Name,
                Surname = model.Surname,
                UserName = model.UserName,
                PlaceId = hostelId,
                EmailConfirmed = true
            };
            var result = UserManager.CreateAsync(user, model.Password).Result;
            if (result.Succeeded)
            {
                UserManager.AddToRoleAsync(user, model.Role);
                return Redirect("~/Menu");
            }
            else
            {
                model.Errors = new List<string>();
                foreach (var error in result.Errors)
                    model.Errors.Add(error.Description);
                return View(model);
            }
        }
    }
}
