using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        private SortedDictionary<int, Queue<SupportTicket>> _data = new SortedDictionary<int, Queue<SupportTicket>>();

        public void AddTicket(SupportTicket ticket)
        {
            if (ticket.SeverityLevel < 1 || ticket.SeverityLevel > 5)
                throw new InvalidSeverityException("Invalid Severity Level");

            if (!_data.ContainsKey(ticket.SeverityLevel))
                _data[ticket.SeverityLevel] = new Queue<SupportTicket>();

            _data[ticket.SeverityLevel].Enqueue(ticket);
        }

        public void GetAllTickets()
        {
            foreach (var kvp in _data)
            {
                foreach (var t in kvp.Value)
                {
                    Console.WriteLine($"Details: {t.TicketId} {t.IssueDescription} {t.SeverityLevel}");
                }
            }
        }

        public void EscalateTicket(string ticketId, int newSeverity)
        {
            if (newSeverity < 1 || newSeverity > 5)
                throw new InvalidSeverityException("Invalid Severity Level");

            foreach (var kvp in _data)
            {
                var queue = kvp.Value;
                var tempList = new List<SupportTicket>(queue);
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i].TicketId == ticketId)
                    {
                        SupportTicket found = tempList[i];
                        int oldSeverity = found.SeverityLevel;

                        tempList.RemoveAt(i);
                        queue.Clear();
                        foreach (var t in tempList)
                            queue.Enqueue(t);
                        if (queue.Count == 0)
                            _data.Remove(oldSeverity);

                        found.SeverityLevel = newSeverity;
                        if (!_data.ContainsKey(newSeverity))
                            _data[newSeverity] = new Queue<SupportTicket>();
                        _data[newSeverity].Enqueue(found);

                        Console.WriteLine($"Escalated Severity: {newSeverity}");
                        return;
                    }
                }
            }
            throw new TicketNotFoundException("Ticket Not Found");
        }
    }
}
