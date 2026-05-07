import apiClient from './apiClient';

export const userApi = {
  getProfile: () => apiClient.get('/users/me'),
  updateProfile: (payload) => apiClient.put('/users/me', payload),
  updateSellThreshold: (defaultSellThreshold) =>
    apiClient.put('/users/me/sell-threshold', { defaultSellThreshold }),
  changePassword: (payload) => apiClient.post('/users/me/password', payload),
};
