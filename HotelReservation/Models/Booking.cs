using System;
using System.Collections.Generic;

namespace HotelReservation.Models;

public partial class Booking
{
    public long BookId { get; set; }

    public string? Status { get; set; }

    public int? GuestId { get; set; }

    public int? RoomId { get; set; }

    public int? TotalPrice { get; set; }

    public DateOnly? CheckinDate { get; set; }

    public DateOnly? ChenckoutDate { get; set; }

    public DateOnly? PayDate { get; set; }

    public int? AmountPayed { get; set; }

    public long BillId { get; set; }

    public virtual Guest? Guest { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
