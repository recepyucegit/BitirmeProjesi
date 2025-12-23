import React, { useEffect, useState } from 'react';
import { employeeAPI } from '../services/api';

function EmployeeList() {
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

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
        <button className="btn btn-primary">Yeni Çalışan Ekle</button>
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
                <button className="btn btn-sm btn-info" style={{ marginRight: '5px' }}>
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
    </div>
  );
}

export default EmployeeList;
