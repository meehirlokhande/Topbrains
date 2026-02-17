using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        // Descending by OrderAmount
        private SortedDictionary<int, List<Order>> _data =
            new SortedDictionary<int, List<Order>>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

        public void AddOrder(Order order)
        {
            if (order.OrderAmount <= 0)
                throw new InvalidOrderAmountException("Invalid Order Amount");

            foreach (var kvp in _data)
            {
                foreach (var o in kvp.Value)
                {
                    if (o.OrderId == order.OrderId)
                        throw new OrderNotFoundException("Duplicate Order");
                }
            }

            if (!_data.ContainsKey(order.OrderAmount))
                _data[order.OrderAmount] = new List<Order>();

            _data[order.OrderAmount].Add(order);
        }

        public void GetAllOrders()
        {
            foreach (var kvp in _data)
            {
                foreach (var o in kvp.Value)
                {
                    Console.WriteLine($"Details: {o.OrderId} {o.CustomerName} {o.OrderAmount}");
                }
            }
        }

        public void UpdateOrder(string orderId, int newAmount)
        {
            if (newAmount <= 0)
                throw new InvalidOrderAmountException("Invalid Order Amount");

            foreach (var kvp in _data)
            {
                foreach (var o in kvp.Value)
                {
                    if (o.OrderId == orderId)
                    {
                        int oldAmount = o.OrderAmount;
                        kvp.Value.Remove(o);
                        if (kvp.Value.Count == 0)
                            _data.Remove(oldAmount);

                        o.OrderAmount = newAmount;
                        if (!_data.ContainsKey(newAmount))
                            _data[newAmount] = new List<Order>();
                        _data[newAmount].Add(o);

                        Console.WriteLine($"Updated Order Amount: {newAmount}");
                        return;
                    }
                }
            }
            throw new OrderNotFoundException("Order Not Found");
        }
    }
}
