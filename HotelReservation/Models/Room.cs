using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Room
{
    public int RId { get; set; }

    public string? RType { get; set; }

    public byte? Availability { get; set; }

    public int? Price { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
