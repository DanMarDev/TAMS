// Client-side warranty status computation per spec § 6.1
// Returns one of: 'Active', 'Expiring Soon', 'Expired', or null when no end date.

export const WARRANTY_STATUS = {
  ACTIVE: 'Active',
  EXPIRING_SOON: 'Expiring Soon',
  EXPIRED: 'Expired',
};

const MS_PER_DAY = 1000 * 60 * 60 * 24;
const EXPIRING_WINDOW_DAYS = 30;

function toDate(value) {
  if (!value) return null;
  if (value instanceof Date) return value;
  // Accept "YYYY-MM-DD" or full ISO strings
  const d = new Date(value);
  return Number.isNaN(d.getTime()) ? null : d;
}

function startOfDay(d) {
  return new Date(d.getFullYear(), d.getMonth(), d.getDate());
}

export function getWarrantyStatus(endDate, now = new Date()) {
  const end = toDate(endDate);
  if (!end) return null;

  const today = startOfDay(now);
  const endDay = startOfDay(end);
  const diffDays = Math.round((endDay - today) / MS_PER_DAY);

  if (diffDays < 0) return WARRANTY_STATUS.EXPIRED;
  if (diffDays <= EXPIRING_WINDOW_DAYS) return WARRANTY_STATUS.EXPIRING_SOON;
  return WARRANTY_STATUS.ACTIVE;
}

export function getWarrantyStatusBadgeClasses(status) {
  switch (status) {
    case WARRANTY_STATUS.ACTIVE:
      return 'bg-green-100 text-green-800 border border-green-200';
    case WARRANTY_STATUS.EXPIRING_SOON:
      return 'bg-amber-100 text-amber-800 border border-amber-200';
    case WARRANTY_STATUS.EXPIRED:
      return 'bg-red-100 text-red-800 border border-red-200';
    default:
      return 'bg-slate-100 text-slate-700 border border-slate-200';
  }
}

export function daysUntil(endDate, now = new Date()) {
  const end = toDate(endDate);
  if (!end) return null;
  return Math.round((startOfDay(end) - startOfDay(now)) / MS_PER_DAY);
}
