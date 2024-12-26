using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Guest
{
    public int GId { get; set; }

    public string Gname { get; set; } = null!;

    public string? Gender { get; set; }

    public long? Phone { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
