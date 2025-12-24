import { useState, useEffect } from 'react';

const SupplierForm = ({ supplier, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    companyName: '',
    contactName: '',
    contactTitle: '',
    email: '',
    phone: '',
    address: '',
    city: '',
    country: '',
    postalCode: '',
    taxNumber: '',
    isActive: true
  });

  const [errors, setErrors] = useState({});

  // Tedarikçi düzenleme modunda formu doldur
  useEffect(() => {
    if (supplier) {
      setFormData({
        companyName: supplier.companyName || '',
        contactName: supplier.contactName || '',
        contactTitle: supplier.contactTitle || '',
        email: supplier.email || '',
        phone: supplier.phone || '',
        address: supplier.address || '',
        city: supplier.city || '',
        country: supplier.country || '',
        postalCode: supplier.postalCode || '',
        taxNumber: supplier.taxNumber || '',
        isActive: supplier.isActive !== undefined ? supplier.isActive : true
      });
    } else {
      // Yeni tedarikçi modunda formu temizle
      setFormData({
        companyName: '',
        contactName: '',
        contactTitle: '',
        email: '',
        phone: '',
        address: '',
        city: '',
        country: '',
        postalCode: '',
        taxNumber: '',
        isActive: true
      });
    }
  }, [supplier]);

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

    // CompanyName validation (required)
    if (!formData.companyName.trim()) {
      newErrors.companyName = 'Firma adı zorunludur';
    } else if (formData.companyName.trim().length > 200) {
      newErrors.companyName = 'Firma adı en fazla 200 karakter olmalıdır';
    }

    // ContactName validation (optional)
    if (formData.contactName && formData.contactName.trim().length > 100) {
      newErrors.contactName = 'İlgili kişi adı en fazla 100 karakter olmalıdır';
    }

    // ContactTitle validation (optional)
    if (formData.contactTitle && formData.contactTitle.trim().length > 100) {
      newErrors.contactTitle = 'İlgili kişi ünvanı en fazla 100 karakter olmalıdır';
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
    if (formData.phone && formData.phone.trim().length > 20) {
      newErrors.phone = 'Telefon en fazla 20 karakter olmalıdır';
    }

    // Address validation (optional)
    if (formData.address && formData.address.trim().length > 500) {
      newErrors.address = 'Adres en fazla 500 karakter olmalıdır';
    }

    // City validation (optional)
    if (formData.city && formData.city.trim().length > 100) {
      newErrors.city = 'Şehir en fazla 100 karakter olmalıdır';
    }

    // Country validation (optional)
    if (formData.country && formData.country.trim().length > 100) {
      newErrors.country = 'Ülke en fazla 100 karakter olmalıdır';
    }

    // PostalCode validation (optional)
    if (formData.postalCode && formData.postalCode.trim().length > 10) {
      newErrors.postalCode = 'Posta kodu en fazla 10 karakter olmalıdır';
    }

    // TaxNumber validation (optional)
    if (formData.taxNumber && formData.taxNumber.trim().length > 20) {
      newErrors.taxNumber = 'Vergi numarası en fazla 20 karakter olmalıdır';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (validate()) {
      // Convert empty strings to null for optional fields
      const submitData = {
        companyName: formData.companyName.trim(),
        contactName: formData.contactName.trim() || null,
        contactTitle: formData.contactTitle.trim() || null,
        email: formData.email.trim() || null,
        phone: formData.phone.trim() || null,
        address: formData.address.trim() || null,
        city: formData.city.trim() || null,
        country: formData.country.trim() || null,
        postalCode: formData.postalCode.trim() || null,
        taxNumber: formData.taxNumber.trim() || null,
        isActive: formData.isActive
      };

      onSubmit(submitData);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="companyName">
          Firma Adı <span style={{ color: 'red' }}>*</span>
        </label>
        <input
          type="text"
          id="companyName"
          name="companyName"
          value={formData.companyName}
          onChange={handleChange}
          placeholder="Örn: ABC Elektronik Ltd."
          disabled={loading}
        />
        {errors.companyName && <span className="error-text">{errors.companyName}</span>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="contactName">İlgili Kişi Adı</label>
          <input
            type="text"
            id="contactName"
            name="contactName"
            value={formData.contactName}
            onChange={handleChange}
            placeholder="Örn: Mehmet Yılmaz"
            disabled={loading}
          />
          {errors.contactName && <span className="error-text">{errors.contactName}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="contactTitle">İlgili Kişi Ünvanı</label>
          <input
            type="text"
            id="contactTitle"
            name="contactTitle"
            value={formData.contactTitle}
            onChange={handleChange}
            placeholder="Örn: Satış Müdürü"
            disabled={loading}
          />
          {errors.contactTitle && <span className="error-text">{errors.contactTitle}</span>}
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
            placeholder="ornek@firma.com"
            disabled={loading}
          />
          {errors.email && <span className="error-text">{errors.email}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="phone">Telefon</label>
          <input
            type="tel"
            id="phone"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            placeholder="0212 123 45 67"
            pattern="[0-9]*"
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

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '1rem' }}>
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
          <label htmlFor="country">Ülke</label>
          <input
            type="text"
            id="country"
            name="country"
            value={formData.country}
            onChange={handleChange}
            placeholder="Örn: Türkiye"
            disabled={loading}
          />
          {errors.country && <span className="error-text">{errors.country}</span>}
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
        <label htmlFor="taxNumber">Vergi Numarası</label>
        <input
          type="text"
          id="taxNumber"
          name="taxNumber"
          value={formData.taxNumber}
          onChange={handleChange}
          placeholder="Örn: 1234567890"
          pattern="[0-9]*"
          inputMode="numeric"
          disabled={loading}
        />
        {errors.taxNumber && <span className="error-text">{errors.taxNumber}</span>}
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
          {loading ? 'Kaydediliyor...' : (supplier ? 'Güncelle' : 'Kaydet')}
        </button>
      </div>
    </form>
  );
};

export default SupplierForm;
