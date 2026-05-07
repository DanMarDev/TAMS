import { useEffect, useState } from 'react';
import { useWarranty } from '../../hooks/useWarranties';
import { useNotification } from '../../context/NotificationContext';
import ConfirmDialog from '../common/ConfirmDialog';
import {
  getWarrantyStatus,
  getWarrantyStatusBadgeClasses,
  daysUntil,
} from '../../utils/warrantyStatus';

const MODE_TERM = 'term';
const MODE_END = 'end';

const todayIso = () => new Date().toISOString().slice(0, 10);

function StatusBadge({ endDate }) {
  const status = getWarrantyStatus(endDate);
  if (!status) return null;
  const days = daysUntil(endDate);
  const suffix =
    status === 'Expired'
      ? `${Math.abs(days)} day${Math.abs(days) === 1 ? '' : 's'} ago`
      : `${days} day${days === 1 ? '' : 's'} left`;
  return (
    <span
      className={`inline-flex items-center gap-2 text-xs font-medium px-2.5 py-1 rounded-full ${getWarrantyStatusBadgeClasses(status)}`}
    >
      <span className="font-semibold">{status}</span>
      <span className="opacity-75">· {suffix}</span>
    </span>
  );
}

function fmtDate(v) {
  if (!v) return '—';
  return new Date(v).toLocaleDateString();
}

export default function WarrantyCard({ itemId }) {
  const { warranty, loading, error, saveWarranty, deleteWarranty } = useWarranty(itemId);
  const { notify } = useNotification();

  const [editing, setEditing] = useState(false);
  const [mode, setMode] = useState(MODE_TERM);
  const [startDate, setStartDate] = useState('');
  const [termMonths, setTermMonths] = useState('');
  const [endDate, setEndDate] = useState('');
  const [notes, setNotes] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const [formError, setFormError] = useState(null);

  useEffect(() => {
    if (editing) {
      setStartDate(warranty?.warrantyStartDate ?? todayIso());
      setEndDate(warranty?.warrantyEndDate ?? '');
      setTermMonths('');
      setMode(warranty?.warrantyEndDate && !warranty?.warrantyStartDate ? MODE_END : MODE_TERM);
      setNotes(warranty?.notes ?? '');
      setFormError(null);
    }
  }, [editing, warranty]);

  const handleSave = async (e) => {
    e.preventDefault();
    setFormError(null);

    const payload = { itemId: Number(itemId), notes: notes || null, isManualEntry: true };

    if (mode === MODE_TERM) {
      if (!startDate || !termMonths) {
        setFormError('Start date and term length are required.');
        return;
      }
      const months = Number(termMonths);
      if (!Number.isFinite(months) || months <= 0) {
        setFormError('Term length must be a positive number of months.');
        return;
      }
      payload.warrantyStartDate = startDate;
      payload.termMonths = months;
    } else {
      if (!endDate) {
        setFormError('End date is required.');
        return;
      }
      payload.warrantyEndDate = endDate;
      if (startDate) payload.warrantyStartDate = startDate;
    }

    setSubmitting(true);
    try {
      await saveWarranty(payload);
      notify('Warranty saved', 'success');
      setEditing(false);
    } catch (err) {
      const msg = err.response?.data?.message || err.response?.data || 'Failed to save warranty';
      setFormError(typeof msg === 'string' ? msg : 'Failed to save warranty');
    } finally {
      setSubmitting(false);
    }
  };

  const handleDelete = async () => {
    try {
      await deleteWarranty();
      notify('Warranty removed', 'success');
    } catch (err) {
      notify(err.response?.data?.message || 'Delete failed', 'error');
    } finally {
      setConfirmDelete(false);
    }
  };

  return (
    <div className="bg-white border rounded-lg p-6 space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-lg font-semibold">Warranty</h2>
        {warranty?.warrantyEndDate && <StatusBadge endDate={warranty.warrantyEndDate} />}
      </div>

      {loading ? (
        <div className="text-slate-500">Loading…</div>
      ) : error ? (
        <div className="text-red-600">{error}</div>
      ) : !editing ? (
        warranty ? (
          <div className="space-y-3">
            <dl className="grid grid-cols-2 gap-4">
              <div>
                <dt className="text-xs uppercase text-slate-500">Start Date</dt>
                <dd>{fmtDate(warranty.warrantyStartDate)}</dd>
              </div>
              <div>
                <dt className="text-xs uppercase text-slate-500">End Date</dt>
                <dd>{fmtDate(warranty.warrantyEndDate)}</dd>
              </div>
            </dl>
            {warranty.notes && (
              <div>
                <dt className="text-xs uppercase text-slate-500">Notes</dt>
                <dd className="whitespace-pre-wrap">{warranty.notes}</dd>
              </div>
            )}
            <div className="flex gap-2 pt-2">
              <button
                onClick={() => setEditing(true)}
                className="bg-slate-900 text-white px-3 py-1.5 rounded hover:bg-slate-800 text-sm"
              >
                Edit
              </button>
              <button
                onClick={() => setConfirmDelete(true)}
                className="bg-red-600 text-white px-3 py-1.5 rounded hover:bg-red-700 text-sm"
              >
                Remove
              </button>
            </div>
          </div>
        ) : (
          <div className="space-y-3">
            <p className="text-slate-500 text-sm">No warranty recorded for this item.</p>
            <button
              onClick={() => setEditing(true)}
              className="bg-slate-900 text-white px-3 py-1.5 rounded hover:bg-slate-800 text-sm"
            >
              Add warranty
            </button>
          </div>
        )
      ) : (
        <form onSubmit={handleSave} className="space-y-4">
          <div className="flex gap-2 text-sm">
            <button
              type="button"
              onClick={() => setMode(MODE_TERM)}
              className={`px-3 py-1.5 rounded border ${
                mode === MODE_TERM
                  ? 'bg-slate-900 text-white border-slate-900'
                  : 'bg-white text-slate-700 border-slate-300 hover:bg-slate-50'
              }`}
            >
              Length in months
            </button>
            <button
              type="button"
              onClick={() => setMode(MODE_END)}
              className={`px-3 py-1.5 rounded border ${
                mode === MODE_END
                  ? 'bg-slate-900 text-white border-slate-900'
                  : 'bg-white text-slate-700 border-slate-300 hover:bg-slate-50'
              }`}
            >
              Explicit end date
            </button>
          </div>

          {mode === MODE_TERM ? (
            <div className="grid grid-cols-2 gap-4">
              <label className="block text-sm">
                <span className="text-slate-600">Start date</span>
                <input
                  type="date"
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                  className="mt-1 w-full border rounded px-2 py-1.5"
                  required
                />
              </label>
              <label className="block text-sm">
                <span className="text-slate-600">Term (months)</span>
                <input
                  type="number"
                  min="1"
                  step="1"
                  value={termMonths}
                  onChange={(e) => setTermMonths(e.target.value)}
                  className="mt-1 w-full border rounded px-2 py-1.5"
                  placeholder="e.g. 12"
                  required
                />
              </label>
            </div>
          ) : (
            <div className="grid grid-cols-2 gap-4">
              <label className="block text-sm">
                <span className="text-slate-600">Start date (optional)</span>
                <input
                  type="date"
                  value={startDate}
                  onChange={(e) => setStartDate(e.target.value)}
                  className="mt-1 w-full border rounded px-2 py-1.5"
                />
              </label>
              <label className="block text-sm">
                <span className="text-slate-600">End date</span>
                <input
                  type="date"
                  value={endDate}
                  onChange={(e) => setEndDate(e.target.value)}
                  className="mt-1 w-full border rounded px-2 py-1.5"
                  required
                />
              </label>
            </div>
          )}

          <label className="block text-sm">
            <span className="text-slate-600">Notes</span>
            <textarea
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
              className="mt-1 w-full border rounded px-2 py-1.5"
              rows={2}
            />
          </label>

          {formError && <div className="text-red-600 text-sm">{formError}</div>}

          <div className="flex gap-2">
            <button
              type="submit"
              disabled={submitting}
              className="bg-slate-900 text-white px-3 py-1.5 rounded hover:bg-slate-800 text-sm disabled:opacity-50"
            >
              {submitting ? 'Saving…' : 'Save'}
            </button>
            <button
              type="button"
              onClick={() => setEditing(false)}
              className="px-3 py-1.5 rounded border border-slate-300 hover:bg-slate-50 text-sm"
            >
              Cancel
            </button>
          </div>
        </form>
      )}

      {confirmDelete && (
        <ConfirmDialog
          title="Remove warranty?"
          message="Permanently remove warranty info for this item?"
          confirmLabel="Remove"
          danger
          onConfirm={handleDelete}
          onCancel={() => setConfirmDelete(false)}
        />
      )}
    </div>
  );
}
