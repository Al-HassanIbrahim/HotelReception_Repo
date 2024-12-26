using HotelReservation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace HotelReservation.Controllers
{
    public class HomeController : Controller
    {
        private readonly Hotel1Context _context;

        public HomeController(Hotel1Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int x = 0, y = 0, z = 0;
            foreach (var room in _context.Rooms) 
            {
                if(room.RType=="Single"&&room.Availability==1)
                {
                    x++;
                }
                else if(room.RType=="Double"&&room.Availability==1)
                {
                    y++;
                }
                else if(room.RType== "Suites"&&room.Availability==1)
                {
                    z++;
                }
            }
            var model = new RoomViewModel
            {
                X=x,Y=y,Z=z,
                Rooms = _context.Rooms.ToList(),
                Services = _context.Services.ToList()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult AddGuest(string name, long phone, string gender)
        {
            int newGuestId = _context.Guests.Any()
               ? _context.Guests.Max(g => g.GId) + 1
               : 1;
            var newGuest = new Guest
            {
                GId= newGuestId,
                Gname = name,
                Phone = phone,
                Gender = gender
            };

            _context.Guests.Add(newGuest);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
       
        public IActionResult GuestSearch(long phone)
        {
           
            var guest = _context.Guests.FirstOrDefault(g => g.Phone == phone);
            if (guest == null)
            {
                return NotFound("Guest not found. Please check the phone number.");
            }

            
            var bookings = _context.Bookings
                .Where(b => b.GuestId == guest.GId)
                .Select(b => new
                {
                    b.RoomId,
                    b.Status,
                    CheckinDate = b.CheckinDate.HasValue ? b.CheckinDate.Value.ToString("yyyy-MM-dd") : null,
                    ChenckoutDate = b.ChenckoutDate.HasValue ? b.ChenckoutDate.Value.ToString("yyyy-MM-dd") : null,
                    b.TotalPrice,
                    b.AmountPayed,
                    b.PayDate
                })
                .ToList();

            return Json(bookings); 
        }


        [HttpPost]
        public IActionResult CheckIn(long phone, int roomId, int[] serviceIds, DateTime checkOutDate)
        {
           
            var guest = _context.Guests.FirstOrDefault(g => g.Phone == phone);
            if (guest == null)
            {
                ViewBag.Error = "Guest not found. Please register first.";
                return RedirectToAction("Index");
            }

            var room = _context.Rooms.FirstOrDefault(r => r.RId == roomId && r.Availability == 1);
            if (room == null)
            {
                ViewBag.Error = "Room is not available.";
                return RedirectToAction("Index");
            }

       
            room.Availability = 0;

            var services = _context.Services.Where(s => serviceIds.Contains(s.SId)).ToList();

           
            var days = (checkOutDate - DateTime.Now.Date).Days;
            var serviceTotal = services.Sum(s => s.Price) ?? 0;
            var totalPrice = (days * room.Price) + serviceTotal;
            long newBookingId = _context.Bookings.Any()
              ? _context.Bookings.Max(b => b.BookId) + 1
                  : 1;
       
            var booking = new Booking
            {
                BookId = newBookingId,
                GuestId = guest.GId,
                RoomId = roomId,
                CheckinDate = DateOnly.FromDateTime(DateTime.Now),
                ChenckoutDate = DateOnly.FromDateTime(checkOutDate),
                TotalPrice = totalPrice,
                Status = "Checked In",
                Services = services 
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CheckOut(long phone, int amountPaid)
        {
            
            var guest = _context.Guests.FirstOrDefault(g => g.Phone == phone);
            if (guest == null)
            {
                return NotFound("Guest not found. Please check the phone number.");
            }

          
            var booking = _context.Bookings
                .FirstOrDefault(b => b.GuestId == guest.GId && b.Status == "Checked In");

            if (booking == null)
            {
                return NotFound("No active bookings found for this guest.");
            }

            
            booking.AmountPayed = amountPaid;
            booking.PayDate = DateOnly.FromDateTime(DateTime.Now); 
            booking.Status = "Checked Out";

            
            var room = _context.Rooms.FirstOrDefault(r => r.RId == booking.RoomId);
            if (room != null)
            {
                room.Availability = 1; 
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
