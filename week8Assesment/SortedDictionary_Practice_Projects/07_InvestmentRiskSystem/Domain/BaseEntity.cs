namespace Domain
{
    public class Investment
    {
        public string InvestmentId { get; set; }
        public string AssetName { get; set; }
        public int RiskRating { get; set; }

        public Investment(string investmentId, string assetName, int riskRating)
        {
            InvestmentId = investmentId;
            AssetName = assetName;
            RiskRating = riskRating;
        }
    }
}
