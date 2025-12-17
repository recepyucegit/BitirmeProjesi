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

export default api;
