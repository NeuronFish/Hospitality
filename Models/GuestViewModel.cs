using System.ComponentModel.DataAnnotations;

namespace Hospitality.Models
{
    public class GuestViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string RoomName { get; set; }
        public int Stage { get; set; }
    }
}
