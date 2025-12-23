import React, { createContext, useState, useContext, useEffect } from 'react';
import { authAPI, setAuthToken } from '../services/api';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [token, setToken] = useState(null);
  const [loading, setLoading] = useState(true);

  // Sayfa yüklendiğinde localStorage'dan token kontrolü
  useEffect(() => {
    const storedToken = localStorage.getItem('token');
    const storedUser = localStorage.getItem('user');

    if (storedToken && storedUser) {
      setToken(storedToken);
      setUser(JSON.parse(storedUser));
      setAuthToken(storedToken);
    }
    setLoading(false);
  }, []);

  const login = async (username, password) => {
    const response = await authAPI.login(username, password);
    const { token: newToken, user: userData } = response.data;

    // Token ve user bilgilerini kaydet
    setToken(newToken);
    setUser(userData);
    setAuthToken(newToken);

    // localStorage'a kaydet
    localStorage.setItem('token', newToken);
    localStorage.setItem('user', JSON.stringify(userData));

    return response.data;
  };

  const logout = () => {
    setToken(null);
    setUser(null);
    setAuthToken(null);
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  };

  const isAuthenticated = () => {
    return !!token;
  };

  const value = {
    user,
    token,
    login,
    logout,
    isAuthenticated,
    loading,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
