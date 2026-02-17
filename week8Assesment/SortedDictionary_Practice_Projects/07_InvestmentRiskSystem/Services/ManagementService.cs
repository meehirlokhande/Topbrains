using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        private SortedDictionary<int, List<Investment>> _data = new SortedDictionary<int, List<Investment>>();

        public void AddInvestment(Investment investment)
        {
            if (investment.RiskRating < 1 || investment.RiskRating > 5)
                throw new InvalidRiskRatingException("Invalid Risk Rating");

            foreach (var kvp in _data)
            {
                foreach (var inv in kvp.Value)
                {
                    if (inv.InvestmentId == investment.InvestmentId)
                        throw new DuplicateInvestmentException("Duplicate Investment");
                }
            }

            if (!_data.ContainsKey(investment.RiskRating))
                _data[investment.RiskRating] = new List<Investment>();

            _data[investment.RiskRating].Add(investment);
        }

        public void GetAllInvestments()
        {
            foreach (var kvp in _data)
            {
                foreach (var inv in kvp.Value)
                {
                    Console.WriteLine($"Details: {inv.InvestmentId} {inv.AssetName} {inv.RiskRating}");
                }
            }
        }

        public void UpdateRisk(string investmentId, int newRisk)
        {
            if (newRisk < 1 || newRisk > 5)
                throw new InvalidRiskRatingException("Invalid Risk Rating");

            foreach (var kvp in _data)
            {
                foreach (var inv in kvp.Value)
                {
                    if (inv.InvestmentId == investmentId)
                    {
                        int oldRisk = inv.RiskRating;
                        kvp.Value.Remove(inv);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldRisk);

                        inv.RiskRating = newRisk;
                        if (!_data.ContainsKey(newRisk))
                            _data[newRisk] = new List<Investment>();
                        _data[newRisk].Add(inv);

                        Console.WriteLine($"Updated Risk Rating: {newRisk}");
                        return;
                    }
                }
            }
            throw new InvalidRiskRatingException("Investment Not Found");
        }
    }
}
