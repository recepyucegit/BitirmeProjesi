import { useState, useEffect } from 'react';
import { roleAPI, employeeAPI } from '../services/api';

const UserForm = ({ user, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    phoneNumber: '',
    employeeId: '',
    roleIds: [],
    isActive: true
  });

  const [roles, setRoles] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [loadingData, setLoadingData] = useState(true);

  useEffect(() => {
    fetchRolesAndEmployees();
  }, []);

  useEffect(() => {
    if (user) {
      setFormData({
        username: user.username || '',
        email: user.email || '',
        password: '', // Password is not shown when editing
        phoneNumber: user.phoneNumber || '',
        employeeId: user.employeeId || '',
        roleIds: user.roleIds || [],
        isActive: user.isActive !== undefined ? user.isActive : true
      });
    }
  }, [user]);

  const fetchRolesAndEmployees = async () => {
    try {
      setLoadingData(true);
      const [rolesData, employeesData] = await Promise.all([
        roleAPI.getAll(),
        employeeAPI.getAll()
      ]);
      setRoles(rolesData || []);
      setEmployees(employeesData || []);
    } catch (err) {
      console.error('Error fetching roles and employees:', err);
      alert('Roller ve çalışanlar yüklenirken bir hata oluştu');
    } finally {
      setLoadingData(false);
    }
  };

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
  };

  const handleRoleChange = (e) => {
    const options = Array.from(e.target.selectedOptions);
    const selectedRoleIds = options.map(option => parseInt(option.value));
    setFormData(prev => ({
      ...prev,
      roleIds: selectedRoleIds
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    // Validation
    if (!formData.username.trim()) {
      alert('Kullanıcı adı gereklidir');
      return;
    }
    if (!formData.email.trim()) {
      alert('Email gereklidir');
      return;
    }
    if (!user && !formData.password.trim()) {
      alert('Şifre gereklidir');
      return;
    }
    if (formData.roleIds.length === 0) {
      alert('En az bir rol seçilmelidir');
      return;
    }

    // Prepare data
    const submitData = {
      username: formData.username,
      email: formData.email,
      phoneNumber: formData.phoneNumber || null,
      employeeId: formData.employeeId ? parseInt(formData.employeeId) : null,
      roleIds: formData.roleIds,
      isActive: formData.isActive
    };

    // Add password only for new users (UpdateUserDto doesn't have password field)
    if (!user) {
      submitData.password = formData.password;
    }

    onSubmit(submitData);
  };

  if (loadingData) {
    return <div className="text-center">Yükleniyor...</div>;
  }

  return (
    <form onSubmit={handleSubmit}>
      <div className="row">
        <div className="col-md-6 mb-3">
          <label className="form-label">Kullanıcı Adı *</label>
          <input
            type="text"
            className="form-control"
            name="username"
            value={formData.username}
            onChange={handleChange}
            disabled={!!user} // Username cannot be changed when editing
            required
          />
          {user && <small className="text-muted">Kullanıcı adı değiştirilemez</small>}
        </div>

        <div className="col-md-6 mb-3">
          <label className="form-label">Email *</label>
          <input
            type="email"
            className="form-control"
            name="email"
            value={formData.email}
            onChange={handleChange}
            required
          />
        </div>
      </div>

      <div className="row">
        {!user && (
          <div className="col-md-6 mb-3">
            <label className="form-label">Şifre *</label>
            <input
              type="password"
              className="form-control"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>
        )}

        <div className={`col-md-${user ? '12' : '6'} mb-3`}>
          <label className="form-label">Telefon</label>
          <input
            type="tel"
            className="form-control"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
          />
        </div>
      </div>

      <div className="row">
        <div className="col-md-6 mb-3">
          <label className="form-label">Çalışan</label>
          <select
            className="form-select"
            name="employeeId"
            value={formData.employeeId}
            onChange={handleChange}
          >
            <option value="">Seçiniz (opsiyonel)</option>
            {employees.map(emp => (
              <option key={emp.id} value={emp.id}>
                {emp.firstName} {emp.lastName} - {emp.department || 'Departman Yok'}
              </option>
            ))}
          </select>
          <small className="text-muted">Bu kullanıcıyı bir çalışana bağlayın</small>
        </div>

        <div className="col-md-6 mb-3">
          <label className="form-label">Roller * (Ctrl+Click ile çoklu seçim)</label>
          <select
            className="form-select"
            name="roleIds"
            value={formData.roleIds}
            onChange={handleRoleChange}
            multiple
            size="5"
            required
          >
            {roles.map(role => (
              <option key={role.id} value={role.id}>
                {role.name} - {role.description || ''}
              </option>
            ))}
          </select>
          <small className="text-muted">En az bir rol seçilmelidir</small>
        </div>
      </div>

      <div className="row">
        <div className="col-12 mb-3">
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
          {loading ? 'Kaydediliyor...' : (user ? 'Güncelle' : 'Ekle')}
        </button>
      </div>
    </form>
  );
};

export default UserForm;
