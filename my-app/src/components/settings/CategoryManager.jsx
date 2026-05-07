import { useState } from 'react';
import { inventoryApi } from '../../api/inventoryApi';
import { useCategories } from '../../hooks/useCategories';
import { useNotification } from '../../context/NotificationContext';
import ConfirmDialog from '../common/ConfirmDialog';

export default function CategoryManager() {
  const { categories, loading, error, refresh, createCategory } = useCategories();
  const { notify } = useNotification();

  const [newName, setNewName] = useState('');
  const [newDesc, setNewDesc] = useState('');
  const [editingId, setEditingId] = useState(null);
  const [editName, setEditName] = useState('');
  const [editDesc, setEditDesc] = useState('');
  const [confirmDelete, setConfirmDelete] = useState(null);

  const userCategories = categories.filter((c) => !c.isOfficial);
  const officialCategories = categories.filter((c) => c.isOfficial);

  const handleAdd = async (e) => {
    e.preventDefault();
    if (!newName.trim()) return;
    try {
      await createCategory(newName.trim(), newDesc.trim() || null);
      setNewName('');
      setNewDesc('');
      notify('Category created', 'success');
    } catch (err) {
      notify(err.response?.data?.message || 'Failed to create category', 'error');
    }
  };

  const startEdit = (c) => {
    setEditingId(c.categoryId);
    setEditName(c.name);
    setEditDesc(c.description ?? '');
  };

  const cancelEdit = () => {
    setEditingId(null);
    setEditName('');
    setEditDesc('');
  };

  const saveEdit = async (id) => {
    if (!editName.trim()) return;
    try {
      await inventoryApi.updateCategory(id, {
        name: editName.trim(),
        description: editDesc.trim() || null,
      });
      await refresh();
      cancelEdit();
      notify('Category updated', 'success');
    } catch (err) {
      notify(err.response?.data?.message || 'Failed to update category', 'error');
    }
  };

  const handleDelete = async () => {
    if (!confirmDelete) return;
    try {
      await inventoryApi.deleteCategory(confirmDelete.categoryId);
      await refresh();
      notify('Category deleted', 'success');
    } catch (err) {
      notify(err.response?.data?.message || 'Failed to delete category', 'error');
    } finally {
      setConfirmDelete(null);
    }
  };

  return (
    <section className="bg-white rounded-lg shadow p-6 space-y-4">
      <h2 className="text-lg font-semibold">Categories</h2>

      <form onSubmit={handleAdd} className="grid grid-cols-1 md:grid-cols-[1fr,1fr,auto] gap-2">
        <input
          type="text"
          placeholder="New category name"
          value={newName}
          onChange={(e) => setNewName(e.target.value)}
          className="border rounded px-3 py-2"
        />
        <input
          type="text"
          placeholder="Description (optional)"
          value={newDesc}
          onChange={(e) => setNewDesc(e.target.value)}
          className="border rounded px-3 py-2"
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
            <h3 className="text-sm font-semibold text-slate-700 mb-2">Your Categories</h3>
            {userCategories.length === 0 ? (
              <p className="text-sm text-slate-500">No custom categories yet.</p>
            ) : (
              <ul className="divide-y border rounded">
                {userCategories.map((c) => (
                  <li key={c.categoryId} className="p-3">
                    {editingId === c.categoryId ? (
                      <div className="flex flex-col md:flex-row gap-2">
                        <input
                          type="text"
                          value={editName}
                          onChange={(e) => setEditName(e.target.value)}
                          className="border rounded px-3 py-2 flex-1"
                        />
                        <input
                          type="text"
                          value={editDesc}
                          onChange={(e) => setEditDesc(e.target.value)}
                          placeholder="Description"
                          className="border rounded px-3 py-2 flex-1"
                        />
                        <div className="flex gap-2">
                          <button
                            onClick={() => saveEdit(c.categoryId)}
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
                      </div>
                    ) : (
                      <div className="flex items-center justify-between gap-2">
                        <div>
                          <p className="font-medium">{c.name}</p>
                          {c.description && (
                            <p className="text-sm text-slate-500">{c.description}</p>
                          )}
                        </div>
                        <div className="flex gap-2">
                          <button
                            onClick={() => startEdit(c)}
                            className="text-sm border px-3 py-1 rounded hover:bg-slate-50"
                          >
                            Edit
                          </button>
                          <button
                            onClick={() => setConfirmDelete(c)}
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

          {officialCategories.length > 0 && (
            <div>
              <h3 className="text-sm font-semibold text-slate-700 mb-2">Built-in Categories</h3>
              <ul className="flex flex-wrap gap-2">
                {officialCategories.map((c) => (
                  <li
                    key={c.categoryId}
                    className="text-sm bg-slate-100 text-slate-700 px-2 py-1 rounded"
                  >
                    {c.name}
                  </li>
                ))}
              </ul>
            </div>
          )}
        </div>
      )}

      {confirmDelete && (
        <ConfirmDialog
          title="Delete Category"
          message={`Delete category "${confirmDelete.name}"? Items using it may be affected.`}
          confirmLabel="Delete"
          onConfirm={handleDelete}
          onCancel={() => setConfirmDelete(null)}
        />
      )}
    </section>
  );
}
