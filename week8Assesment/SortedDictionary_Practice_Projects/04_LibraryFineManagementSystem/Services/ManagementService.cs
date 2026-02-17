using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        private SortedDictionary<int, List<Member>> _data =
            new SortedDictionary<int, List<Member>>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

        public void AddMember(Member member)
        {
            if (member.FineAmount < 0)
                throw new InvalidFineException("Invalid Fine Amount");

            foreach (var kvp in _data)
            {
                foreach (var m in kvp.Value)
                {
                    if (m.MemberId == member.MemberId)
                        throw new MemberNotFoundException("Duplicate Member");
                }
            }

            if (!_data.ContainsKey(member.FineAmount))
                _data[member.FineAmount] = new List<Member>();

            _data[member.FineAmount].Add(member);
        }

        public void GetAllMembers()
        {
            foreach (var kvp in _data)
            {
                foreach (var m in kvp.Value)
                {
                    Console.WriteLine($"Details: {m.MemberId} {m.Name} {m.FineAmount}");
                }
            }
        }

        public void PayFine(string memberId, int payAmount)
        {
            if (payAmount <= 0)
                throw new InvalidFineException("Invalid Pay Amount");

            foreach (var kvp in _data)
            {
                foreach (var m in kvp.Value)
                {
                    if (m.MemberId == memberId)
                    {
                        if (payAmount > m.FineAmount)
                            throw new InvalidFineException("Pay Amount Exceeds Fine");

                        int oldFine = m.FineAmount;
                        kvp.Value.Remove(m);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldFine);

                        m.FineAmount -= payAmount;

                        if (!_data.ContainsKey(m.FineAmount))
                            _data[m.FineAmount] = new List<Member>();
                        _data[m.FineAmount].Add(m);

                        Console.WriteLine($"Updated Fine: {m.FineAmount}");
                        return;
                    }
                }
            }
            throw new MemberNotFoundException("Member Not Found");
        }
    }
}
