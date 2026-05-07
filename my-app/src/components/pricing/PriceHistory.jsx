const SOURCE_LABELS = {
  manual: 'Manual',
  ebay_api: 'eBay',
  third_party_api: 'Third party',
};

const fmtMoney = (v) => (v != null ? `$${Number(v).toFixed(2)}` : '—');
const fmtDate = (v) => (v ? new Date(v).toLocaleString() : '—');

export default function PriceHistory({ history }) {
  if (!history || history.length === 0) {
    return <p className="text-sm text-slate-500">No price history yet.</p>;
  }

  return (
    <div className="overflow-x-auto">
      <table className="w-full text-sm">
        <thead>
          <tr className="text-left text-xs uppercase text-slate-500 border-b">
            <th className="py-2 pr-4">Date</th>
            <th className="py-2 pr-4">Source</th>
            <th className="py-2 pr-4 text-right">Estimated Value</th>
          </tr>
        </thead>
        <tbody>
          {history.map((v) => (
            <tr key={v.valuationId} className="border-b last:border-b-0">
              <td className="py-2 pr-4">{fmtDate(v.retrievedAt)}</td>
              <td className="py-2 pr-4">
                <span className="inline-block text-xs px-2 py-0.5 rounded bg-slate-100 text-slate-700">
                  {SOURCE_LABELS[v.source] ?? v.source}
                </span>
              </td>
              <td className="py-2 pr-4 text-right font-medium">
                {fmtMoney(v.estimatedValue)}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
