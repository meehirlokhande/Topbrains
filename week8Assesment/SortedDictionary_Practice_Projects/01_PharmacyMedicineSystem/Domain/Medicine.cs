namespace Domain
{
    public class Medicine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int ExpiryYear { get; set; }

        public Medicine(string id, string name, int price, int expiryYear)
        {
            Id = id;
            Name = name;
            Price = price;
            ExpiryYear = expiryYear;
        }
    }
}
