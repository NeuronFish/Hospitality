using Hospitality.Data;
using Microsoft.AspNetCore.Mvc;

namespace Hospitality.Models
{
    public class Floor
    {
        public int Id { get; set; }
		[BindProperty]
		public int HostelId { get; set; }
        public int Index { get; set; }
        public Hostel MHostel { get; set; }
        public List<Room> Rooms { get; set; }

        public static int LastIndex(int id, HospitalityContext context)
        {
            return context.Rooms.Where(room => room.FloorId == id).Count();
        }
	}
}
