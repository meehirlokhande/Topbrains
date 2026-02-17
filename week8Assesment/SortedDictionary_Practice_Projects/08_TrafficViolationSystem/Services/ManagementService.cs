using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        // Descending by FineAmount
        private SortedDictionary<int, List<Violation>> _data =
            new SortedDictionary<int, List<Violation>>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

        public void AddViolation(Violation violation)
        {
            if (violation.FineAmount <= 0)
                throw new InvalidFineAmountException("Invalid Fine Amount");

            foreach (var kvp in _data)
            {
                foreach (var v in kvp.Value)
                {
                    if (v.VehicleNumber == violation.VehicleNumber)
                        throw new DuplicateViolationException("Duplicate Violation");
                }
            }

            if (!_data.ContainsKey(violation.FineAmount))
                _data[violation.FineAmount] = new List<Violation>();

            _data[violation.FineAmount].Add(violation);
        }

        public void GetAllViolations()
        {
            foreach (var kvp in _data)
            {
                foreach (var v in kvp.Value)
                {
                    Console.WriteLine($"Details: {v.VehicleNumber} {v.OwnerName} {v.FineAmount}");
                }
            }
        }

        public void PayFine(string vehicleNumber, int payAmount)
        {
            if (payAmount <= 0)
                throw new InvalidFineAmountException("Invalid Pay Amount");

            foreach (var kvp in _data)
            {
                foreach (var v in kvp.Value)
                {
                    if (v.VehicleNumber == vehicleNumber)
                    {
                        if (payAmount > v.FineAmount)
                            throw new InvalidFineAmountException("Pay Amount Exceeds Fine");

                        int oldFine = v.FineAmount;
                        kvp.Value.Remove(v);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldFine);

                        v.FineAmount -= payAmount;

                        if (v.FineAmount > 0)
                        {
                            if (!_data.ContainsKey(v.FineAmount))
                                _data[v.FineAmount] = new List<Violation>();
                            _data[v.FineAmount].Add(v);
                        }

                        Console.WriteLine($"Updated Fine: {v.FineAmount}");
                        return;
                    }
                }
            }
            throw new InvalidFineAmountException("Violation Not Found");
        }
    }
}
