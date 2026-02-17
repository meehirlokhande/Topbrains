using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class MedicineUtility
    {
        private SortedDictionary<int, List<Medicine>> _data = new SortedDictionary<int, List<Medicine>>();

        public void AddMedicine(Medicine medicine)
        {
            if (medicine.Price <= 0)
                throw new InvalidPriceException("Invalid Price");

            if (medicine.ExpiryYear < 2024)
                throw new InvalidExpiryYearException("Invalid Expiry Year");

            foreach (var kvp in _data)
            {
                foreach (var m in kvp.Value)
                {
                    if (m.Id == medicine.Id)
                        throw new DuplicateMedicineException("Duplicate Medicine");
                }
            }

            if (!_data.ContainsKey(medicine.ExpiryYear))
                _data[medicine.ExpiryYear] = new List<Medicine>();

            _data[medicine.ExpiryYear].Add(medicine);
        }

        public void GetAllMedicines()
        {
            foreach (var kvp in _data)
            {
                foreach (var m in kvp.Value)
                {
                    Console.WriteLine($"Details: {m.Id} {m.Name} {m.Price} {m.ExpiryYear}");
                }
            }
        }

        public void UpdateMedicinePrice(string id, int newPrice)
        {
            if (newPrice <= 0)
                throw new InvalidPriceException("Invalid Price");

            foreach (var kvp in _data)
            {
                foreach (var m in kvp.Value)
                {
                    if (m.Id == id)
                    {
                        m.Price = newPrice;
                        Console.WriteLine($"Updated Price: {newPrice}");
                        return;
                    }
                }
            }

            throw new MedicineNotFoundException("Medicine Not Found");
        }
    }
}
