import axios from 'axios';

// API base URL - Backend API'mizin adresi
const API_BASE_URL = 'http://localhost:5085/api';

// Axios instance oluştur
const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Category API endpoints
export const categoryAPI = {
  // Tüm kategorileri getir
  getAll: () => api.get('/category'),

  // Aktif kategorileri getir
  getActive: () => api.get('/category/active'),

  // ID'ye göre kategori getir
  getById: (id) => api.get(`/category/${id}`),

  // Yeni kategori oluştur
  create: (data) => api.post('/category', data),

  // Kategori güncelle
  update: (id, data) => api.put(`/category/${id}`, data),

  // Kategori sil
  delete: (id) => api.delete(`/category/${id}`),

  // Kategori var mı kontrol et
  exists: (id) => api.get(`/category/${id}/exists`),
};

// Product API endpoints
export const productAPI = {
  // Tüm ürünleri getir
  getAll: () => api.get('/product'),

  // Aktif ürünleri getir
  getActive: () => api.get('/product/active'),

  // Stok seviyesi düşük ürünleri getir
  getLowStock: () => api.get('/product/low-stock'),

  // Kategoriye göre ürünleri getir
  getByCategory: (categoryId) => api.get(`/product/category/${categoryId}`),

  // ID'ye göre ürün getir
  getById: (id) => api.get(`/product/${id}`),

  // Barkoda göre ürün getir
  getByBarcode: (barcode) => api.get(`/product/barcode/${barcode}`),

  // Yeni ürün oluştur
  create: (data) => api.post('/product', data),

  // Ürün güncelle
  update: (id, data) => api.put(`/product/${id}`, data),

  // Ürün sil
  delete: (id) => api.delete(`/product/${id}`),

  // Ürün var mı kontrol et
  exists: (id) => api.get(`/product/${id}/exists`),

  // Barkod var mı kontrol et
  barcodeExists: (barcode) => api.get(`/product/barcode/${barcode}/exists`),
};

// Supplier API endpoints
export const supplierAPI = {
  // Tüm tedarikçileri getir
  getAll: () => api.get('/supplier'),

  // Aktif tedarikçileri getir
  getActive: () => api.get('/supplier/active'),

  // ID'ye göre tedarikçi getir
  getById: (id) => api.get(`/supplier/${id}`),

  // Vergi numarasına göre tedarikçi getir
  getByTaxNumber: (taxNumber) => api.get(`/supplier/tax/${taxNumber}`),

  // Yeni tedarikçi oluştur
  create: (data) => api.post('/supplier', data),

  // Tedarikçi güncelle
  update: (id, data) => api.put(`/supplier/${id}`, data),

  // Tedarikçi sil
  delete: (id) => api.delete(`/supplier/${id}`),

  // Tedarikçi var mı kontrol et
  exists: (id) => api.get(`/supplier/${id}/exists`),

  // Vergi numarası var mı kontrol et
  taxNumberExists: (taxNumber) => api.get(`/supplier/tax/${taxNumber}/exists`),
};

// Employee API endpoints
export const employeeAPI = {
  // Tüm çalışanları getir
  getAll: () => api.get('/employee'),

  // Aktif çalışanları getir
  getActive: () => api.get('/employee/active'),

  // Role göre çalışanları getir
  getByRole: (role) => api.get(`/employee/role/${role}`),

  // ID'ye göre çalışan getir
  getById: (id) => api.get(`/employee/${id}`),

  // Kullanıcı adına göre çalışan getir
  getByUsername: (username) => api.get(`/employee/username/${username}`),

  // TC kimlik numarasına göre çalışan getir
  getByIdentityNumber: (identityNumber) => api.get(`/employee/identity/${identityNumber}`),

  // Yeni çalışan oluştur
  create: (data) => api.post('/employee', data),

  // Çalışan güncelle
  update: (id, data) => api.put(`/employee/${id}`, data),

  // Çalışan sil
  delete: (id) => api.delete(`/employee/${id}`),

  // Çalışan var mı kontrol et
  exists: (id) => api.get(`/employee/${id}/exists`),

  // Kullanıcı adı var mı kontrol et
  usernameExists: (username) => api.get(`/employee/username/${username}/exists`),

  // TC kimlik numarası var mı kontrol et
  identityNumberExists: (identityNumber) => api.get(`/employee/identity/${identityNumber}/exists`),
};

// Auth API endpoints
export const authAPI = {
  // Login
  login: (username, password) => api.post('/auth/login', { username, password }),

  // Register (if needed in future)
  register: (data) => api.post('/auth/register', data),
};

// Token yönetimi için interceptor
export const setAuthToken = (token) => {
  if (token) {
    api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
  } else {
    delete api.defaults.headers.common['Authorization'];
  }
};

export default api;
