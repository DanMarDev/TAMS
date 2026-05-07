import { useCallback, useEffect, useState } from 'react';
import { inventoryApi } from '../api/inventoryApi';

export function useBrands() {
  const [brands, setBrands] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await inventoryApi.getBrands();
      setBrands(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load brands');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const createBrand = useCallback(async (name) => {
    const { data } = await inventoryApi.createBrand({ name });
    await refresh();
    return data.brandId;
  }, [refresh]);

  return { brands, loading, error, refresh, createBrand };
}
