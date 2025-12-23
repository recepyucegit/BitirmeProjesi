import React, { useEffect, useState } from 'react';
import { employeeAPI } from '../services/api';
import Modal from '../components/Modal';
import EmployeeForm from '../components/EmployeeForm';

function EmployeeList() {
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState(null);

  useEffect(() => {
    fetchEmployees();
  }, []);

  const fetchEmployees = async () => {
    try {
      setLoading(true);
      const response = await employeeAPI.getAll();
      setEmployees(response.data);
      setError(null);
    } catch (err) {
      setError('Çalışanlar yüklenirken bir hata oluştu: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = () => {
    setSelectedEmployee(null);
    setIsModalOpen(true);
  };

  const handleEdit = (employee) => {
    setSelectedEmployee(employee);
    setIsModalOpen(true);
  };

  const handleSubmit = async (formData) => {
    try {
      if (selectedEmployee) {
        // Update existing employee
        const response = await employeeAPI.update(selectedEmployee.id, { ...formData, id: selectedEmployee.id });
        setEmployees(employees.map(e => e.id === selectedEmployee.id ? response.data : e));
      } else {
        // Create new employee
        const response = await employeeAPI.create(formData);
        setEmployees([...employees, response.data]);
      }
      setIsModalOpen(false);
      setSelectedEmployee(null);
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
    }
  };

  const handleDelete = async (id, firstName, lastName) => {
    if (!window.confirm(`${firstName} ${lastName} çalışanını silmek istediğinize emin misiniz?`)) {
      return;
    }

    try {
      await employeeAPI.delete(id);
      setEmployees(employees.filter(e => e.id !== id));
    } catch (err) {
      alert('Çalışan silinirken bir hata oluştu: ' + err.message);
    }
  };

  const formatCurrency = (value) => {
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: 'TRY'
    }).format(value);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('tr-TR');
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="card">
      <h2>Çalışan Listesi</h2>

      <div style={{ marginBottom: '20px' }}>
        <button className="btn btn-primary" onClick={handleAdd}>
          Yeni Çalışan Ekle
        </button>
      </div>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Ad Soyad</th>
            <th>Email</th>
            <th>Telefon</th>
            <th>Şehir</th>
            <th>Pozisyon</th>
            <th>İşe Giriş</th>
            <th>Maaş</th>
            <th>Durum</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {employees.map((employee) => (
            <tr key={employee.id}>
              <td>{employee.id}</td>
              <td>{employee.firstName} {employee.lastName}</td>
              <td>{employee.email}</td>
              <td>{employee.phone}</td>
              <td>{employee.city}</td>
              <td>{employee.role}</td>
              <td>{formatDate(employee.hireDate)}</td>
              <td>{formatCurrency(employee.salary)}</td>
              <td>
                <span className={`badge ${employee.isActive ? 'badge-success' : 'badge-danger'}`}>
                  {employee.isActive ? 'Aktif' : 'Pasif'}
                </span>
              </td>
              <td>
                <button
                  className="btn btn-sm btn-info"
                  style={{ marginRight: '5px' }}
                  onClick={() => handleEdit(employee)}
                >
                  Düzenle
                </button>
                <button
                  className="btn btn-sm btn-danger"
                  onClick={() => handleDelete(employee.id, employee.firstName, employee.lastName)}
                >
                  Sil
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {employees.length === 0 && (
        <p style={{ textAlign: 'center', padding: '20px', color: '#666' }}>
          Henüz çalışan bulunmamaktadır.
        </p>
      )}

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={selectedEmployee ? 'Çalışan Düzenle' : 'Yeni Çalışan Ekle'}
      >
        <EmployeeForm
          employee={selectedEmployee}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
        />
      </Modal>
    </div>
  );
}

export default EmployeeList;
