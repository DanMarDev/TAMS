import { describe, it, expect } from 'vitest';
import {
  getWarrantyStatus,
  daysUntil,
  WARRANTY_STATUS,
} from './warrantyStatus';

// Construct dates with local-time constructors so tests are timezone-independent.
const d = (y, m, day) => new Date(y, m - 1, day);

describe('getWarrantyStatus', () => {
  const now = d(2026, 6, 1); // June 1, 2026 (local)

  it('returns null when end date is missing', () => {
    expect(getWarrantyStatus(null, now)).toBeNull();
    expect(getWarrantyStatus(undefined, now)).toBeNull();
    expect(getWarrantyStatus('', now)).toBeNull();
  });

  it('returns Expired when end date is in the past', () => {
    expect(getWarrantyStatus(d(2026, 5, 31), now)).toBe(WARRANTY_STATUS.EXPIRED);
    expect(getWarrantyStatus(d(2024, 1, 1), now)).toBe(WARRANTY_STATUS.EXPIRED);
  });

  it('returns Expiring Soon when end date is within 30 days', () => {
    expect(getWarrantyStatus(d(2026, 6, 1), now)).toBe(WARRANTY_STATUS.EXPIRING_SOON);
    expect(getWarrantyStatus(d(2026, 6, 15), now)).toBe(WARRANTY_STATUS.EXPIRING_SOON);
    expect(getWarrantyStatus(d(2026, 7, 1), now)).toBe(WARRANTY_STATUS.EXPIRING_SOON);
  });

  it('returns Active when end date is more than 30 days out', () => {
    expect(getWarrantyStatus(d(2026, 7, 2), now)).toBe(WARRANTY_STATUS.ACTIVE);
    expect(getWarrantyStatus(d(2027, 1, 1), now)).toBe(WARRANTY_STATUS.ACTIVE);
  });

  it('returns null for unparseable input', () => {
    expect(getWarrantyStatus('not-a-date', now)).toBeNull();
  });
});

describe('daysUntil', () => {
  const now = d(2026, 6, 1);

  it('returns positive days for future dates', () => {
    expect(daysUntil(d(2026, 6, 11), now)).toBe(10);
  });

  it('returns negative days for past dates', () => {
    expect(daysUntil(d(2026, 5, 22), now)).toBe(-10);
  });

  it('returns 0 for today', () => {
    expect(daysUntil(d(2026, 6, 1), now)).toBe(0);
  });

  it('returns null for missing input', () => {
    expect(daysUntil(null, now)).toBeNull();
  });
});
