using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        private SortedDictionary<int, List<Ticket>> _data = new SortedDictionary<int, List<Ticket>>();

        public void AddTicket(Ticket ticket)
        {
            if (ticket.Fare <= 0)
                throw new InvalidFareException("Invalid Fare");

            foreach (var kvp in _data)
            {
                foreach (var t in kvp.Value)
                {
                    if (t.TicketId == ticket.TicketId)
                        throw new DuplicateTicketException("Duplicate Ticket");
                }
            }

            if (!_data.ContainsKey(ticket.Fare))
                _data[ticket.Fare] = new List<Ticket>();

            _data[ticket.Fare].Add(ticket);
        }

        public void GetAllTickets()
        {
            foreach (var kvp in _data)
            {
                foreach (var t in kvp.Value)
                {
                    Console.WriteLine($"Details: {t.TicketId} {t.PassengerName} {t.Fare}");
                }
            }
        }

        public void UpdateFare(string ticketId, int newFare)
        {
            if (newFare <= 0)
                throw new InvalidFareException("Invalid Fare");

            foreach (var kvp in _data)
            {
                foreach (var t in kvp.Value)
                {
                    if (t.TicketId == ticketId)
                    {
                        int oldFare = t.Fare;
                        kvp.Value.Remove(t);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldFare);

                        t.Fare = newFare;
                        if (!_data.ContainsKey(newFare))
                            _data[newFare] = new List<Ticket>();
                        _data[newFare].Add(t);

                        Console.WriteLine($"Updated Fare: {newFare}");
                        return;
                    }
                }
            }
            throw new TicketNotFoundException("Ticket Not Found");
        }
    }
}
