import apiClient from "./apiClient";

export const authApi = {
    register: (payload) => apiClient.post("/auth/register", payload),
    login: (payload) => apiClient.post("/auth/login", payload),
    logout: () => apiClient.post("/auth/logout"),
    forgotPassword: (email) => apiClient.post("/auth/forgot-password", { email }),
    resetPassword: (payload) => apiClient.post("/auth/reset-password", payload)
};