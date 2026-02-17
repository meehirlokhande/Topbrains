namespace Domain
{
    public class Ticket
    {
        public string TicketId { get; set; }
        public string PassengerName { get; set; }
        public int Fare { get; set; }

        public Ticket(string ticketId, string passengerName, int fare)
        {
            TicketId = ticketId;
            PassengerName = passengerName;
            Fare = fare;
        }
    }
}
