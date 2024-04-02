using System.ComponentModel.DataAnnotations;

namespace BookingApplication.Models
{
    public class BookReservation
    {
        [Key]
        public Guid Id { get; set; }
        public int NumberOfNights { get; set; }
        public virtual BookingList? BookingList { get; set; }
        public virtual Reservation? Reservation { get; set; }
    }
}
