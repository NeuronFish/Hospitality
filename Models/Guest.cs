using Hospitality.Data;
using System.ComponentModel.DataAnnotations;

namespace Hospitality.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public int RoomId { get; set; }
        public Room MRoom { get; set; }

        public static void SetFreeRoom(int roomId, HospitalityContext context)
        {
            foreach (Guest guest in context.Guests.Where(guest => guest.RoomId == roomId))
                context.Guests.Remove(guest);
            context.SaveChanges();
        }
    }
}
