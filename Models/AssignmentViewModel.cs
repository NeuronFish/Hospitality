using Hospitality.Data;
using Microsoft.EntityFrameworkCore;

namespace Hospitality.Models
{
    public class AssignmentViewModel
    {
        public Dictionary<int, string> Rooms { get; private set; }
        public Dictionary<string, string> Personnel { get; private set; }
        public int SelectedRoom { get; set; }
        public string? SelectedPersonnel { get; set; }
        public string Description { get; set; }

        public void SetData(int hostelId, HospitalityContext context)
        {
            Rooms = new Dictionary<int, string>();
            Personnel = new Dictionary<string, string>();
            foreach (Floor floor in context.Floors.Where(fl => fl.HostelId == hostelId).Include(fl => fl.Rooms))
            {
                foreach (Room room in floor.Rooms)
                {
                    Rooms.Add(room.Id, room.Name);
                }
            }
            Personnel.Add("None", "Без назначення");
            foreach (UserIdent user in context.Users.Where(us => us.PlaceId == hostelId))
                Personnel.Add(user.Id, user.Surname + " " + user.Name);
        }
    }
}
