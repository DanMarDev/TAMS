const WIDTH = 480;
const HEIGHT = 160;
const PADDING = { top: 12, right: 12, bottom: 24, left: 40 };

export default function PriceChart({ history }) {
  if (!history || history.length < 2) {
    return (
      <p className="text-sm text-slate-500">
        Chart will appear once at least two valuations are recorded.
      </p>
    );
  }

  const points = [...history]
    .filter((v) => v.estimatedValue != null && v.retrievedAt)
    .map((v) => ({ t: new Date(v.retrievedAt).getTime(), y: Number(v.estimatedValue) }))
    .sort((a, b) => a.t - b.t);

  if (points.length < 2) {
    return (
      <p className="text-sm text-slate-500">
        Chart will appear once at least two valuations are recorded.
      </p>
    );
  }

  const tMin = points[0].t;
  const tMax = points[points.length - 1].t;
  const yMin = Math.min(...points.map((p) => p.y));
  const yMax = Math.max(...points.map((p) => p.y));
  const yPad = (yMax - yMin) * 0.1 || Math.max(yMax * 0.1, 1);
  const yLo = Math.max(0, yMin - yPad);
  const yHi = yMax + yPad;

  const innerW = WIDTH - PADDING.left - PADDING.right;
  const innerH = HEIGHT - PADDING.top - PADDING.bottom;

  const xFor = (t) =>
    PADDING.left + (tMax === tMin ? innerW / 2 : ((t - tMin) / (tMax - tMin)) * innerW);
  const yFor = (y) =>
    PADDING.top + (yHi === yLo ? innerH / 2 : (1 - (y - yLo) / (yHi - yLo)) * innerH);

  const path = points
    .map((p, i) => `${i === 0 ? 'M' : 'L'} ${xFor(p.t).toFixed(1)} ${yFor(p.y).toFixed(1)}`)
    .join(' ');

  const fmtMoney = (v) => `$${v.toFixed(0)}`;
  const fmtDate = (t) => new Date(t).toLocaleDateString();

  return (
    <svg
      viewBox={`0 0 ${WIDTH} ${HEIGHT}`}
      className="w-full h-40"
      role="img"
      aria-label="Price history chart"
    >
      <line
        x1={PADDING.left}
        y1={PADDING.top}
        x2={PADDING.left}
        y2={HEIGHT - PADDING.bottom}
        stroke="#cbd5e1"
        strokeWidth="1"
      />
      <line
        x1={PADDING.left}
        y1={HEIGHT - PADDING.bottom}
        x2={WIDTH - PADDING.right}
        y2={HEIGHT - PADDING.bottom}
        stroke="#cbd5e1"
        strokeWidth="1"
      />
      <text x={PADDING.left - 6} y={PADDING.top + 4} fontSize="10" fill="#64748b" textAnchor="end">
        {fmtMoney(yHi)}
      </text>
      <text
        x={PADDING.left - 6}
        y={HEIGHT - PADDING.bottom}
        fontSize="10"
        fill="#64748b"
        textAnchor="end"
      >
        {fmtMoney(yLo)}
      </text>
      <text
        x={PADDING.left}
        y={HEIGHT - PADDING.bottom + 14}
        fontSize="10"
        fill="#64748b"
        textAnchor="start"
      >
        {fmtDate(tMin)}
      </text>
      <text
        x={WIDTH - PADDING.right}
        y={HEIGHT - PADDING.bottom + 14}
        fontSize="10"
        fill="#64748b"
        textAnchor="end"
      >
        {fmtDate(tMax)}
      </text>
      <path d={path} fill="none" stroke="#0f172a" strokeWidth="1.5" />
      {points.map((p, i) => (
        <circle key={i} cx={xFor(p.t)} cy={yFor(p.y)} r="2.5" fill="#0f172a" />
      ))}
    </svg>
  );
}
