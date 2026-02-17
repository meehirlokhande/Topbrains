namespace Domain
{
    public class Member
    {
        public string MemberId { get; set; }
        public string Name { get; set; }
        public int FineAmount { get; set; }

        public Member(string memberId, string name, int fineAmount)
        {
            MemberId = memberId;
            Name = name;
            FineAmount = fineAmount;
        }
    }
}
