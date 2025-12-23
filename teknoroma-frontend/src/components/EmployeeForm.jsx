import React, { useState, useEffect } from 'react';

function EmployeeForm({ employee, onSubmit, onCancel }) {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
    city: '',
    identityNumber: '',
    hireDate: new Date().toISOString().split('T')[0],
    salary: 0,
    salesQuota: 0,
    commissionRate: 0.05,
    role: 'Satış Danışmanı',
    username: '',
    passwordHash: '',
    isActive: true
  });

  useEffect(() => {
    if (employee) {
      setFormData({
        ...employee,
        hireDate: employee.hireDate ? new Date(employee.hireDate).toISOString().split('T')[0] : '',
        passwordHash: '' // Don't show password in edit mode
      });
    }
  }, [employee]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : type === 'number' ? parseFloat(value) : value
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="form">
      <div className="form-row">
        <div className="form-group">
          <label htmlFor="firstName">Ad *</label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="lastName">Soyad *</label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
          />
        </div>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="email">Email *</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="phone">Telefon *</label>
          <input
            type="tel"
            id="phone"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            required
          />
        </div>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="identityNumber">TC Kimlik No *</label>
          <input
            type="text"
            id="identityNumber"
            name="identityNumber"
            value={formData.identityNumber}
            onChange={handleChange}
            required
            maxLength="11"
          />
        </div>

        <div className="form-group">
          <label htmlFor="city">Şehir</label>
          <input
            type="text"
            id="city"
            name="city"
            value={formData.city}
            onChange={handleChange}
          />
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="address">Adres</label>
        <textarea
          id="address"
          name="address"
          value={formData.address}
          onChange={handleChange}
          rows="2"
        />
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="role">Pozisyon *</label>
          <select
            id="role"
            name="role"
            value={formData.role}
            onChange={handleChange}
            required
          >
            <option value="Satış Danışmanı">Satış Danışmanı</option>
            <option value="Mağaza Müdürü">Mağaza Müdürü</option>
            <option value="Kasa Görevlisi">Kasa Görevlisi</option>
            <option value="Muhasebe">Muhasebe</option>
            <option value="IT">IT</option>
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="hireDate">İşe Giriş Tarihi *</label>
          <input
            type="date"
            id="hireDate"
            name="hireDate"
            value={formData.hireDate}
            onChange={handleChange}
            required
          />
        </div>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="salary">Maaş (TL) *</label>
          <input
            type="number"
            id="salary"
            name="salary"
            value={formData.salary}
            onChange={handleChange}
            required
            min="0"
            step="0.01"
          />
        </div>

        <div className="form-group">
          <label htmlFor="salesQuota">Satış Hedefi (TL)</label>
          <input
            type="number"
            id="salesQuota"
            name="salesQuota"
            value={formData.salesQuota}
            onChange={handleChange}
            min="0"
            step="0.01"
          />
        </div>

        <div className="form-group">
          <label htmlFor="commissionRate">Komisyon Oranı</label>
          <input
            type="number"
            id="commissionRate"
            name="commissionRate"
            value={formData.commissionRate}
            onChange={handleChange}
            min="0"
            max="1"
            step="0.01"
          />
        </div>
      </div>

      <div className="form-row">
        <div className="form-group">
          <label htmlFor="username">Kullanıcı Adı *</label>
          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleChange}
            required
          />
        </div>

        <div className="form-group">
          <label htmlFor="passwordHash">
            {employee ? 'Yeni Şifre (boş bırakılırsa değişmez)' : 'Şifre *'}
          </label>
          <input
            type="password"
            id="passwordHash"
            name="passwordHash"
            value={formData.passwordHash}
            onChange={handleChange}
            required={!employee}
          />
        </div>
      </div>

      <div className="form-group">
        <label className="checkbox-label">
          <input
            type="checkbox"
            name="isActive"
            checked={formData.isActive}
            onChange={handleChange}
          />
          <span>Aktif</span>
        </label>
      </div>

      <div className="form-actions">
        <button type="button" className="btn btn-secondary" onClick={onCancel}>
          İptal
        </button>
        <button type="submit" className="btn btn-primary">
          {employee ? 'Güncelle' : 'Kaydet'}
        </button>
      </div>
    </form>
  );
}

export default EmployeeForm;
