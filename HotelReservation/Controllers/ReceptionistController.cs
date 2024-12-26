using HotelReservation.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace HotelReservation.Controllers
{
    public class ReceptionistController : Controller
    {
        private readonly Hotel1Context _context;

        public ReceptionistController(Hotel1Context context)
        {
            _context = context;
        }

        public IActionResult Home_Page()
        {
            var model = new RoomViewModel
            {
                Rooms = _context.Rooms.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult CheckIn(int guestId, int roomId, DateOnly? checkInDate)
        {
            var room = _context.Rooms.FirstOrDefault(r => r.RId == roomId);
            if (room != null && room.Availability == 1)
            {
                room.Availability = 0;
                _context.SaveChanges();

                // Add booking logic
                _context.Bookings.Add(new Booking
                {
                    GuestId = guestId,
                    RoomId = roomId,
                    CheckinDate = checkInDate,
                    Status = "Checked In"
                });
                _context.SaveChanges();
            }

            return RedirectToAction("Home_Page");
        }

        [HttpPost]
        public IActionResult CheckOut(int bookingId, DateOnly? checkOutDate)
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookId == bookingId);
            if (booking != null)
            {
                booking.ChenckoutDate = checkOutDate;
                booking.Status = "Checked Out";

                var room = _context.Rooms.FirstOrDefault(r => r.RId == booking.RoomId);
                if (room != null)
                {
                    room.Availability = 1;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Home_Page");
        }
    }
}
