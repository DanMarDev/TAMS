import { Link } from 'react-router-dom';
import {
  getWarrantyStatus,
  getWarrantyStatusBadgeClasses,
  daysUntil,
} from '../../utils/warrantyStatus';

function fmtDate(v) {
  if (!v) return '—';
  return new Date(v).toLocaleDateString();
}

export default function ExpiringWarranties({ warranties, itemNameById }) {
  const sorted = [...(warranties ?? [])].sort((a, b) => {
    const ad = a.warrantyEndDate ? new Date(a.warrantyEndDate).getTime() : Infinity;
    const bd = b.warrantyEndDate ? new Date(b.warrantyEndDate).getTime() : Infinity;
    return ad - bd;
  });

  return (
    <section className="bg-white border rounded-lg p-5 space-y-3">
      <h2 className="text-lg font-semibold">Expiring warranties</h2>
      {sorted.length === 0 ? (
        <p className="text-slate-500 italic">No warranties expiring in the next 30 days.</p>
      ) : (
        <ul className="divide-y">
          {sorted.map((w) => {
            const status = getWarrantyStatus(w.warrantyEndDate);
            const days = daysUntil(w.warrantyEndDate);
            const name = itemNameById?.get(w.itemId) ?? `Item #${w.itemId}`;
            return (
              <li key={w.itemWarrantyId} className="py-2 flex items-center justify-between gap-3">
                <Link
                  to={`/inventory/${w.itemId}`}
                  className="text-blue-600 hover:underline truncate"
                >
                  {name}
                </Link>
                <div className="flex items-center gap-3 shrink-0">
                  <span className="text-sm text-slate-500">
                    Ends {fmtDate(w.warrantyEndDate)}
                    {days != null && ` · ${days} day${days === 1 ? '' : 's'}`}
                  </span>
                  {status && (
                    <span
                      className={`inline-flex items-center text-xs font-medium px-2.5 py-1 rounded-full ${getWarrantyStatusBadgeClasses(status)}`}
                    >
                      {status}
                    </span>
                  )}
                </div>
              </li>
            );
          })}
        </ul>
      )}
    </section>
  );
}
