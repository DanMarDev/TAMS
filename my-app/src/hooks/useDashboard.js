import { useCallback, useEffect, useState } from 'react';
import { dashboardApi } from '../api/dashboardApi';
import { warrantyApi } from '../api/warrantyApi';

export function useDashboard() {
  const [dashboard, setDashboard] = useState(null);
  const [expiringWarranties, setExpiringWarranties] = useState([]);
  const [alerts, setAlerts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const [dashRes, expiringRes, alertsRes] = await Promise.all([
        dashboardApi.get(),
        warrantyApi.getExpiring(),
        warrantyApi.getAlerts(),
      ]);
      setDashboard(dashRes.data);
      setExpiringWarranties(expiringRes.data ?? []);
      setAlerts(alertsRes.data ?? []);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load dashboard');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const dismissAlert = useCallback(async (alertId) => {
    await warrantyApi.dismissAlert(alertId);
    setAlerts((prev) => prev.filter((a) => a.alertId !== alertId));
  }, []);

  return {
    dashboard,
    expiringWarranties,
    alerts,
    loading,
    error,
    refresh,
    dismissAlert,
  };
}
