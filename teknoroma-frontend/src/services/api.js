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

// Request interceptor - Token'ı her istekte ekle
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor - Hata yönetimi
api.interceptors.response.use(
  (response) => response.data,
  (error) => {
    if (error.response?.status === 401) {
      // Token geçersiz veya yok - Logout yap
      localStorage.removeItem('token');
      localStorage.removeItem('user');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth API endpoints
export const authAPI = {
  // Kullanıcı girişi
  login: (data) => api.post('/auth/login', data),

  // Token yenileme
  refreshToken: (refreshToken) => api.post('/auth/refresh-token', { refreshToken }),

  // Kullanıcı kaydı (admin için)
  register: (data) => api.post('/auth/register', data),
};

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

// Customer API endpoints
export const customerAPI = {
  // Tüm müşterileri getir
  getAll: () => api.get('/customer'),

  // Aktif müşterileri getir
  getActive: () => api.get('/customer/active'),

  // Müşteri tipine göre getir (Individual veya Corporate)
  getByType: (type) => api.get(`/customer/type/${type}`),

  // ID'ye göre müşteri getir
  getById: (id) => api.get(`/customer/${id}`),

  // TC Kimlik numarasına göre müşteri getir
  getByIdentityNumber: (identityNumber) => api.get(`/customer/identity/${identityNumber}`),

  // Yeni müşteri oluştur
  create: (data) => api.post('/customer', data),

  // Müşteri güncelle
  update: (id, data) => api.put(`/customer/${id}`, data),

  // Müşteri sil
  delete: (id) => api.delete(`/customer/${id}`),

  // Müşteri var mı kontrol et
  exists: (id) => api.get(`/customer/${id}/exists`),

  // TC Kimlik numarası var mı kontrol et
  identityNumberExists: (identityNumber) => api.get(`/customer/identity/${identityNumber}/exists`),
};

// Supplier API endpoints
export const supplierAPI = {
  // Tüm tedarikçileri getir
  getAll: () => api.get('/supplier'),

  // Aktif tedarikçileri getir
  getActive: () => api.get('/supplier/active'),

  // ID'ye göre tedarikçi getir
  getById: (id) => api.get(`/supplier/${id}`),

  // Yeni tedarikçi oluştur
  create: (data) => api.post('/supplier', data),

  // Tedarikçi güncelle
  update: (id, data) => api.put(`/supplier/${id}`, data),

  // Tedarikçi sil
  delete: (id) => api.delete(`/supplier/${id}`),

  // Tedarikçi var mı kontrol et
  exists: (id) => api.get(`/supplier/${id}/exists`),
};

// Employee API endpoints
export const employeeAPI = {
  // Tüm çalışanları getir
  getAll: () => api.get('/employee'),

  // Aktif çalışanları getir
  getActive: () => api.get('/employee/active'),

  // ID'ye göre çalışan getir
  getById: (id) => api.get(`/employee/${id}`),

  // Yeni çalışan oluştur
  create: (data) => api.post('/employee', data),

  // Çalışan güncelle
  update: (id, data) => api.put(`/employee/${id}`, data),

  // Çalışan sil
  delete: (id) => api.delete(`/employee/${id}`),

  // Çalışan var mı kontrol et
  exists: (id) => api.get(`/employee/${id}/exists`),
};

// Sale API endpoints
export const saleAPI = {
  // Tüm satışları getir
  getAll: () => api.get('/sale'),

  // ID'ye göre satış getir
  getById: (id) => api.get(`/sale/${id}`),

  // Müşteriye göre satışları getir
  getByCustomerId: (customerId) => api.get(`/sale/customer/${customerId}`),

  // Çalışana göre satışları getir
  getByEmployeeId: (employeeId) => api.get(`/sale/employee/${employeeId}`),

  // Tarih aralığına göre satışları getir
  getByDateRange: (startDate, endDate) => api.get('/sale/daterange', {
    params: { startDate, endDate }
  }),

  // Duruma göre satışları getir
  getByStatus: (status) => api.get(`/sale/status/${status}`),

  // Çalışan satış toplamlarını getir
  getEmployeeTotals: (employeeId, startDate, endDate) => api.get(`/sale/employee/${employeeId}/totals`, {
    params: { startDate, endDate }
  }),

  // Yeni satış oluştur
  create: (data) => api.post('/sale', data),

  // Satış güncelle
  update: (id, data) => api.put(`/sale/${id}`, data),

  // Satış sil
  delete: (id) => api.delete(`/sale/${id}`),
};

// Store API endpoints
export const storeAPI = {
  // Tüm mağazaları getir
  getAll: () => api.get('/store'),

  // Aktif mağazaları getir
  getActive: () => api.get('/store/active'),

  // Şehre göre mağazaları getir
  getByCity: (city) => api.get(`/store/city/${city}`),

  // ID'ye göre mağaza getir
  getById: (id) => api.get(`/store/${id}`),

  // Mağaza koduna göre mağaza getir
  getByStoreCode: (storeCode) => api.get(`/store/code/${storeCode}`),

  // Yeni mağaza oluştur
  create: (data) => api.post('/store', data),

  // Mağaza güncelle
  update: (id, data) => api.put(`/store/${id}`, data),

  // Mağaza sil
  delete: (id) => api.delete(`/store/${id}`),

  // Mağaza kodu var mı kontrol et
  storeCodeExists: (storeCode) => api.get(`/store/code/${storeCode}/exists`),
};

// Expense API endpoints
export const expenseAPI = {
  // Tüm giderleri getir
  getAll: () => api.get('/expense'),

  // Onay bekleyenleri getir
  getPending: () => api.get('/expense/pending'),

  // Duruma göre giderleri getir
  getByStatus: (status) => api.get(`/expense/status/${status}`),

  // Mağazaya göre giderleri getir
  getByStoreId: (storeId) => api.get(`/expense/store/${storeId}`),

  // Tarih aralığına göre giderleri getir
  getByDateRange: (startDate, endDate) => api.get('/expense/date-range', {
    params: { startDate, endDate }
  }),

  // Mağazanın toplam giderini getir
  getTotalByStore: (storeId, startDate, endDate) => api.get(`/expense/store/${storeId}/total`, {
    params: { startDate, endDate }
  }),

  // Kategoriye göre toplam gider
  getTotalByCategory: (category, startDate, endDate) => api.get(`/expense/category/${category}/total`, {
    params: { startDate, endDate }
  }),

  // ID'ye göre gider getir
  getById: (id) => api.get(`/expense/${id}`),

  // Yeni gider oluştur
  create: (data) => api.post('/expense', data),

  // Gider güncelle
  update: (id, data) => api.put(`/expense/${id}`, data),

  // Gideri onayla veya reddet
  approve: (id, data) => api.post(`/expense/${id}/approve`, data),

  // Gider sil
  delete: (id) => api.delete(`/expense/${id}`),
};

// SupplierTransaction API endpoints
export const supplierTransactionAPI = {
  // Tüm tedarikçi işlemlerini getir
  getAll: () => api.get('/suppliertransaction'),

  // ID'ye göre tedarikçi işlemi getir
  getById: (id) => api.get(`/suppliertransaction/${id}`),

  // Tedarikçiye göre işlemleri getir
  getBySupplierId: (supplierId) => api.get(`/suppliertransaction/supplier/${supplierId}`),

  // Çalışana göre işlemleri getir
  getByEmployeeId: (employeeId) => api.get(`/suppliertransaction/employee/${employeeId}`),

  // Duruma göre işlemleri getir
  getByStatus: (status) => api.get(`/suppliertransaction/status/${status}`),

  // Tarih aralığına göre işlemleri getir
  getByDateRange: (startDate, endDate) => api.get('/suppliertransaction/daterange', {
    params: { startDate, endDate }
  }),

  // Yeni tedarikçi işlemi oluştur
  create: (data) => api.post('/suppliertransaction', data),

  // Tedarikçi işlemini güncelle
  update: (id, data) => api.put(`/suppliertransaction/${id}`, data),

  // Tedarikçi işlemini sil
  delete: (id) => api.delete(`/suppliertransaction/${id}`),
};

// Report API endpoints
// User API endpoints
export const userAPI = {
  // Tüm kullanıcıları getir
  getAll: () => api.get('/user'),

  // ID'ye göre kullanıcı getir
  getById: (id) => api.get(`/user/${id}`),

  // Username'e göre kullanıcı getir
  getByUsername: (username) => api.get(`/user/username/${username}`),

  // Username var mı kontrol et
  usernameExists: (username) => api.get(`/user/username/${username}/exists`),

  // Email var mı kontrol et
  emailExists: (email) => api.get(`/user/email/${email}/exists`),

  // Yeni kullanıcı oluştur
  create: (data) => api.post('/user', data),

  // Kullanıcı güncelle
  update: (id, data) => api.put(`/user/${id}`, data),

  // Kullanıcı sil
  delete: (id) => api.delete(`/user/${id}`),
};

// Department API endpoints
export const departmentAPI = {
  // Tüm departmanları getir
  getAll: () => api.get('/department'),

  // Aktif departmanları getir
  getActive: () => api.get('/department/active'),

  // ID'ye göre departman getir
  getById: (id) => api.get(`/department/${id}`),

  // Koda göre departman getir
  getByCode: (code) => api.get(`/department/code/${code}`),

  // İsme göre departman getir
  getByName: (name) => api.get(`/department/name/${name}`),

  // Müdüre göre departmanları getir
  getByManager: (managerId) => api.get(`/department/manager/${managerId}`),

  // Toplam çalışan sayısı
  getTotalEmployees: () => api.get('/department/statistics/total-employees'),

  // Toplam bütçe
  getTotalBudget: () => api.get('/department/statistics/total-budget'),

  // Yeni departman oluştur
  create: (data) => api.post('/department', data),

  // Departman güncelle
  update: (id, data) => api.put(`/department/${id}`, data),

  // Departman sil
  delete: (id) => api.delete(`/department/${id}`),
};

// Role API endpoints
export const roleAPI = {
  // Tüm rolleri getir
  getAll: () => api.get('/role'),

  // ID'ye göre rol getir
  getById: (id) => api.get(`/role/${id}`),

  // İsme göre rol getir
  getByName: (name) => api.get(`/role/name/${name}`),

  // Rol adı var mı kontrol et
  roleNameExists: (name) => api.get(`/role/name/${name}/exists`),

  // Yeni rol oluştur
  create: (data) => api.post('/role', data),

  // Rol güncelle
  update: (id, data) => api.put(`/role/${id}`, data),

  // Rol sil
  delete: (id) => api.delete(`/role/${id}`),
};

export const reportAPI = {
  // Dashboard stats
  getDashboardStats: () => api.get('/report/dashboard'),

  // Sales reports
  getSalesReport: (params) => api.get('/report/sales', { params }),
  getSalesSummary: (startDate, endDate) => api.get('/report/sales/summary', {
    params: { startDate, endDate }
  }),
  getTopSellingProducts: (count = 10, startDate, endDate) => api.get('/report/sales/top-products', {
    params: { count, startDate, endDate }
  }),
  getTopCustomers: (count = 10, startDate, endDate) => api.get('/report/sales/top-customers', {
    params: { count, startDate, endDate }
  }),
  getTopEmployees: (count = 10, startDate, endDate) => api.get('/report/sales/top-employees', {
    params: { count, startDate, endDate }
  }),
  exportSalesReport: (params) => api.post('/report/sales/export', null, {
    params,
    responseType: 'blob'
  }),

  // Stock reports
  getStockReport: (params) => api.get('/report/stock', { params }),
  getLowStockAlerts: () => api.get('/report/stock/low-stock-alerts'),
  getStockSummary: () => api.get('/report/stock/summary'),
  exportStockReport: (params) => api.post('/report/stock/export', null, {
    params,
    responseType: 'blob'
  }),

  // Expense reports
  getExpenseReport: (params) => api.get('/report/expenses', { params }),
  getExpenseSummary: (startDate, endDate) => api.get('/report/expenses/summary', {
    params: { startDate, endDate }
  }),
  exportExpenseReport: (params) => api.post('/report/expenses/export', null, {
    params,
    responseType: 'blob'
  }),
};

export default api;
