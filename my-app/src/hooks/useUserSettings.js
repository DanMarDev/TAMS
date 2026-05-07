import { useCallback, useEffect, useState } from 'react';
import { userApi } from '../api/userApi';

export function useUserSettings() {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const refresh = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const { data } = await userApi.getProfile();
      setProfile(data);
    } catch (err) {
      setError(err.response?.data?.message || err.response?.data || 'Failed to load profile');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    refresh();
  }, [refresh]);

  const updateProfile = useCallback(async (payload) => {
    const { data } = await userApi.updateProfile(payload);
    setProfile(data);
    return data;
  }, []);

  const updateSellThreshold = useCallback(async (defaultSellThreshold) => {
    const { data } = await userApi.updateSellThreshold(defaultSellThreshold);
    setProfile(data);
    return data;
  }, []);

  const changePassword = useCallback(async (payload) => {
    await userApi.changePassword(payload);
  }, []);

  return { profile, loading, error, refresh, updateProfile, updateSellThreshold, changePassword };
}
