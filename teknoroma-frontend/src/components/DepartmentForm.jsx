import { useState, useEffect } from 'react';
import { employeeAPI } from '../services/api';

const DepartmentForm = ({ department, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    departmentName: '',
    departmentCode: '',
    description: '',
    managerId: '',
    budget: '',
    employeeCount: '',
    isActive: true
  });

  const [employees, setEmployees] = useState([]);
  const [loadingEmployees, setLoadingEmployees] = useState(true);

  useEffect(() => {
    fetchEmployees();
  }, []);

  useEffect(() => {
    if (department) {
      setFormData({
        departmentName: department.departmentName || '',
        departmentCode: department.departmentCode || '',
        description: department.description || '',
        managerId: department.managerId || '',
        budget: department.budget || '',
        employeeCount: department.employeeCount || '',
        isActive: department.isActive !== undefined ? department.isActive : true
      });
    }
  }, [department]);

  const fetchEmployees = async () => {
    try {
      const data = await employeeAPI.getAll();
      setEmployees(data || []);
    } catch (err) {
      console.error('Error fetching employees:', err);
    } finally {
      setLoadingEmployees(false);
    }
  };

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.departmentName.trim()) {
      alert('Departman adı gereklidir');
      return;
    }
    if (!formData.departmentCode.trim()) {
      alert('Departman kodu gereklidir');
      return;
    }

    const submitData = {
      departmentName: formData.departmentName.trim(),
      departmentCode: formData.departmentCode.trim(),
      description: formData.description.trim() || null,
      managerId: formData.managerId ? parseInt(formData.managerId) : null,
      budget: formData.budget ? parseFloat(formData.budget) : null,
      employeeCount: formData.employeeCount ? parseInt(formData.employeeCount) : null,
      isActive: formData.isActive
    };

    onSubmit(submitData);
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="row">
        <div className="col-md-6 mb-3">
          <label className="form-label">Departman Adı *</label>
          <input
            type="text"
            className="form-control"
            name="departmentName"
            value={formData.departmentName}
            onChange={handleChange}
            placeholder="Örn: Bilgi Teknolojileri"
            required
          />
        </div>

        <div className="col-md-6 mb-3">
          <label className="form-label">Departman Kodu *</label>
          <input
            type="text"
            className="form-control"
            name="departmentCode"
            value={formData.departmentCode}
            onChange={handleChange}
            placeholder="Örn: IT-001"
            required
          />
        </div>
      </div>

      <div className="mb-3">
        <label className="form-label">Açıklama</label>
        <textarea
          className="form-control"
          name="description"
          value={formData.description}
          onChange={handleChange}
          rows="3"
          placeholder="Departman açıklaması..."
        />
      </div>

      <div className="row">
        <div className="col-md-6 mb-3">
          <label className="form-label">Departman Müdürü</label>
          <select
            className="form-select"
            name="managerId"
            value={formData.managerId}
            onChange={handleChange}
            disabled={loadingEmployees}
          >
            <option value="">Seçiniz (opsiyonel)</option>
            {employees.map(emp => (
              <option key={emp.id} value={emp.id}>
                {emp.firstName} {emp.lastName}
              </option>
            ))}
          </select>
        </div>

        <div className="col-md-6 mb-3">
          <label className="form-label">Bütçe (TL)</label>
          <input
            type="number"
            className="form-control"
            name="budget"
            value={formData.budget}
            onChange={handleChange}
            min="0"
            step="0.01"
            placeholder="0.00"
          />
        </div>
      </div>

      <div className="row">
        <div className="col-md-6 mb-3">
          <label className="form-label">Çalışan Sayısı</label>
          <input
            type="number"
            className="form-control"
            name="employeeCount"
            value={formData.employeeCount}
            onChange={handleChange}
            min="0"
            placeholder="0"
          />
        </div>

        <div className="col-md-6 mb-3 d-flex align-items-end">
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
          {loading ? 'Kaydediliyor...' : (department ? 'Güncelle' : 'Ekle')}
        </button>
      </div>
    </form>
  );
};

export default DepartmentForm;
