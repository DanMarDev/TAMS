using System.ComponentModel.DataAnnotations;

namespace Tams.Api.Services.Warranty
{
    public class WarrantyRequest
    {
        [Required]
        public int ItemId { get; set; }

        public DateOnly? WarrantyStartDate { get; set; }

        public DateOnly? WarrantyEndDate { get; set; }
        public int? TermMonths { get; set; }

        public bool IsManualEntry { get; set; } = true;

        public string? Notes { get; set; }
    }
}