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

		public static void AddRoom(int floorId, HospitalityContext context)
		{
			int index = context.Rooms.Where(room => room.FloorId == floorId).Count() + 1;
			context.Add(new Room()
			{
				FloorId = floorId,
				Index = index,
				Name = "Name"
			});
			context.SaveChanges();
		}
	}
}
