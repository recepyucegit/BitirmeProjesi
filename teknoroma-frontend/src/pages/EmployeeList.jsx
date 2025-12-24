import { useState, useEffect } from 'react';
import { employeeAPI } from '../services/api';
import Modal from '../components/Modal';
import EmployeeForm from '../components/EmployeeForm';

const EmployeeList = () => {
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  // Component yüklendiğinde çalışanları API'den çek
  useEffect(() => {
    fetchEmployees();
  }, []);

  const fetchEmployees = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await employeeAPI.getAll();
      setEmployees(data || []);
    } catch (err) {
      setError('Çalışanlar yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching employees:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="loading">Çalışanlar yükleniyor...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  const handleAdd = () => {
    setSelectedEmployee(null);
    setIsModalOpen(true);
  };

  const handleEdit = (employee) => {
    setSelectedEmployee(employee);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu çalışanı silmek istediğinizden emin misiniz?')) {
      try {
        await employeeAPI.delete(id);
        await fetchEmployees();
        alert('Çalışan başarıyla silindi');
      } catch (err) {
        alert('Çalışan silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting employee:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedEmployee) {
        // Update - include id in the request body
        await employeeAPI.update(selectedEmployee.id, {
          ...formData,
          id: selectedEmployee.id
        });
      } else {
        // Create
        await employeeAPI.create(formData);
      }

      // Önce verileri yenile
      await fetchEmployees();

      // Sonra modal'ı kapat ve mesajı göster
      setIsModalOpen(false);
      setSelectedEmployee(null);

      alert(selectedEmployee ? 'Çalışan başarıyla güncellendi' : 'Çalışan başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving employee:', err);
    } finally {
      setFormLoading(false);
    }
  };

  // Komisyon hesaplama fonksiyonu
  const calculateCommission = (employee) => {
    // Bu örnekte sadece formülü gösteriyoruz
    // Gerçek hesaplama backend'den gelmeli
    const commissionRate = employee.commissionRate || 0.10;
    return `${(commissionRate * 100).toFixed(0)}%`;
  };

  // Tarihi formatla
  const formatDate = (dateString) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR');
  };

  // Rol Türkçeleştir
  const getRoleText = (role) => {
    const roles = {
      'Sales': 'Satış',
      'Manager': 'Yönetici',
      'Admin': 'Admin'
    };
    return roles[role] || role;
  };

  return (
    <>
      <div className="card">
        <div className="page-header">
          <h2>Çalışanlar ({employees.length})</h2>
          <button className="btn btn-success" onClick={handleAdd}>
            + Yeni Çalışan
          </button>
        </div>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Ad Soyad</th>
              <th>TC Kimlik</th>
              <th>E-posta</th>
              <th>Telefon</th>
              <th>Rol</th>
              <th>Maaş</th>
              <th>Satış Kotası</th>
              <th>Komisyon</th>
              <th>İşe Giriş</th>
              <th>Durum</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {employees.map((employee) => (
              <tr key={employee.id}>
                <td>{employee.id}</td>
                <td>
                  <strong>{employee.firstName} {employee.lastName}</strong>
                  <br />
                  <small style={{ color: '#666' }}>{employee.username}</small>
                </td>
                <td>{employee.identityNumber || '-'}</td>
                <td>{employee.email || '-'}</td>
                <td>{employee.phone || '-'}</td>
                <td>
                  <span className={`badge ${
                    employee.role === 'Admin' ? 'badge-danger' :
                    employee.role === 'Manager' ? 'badge-warning' :
                    'badge-info'
                  }`}>
                    {getRoleText(employee.role)}
                  </span>
                </td>
                <td>{employee.salary ? `₺${employee.salary.toLocaleString('tr-TR', { minimumFractionDigits: 2 })}` : '-'}</td>
                <td>{employee.salesQuota ? `₺${employee.salesQuota.toLocaleString('tr-TR')}` : '-'}</td>
                <td>{calculateCommission(employee)}</td>
                <td>{formatDate(employee.hireDate)}</td>
                <td>
                  <span className={`badge ${employee.isActive ? 'badge-success' : 'badge-danger'}`}>
                    {employee.isActive ? 'Aktif' : 'Pasif'}
                  </span>
                </td>
                <td>
                  <div className="btn-group">
                    <button
                      className="btn btn-sm btn-warning"
                      onClick={() => handleEdit(employee)}
                    >
                      Düzenle
                    </button>
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() => handleDelete(employee.id)}
                    >
                      Sil
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {employees.length === 0 && (
          <div style={{ textAlign: 'center', padding: '2rem', color: '#666' }}>
            Henüz çalışan bulunmamaktadır.
          </div>
        )}
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={selectedEmployee ? 'Çalışan Düzenle' : 'Yeni Çalışan'}
      >
        <EmployeeForm
          employee={selectedEmployee}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
          loading={formLoading}
        />
      </Modal>
    </>
  );
};

export default EmployeeList;
