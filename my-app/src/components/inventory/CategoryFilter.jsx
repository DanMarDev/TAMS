export default function CategoryFilter({ categories, value, onChange }) {
  return (
    <div className="flex items-center gap-2">
      <label className="text-sm font-medium">Category:</label>
      <select
        value={value}
        onChange={(e) => onChange(e.target.value)}
        className="border rounded px-2 py-1 focus:outline-none focus:ring-2 focus:ring-slate-500"
      >
        <option value="all">All</option>
        {categories.map((c) => (
          <option key={c.categoryId} value={c.categoryId}>
            {c.name}
          </option>
        ))}
      </select>
    </div>
  );
}
