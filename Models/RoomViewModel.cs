namespace Hospitality.Models
{
    public class RoomViewModel
    {
        public int Id { get; set; }
        public int Stage { get; set; }
        public string Name { get; set; }
        public int Sleepplaces { get; set; }
        public string SleepDesc { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public bool IsOccupied { get; set; }
    }
}
