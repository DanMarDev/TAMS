using Tams.Api.Models;
using Tams.Api.Repos;

namespace Tams.Api.Services.Warranty
{
    internal class WarrantyService(IWarrantyRepository warrantyRepository) : IWarrantyService
    {
        // =========== Warranty CRUD ============

        public async Task<ItemWarranty?> GetWarrantyByItemIdAsync(int itemId, int userId)
        {
            return await warrantyRepository.GetWarrantyByItemIdAsync(itemId);
        }

        public async Task<IEnumerable<ItemWarranty>> GetWarrantiesByUserIdAsync(int userId)
        {
            return await warrantyRepository.GetWarrantyByUserId(userId);
        }

        public async Task<int> SaveWarrantyAsync(ItemWarranty warranty)
        {
            return await warrantyRepository.UpsertWarrantyAsync(warranty);
        }

        public async Task<bool> DeleteWarrantyAsync(int warrantyId, int userId)
        {
            return await warrantyRepository.DeleteWarrantyAsync(warrantyId, userId);
        }

        // ====== Warranty Status ======

        public string? ComputeWarrantyStatus(ItemWarranty warranty)
        {
            if (warranty.WarrantyEndDate is null)
            {
                return null;
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var endDate = warranty.WarrantyEndDate.Value;

            if (endDate < today)
            {
                return "Expired";
            }
            else if (endDate <= today.AddDays(30))
            {
                return "Expiring Soon";
            }
            else
            {
                return "Active";
            }
        }

        public DateOnly ComputeEndDate(DateOnly startDate, int termMonths)
        {
            var endDate = startDate.AddMonths(termMonths);
            return endDate;
        }


        // ====== Expiring Warranties ======
        public async Task<IEnumerable<ItemWarranty>> GetExpiringWarrantiesAsync(int userId)
        {
            return await warrantyRepository.GetExpiringWarrantiesAsync(userId, 30);
        }


        // ====== Warranty Alerts ======
        public async Task<IEnumerable<WarrantyAlert>> GetActiveAlertsAsync(int userId)
        {
            return await warrantyRepository.GetAlertsByUserIdAsync(userId);
        }

        public async Task<bool> DismissAlertAsync(int alertId, int userId)
        {
            return await warrantyRepository.DismissAlertAsync(alertId, userId);
        }
    }
}