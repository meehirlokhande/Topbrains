using System;
using System.Collections.Generic;
using Domain;
using Exceptions;

namespace Services
{
    public class ManagementService
    {
        // Ascending by severity (1 = highest priority first)
        private SortedDictionary<int, Queue<Patient>> _data = new SortedDictionary<int, Queue<Patient>>();

        public void AddPatient(Patient patient)
        {
            if (patient.SeverityLevel < 1 || patient.SeverityLevel > 5)
                throw new InvalidSeverityLevelException("Invalid Severity Level");

            foreach (var kvp in _data)
            {
                foreach (var p in kvp.Value)
                {
                    if (p.PatientId == patient.PatientId)
                        throw new PatientNotFoundException("Duplicate Patient");
                }
            }

            if (!_data.ContainsKey(patient.SeverityLevel))
                _data[patient.SeverityLevel] = new Queue<Patient>();

            _data[patient.SeverityLevel].Enqueue(patient);
        }

        public void GetAllPatients()
        {
            foreach (var kvp in _data)
            {
                foreach (var p in kvp.Value)
                {
                    Console.WriteLine($"Details: {p.PatientId} {p.Name} {p.SeverityLevel}");
                }
            }
        }

        public void UpdateSeverity(string patientId, int newSeverity)
        {
            if (newSeverity < 1 || newSeverity > 5)
                throw new InvalidSeverityLevelException("Invalid Severity Level");

            foreach (var kvp in _data)
            {
                var queue = kvp.Value;
                var tempList = new List<Patient>(queue);
                for (int i = 0; i < tempList.Count; i++)
                {
                    if (tempList[i].PatientId == patientId)
                    {
                        Patient found = tempList[i];
                        int oldSeverity = found.SeverityLevel;

                        tempList.RemoveAt(i);
                        queue.Clear();
                        foreach (var p in tempList)
                            queue.Enqueue(p);
                        if (queue.Count == 0)
                            _data.Remove(oldSeverity);

                        found.SeverityLevel = newSeverity;
                        if (!_data.ContainsKey(newSeverity))
                            _data[newSeverity] = new Queue<Patient>();
                        _data[newSeverity].Enqueue(found);

                        Console.WriteLine($"Updated Severity: {newSeverity}");
                        return;
                    }
                }
            }
            throw new PatientNotFoundException("Patient Not Found");
        }
    }
}
