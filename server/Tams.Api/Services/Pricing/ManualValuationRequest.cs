using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Pricing
{
    public class ManualValuationRequest
    {
        [Required]
        public int ItemId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than zero.")]
        public decimal Value { get; set; }
    }
}