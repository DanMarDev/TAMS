import apiClient from './apiClient';

export const inventoryApi = {
  // Items
  getItems: () => apiClient.get('/inventory'),
  getItem: (itemId) => apiClient.get(`/inventory/${itemId}`),
  createItem: (payload) => apiClient.post('/inventory', payload),
  updateItem: (itemId, payload) => apiClient.put(`/inventory/${itemId}`, payload),
  deleteItem: (itemId) => apiClient.delete(`/inventory/${itemId}`),

  // Categories
  getCategories: () => apiClient.get('/inventory/categories'),
  getCategory: (id) => apiClient.get(`/inventory/categories/${id}`),
  createCategory: (payload) => apiClient.post('/inventory/categories', payload),
  updateCategory: (id, payload) => apiClient.put(`/inventory/categories/${id}`, payload),
  deleteCategory: (id) => apiClient.delete(`/inventory/categories/${id}`),

  // Brands
  getBrands: () => apiClient.get('/inventory/brands'),
  getBrand: (id) => apiClient.get(`/inventory/brands/${id}`),
  createBrand: (payload) => apiClient.post('/inventory/brands', payload),
  updateBrand: (id, payload) => apiClient.put(`/inventory/brands/${id}`, payload),
  deleteBrand: (id) => apiClient.delete(`/inventory/brands/${id}`),
};
