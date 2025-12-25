import { useState, useEffect } from 'react';
import { employeeAPI } from '../services/api';

const StoreForm = ({ store, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    storeName: '',
    storeCode: '',
    address: '',
    city: '',
    district: '',
    phone: '',
    email: '',
    managerId: '',
    openingDate: new Date().toISOString().split('T')[0],
    monthlyTarget: '',
    capacity: '',
    isActive: true
  });

  const [employees, setEmployees] = useState([]);
  const [errors, setErrors] = useState({});
  const [employeesLoading, setEmployeesLoading] = useState(true);

  // Çalışanları yükle (manager için)
  useEffect(() => {
    const fetchEmployees = async () => {
      try {
        setEmployeesLoading(true);
        const data = await employeeAPI.getActive();
        // Sadece Manager ve Admin rolündeki çalışanları filtrele
        const managers = data.filter(emp =>
          emp.role === 'Manager' || emp.role === 'Admin'
        );
        setEmployees(managers || []);
      } catch (err) {
        console.error('Error fetching employees:', err);
        setEmployees([]);
      } finally {
        setEmployeesLoading(false);
      }
    };

    fetchEmployees();
  }, []);

  // Mağaza düzenleme modunda formu doldur
  useEffect(() => {
    if (store) {
      setFormData({
        storeName: store.storeName || '',
        storeCode: store.storeCode || '',
        address: store.address || '',
        city: store.city || '',
        district: store.district || '',
        phone: store.phone || '',
        email: store.email || '',
        managerId: store.managerId?.toString() || '',
        openingDate: store.openingDate ? new Date(store.openingDate).toISOString().split('T')[0] : new Date().toISOString().split('T')[0],
        monthlyTarget: store.monthlyTarget?.toString() || '',
        capacity: store.capacity?.toString() || '',
        isActive: store.isActive !== undefined ? store.isActive : true
      });
    } else {
      // Yeni mağaza modunda formu temizle
      setFormData({
        storeName: '',
        storeCode: '',
        address: '',
        city: '',
        district: '',
        phone: '',
        email: '',
        managerId: '',
        openingDate: new Date().toISOString().split('T')[0],
        monthlyTarget: '',
        capacity: '',
        isActive: true
      });
    }
  }, [store]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const validate = () => {
    const newErrors = {};

    // StoreName validation
    if (!formData.storeName.trim()) {
      newErrors.storeName = 'Mağaza adı gereklidir';
    } else if (formData.storeName.trim().length > 100) {
      newErrors.storeName = 'Mağaza adı en fazla 100 karakter olmalıdır';
    }

    // StoreCode validation
    if (!formData.storeCode.trim()) {
      newErrors.storeCode = 'Mağaza kodu gereklidir';
    } else if (formData.storeCode.trim().length > 20) {
      newErrors.storeCode = 'Mağaza kodu en fazla 20 karakter olmalıdır';
    }

    // City validation
    if (formData.city && formData.city.length > 50) {
      newErrors.city = 'Şehir adı en fazla 50 karakter olmalıdır';
    }

    // District validation
    if (formData.district && formData.district.length > 50) {
      newErrors.district = 'İlçe adı en fazla 50 karakter olmalıdır';
    }

    // Phone validation
    if (formData.phone && formData.phone.length > 20) {
      newErrors.phone = 'Telefon en fazla 20 karakter olmalıdır';
    }

    // Email validation
    if (formData.email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(formData.email)) {
        newErrors.email = 'Geçerli bir email adresi giriniz';
      } else if (formData.email.length > 100) {
        newErrors.email = 'Email en fazla 100 karakter olmalıdır';
      }
    }

    // Address validation
    if (formData.address && formData.address.length > 500) {
      newErrors.address = 'Adres en fazla 500 karakter olmalıdır';
    }

    // OpeningDate validation
    if (!formData.openingDate) {
      newErrors.openingDate = 'Açılış tarihi gereklidir';
    }

    // MonthlyTarget validation
    if (formData.monthlyTarget && (isNaN(formData.monthlyTarget) || parseFloat(formData.monthlyTarget) < 0)) {
      newErrors.monthlyTarget = 'Aylık hedef 0 veya daha büyük olmalıdır';
    }

    // Capacity validation
    if (formData.capacity && (isNaN(formData.capacity) || parseInt(formData.capacity) < 0)) {
      newErrors.capacity = 'Kapasite 0 veya daha büyük olmalıdır';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (validate()) {
      // Convert string values to appropriate types
      const submitData = {
        storeName: formData.storeName.trim(),
        storeCode: formData.storeCode.trim().toUpperCase().replace(/[^A-Z0-9]/g, ''), // Sadece harf ve rakam
        address: formData.address.trim() || null,
        city: formData.city.trim() || null,
        district: formData.district.trim() || null,
        phone: formData.phone.trim().replace(/[\s\-\(\)]/g, '') || null, // Tire, boşluk, parantez temizle
        email: formData.email.trim() || null,
        managerId: formData.managerId ? parseInt(formData.managerId) : null,
        openingDate: formData.openingDate,
        monthlyTarget: formData.monthlyTarget ? parseFloat(formData.monthlyTarget) : null,
        capacity: formData.capacity ? parseInt(formData.capacity) : null
      };

      // Update modunda isActive ekle
      if (store) {
        submitData.isActive = formData.isActive;
      }

      onSubmit(submitData);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="storeName">
          Mağaza Adı <span style={{ color: 'red' }}>*</span>
        </label>
        <input
          type="text"
          id="storeName"
          name="storeName"
          value={formData.storeName}
          onChange={handleChange}
          placeholder="Örn: TeknoRoma İstanbul"
          disabled={loading}
        />
        {errors.storeName && <span className="error-text">{errors.storeName}</span>}
      </div>

      <div className="form-group">
        <label htmlFor="storeCode">
          Mağaza Kodu <span style={{ color: 'red' }}>*</span>
        </label>
        <input
          type="text"
          id="storeCode"
          name="storeCode"
          value={formData.storeCode}
          onChange={handleChange}
          placeholder="Örn: IST001"
          disabled={loading || store} // Kod düzenlenemez
          style={{ textTransform: 'uppercase' }}
          maxLength="20"
        />
        {errors.storeCode && <span className="error-text">{errors.storeCode}</span>}
        {store && <small style={{ color: '#666' }}>Mağaza kodu düzenlenemez</small>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="city">Şehir</label>
          <input
            type="text"
            id="city"
            name="city"
            value={formData.city}
            onChange={handleChange}
            placeholder="Örn: İstanbul"
            disabled={loading}
          />
          {errors.city && <span className="error-text">{errors.city}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="district">İlçe</label>
          <input
            type="text"
            id="district"
            name="district"
            value={formData.district}
            onChange={handleChange}
            placeholder="Örn: Kadıköy"
            disabled={loading}
          />
          {errors.district && <span className="error-text">{errors.district}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="address">Adres</label>
        <textarea
          id="address"
          name="address"
          value={formData.address}
          onChange={handleChange}
          placeholder="Tam adres"
          disabled={loading}
          rows="2"
        />
        {errors.address && <span className="error-text">{errors.address}</span>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="phone">Telefon</label>
          <input
            type="text"
            id="phone"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            placeholder="0212-555-0001"
            pattern="[0-9\-\s\(\)]+"
            title="Sadece rakam, tire, boşluk ve parantez kullanabilirsiniz"
            disabled={loading}
          />
          {errors.phone && <span className="error-text">{errors.phone}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="email">Email</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            placeholder="store@teknoroms.com"
            disabled={loading}
          />
          {errors.email && <span className="error-text">{errors.email}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="managerId">Mağaza Müdürü</label>
        <select
          id="managerId"
          name="managerId"
          value={formData.managerId}
          onChange={handleChange}
          disabled={loading || employeesLoading}
        >
          <option value="">Müdür Seçiniz (Opsiyonel)</option>
          {employees.map((employee) => (
            <option key={employee.id} value={employee.id}>
              {employee.fullName} ({employee.role})
            </option>
          ))}
        </select>
        {errors.managerId && <span className="error-text">{errors.managerId}</span>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="openingDate">
            Açılış Tarihi <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="date"
            id="openingDate"
            name="openingDate"
            value={formData.openingDate}
            onChange={handleChange}
            disabled={loading}
          />
          {errors.openingDate && <span className="error-text">{errors.openingDate}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="monthlyTarget">Aylık Hedef (₺)</label>
          <input
            type="number"
            id="monthlyTarget"
            name="monthlyTarget"
            value={formData.monthlyTarget}
            onChange={handleChange}
            placeholder="0.00"
            step="0.01"
            min="0"
            disabled={loading}
          />
          {errors.monthlyTarget && <span className="error-text">{errors.monthlyTarget}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="capacity">Kapasite (Kişi)</label>
          <input
            type="number"
            id="capacity"
            name="capacity"
            value={formData.capacity}
            onChange={handleChange}
            placeholder="0"
            min="0"
            disabled={loading}
          />
          {errors.capacity && <span className="error-text">{errors.capacity}</span>}
        </div>
      </div>

      {store && (
        <div className="form-group">
          <div className="checkbox-wrapper">
            <input
              type="checkbox"
              id="isActive"
              name="isActive"
              checked={formData.isActive}
              onChange={handleChange}
              disabled={loading}
            />
            <label htmlFor="isActive">Aktif</label>
          </div>
        </div>
      )}

      <div className="form-actions">
        <button
          type="button"
          className="btn btn-secondary"
          onClick={onCancel}
          disabled={loading}
        >
          İptal
        </button>
        <button
          type="submit"
          className="btn btn-primary"
          disabled={loading || employeesLoading}
        >
          {loading ? 'Kaydediliyor...' : (store ? 'Güncelle' : 'Kaydet')}
        </button>
      </div>
    </form>
  );
};

export default StoreForm;
