import { createContext, useState, useCallback } from 'react';
import apiClient from '../api/apiClient';

  export const AuthContext = createContext(null);

  export function AuthProvider({ children }) {
    const [user, setUser] = useState(() => {
      const raw = sessionStorage.getItem('tams_user');
      return raw ? JSON.parse(raw) : null;
    });
    const [token, setToken] = useState(() => sessionStorage.getItem('tams_token'));

    const persist = (auth) => {
      sessionStorage.setItem('tams_token', auth.token);
      sessionStorage.setItem(
        'tams_user',
        JSON.stringify({ userId: auth.userId, userName: auth.userName, email: auth.email })
      );
      setToken(auth.token);
      setUser({ userId: auth.userId, userName: auth.userName, email: auth.email });
    };

    const login = useCallback(async (email, password) => {
      const { data } = await apiClient.post('/auth/login', { email, password });
      persist(data);
      return data;
    }, []);

    const register = useCallback(async (email, userName, password, confirmPassword) => {
      const { data } = await apiClient.post('/auth/register', {
        email, userName, password, confirmPassword,
      });
      persist(data);
      return data;
    }, []);

    const logout = useCallback(async () => {
      try { await apiClient.post('/auth/logout'); } catch { /* ignore */ }
      sessionStorage.removeItem('tams_token');
      sessionStorage.removeItem('tams_user');
      setToken(null);
      setUser(null);
    }, []);

    return (
      <AuthContext.Provider value={{ user, token, isAuthenticated: !!token, login, register, logout }}>
        {children}
      </AuthContext.Provider>
    );
  }