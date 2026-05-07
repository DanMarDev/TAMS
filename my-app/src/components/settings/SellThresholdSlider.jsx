import { useEffect, useState } from 'react';
import { useNotification } from '../../context/NotificationContext';

const MIN = 0;
const MAX = 1000;
const STEP = 5;

export default function SellThresholdSlider({ profile, onUpdate }) {
  const { notify } = useNotification();
  const [value, setValue] = useState(50);
  const [submitting, setSubmitting] = useState(false);

  useEffect(() => {
    if (profile?.defaultSellThreshold != null) {
      setValue(Number(profile.defaultSellThreshold));
    }
  }, [profile]);

  const dirty = profile && Number(profile.defaultSellThreshold) !== Number(value);

  const handleSave = async () => {
    setSubmitting(true);
    try {
      await onUpdate(Number(value));
      notify('Default sell threshold updated', 'success');
    } catch (err) {
      const msg = err.response?.data?.message || err.response?.data || 'Update failed';
      notify(msg, 'error');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <section className="bg-white rounded-lg shadow p-6 space-y-4">
      <div>
        <h2 className="text-lg font-semibold">Default Sell Threshold</h2>
        <p className="text-sm text-slate-600">
          Items whose estimated value reaches this amount will appear in your "Maybe Sell" list.
          Used as the default when creating new items.
        </p>
      </div>

      <div className="space-y-2">
        <div className="flex items-baseline justify-between">
          <span className="text-sm font-medium">Threshold</span>
          <span className="text-2xl font-semibold text-slate-900">
            ${Number(value).toFixed(2)}
          </span>
        </div>
        <input
          type="range"
          min={MIN}
          max={MAX}
          step={STEP}
          value={value}
          onChange={(e) => setValue(e.target.value)}
          className="w-full"
        />
        <div className="flex justify-between text-xs text-slate-500">
          <span>${MIN}</span>
          <span>${MAX}</span>
        </div>
        <div>
          <input
            type="number"
            min={0}
            step="0.01"
            value={value}
            onChange={(e) => setValue(e.target.value)}
            className="w-32 border rounded px-3 py-2"
          />
        </div>
      </div>

      <div className="flex justify-end">
        <button
          type="button"
          onClick={handleSave}
          disabled={submitting || !dirty}
          className="bg-slate-900 text-white px-4 py-2 rounded hover:bg-slate-800 disabled:opacity-50"
        >
          {submitting ? 'Saving…' : 'Save Threshold'}
        </button>
      </div>
    </section>
  );
}
