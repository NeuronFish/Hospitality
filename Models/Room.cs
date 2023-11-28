using Hospitality.Data;

namespace Hospitality.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int FloorId { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public Floor MFloor { get; set; }
        public List<Assignment> Assignments { get; set; }

        public static void ChangeName(int id, string name, HospitalityContext context)
        {
            context.Rooms.First(room => room.Id == id).Name = name;
            context.SaveChanges();
        }
    }
}
