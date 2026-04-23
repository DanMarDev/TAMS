namespace Tams.Api.Models
{
    internal class ItemConditions
    {
        public const string New = "New";
        public const string LikeNew = "Like New";
        public const string Good = "Good";
        public const string Fair = "Fair";
        public const string Poor = "Poor";
        public static readonly string[] AllConditions = new[] { New, LikeNew, Good, Fair, Poor };
    }
}