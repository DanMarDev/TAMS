const fmtCurrency = (n) =>
  new Intl.NumberFormat(undefined, { style: 'currency', currency: 'USD' }).format(Number(n) || 0);

function Card({ label, value, accent }) {
  return (
    <div className="bg-white border rounded-lg p-5 flex flex-col gap-1">
      <span className="text-xs uppercase tracking-wide text-slate-500">{label}</span>
      <span className={`text-2xl font-semibold ${accent ?? 'text-slate-900'}`}>{value}</span>
    </div>
  );
}

export default function SummaryCards({ summary }) {
  const totalItems = summary?.totalItems ?? 0;
  const totalValue = summary?.totalEstimatedValue ?? 0;
  const maybeSell = summary?.maybeSellCount ?? 0;
  const expiring = summary?.expiringWarrantyCount ?? 0;

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
      <Card label="Total items" value={totalItems} />
      <Card label="Estimated value" value={fmtCurrency(totalValue)} />
      <Card
        label="Maybe sell"
        value={maybeSell}
        accent={maybeSell > 0 ? 'text-amber-700' : undefined}
      />
      <Card
        label="Warranties expiring"
        value={expiring}
        accent={expiring > 0 ? 'text-amber-700' : undefined}
      />
    </div>
  );
}
