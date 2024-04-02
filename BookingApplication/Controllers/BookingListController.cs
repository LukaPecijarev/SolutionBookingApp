using BookingApplication.Data;
using BookingApplication.Models;
using BookingApplication.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookingApplication.Controllers
{
    public class BookingListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingListController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            if (userId != null)
            {
                var loggedInUser = await _context.Users
                    .Include("BookingList")
                    .Include("BookingList.BookReservations")
                    .Include("BookingList.BookReservations.Reservation")
                    .Include("BookingList.BookReservations.Reservation.Apartment")
                    .FirstOrDefaultAsync(z => z.Id == userId);
                
                if(loggedInUser.BookingList == null)
                {
                    return RedirectToAction("NewBookingList");
                }
                var allBookReservations = loggedInUser?.BookingList.BookReservations.ToList();

                var totalPrice = 0.0;
                foreach (var item in allBookReservations)
                {
                    totalPrice += Double.Round((item.NumberOfNights * item.Reservation.Apartment.Price_per_night), 2);
                }

                var model = new BookingListDTO
                {
                    BookReservations = allBookReservations,
                    TotalPrice = totalPrice,
                    SelectReservations = _context.Reservations.Include("Apartment").ToList(),
                };
                return View(model);
            }

            return View();
        }
        public async Task<IActionResult> NewBookingList()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;

            if (userId != null)
            {
                var loggedInUser = await _context.Users
                    .Include("BookingList")
                    .Include("BookingList.BookReservations")
                    .Include("BookingList.BookReservations.Reservation")
                    .Include("BookingList.BookReservations.Reservation.Apartment")
                    .FirstOrDefaultAsync(z => z.Id == userId);

                var newBookingList = new BookingList
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    User = loggedInUser,
                    BookReservations = new List<BookReservation>()
                };
                _context.BookingLists.Add(newBookingList);
                _context.SaveChanges();
                loggedInUser.BookingList = newBookingList;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AddBookReservation([FromForm] string reservationId, [FromForm] int numberOfNights)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? null;
            var reservation = _context.Reservations.FirstOrDefault(r => r.Id == Guid.Parse(reservationId));

            if (userId != null)
            {
                var loggedInUser = await _context.Users
                    .Include("BookingList")
                    .Include("BookingList.BookReservations")
                    .Include("BookingList.BookReservations.Reservation")
                    .Include("BookingList.BookReservations.Reservation.Apartment")
                    .FirstOrDefaultAsync(z => z.Id == userId);

                var newBookReservation = new BookReservation
                {
                    Id = Guid.NewGuid(),
                    NumberOfNights = numberOfNights,
                    BookingList = loggedInUser.BookingList,
                    Reservation = reservation
                };
                _context.BookReservations.Add(newBookReservation);
                _context.SaveChanges();
                loggedInUser.BookingList.BookReservations.Add(newBookReservation);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
