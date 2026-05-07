import { useState } from 'react';
import { inventoryApi } from '../../api/inventoryApi';
import { useBrands } from '../../hooks/useBrands';
import { useNotification } from '../../context/NotificationContext';
import ConfirmDialog from '../common/ConfirmDialog';

export default function BrandManager() {
  const { brands, loading, error, refresh, createBrand } = useBrands();
  const { notify } = useNotification();

  const [newName, setNewName] = useState('');
  const [editingId, setEditingId] = useState(null);
  const [editName, setEditName] = useState('');
  const [confirmDelete, setConfirmDelete] = useState(null);

  const userBrands = brands.filter((b) => !b.isOfficial);
  const officialBrands = brands.filter((b) => b.isOfficial);

  const handleAdd = async (e) => {
    e.preventDefault();
    if (!newName.trim()) return;
    try {
      await createBrand(newName.trim());
      setNewName('');
      notify('Brand created', 'success');
    } catch (err) {
      notify(err.response?.data?.message || 'Failed to create brand', 'error');
    }
  };

  const startEdit = (b) => {
    setEditingId(b.brandId);
    setEditName(b.name);
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditName('');
  };

  const saveEdit = async (id) => {
    if (!editName.trim()) return;
    try {
      await inventoryApi.updateBrand(id, { name: editName.trim() });
      await refresh();
      cancelEdit();
      notify('Brand updated', 'success');
    } catch (err) {
      notify(err.response?.data?.message || 'Failed to update brand', 'error');
    }
  };

  const handleDelete = async () => {
    if (!confirmDelete) return;
    try {
      await inventoryApi.deleteBrand(confirmDelete.brandId);
      await refresh();
      notify('Brand deleted', 'success');
    } catch (err) {
      notify(err.response?.data?.message || 'Failed to delete brand', 'error');
    } finally {
      setConfirmDelete(null);
    }
  };

  return (
    <section className="bg-white rounded-lg shadow p-6 space-y-4">
      <h2 className="text-lg font-semibold">Brands</h2>

      <form onSubmit={handleAdd} className="flex gap-2">
        <input
          type="text"
          placeholder="New brand name"
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
          className="border rounded px-3 py-2 flex-1"
        />
        <button
          type="submit"
          className="bg-slate-900 text-white px-4 py-2 rounded hover:bg-slate-800"
        >
          Add
        </button>
      </form>

      {error && <p className="text-sm text-red-600">{error}</p>}
      {loading ? (
        <p className="text-sm text-slate-500">Loading…</p>
      ) : (
        <div className="space-y-4">
          <div>
            <h3 className="text-sm font-semibold text-slate-700 mb-2">Your Brands</h3>
            {userBrands.length === 0 ? (
              <p className="text-sm text-slate-500">No custom brands yet.</p>
            ) : (
              <ul className="divide-y border rounded">
                {userBrands.map((b) => (
                  <li key={b.brandId} className="p-3">
                    {editingId === b.brandId ? (
                      <div className="flex gap-2">
                        <input
                          type="text"
                          value={editName}
                          onChange={(e) => setEditName(e.target.value)}
                          className="border rounded px-3 py-2 flex-1"
                        />
                        <button
                          onClick={() => saveEdit(b.brandId)}
                          className="bg-slate-900 text-white px-3 py-2 rounded"
                        >
                          Save
                        </button>
                        <button
                          onClick={cancelEdit}
                          className="border px-3 py-2 rounded hover:bg-slate-50"
                        >
                          Cancel
                        </button>
                      </div>
                    ) : (
                      <div className="flex items-center justify-between gap-2">
                        <p className="font-medium">{b.name}</p>
                        <div className="flex gap-2">
                          <button
                            onClick={() => startEdit(b)}
                            className="text-sm border px-3 py-1 rounded hover:bg-slate-50"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => setConfirmDelete(b)}
                            className="text-sm border border-red-300 text-red-600 px-3 py-1 rounded hover:bg-red-50"
                          >
                            Delete
                          </button>
                        </div>
                      </div>
                    )}
                  </li>
                ))}
              </ul>
            )}
          </div>

          {officialBrands.length > 0 && (
            <div>
              <h3 className="text-sm font-semibold text-slate-700 mb-2">Built-in Brands</h3>
              <ul className="flex flex-wrap gap-2">
                {officialBrands.map((b) => (
                  <li
                    key={b.brandId}
                    className="text-sm bg-slate-100 text-slate-700 px-2 py-1 rounded"
                  >
                    {b.name}
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>
      )}

      {confirmDelete && (
        <ConfirmDialog
          title="Delete Brand"
          message={`Delete brand "${confirmDelete.name}"? Items using it may be affected.`}
          confirmLabel="Delete"
          danger
          onConfirm={handleDelete}
          onCancel={() => setConfirmDelete(null)}
        />
      )}
    </section>
  );
}
