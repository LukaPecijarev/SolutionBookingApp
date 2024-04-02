namespace BookingApplication.Models.DTO
{
    public class BookingListDTO
    {
        public List<BookReservation>BookReservations { get; set; }
        public double TotalPrice { get; set; }
        public List<Reservation> SelectReservations { get; set; }
    }
}
