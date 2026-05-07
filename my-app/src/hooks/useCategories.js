import { useCallback, useEffect, useState } from 'react';
import { inventoryApi } from '../api/inventoryApi';

export function useCategories() {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await inventoryApi.getCategories();
      setCategories(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load categories');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const createCategory = useCallback(async (name, description = null) => {
    const { data } = await inventoryApi.createCategory({ name, description });
    await refresh();
    return data.categoryId;
  }, [refresh]);

  return { categories, loading, error, refresh, createCategory };
}
