import { useState } from 'react';
import { useNotification } from '../../context/NotificationContext';

const CONDITIONS = ['New', 'Like New', 'Good', 'Fair', 'Poor'];
const CREATE_NEW = '__create_new__';
const DEFAULT_SELL_THRESHOLD = 50.0;

const buildInitial = (item) => ({
  name: item?.name ?? '',
  model: item?.model ?? '',
  categoryId: item?.categoryId != null ? String(item.categoryId) : '',
  brandId: item?.brandId != null ? String(item.brandId) : '',
  purchaseDate: item?.purchaseDate ? String(item.purchaseDate).slice(0, 10) : '',
  purchasePrice: item?.purchasePrice != null ? String(item.purchasePrice) : '',
  condition: item?.condition ?? 'Good',
  originalValue: item?.originalValue != null ? String(item.originalValue) : '',
  maybeSellThreshold:
    item?.maybeSellThreshold != null ? String(item.maybeSellThreshold) : '',
  notes: item?.notes ?? '',
});

export default function ItemFormModal({
  mode,
  initial,
  categories,
  brands,
  onCreateCategory,
  onCreateBrand,
  onSubmit,
  onClose,
}) {
  const { notify } = useNotification();
  const [form, setForm] = useState(() => buildInitial(initial));
  const [newCategoryName, setNewCategoryName] = useState('');
  const [newBrandName, setNewBrandName] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState('');

  const update = (key) => (e) => setForm({ ...form, [key]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    if (!form.name.trim()) {
      setError('Name is required');
      return;
    }

    setSubmitting(true);
    try {
      let categoryId = form.categoryId;
      let brandId = form.brandId;

      if (categoryId === CREATE_NEW) {
        if (!newCategoryName.trim()) throw new Error('New category name is required');
        categoryId = await onCreateCategory(newCategoryName.trim());
      }
      if (brandId === CREATE_NEW) {
        if (!newBrandName.trim()) throw new Error('New brand name is required');
        brandId = await onCreateBrand(newBrandName.trim());
      }

      if (!categoryId) {
        throw new Error('Category is required');
      }

      const payload = {
        name: form.name.trim(),
        categoryId: Number(categoryId),
        brandId: brandId ? Number(brandId) : null,
        model: form.model.trim() || null,
        purchaseDate: form.purchaseDate || null,
        purchasePrice: form.purchasePrice === '' ? null : Number(form.purchasePrice),
        maybeSellThreshold:
          form.maybeSellThreshold === ''
            ? DEFAULT_SELL_THRESHOLD
            : Number(form.maybeSellThreshold),
        originalValue: form.originalValue === '' ? null : Number(form.originalValue),
        condition: form.condition || 'Good',
        notes: form.notes.trim() || null,
      };

      await onSubmit(payload);
      notify(mode === 'add' ? 'Item added' : 'Item updated', 'success');
    } catch (err) {
      const msg = err.response?.data?.message || err.message || 'Save failed';
      setError(msg);
      notify(msg, 'error');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center z-40">
      <div className="bg-white rounded-lg shadow-xl w-full max-w-2xl max-h-[90vh] overflow-y-auto">
        <div className="flex items-center justify-between p-4 border-b">
          <h2 className="text-xl font-semibold">
            {mode === 'add' ? 'Add Item' : 'Edit Item'}
          </h2>
          <button
            type="button"
            onClick={onClose}
            className="text-slate-500 hover:text-slate-700 text-xl leading-none"
            aria-label="Close"
          >
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit} className="p-4 space-y-4">
          <div>
            <label className="block text-sm font-medium mb-1">Name *</label>
            <input
              type="text"
              required
              maxLength={255}
              value={form.name}
              onChange={update('name')}
              className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium mb-1">Category *</label>
              <select
                value={form.categoryId}
                onChange={update('categoryId')}
                required
                className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
              >
                <option value="">— Select —</option>
                {categories.map((c) => (
                  <option key={c.categoryId} value={c.categoryId}>
                    {c.name}
                  </option>
                ))}
                <option value={CREATE_NEW}>+ Create new…</option>
              </select>
              {form.categoryId === CREATE_NEW && (
                <input
                  type="text"
                  placeholder="New category name"
                  value={newCategoryName}
                  onChange={(e) => setNewCategoryName(e.target.value)}
                  className="mt-2 w-full border rounded px-3 py-2"
                />
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-1">Brand</label>
              <select
                value={form.brandId}
                onChange={update('brandId')}
                className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
              >
                <option value="">— None —</option>
                {brands.map((b) => (
                  <option key={b.brandId} value={b.brandId}>
                    {b.name}
                  </option>
                ))}
                <option value={CREATE_NEW}>+ Create new…</option>
              </select>
              {form.brandId === CREATE_NEW && (
                <input
                  type="text"
                  placeholder="New brand name"
                  value={newBrandName}
                  onChange={(e) => setNewBrandName(e.target.value)}
                  className="mt-2 w-full border rounded px-3 py-2"
                />
              )}
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium mb-1">Model</label>
            <input
              type="text"
              maxLength={255}
              value={form.model}
              onChange={update('model')}
              className="w-full border rounded px-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
            />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium mb-1">Purchase Date</label>
              <input
                type="date"
                value={form.purchaseDate}
                onChange={update('purchaseDate')}
                className="w-full border rounded px-3 py-2"
              />
            </div>
            <div>
              <label className="block text-sm font-medium mb-1">Purchase Price</label>
              <input
                type="number"
                min="0"
                step="0.01"
                value={form.purchasePrice}
                onChange={update('purchasePrice')}
                className="w-full border rounded px-3 py-2"
              />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium mb-1">Condition</label>
              <select
                value={form.condition}
                onChange={update('condition')}
                className="w-full border rounded px-3 py-2"
              >
                {CONDITIONS.map((c) => (
                  <option key={c} value={c}>
                    {c}
                  </option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-medium mb-1">Original Value</label>
              <input
                type="number"
                min="0"
                step="0.01"
                value={form.originalValue}
                onChange={update('originalValue')}
                className="w-full border rounded px-3 py-2"
              />
            </div>
          </div>

          <div>
            <label className="block text-sm font-medium mb-1">
              Sell Threshold (default $50.00)
            </label>
            <input
              type="number"
              min="0"
              step="0.01"
              value={form.maybeSellThreshold}
              onChange={update('maybeSellThreshold')}
              className="w-full border rounded px-3 py-2"
            />
          </div>

          <div>
            <label className="block text-sm font-medium mb-1">Notes</label>
            <textarea
              rows={3}
              maxLength={1000}
              value={form.notes}
              onChange={update('notes')}
              className="w-full border rounded px-3 py-2"
            />
          </div>

          {error && <p className="text-sm text-red-600">{error}</p>}

          <div className="flex justify-end gap-2 pt-2 border-t">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 rounded border hover:bg-slate-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={submitting}
              className="bg-slate-900 text-white px-4 py-2 rounded hover:bg-slate-800 disabled:opacity-50"
            >
              {submitting ? 'Saving…' : mode === 'add' ? 'Add Item' : 'Save Changes'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
