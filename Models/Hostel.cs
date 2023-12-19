using Hospitality.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using System.ComponentModel.DataAnnotations;

namespace Hospitality.Models
{
    public class Hostel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? ChiefId { get; set; }
        public UserIdent? Chief { get; set; }
        public List<UserIdent> Staff { get; set; }
        public List<Floor> Floors { get; set; }

        public static IQueryable<Floor> GetFloors(int hostelId, HospitalityContext context)
        {
			IQueryable<Floor> floors = context.Floors.Where(floor => floor.HostelId == hostelId)
				.OrderByDescending(floor => floor.Index).Include(floor => floor.Rooms);
			foreach (Floor floor in floors)
				floor.Rooms.OrderBy(room => room.Index);
            return floors;
		}
		public static IEnumerable<RoomViewModel> GetRooms(int hostelId, HospitalityContext context)
		{
			List<RoomViewModel> rooms = new List<RoomViewModel>();
            IQueryable<Floor> floors = context.Floors.Where(floor => floor.HostelId == hostelId)
                .OrderByDescending(floor => floor.Index).Include(floor => floor.Rooms)
				.ThenInclude(room => room.Guests);
			foreach (Floor floor in floors)
			{
				floor.Rooms.OrderBy(room => room.Index);
				foreach (Room room in floor.Rooms)
				{
					bool isOccupied = false;
					if (room.Guests.Any())
						isOccupied = true;
                    rooms.Add(new RoomViewModel()
					{
						Id = room.Id,
						Stage = floor.Index,
						Name = room.Name,
						Sleepplaces = room.Sleepplaces,
						SleepDesc = room.SleepDesc,
						Description = room.Description,
						Price = room.Price,
                        IsOccupied = isOccupied
                    });
				}
			}
			return rooms;
        }
        public static void AddFloor(int hostelId, HospitalityContext context)
        {
			int index = context.Floors.Where(floor => floor.HostelId == hostelId).Count() + 1;
			Floor floor = new Floor()
			{
				HostelId = hostelId,
				Index = index
			};
			context.Add(floor);
			context.SaveChanges();
		}
		public static void Remove(int floorId, HospitalityContext context)
		{
			if (context.Rooms.Where(room => room.FloorId == floorId).Count() > 0)
			{
				IQueryable<Room> rooms = context.Rooms.Where(room => room.FloorId == floorId);
				context.Rooms.Remove(rooms.First(room => room.Index == rooms.Max(r => r.Index)));
				context.SaveChanges();
				return;
			}
			Floor selected_floor = context.Floors.First(floor => floor.Id == floorId);
			IQueryable<Floor> floors = context.Floors.Where(floor => floor.HostelId == selected_floor.HostelId);
			if (floors.Count() == 1)
				return;
			if (selected_floor.Index != floors.Max(f => f.Index))
			{
				for(int index = selected_floor.Index + 1; index <= floors.Count(); index++)
				{
					floors.First(floor => floor.Index == index).Index = index - 1;
				}
			}
			context.Floors.Remove(selected_floor);
			context.SaveChanges();
		}
		public static bool RemoveAssignment(int assignId, int hostelId, HospitalityContext context)
		{
			Assignment? assign = context.Assignments.FirstOrDefault(assign => assign.Id == assignId);
			if (assign == null)
				return false;
			int floorId = context.Rooms.First(room => room.Id == assign.RoomId).FloorId;
			if (context.Floors.First(floor => floor.Id == floorId).HostelId != hostelId)
				return false;
			context.Remove(assign);
			context.SaveChanges();
			return true;
		}
		public static List<GuestViewModel> GetGuests(int hostelId, HospitalityContext context)
		{
			List<GuestViewModel> model = new List<GuestViewModel>();
			foreach (Floor floor in context.Floors.Where(floor => floor.HostelId == hostelId)
				.Include(floor => floor.Rooms).ThenInclude(rooms => rooms.Guests))
				foreach (Room room in floor.Rooms)
					foreach (Guest guest in room.Guests)
						model.Add(new GuestViewModel()
						{
							Name = guest.Name,
							Surname = guest.Surname,
							PhoneNumber = guest.PhoneNumber,
							RoomName = room.Name,
							Stage = floor.Index
						});
			return model;
		}
	}
}
