import apiClient from './apiClient';

export const warrantyApi = {
  getAll: () => apiClient.get('/warranty'),
  getByItemId: (itemId) => apiClient.get(`/warranty/${itemId}`),
  create: (payload) => apiClient.post('/warranty', payload),
  update: (warrantyId, payload) => apiClient.put(`/warranty/${warrantyId}`, payload),
  remove: (warrantyId, itemId) =>
    apiClient.delete(`/warranty/${warrantyId}`, { params: { itemId } }),

  getExpiring: () => apiClient.get('/warranty/expiring'),
  getAlerts: () => apiClient.get('/warranty/alerts'),
  dismissAlert: (alertId) => apiClient.delete(`/warranty/alerts/${alertId}`),
};
