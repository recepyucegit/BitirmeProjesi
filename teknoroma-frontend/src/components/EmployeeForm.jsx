import { useState, useEffect } from 'react';

const EmployeeForm = ({ employee, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    identityNumber: '',
    email: '',
    phone: '',
    address: '',
    city: '',
    hireDate: '',
    terminationDate: '',
    salary: '',
    salesQuota: '10000',
    commissionRate: '0.10',
    role: 'Sales',
    username: '',
    password: '',
    isActive: true
  });

  const [errors, setErrors] = useState({});

  // Çalışan düzenleme modunda formu doldur
  useEffect(() => {
    if (employee) {
      setFormData({
        firstName: employee.firstName || '',
        lastName: employee.lastName || '',
        identityNumber: employee.identityNumber || '',
        email: employee.email || '',
        phone: employee.phone || '',
        address: employee.address || '',
        city: employee.city || '',
        hireDate: employee.hireDate ? employee.hireDate.split('T')[0] : '',
        terminationDate: employee.terminationDate ? employee.terminationDate.split('T')[0] : '',
        salary: employee.salary?.toString() || '',
        salesQuota: employee.salesQuota?.toString() || '10000',
        commissionRate: employee.commissionRate?.toString() || '0.10',
        role: employee.role || 'Sales',
        username: employee.username || '',
        password: '', // Şifre güncelleme opsiyonel
        isActive: employee.isActive !== undefined ? employee.isActive : true
      });
    } else {
      // Yeni çalışan modunda formu temizle
      setFormData({
        firstName: '',
        lastName: '',
        identityNumber: '',
        email: '',
        phone: '',
        address: '',
        city: '',
        hireDate: '',
        terminationDate: '',
        salary: '',
        salesQuota: '10000',
        commissionRate: '0.10',
        role: 'Sales',
        username: '',
        password: '',
        isActive: true
      });
    }
  }, [employee]);

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

    // FirstName validation
    if (!formData.firstName.trim()) {
      newErrors.firstName = 'Ad zorunludur';
    }

    // LastName validation
    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Soyad zorunludur';
    }

    // IdentityNumber validation (TC Kimlik No - 11 digits)
    if (!formData.identityNumber.trim()) {
      newErrors.identityNumber = 'TC Kimlik Numarası zorunludur';
    } else if (!/^\d{11}$/.test(formData.identityNumber.trim())) {
      newErrors.identityNumber = 'TC Kimlik Numarası 11 haneli olmalıdır';
    }

    // Email validation
    if (!formData.email.trim()) {
      newErrors.email = 'E-posta zorunludur';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'Geçerli bir e-posta adresi giriniz';
    }

    // HireDate validation
    if (!formData.hireDate) {
      newErrors.hireDate = 'İşe giriş tarihi zorunludur';
    }

    // Salary validation
    if (!formData.salary) {
      newErrors.salary = 'Maaş zorunludur';
    } else if (isNaN(formData.salary) || parseFloat(formData.salary) < 0) {
      newErrors.salary = 'Maaş 0 veya daha büyük olmalıdır';
    }

    // SalesQuota validation
    if (!formData.salesQuota) {
      newErrors.salesQuota = 'Satış kotası zorunludur';
    } else if (isNaN(formData.salesQuota) || parseFloat(formData.salesQuota) < 0) {
      newErrors.salesQuota = 'Satış kotası 0 veya daha büyük olmalıdır';
    }

    // CommissionRate validation
    if (!formData.commissionRate) {
      newErrors.commissionRate = 'Komisyon oranı zorunludur';
    } else if (isNaN(formData.commissionRate) || parseFloat(formData.commissionRate) < 0 || parseFloat(formData.commissionRate) > 1) {
      newErrors.commissionRate = 'Komisyon oranı 0 ile 1 arasında olmalıdır';
    }

    // Role validation
    if (!formData.role) {
      newErrors.role = 'Rol zorunludur';
    }

    // Username validation
    if (!formData.username.trim()) {
      newErrors.username = 'Kullanıcı adı zorunludur';
    }

    // Password validation (only required for new employees)
    if (!employee && !formData.password) {
      newErrors.password = 'Şifre zorunludur';
    } else if (formData.password && formData.password.length < 6) {
      newErrors.password = 'Şifre en az 6 karakter olmalıdır';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (validate()) {
      const submitData = {
        firstName: formData.firstName.trim(),
        lastName: formData.lastName.trim(),
        identityNumber: formData.identityNumber.trim(),
        email: formData.email.trim(),
        phone: formData.phone.trim().replace(/[\s\-\(\)]/g, '') || null, // Boşluk, tire, parantez temizle
        address: formData.address.trim() || null,
        city: formData.city.trim() || null,
        hireDate: formData.hireDate,
        terminationDate: formData.terminationDate || null,
        salary: parseFloat(formData.salary),
        salesQuota: parseFloat(formData.salesQuota),
        commissionRate: parseFloat(formData.commissionRate),
        role: formData.role,
        username: formData.username.trim(),
        isActive: formData.isActive
      };

      // Password sadece yeni çalışan veya değiştiriliyorsa ekle
      if (!employee || formData.password) {
        submitData.password = formData.password;
      }

      onSubmit(submitData);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="firstName">
            Ad <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            placeholder="Örn: Ahmet"
            disabled={loading}
          />
          {errors.firstName && <span className="error-text">{errors.firstName}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="lastName">
            Soyad <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            placeholder="Örn: Yılmaz"
            disabled={loading}
          />
          {errors.lastName && <span className="error-text">{errors.lastName}</span>}
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="identityNumber">
            TC Kimlik Numarası <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="text"
            id="identityNumber"
            name="identityNumber"
            value={formData.identityNumber}
            onChange={handleChange}
            placeholder="11 haneli TC kimlik no"
            maxLength="11"
            pattern="[0-9]*"
            inputMode="numeric"
            disabled={loading}
          />
          {errors.identityNumber && <span className="error-text">{errors.identityNumber}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="phone">Telefon</label>
          <input
            type="text"
            id="phone"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            placeholder="0555 123 45 67"
            pattern="[0-9\-\s\(\)]+"
            title="Sadece rakam, tire, boşluk ve parantez kullanabilirsiniz"
            inputMode="numeric"
            disabled={loading}
          />
          {errors.phone && <span className="error-text">{errors.phone}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="email">
          E-posta <span style={{ color: 'red' }}>*</span>
        </label>
        <input
          type="email"
          id="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          placeholder="ornek@email.com"
          disabled={loading}
        />
        {errors.email && <span className="error-text">{errors.email}</span>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '2fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="address">Adres</label>
          <input
            type="text"
            id="address"
            name="address"
            value={formData.address}
            onChange={handleChange}
            placeholder="Tam adres"
            disabled={loading}
          />
          {errors.address && <span className="error-text">{errors.address}</span>}
        </div>

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
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="hireDate">
            İşe Giriş Tarihi <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="date"
            id="hireDate"
            name="hireDate"
            value={formData.hireDate}
            onChange={handleChange}
            disabled={loading}
          />
          {errors.hireDate && <span className="error-text">{errors.hireDate}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="terminationDate">İşten Çıkış Tarihi</label>
          <input
            type="date"
            id="terminationDate"
            name="terminationDate"
            value={formData.terminationDate}
            onChange={handleChange}
            disabled={loading}
          />
          {errors.terminationDate && <span className="error-text">{errors.terminationDate}</span>}
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="salary">
            Maaş (₺) <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="salary"
            name="salary"
            value={formData.salary}
            onChange={handleChange}
            placeholder="0.00"
            step="0.01"
            min="0"
            disabled={loading}
          />
          {errors.salary && <span className="error-text">{errors.salary}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="salesQuota">
            Satış Kotası (₺) <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="salesQuota"
            name="salesQuota"
            value={formData.salesQuota}
            onChange={handleChange}
            placeholder="10000"
            step="0.01"
            min="0"
            disabled={loading}
          />
          {errors.salesQuota && <span className="error-text">{errors.salesQuota}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="commissionRate">
            Komisyon Oranı <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="commissionRate"
            name="commissionRate"
            value={formData.commissionRate}
            onChange={handleChange}
            placeholder="0.10"
            step="0.01"
            min="0"
            max="1"
            disabled={loading}
          />
          {errors.commissionRate && <span className="error-text">{errors.commissionRate}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="role">
          Rol <span style={{ color: 'red' }}>*</span>
        </label>
        <select
          id="role"
          name="role"
          value={formData.role}
          onChange={handleChange}
          disabled={loading}
        >
          <option value="Sales">Satış</option>
          <option value="Manager">Yönetici</option>
          <option value="Admin">Admin</option>
        </select>
        {errors.role && <span className="error-text">{errors.role}</span>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="username">
            Kullanıcı Adı <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleChange}
            placeholder="Kullanıcı adı"
            disabled={loading}
          />
          {errors.username && <span className="error-text">{errors.username}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="password">
            Şifre {!employee && <span style={{ color: 'red' }}>*</span>}
            {employee && <small style={{ color: '#666' }}> (değiştirmek için doldurun)</small>}
          </label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            placeholder={employee ? "Yeni şifre (opsiyonel)" : "Şifre"}
            disabled={loading}
          />
          {errors.password && <span className="error-text">{errors.password}</span>}
        </div>
      </div>

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
          disabled={loading}
        >
          {loading ? 'Kaydediliyor...' : (employee ? 'Güncelle' : 'Kaydet')}
        </button>
      </div>
    </form>
  );
};

export default EmployeeForm;
