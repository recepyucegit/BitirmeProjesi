import { useState, useEffect } from 'react';

const RoleForm = ({ role, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    isActive: true
  });

  useEffect(() => {
    if (role) {
      setFormData({
        name: role.name || '',
        description: role.description || '',
        isActive: role.isActive !== undefined ? role.isActive : true
      });
    }
  }, [role]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    // Validation
    if (!formData.name.trim()) {
      alert('Rol adı gereklidir');
      return;
    }

    // Prepare data
    const submitData = {
      name: formData.name.trim(),
      description: formData.description.trim() || null,
      isActive: formData.isActive
    };

    onSubmit(submitData);
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="mb-3">
        <label className="form-label">Rol Adı *</label>
        <input
          type="text"
          className="form-control"
          name="name"
          value={formData.name}
          onChange={handleChange}
          placeholder="Örn: Admin, BranchManager, Cashier"
          required
        />
        <small className="text-muted">
          Önerilen roller: Admin, BranchManager, Cashier, Warehouse, Accounting, TechnicalService
        </small>
      </div>

      <div className="mb-3">
        <label className="form-label">Açıklama</label>
        <textarea
          className="form-control"
          name="description"
          value={formData.description}
          onChange={handleChange}
          rows="3"
          placeholder="Rolün görev ve yetkilerini açıklayın..."
        />
      </div>

      <div className="mb-3">
        <div className="form-check">
          <input
            type="checkbox"
            className="form-check-input"
            name="isActive"
            id="isActive"
            checked={formData.isActive}
            onChange={handleChange}
          />
          <label className="form-check-label" htmlFor="isActive">
            Aktif
          </label>
        </div>
      </div>

      <div className="d-flex justify-content-end gap-2">
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
          {loading ? 'Kaydediliyor...' : (role ? 'Güncelle' : 'Ekle')}
        </button>
      </div>
    </form>
  );
};

export default RoleForm;
