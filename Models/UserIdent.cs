using Microsoft.AspNetCore.Identity;

namespace Hospitality.Models
{
    public class UserIdent : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PlaceId { get; set; }
        public Hostel Place { get; set; }
        public Hostel? Owned { get; set; }
        public List<Assignment> Assignments { get; set; }
    }
}
