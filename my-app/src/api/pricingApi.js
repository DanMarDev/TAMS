import apiClient from './apiClient';

const LOOKUP_TIMEOUT_MS = 10000;

export const pricingApi = {
  getLatest: (itemId) => apiClient.get(`/pricing/latest/${itemId}`),
  getHistory: (itemId) => apiClient.get(`/pricing/history/${itemId}`),
  generateEstimate: (itemId) =>
    apiClient.get(`/pricing/estimate/${itemId}`, { timeout: LOOKUP_TIMEOUT_MS }),
  submitManual: (itemId, value) =>
    apiClient.post(`/pricing/manual/${itemId}`, { itemId: Number(itemId), value: Number(value) }),
};
