namespace Domain
{
    public class Violation
    {
        public string VehicleNumber { get; set; }
        public string OwnerName { get; set; }
        public int FineAmount { get; set; }

        public Violation(string vehicleNumber, string ownerName, int fineAmount)
        {
            VehicleNumber = vehicleNumber;
            OwnerName = ownerName;
            FineAmount = fineAmount;
        }
    }
}
