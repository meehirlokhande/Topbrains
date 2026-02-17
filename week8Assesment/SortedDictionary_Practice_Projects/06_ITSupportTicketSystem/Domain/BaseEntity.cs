namespace Domain
{
    public class SupportTicket
    {
        public string TicketId { get; set; }
        public string IssueDescription { get; set; }
        public int SeverityLevel { get; set; }

        public SupportTicket(string ticketId, string issueDescription, int severityLevel)
        {
            TicketId = ticketId;
            IssueDescription = issueDescription;
            SeverityLevel = severityLevel;
        }
    }
}
