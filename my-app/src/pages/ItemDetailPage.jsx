import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { inventoryApi } from '../api/inventoryApi';
import { useCategories } from '../hooks/useCategories';
import { useBrands } from '../hooks/useBrands';
import { useUserSettings } from '../hooks/useUserSettings';
import { useNotification } from '../context/NotificationContext';
import ItemInfo from '../components/inventory/ItemInfo';
import ItemFormModal from '../components/inventory/ItemFormModal';
import ConfirmDialog from '../components/common/ConfirmDialog';
import WarrantyCard from '../components/warranty/WarrantyCard';
import PricingCard from '../components/pricing/PricingCard';

export default function ItemDetailPage() {
  const { itemId } = useParams();
  const navigate = useNavigate();
  const { notify } = useNotification();
  const { categories, createCategory } = useCategories();
  const { brands, createBrand } = useBrands();
  const { profile } = useUserSettings();

  const [item, setItem] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editing, setEditing] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const [deleting, setDeleting] = useState(false);

  const load = async () => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await inventoryApi.getItem(itemId);
      setItem(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load item');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    load();
  }, [itemId]);

  const handleUpdate = async (payload) => {
    await inventoryApi.updateItem(itemId, payload);
    await load();
    setEditing(false);
  };

  const handleDelete = async () => {
    setDeleting(true);
    try {
      await inventoryApi.deleteItem(itemId);
      notify('Item deleted', 'success');
      navigate('/inventory');
    } catch (err) {
      notify(err.response?.data?.message || 'Delete failed', 'error');
    } finally {
      setDeleting(false);
      setConfirmDelete(false);
    }
  };

  if (loading) return <div className="p-6">Loading…</div>;
  if (error) return <div className="p-6 text-red-600">{error}</div>;
  if (!item) return null;

  const categoryName = categories.find((c) => c.categoryId === item.categoryId)?.name;
  const brandName =
    item.brandId != null
      ? brands.find((b) => b.brandId === item.brandId)?.name
      : null;

  return (
    <div className="p-6 space-y-4">
      <div className="flex items-center justify-between">
        <button
          onClick={() => navigate('/inventory')}
          className="text-blue-600 hover:underline"
        >
          ← Back to Inventory
        </button>
        <div className="flex gap-2">
          <button
            onClick={() => setEditing(true)}
            className="bg-slate-900 text-white px-4 py-2 rounded hover:bg-slate-800"
          >
            Edit
          </button>
          <button
            onClick={() => setConfirmDelete(true)}
            className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
          >
            Delete
          </button>
        </div>
      </div>

      <ItemInfo item={item} categoryName={categoryName} brandName={brandName} />

      <PricingCard itemId={item.itemId} sellThreshold={item.maybeSellThreshold} />

      <WarrantyCard itemId={item.itemId} />

      {editing && (
        <ItemFormModal
          mode="edit"
          initial={item}
          categories={categories}
          brands={brands}
          defaultSellThreshold={profile?.defaultSellThreshold}
          onCreateCategory={createCategory}
          onCreateBrand={createBrand}
          onSubmit={handleUpdate}
          onClose={() => setEditing(false)}
        />
      )}

      {confirmDelete && (
        <ConfirmDialog
          title="Delete item?"
          message={`Permanently delete "${item.name}"? This cannot be undone.`}
          confirmLabel={deleting ? 'Deleting…' : 'Delete'}
          danger
          onConfirm={handleDelete}
          onCancel={() => setConfirmDelete(false)}
        />
      )}
    </div>
  );
}
