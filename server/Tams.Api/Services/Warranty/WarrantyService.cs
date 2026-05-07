using Tams.Api.Models;
using Tams.Api.Repos;

namespace Tams.Api.Services.Warranty
{
    internal class WarrantyService(
        IWarrantyRepository warrantyRepository,
        IItemRepository itemRepository
    ) : IWarrantyService
    {
        // =========== Warranty CRUD ============

        public async Task<ItemWarranty?> GetWarrantyByItemIdAsync(int itemId, int userId)
        {
            var item = await itemRepository.GetItemAsync(itemId, userId);
            if (item == null)
            {
                throw new KeyNotFoundException("Item not found for the given user.");
            }
            return await warrantyRepository.GetWarrantyByItemIdAsync(itemId);
        }

        public async Task<IEnumerable<ItemWarranty>> GetWarrantiesByUserIdAsync(int userId)
        {
            return await warrantyRepository.GetWarrantyByUserId(userId);
        }

        public async Task<int> SaveWarrantyAsync(WarrantyRequest request, int? warrantyId, int userId)
        {
            var item = await itemRepository.GetItemAsync(request.ItemId, userId);
            if (item == null)
                throw new KeyNotFoundException("Item not found for the given user.");

            var endDate = request.WarrantyEndDate;
            if (endDate == null && request.WarrantyStartDate != null && request.TermMonths != null)
                endDate = ComputeEndDate(request.WarrantyStartDate.Value, request.TermMonths.Value);

            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (request.WarrantyStartDate != null && request.WarrantyStartDate > today)
                throw new ArgumentException("Warranty start date cannot be in the future.");
            if (endDate != null && request.WarrantyStartDate != null && endDate < request.WarrantyStartDate)
                throw new ArgumentException("Warranty end date cannot be before start date.");

            var warranty = new ItemWarranty
            {
                ItemWarrantyId = warrantyId ?? 0,
                ItemId = request.ItemId,
                WarrantyStartDate = request.WarrantyStartDate,
                WarrantyEndDate = endDate,
                IsManualEntry = request.IsManualEntry,
                Notes = request.Notes
            };

            if (ComputeWarrantyStatus(warranty) == "Expiring Soon")
            {
                await CreateAlertAsync(new WarrantyAlert
                {
                    UserId = userId,
                    ItemId = warranty.ItemId,
                    AlertType = WarrantyAlertTypes.Expiring30d
                });
            }

            return await warrantyRepository.UpsertWarrantyAsync(warranty);
        }

        public async Task<bool> DeleteWarrantyAsync(int warrantyId, int userId, int itemId)
        {
            var item = await itemRepository.GetItemAsync(itemId, userId);
            if (item == null)
            {
                throw new KeyNotFoundException("Item not found for the given user. Cannot delete warranty.");
            }
            var warranty = await warrantyRepository.GetWarrantyByItemIdAsync(itemId);
            if (warranty == null || warranty.ItemWarrantyId != warrantyId)            {
                throw new KeyNotFoundException("Warranty not found for the given item and user. Cannot delete.");
            }
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
            if (startDate == default(DateOnly))
            {
                throw new ArgumentException("Start date is required to compute end date.");
            }
            var endDate = startDate.AddMonths(termMonths);
            return endDate;
        }


        // ====== Expiring Warranties ======
        public async Task<IEnumerable<ItemWarranty>> GetExpiringWarrantiesAsync(int userId)
        {
            return await warrantyRepository.GetExpiringWarrantiesAsync(userId, 30);
        }


        // ====== Warranty Alerts ======
        public async Task<bool> CreateAlertAsync(WarrantyAlert alert)
        {
            return await warrantyRepository.CreateAlertAsync(alert);
        }
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