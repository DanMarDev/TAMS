import { Link } from 'react-router-dom';

function fmtDate(v) {
  if (!v) return '—';
  return new Date(v).toLocaleDateString();
}

export default function OldestItemsList({ items }) {
  return (
    <section className="bg-white border rounded-lg p-5 space-y-3">
      <h2 className="text-lg font-semibold">Oldest items</h2>
      {items.length === 0 ? (
        <p className="text-slate-500 italic">No items yet.</p>
      ) : (
        <ul className="divide-y">
          {items.map((item) => (
            <li key={item.itemId} className="py-2 flex items-center justify-between">
              <Link
                to={`/inventory/${item.itemId}`}
                className="text-blue-600 hover:underline"
              >
                {item.name}
              </Link>
              <span className="text-sm text-slate-500">
                Purchased {fmtDate(item.purchaseDate)}
              </span>
            </li>
          ))}
        </ul>
      )}
    </section>
  );
}
