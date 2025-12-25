import { useState, useEffect } from 'react';

const CustomerForm = ({ customer, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    identityNumber: '',
    customerType: 'Individual',
    email: '',
    phone: '',
    address: '',
    city: '',
    postalCode: '',
    isActive: true
  });

  const [errors, setErrors] = useState({});

  // Müşteri düzenleme modunda formu doldur
  useEffect(() => {
    if (customer) {
      setFormData({
        firstName: customer.firstName || '',
        lastName: customer.lastName || '',
        identityNumber: customer.identityNumber || '',
        customerType: customer.customerType || 'Individual',
        email: customer.email || '',
        phone: customer.phone || '',
        address: customer.address || '',
        city: customer.city || '',
        postalCode: customer.postalCode || '',
        isActive: customer.isActive !== undefined ? customer.isActive : true
      });
    } else {
      // Yeni müşteri modunda formu temizle
      setFormData({
        firstName: '',
        lastName: '',
        identityNumber: '',
        customerType: 'Individual',
        email: '',
        phone: '',
        address: '',
        city: '',
        postalCode: '',
        isActive: true
      });
    }
  }, [customer]);

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
      newErrors.firstName = 'Ad alanı zorunludur';
    } else if (formData.firstName.trim().length > 100) {
      newErrors.firstName = 'Ad en fazla 100 karakter olmalıdır';
    }

    // LastName validation
    if (!formData.lastName.trim()) {
      newErrors.lastName = 'Soyad alanı zorunludur';
    } else if (formData.lastName.trim().length > 100) {
      newErrors.lastName = 'Soyad en fazla 100 karakter olmalıdır';
    }

    // IdentityNumber validation (TC Kimlik No - 11 digits)
    if (!formData.identityNumber.trim()) {
      newErrors.identityNumber = 'TC Kimlik Numarası zorunludur';
    } else if (!/^\d{11}$/.test(formData.identityNumber.trim())) {
      newErrors.identityNumber = 'TC Kimlik Numarası 11 haneli olmalıdır';
    }

    // CustomerType validation
    if (!formData.customerType) {
      newErrors.customerType = 'Müşteri tipi zorunludur';
    } else if (!['Individual', 'Corporate'].includes(formData.customerType)) {
      newErrors.customerType = 'Müşteri tipi Individual veya Corporate olmalıdır';
    }

    // Email validation (optional but must be valid if provided)
    if (formData.email && formData.email.trim()) {
      if (formData.email.length > 100) {
        newErrors.email = 'E-posta en fazla 100 karakter olmalıdır';
      } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
        newErrors.email = 'Geçerli bir e-posta adresi giriniz';
      }
    }

    // Phone validation (optional)
    if (formData.phone && formData.phone.trim()) {
      if (formData.phone.length > 20) {
        newErrors.phone = 'Telefon en fazla 20 karakter olmalıdır';
      }
    }

    // Address validation (optional)
    if (formData.address && formData.address.trim().length > 500) {
      newErrors.address = 'Adres en fazla 500 karakter olmalıdır';
    }

    // City validation (optional)
    if (formData.city && formData.city.trim().length > 100) {
      newErrors.city = 'Şehir en fazla 100 karakter olmalıdır';
    }

    // PostalCode validation (optional)
    if (formData.postalCode && formData.postalCode.trim().length > 10) {
      newErrors.postalCode = 'Posta kodu en fazla 10 karakter olmalıdır';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (validate()) {
      // Convert empty strings to null for optional fields
      const submitData = {
        firstName: formData.firstName.trim(),
        lastName: formData.lastName.trim(),
        identityNumber: formData.identityNumber.trim(),
        customerType: formData.customerType,
        email: formData.email.trim() || null,
        phone: formData.phone.trim().replace(/[\s\-\(\)]/g, '') || null, // Boşluk, tire, parantez temizle
        address: formData.address.trim() || null,
        city: formData.city.trim() || null,
        postalCode: formData.postalCode.trim() || null,
        isActive: formData.isActive
      };

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
          <label htmlFor="customerType">
            Müşteri Tipi <span style={{ color: 'red' }}>*</span>
          </label>
          <select
            id="customerType"
            name="customerType"
            value={formData.customerType}
            onChange={handleChange}
            disabled={loading}
          >
            <option value="Individual">Bireysel</option>
            <option value="Corporate">Kurumsal</option>
          </select>
          {errors.customerType && <span className="error-text">{errors.customerType}</span>}
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="email">E-posta</label>
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
        <label htmlFor="address">Adres</label>
        <textarea
          id="address"
          name="address"
          value={formData.address}
          onChange={handleChange}
          placeholder="Tam adres bilgisi"
          disabled={loading}
          rows="2"
        />
        {errors.address && <span className="error-text">{errors.address}</span>}
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
          <label htmlFor="postalCode">Posta Kodu</label>
          <input
            type="text"
            id="postalCode"
            name="postalCode"
            value={formData.postalCode}
            onChange={handleChange}
            placeholder="Örn: 34000"
            pattern="[0-9]*"
            inputMode="numeric"
            disabled={loading}
          />
          {errors.postalCode && <span className="error-text">{errors.postalCode}</span>}
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
          {loading ? 'Kaydediliyor...' : (customer ? 'Güncelle' : 'Kaydet')}
        </button>
      </div>
    </form>
  );
};

export default CustomerForm;
