using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Service
{
    public int SId { get; set; }

    public string? SName { get; set; }

    public int? Price { get; set; }

    public virtual ICollection<Booking> Books { get; set; } = new List<Booking>();
}
