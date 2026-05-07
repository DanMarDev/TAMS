import { useMemo } from 'react';
import { useDashboard } from '../hooks/useDashboard';
import { useNotification } from '../context/NotificationContext';
import SummaryCards from '../components/dashboard/SummaryCards';
import OldestItemsList from '../components/dashboard/OldestItemsList';
import ExpiringWarranties from '../components/dashboard/ExpiringWarranties';
import MaybeSellList from '../components/dashboard/MaybeSellList';
import AlertBanners from '../components/dashboard/AlertBanners';

export default function DashboardPage() {
  const {
    dashboard,
    expiringWarranties,
    alerts,
    loading,
    error,
    dismissAlert,
  } = useDashboard();
  const { notify } = useNotification();

  const itemNameById = useMemo(() => {
    const map = new Map();
    for (const i of dashboard?.oldestItems ?? []) map.set(i.itemId, i.name);
    for (const i of dashboard?.maybeSellItems ?? []) map.set(i.itemId, i.name);
    return map;
  }, [dashboard]);

  const handleDismiss = async (alertId) => {
    try {
      await dismissAlert(alertId);
    } catch (err) {
      notify(err.response?.data?.message || 'Failed to dismiss alert', 'error');
    }
  };

  if (loading) return <div className="p-2">Loading…</div>;
  if (error) return <div className="p-2 text-red-600">{error}</div>;

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-semibold">Dashboard</h1>

      <AlertBanners
        alerts={alerts}
        itemNameById={itemNameById}
        onDismiss={handleDismiss}
      />

      <SummaryCards summary={dashboard?.summary} />

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
        <OldestItemsList items={dashboard?.oldestItems ?? []} />
        <ExpiringWarranties
          warranties={expiringWarranties}
          itemNameById={itemNameById}
        />
        <MaybeSellList items={dashboard?.maybeSellItems ?? []} />
      </div>
    </div>
  );
}
