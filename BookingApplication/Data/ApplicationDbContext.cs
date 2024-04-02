using BookingApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<BookingApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BookingApplication.Models.Apartment> Apartments { get; set; }
        public virtual DbSet<BookingApplication.Models.Reservation> Reservations { get; set; }
        public virtual DbSet<BookingApplication.Models.BookReservation> BookReservations { get; set; }
        public virtual DbSet<BookingApplication.Models.BookingList> BookingLists { get; set; }

    }
}
