import { useCallback, useEffect, useState } from 'react';
import { warrantyApi } from '../api/warrantyApi';

export function useWarranties() {
  const [warranties, setWarranties] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await warrantyApi.getAll();
      setWarranties(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load warranties');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  return { warranties, loading, error, refresh };
}

export function useWarranty(itemId) {
  const [warranty, setWarranty] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    if (itemId == null) return;
    setLoading(true);
    setError(null);
    try {
      const { data } = await warrantyApi.getByItemId(itemId);
      setWarranty(data);
    } catch (err) {
      if (err.response?.status === 404) {
        setWarranty(null);
      } else {
        setError(err.response?.data?.message || 'Failed to load warranty');
      }
    } finally {
      setLoading(false);
    }
  }, [itemId]);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const saveWarranty = useCallback(
    async (payload) => {
      if (warranty?.itemWarrantyId) {
        await warrantyApi.update(warranty.itemWarrantyId, payload);
      } else {
        await warrantyApi.create(payload);
      }
      await refresh();
    },
    [warranty, refresh]
  );

  const deleteWarranty = useCallback(async () => {
    if (!warranty?.itemWarrantyId) return;
    await warrantyApi.remove(warranty.itemWarrantyId, warranty.itemId);
    setWarranty(null);
  }, [warranty]);

  return { warranty, loading, error, refresh, saveWarranty, deleteWarranty };
}
