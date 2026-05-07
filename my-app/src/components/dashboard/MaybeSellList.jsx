import { Link } from 'react-router-dom';

const fmtCurrency = (n) =>
  n == null
    ? '—'
    : new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' }).format(Number(n));

export default function MaybeSellList({ items }) {
  return (
    <section className="bg-white border rounded-lg p-5 space-y-3">
      <div className="flex items-center justify-between">
        <h2 className="text-lg font-semibold">Maybe sell</h2>
        <span className="text-xs text-slate-500">
          Latest value at or above your sell threshold
        </span>
      </div>
      {items.length === 0 ? (
        <p className="text-slate-500 italic">Nothing flagged for sale right now.</p>
      ) : (
        <ul className="divide-y">
          {items.map((item) => (
            <li
              key={item.itemId}
              className="py-2 flex items-center justify-between gap-3"
            >
              <Link
                to={`/inventory/${item.itemId}`}
                className="text-blue-600 hover:underline truncate"
              >
                {item.name}
              </Link>
              <div className="text-sm text-slate-600 shrink-0">
                <span className="font-medium text-amber-700">
                  {fmtCurrency(item.latestEstimatedValue)}
                </span>
                <span className="text-slate-400"> / </span>
                <span>threshold {fmtCurrency(item.maybeSellThreshold)}</span>
              </div>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
