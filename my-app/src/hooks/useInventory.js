import { useCallback, useEffect, useState } from 'react';
import { inventoryApi } from '../api/inventoryApi';

export function useInventory() {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await inventoryApi.getItems();
      setItems(data);
    } catch (err) {
      setError(err.response?.data?.message || 'Failed to load items');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const createItem = useCallback(async (payload) => {
    const { data } = await inventoryApi.createItem(payload);
    await refresh();
    return data.itemId;
  }, [refresh]);

  const updateItem = useCallback(async (itemId, payload) => {
    await inventoryApi.updateItem(itemId, payload);
    await refresh();
  }, [refresh]);

  const deleteItem = useCallback(async (itemId) => {
    await inventoryApi.deleteItem(itemId);
    setItems((prev) => prev.filter((i) => i.itemId !== itemId));
  }, []);

  return { items, loading, error, refresh, createItem, updateItem, deleteItem };
}
