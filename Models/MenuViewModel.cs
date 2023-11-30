using Hospitality.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospitality.Models
{
	public class MenuViewModel
	{
		public IQueryable<Floor> Floors { get; private set; }
		public List<AssignData> Assignments { get; private set; }
		public struct AssignData
		{
			public int Id { get; set; }
			public string Room { get; set; }
			public string Personnel { get; set; }
			public string Description { get; set; }
		}

		public void SetData(int hostelId, HospitalityContext context)
		{
			Assignments = new List<AssignData>();
			Floors = context.Floors.Where(floor => floor.HostelId == hostelId)
				.OrderByDescending(floor => floor.Index).Include(floor => floor.Rooms)
				.ThenInclude(room => room.Assignments).ThenInclude(assign => assign.User);
			foreach (Floor floor in Floors)
			{
				floor.Rooms.OrderBy(room => room.Index);
				foreach (Room room in floor.Rooms)
				{
					foreach (Assignment assign in room.Assignments)
					{
						string person;
						if (assign.User != null)
							person = assign.User.Surname + " " + assign.User.Name;
						else
							person = "Нікому";
						Assignments.Add(new AssignData
						{
							Id = assign.Id,
							Room = room.Name,
							Personnel = person,
							Description = assign.Description
						});
					}
				}
			}
		}
	}
}
