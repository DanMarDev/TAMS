import { useCallback, useEffect, useState } from 'react';
import { pricingApi } from '../api/pricingApi';

export function usePricing(itemId) {
  const [latest, setLatest] = useState(null);
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [looking, setLooking] = useState(false);
  const [submittingManual, setSubmittingManual] = useState(false);

  const refresh = useCallback(async () => {
    if (itemId == null) return;
    setLoading(true);
    setError(null);
    try {
      const [latestRes, historyRes] = await Promise.all([
        pricingApi.getLatest(itemId).catch((err) => {
          if (err.response?.status === 404) return { data: null };
          throw err;
        }),
        pricingApi.getHistory(itemId),
      ]);
      setLatest(latestRes.data ?? null);
      setHistory(historyRes.data ?? []);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load pricing');
    } finally {
      setLoading(false);
    }
  }, [itemId]);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const lookup = useCallback(async () => {
    if (itemId == null) return;
    setLooking(true);
    try {
      await pricingApi.generateEstimate(itemId);
      await refresh();
    } finally {
      setLooking(false);
    }
  }, [itemId, refresh]);

  const submitManual = useCallback(
    async (value) => {
      if (itemId == null) return;
      setSubmittingManual(true);
      try {
        await pricingApi.submitManual(itemId, value);
        await refresh();
      } finally {
        setSubmittingManual(false);
      }
    },
    [itemId, refresh]
  );

  return {
    latest,
    history,
    loading,
    error,
    looking,
    submittingManual,
    refresh,
    lookup,
    submitManual,
  };
}
