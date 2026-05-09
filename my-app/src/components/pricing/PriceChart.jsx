import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from 'recharts';

export default function PriceChart({ history }) {
  const points = [...(history ?? [])]
    .filter((v) => v.estimatedValue != null && v.retrievedAt)
    .sort((a, b) => new Date(a.retrievedAt) - new Date(b.retrievedAt))
    .map((v, i) => ({
      i,
      label: new Date(v.retrievedAt).toLocaleString(),
      value: Number(v.estimatedValue),
  }));


  if (points.length < 2) {
    return (
      <p className="text-sm text-slate-500">
        Chart will appear once at least two valuations are recorded.
      </p>
    );
  }

  return (
    <ResponsiveContainer width="100%" height={160}>
      <LineChart data={points}>
        <XAxis 
          dataKey="i" 
          type="number"
          domain={[0, points.length - 1]}
          tickFormatter={(t) => points[t]?.label.split(',')[0] ?? ''}
          tick={{ fontSize: 10 }} />
        <YAxis tickFormatter={(v) => `$${v}`} tick={{ fontSize: 10 }} />
        <Tooltip 
          labelFormatter={(t) => points[t]?.label ?? ''}
          formatter={(v) => [`$${v}`, 'Value']} 
        />
        <Line type="monotone" dataKey="value" stroke="#0f172a" dot={{ r: 2.5 }} strokeWidth={1.5} />
      </LineChart>
    </ResponsiveContainer>
  );
}
