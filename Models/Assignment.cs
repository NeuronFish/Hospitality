namespace Hospitality.Models
{
    public class Assignment
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string? UserId { get; set; }
        public string Description { get; set; }
        public Room MRoom { get; set; }
        public UserIdent? User { get; set; }
    }
}
