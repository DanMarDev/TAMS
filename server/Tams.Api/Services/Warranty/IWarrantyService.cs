using Tams.Api.Models;

namespace Tams.Api.Services.Warranty
{
    public interface IWarrantyService
    {
        // ====== Warranty CRUD ======
        Task<ItemWarranty?> GetWarrantyByItemIdAsync(int itemId, int userId);
        Task<IEnumerable<ItemWarranty>> GetWarrantiesByUserIdAsync(int userId);
        Task<int> SaveWarrantyAsync(ItemWarranty warranty);
        Task<bool> DeleteWarrantyAsync(int warrantyId, int userId);

        // ====== Warranty Status ======

        /// <summary>
        /// Computes the display status of a warranty based on its end date.
        /// Returns "Active", "Expiring Soon" (within 30 days), or "Expired".
        /// Returns null if the warranty has no end date.
        /// </summary>
        string? ComputeWarrantyStatus(ItemWarranty warranty);

        /// <summary>
        /// Computes the warranty end date from a start date and term length in months.
        /// Used when the user provides a term length instead of an explicit end date.
        /// </summary>
        DateOnly ComputeEndDate(DateOnly startDate, int termMonths);

        // ====== Expiring Warranties ======

        /// <summary>
        /// Returns all warranties for the user that expire within the next 30 days
        /// and have not yet expired. Used to populate the dashboard expiring warranties card.
        /// </summary>
        Task<IEnumerable<ItemWarranty>> GetExpiringWarrantiesAsync(int userId);

        // ====== Warranty Alerts ======
        Task<IEnumerable<WarrantyAlert>> GetActiveAlertsAsync(int userId);
        Task<bool> DismissAlertAsync(int alertId, int userId);
    }
}