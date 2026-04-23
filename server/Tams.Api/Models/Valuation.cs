namespace Tams.Api.Models
{
    internal class Valuation
    {
        public int ValuationId { get; set; }
        public int ItemId { get; set; }
        public decimal? EstimatedValue { get; set; }
        public string Source { get; set; } = ValuationSources.Manual;
        public DateTime RetrievedAt { get; set; }
    }

    internal static class ValuationSources
    {
        public const string Manual = "manual";
        public const string Ebay = "ebay_api";
        public const string ThirdPartyApi = "third_party_api";
    }
}