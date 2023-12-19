using Hospitality.Data;

namespace Hospitality.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int FloorId { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public int Sleepplaces { get; set; }
        public string SleepDesc { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public Floor MFloor { get; set; }
        public List<Assignment> Assignments { get; set; }
        public List<Guest> Guests { get; set; }
    }
}
