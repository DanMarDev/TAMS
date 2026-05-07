import { useState } from 'react';
import { usePricing } from '../../hooks/usePricing';
import { useNotification } from '../../context/NotificationContext';
import PriceLookupButton from './PriceLookupButton';
import PriceHistory from './PriceHistory';
import PriceChart from './PriceChart';

const SOURCE_LABELS = {
  manual: 'Manual override',
  ebay_api: 'eBay',
  third_party_api: 'Third-party API',
};

const fmtMoney = (v) => (v != null ? `$${Number(v).toFixed(2)}` : '—');
const fmtDate = (v) => (v ? new Date(v).toLocaleString() : '—');

export default function PricingCard({ itemId, sellThreshold }) {
  const {
    latest,
    history,
    loading,
    error,
    looking,
    submittingManual,
    lookup,
    submitManual,
  } = usePricing(itemId);
  const { notify } = useNotification();

  const [manualValue, setManualValue] = useState('');
  const [manualError, setManualError] = useState(null);

  const handleLookup = async () => {
    try {
      await lookup();
      notify('Price lookup complete', 'success');
    } catch (err) {
      const msg =
        err.code === 'ECONNABORTED'
          ? 'Price lookup timed out after 10 seconds. Please try again.'
          : err.response?.data?.message || err.response?.data || 'Price lookup failed';
      notify(typeof msg === 'string' ? msg : 'Price lookup failed', 'error');
    }
  };

  const handleManualSubmit = async (e) => {
    e.preventDefault();
    setManualError(null);
    const num = Number(manualValue);
    if (!manualValue || !Number.isFinite(num) || num <= 0) {
      setManualError('Enter a value greater than zero.');
      return;
    }
    try {
      await submitManual(num);
      setManualValue('');
      notify('Manual valuation saved', 'success');
    } catch (err) {
      const msg = err.response?.data?.message || err.response?.data || 'Save failed';
      setManualError(typeof msg === 'string' ? msg : 'Save failed');
    }
  };

  const showSellHint =
    latest?.estimatedValue != null &&
    sellThreshold != null &&
    Number(latest.estimatedValue) >= Number(sellThreshold);

  return (
    <div className="bg-white border rounded-lg p-6 space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-lg font-semibold">Pricing</h2>
        <PriceLookupButton onLookup={handleLookup} loading={looking} disabled={loading} />
      </div>

      {loading ? (
        <div className="text-slate-500">Loading…</div>
      ) : error ? (
        <div className="text-red-600">{error}</div>
      ) : (
        <>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <dt className="text-xs uppercase text-slate-500">Latest Estimate</dt>
              <dd className="text-2xl font-semibold">
                {fmtMoney(latest?.estimatedValue)}
              </dd>
              {latest && (
                <p className="text-xs text-slate-500 mt-1">
                  {SOURCE_LABELS[latest.source] ?? latest.source} · {fmtDate(latest.retrievedAt)}
                </p>
              )}
            </div>
            <div>
              <dt className="text-xs uppercase text-slate-500">Sell Threshold</dt>
              <dd className="text-base">{fmtMoney(sellThreshold)}</dd>
              {showSellHint && (
                <p className="mt-1 inline-block text-xs px-2 py-0.5 rounded bg-amber-100 text-amber-800">
                  Above sell threshold — consider selling
                </p>
              )}
            </div>
          </div>

          <div>
            <h3 className="text-sm font-semibold mb-2">Price History</h3>
            <PriceChart history={history} />
            <div className="mt-3">
              <PriceHistory history={history} />
            </div>
          </div>

          <form onSubmit={handleManualSubmit} className="border-t pt-4 space-y-2">
            <label className="block text-sm font-medium">Manual override</label>
            <div className="flex gap-2">
              <div className="relative flex-1">
                <span className="absolute inset-y-0 left-0 flex items-center pl-3 text-slate-500">
                  $
                </span>
                <input
                  type="number"
                  min="0.01"
                  step="0.01"
                  value={manualValue}
                  onChange={(e) => setManualValue(e.target.value)}
                  placeholder="Enter value"
                  className="w-full border rounded pl-7 pr-3 py-2 focus:outline-none focus:ring-2 focus:ring-slate-500"
                />
              </div>
              <button
                type="submit"
                disabled={submittingManual}
                className="bg-slate-900 text-white px-4 py-2 rounded hover:bg-slate-800 text-sm disabled:opacity-50"
              >
                {submittingManual ? 'Saving…' : 'Save'}
              </button>
            </div>
            {manualError && <p className="text-sm text-red-600">{manualError}</p>}
            <p className="text-xs text-slate-500">
              Records a manual valuation that overrides the automated estimate.
            </p>
          </form>
        </>
      )}
    </div>
  );
}
