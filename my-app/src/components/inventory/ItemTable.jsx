import { Link } from 'react-router-dom';

const COLUMNS = [
  { key: 'name', label: 'Name' },
  { key: 'categoryName', label: 'Category' },
  { key: 'brandName', label: 'Brand' },
  { key: 'condition', label: 'Condition' },
  { key: 'purchasePrice', label: 'Purchase Price' },
  { key: 'purchaseDate', label: 'Purchase Date' },
];

export default function ItemTable({ items, categoryMap, brandMap, sort, onSort }) {
  if (items.length === 0) {
    return <p className="text-slate-500 italic">No items yet.</p>;
  }

  return (
    <div className="overflow-x-auto">
      <table className="w-full border-collapse">
        <thead>
          <tr className="border-b text-left bg-slate-50">
            {COLUMNS.map((col) => (
              <th
                key={col.key}
                onClick={() => onSort(col.key)}
                className="py-2 px-3 cursor-pointer select-none hover:bg-slate-100 text-sm font-semibold"
              >
                {col.label}
                {sort.key === col.key && (sort.dir === 'asc' ? ' ▲' : ' ▼')}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {items.map((item) => {
            const categoryName = categoryMap.get(item.categoryId) ?? '—';
            const brandName = item.brandId != null ? (brandMap.get(item.brandId) ?? '—') : '—';
            return (
              <tr key={item.itemId} className="border-b hover:bg-slate-50">
                <td className="py-2 px-3">
                  <Link
                    to={`/inventory/${item.itemId}`}
                    className="text-blue-600 hover:underline"
                  >
                    {item.name}
                  </Link>
                </td>
                <td className="py-2 px-3">{categoryName}</td>
                <td className="py-2 px-3">{brandName}</td>
                <td className="py-2 px-3">{item.condition ?? '—'}</td>
                <td className="py-2 px-3">
                  {item.purchasePrice != null ? `$${Number(item.purchasePrice).toFixed(2)}` : '—'}
                </td>
                <td className="py-2 px-3">
                  {item.purchaseDate ? new Date(item.purchaseDate).toLocaleDateString() : '—'}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
