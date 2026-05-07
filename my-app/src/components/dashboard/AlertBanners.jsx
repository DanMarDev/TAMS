import { Link } from 'react-router-dom';

const TYPE_LABEL = {
  warranty_expiring_30d: 'Warranty expiring in 30 days',
  warranty_expiring_7d: 'Warranty expiring in 7 days',
  warranty_expired: 'Warranty expired',
};

const TYPE_CLASSES = {
  warranty_expiring_30d: 'bg-amber-50 border-amber-200 text-amber-900',
  warranty_expiring_7d: 'bg-orange-50 border-orange-200 text-orange-900',
  warranty_expired: 'bg-red-50 border-red-200 text-red-900',
};

export default function AlertBanners({ alerts, itemNameById, onDismiss }) {
  if (!alerts || alerts.length === 0) return null;

  return (
    <div className="space-y-2">
      {alerts.map((alert) => {
        const label = TYPE_LABEL[alert.alertType] ?? 'Warranty alert';
        const classes = TYPE_CLASSES[alert.alertType] ?? 'bg-slate-50 border-slate-200 text-slate-900';
        const itemName = itemNameById?.get(alert.itemId) ?? `Item #${alert.itemId}`;
        return (
          <div
            key={alert.alertId}
            className={`flex items-center justify-between gap-3 border rounded-lg px-4 py-3 ${classes}`}
            role="alert"
          >
            <div className="flex flex-col text-sm">
              <span className="font-medium">{label}</span>
              <Link
                to={`/inventory/${alert.itemId}`}
                className="hover:underline opacity-90"
              >
                {itemName}
              </Link>
            </div>
            <button
              type="button"
              onClick={() => onDismiss(alert.alertId)}
              className="shrink-0 text-sm font-medium px-2 py-1 rounded hover:bg-white/40"
              aria-label="Dismiss alert"
            >
              Dismiss
            </button>
          </div>
        );
      })}
    </div>
  );
}
