using Tams.Api.Models;

namespace Tams.Api.Repos
{
    internal interface IWarrantyRepository
    {
        Task<ItemWarranty?> GetWarrantyByItemIdAsync(int itemId);
        Task<IEnumerable<ItemWarranty>> GetWarrantyByUserId(int userId);
        Task<IEnumerable<ItemWarranty>> GetExpiringWarrantiesAsync(int userId, int daysAhead);
        Task<int> UpsertWarrantyAsync(ItemWarranty warranty);
        Task<bool> DeleteWarrantyAsync(int warrantyId, int userId);
        Task<IEnumerable<WarrantyAlert>> GetAlertsByUserIdAsync(int userId);
        Task<bool> DismissAlertAsync(int alertId, int userId);
        Task<WarrantyPolicy?> GetPolicyByBrandAndCategoryAsync(int brandId, int categoryId);
    }
}