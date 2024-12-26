using Humanizer;

namespace HotelReservation.Models
{
    public class RoomViewModel
    {
        public int X, Y, Z;
        public List<Room> Rooms { get; set; }
        public List<Service> Services { get; set; }
       
        
    }
}
