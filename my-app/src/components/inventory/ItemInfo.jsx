const Field = ({ label, value }) => (
  <div>
    <dt className="text-xs uppercase text-slate-500">{label}</dt>
    <dd className="text-base">{value ?? '—'}</dd>
  </div>
);

export default function ItemInfo({ item, categoryName, brandName }) {
  const fmtMoney = (v) => (v != null ? `$${Number(v).toFixed(2)}` : null);
  const fmtDate = (v) => (v ? new Date(v).toLocaleDateString() : null);

  return (
    <div className="bg-white border rounded-lg p-6 space-y-4">
      <h1 className="text-2xl font-semibold">{item.name}</h1>
      <dl className="grid grid-cols-2 gap-4">
        <Field label="Category" value={categoryName} />
        <Field label="Brand" value={brandName} />
        <Field label="Model" value={item.model} />
        <Field label="Condition" value={item.condition} />
        <Field label="Purchase Date" value={fmtDate(item.purchaseDate)} />
        <Field label="Purchase Price" value={fmtMoney(item.purchasePrice)} />
        <Field label="Original Value" value={fmtMoney(item.originalValue)} />
        <Field label="Sell Threshold" value={fmtMoney(item.maybeSellThreshold)} />
      </dl>
      {item.notes && (
        <div>
          <dt className="text-xs uppercase text-slate-500">Notes</dt>
          <dd className="whitespace-pre-wrap">{item.notes}</dd>
        </div>
      )}
    </div>
  );
}
