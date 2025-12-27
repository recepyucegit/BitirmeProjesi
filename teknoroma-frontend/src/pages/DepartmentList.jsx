import { useState, useEffect } from 'react';
import { departmentAPI } from '../services/api';
import Modal from '../components/Modal';
import DepartmentForm from '../components/DepartmentForm';

const DepartmentList = () => {
  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedDepartment, setSelectedDepartment] = useState(null);
  const [formLoading, setFormLoading] = useState(false);
  const [stats, setStats] = useState({ totalEmployees: 0, totalBudget: 0 });

  useEffect(() => {
    fetchDepartments();
    fetchStats();
  }, []);

  const fetchDepartments = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await departmentAPI.getAll();
      setDepartments(data || []);
    } catch (err) {
      setError('Departmanlar yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching departments:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchStats = async () => {
    try {
      const [totalEmployees, totalBudget] = await Promise.all([
        departmentAPI.getTotalEmployees(),
        departmentAPI.getTotalBudget()
      ]);
      setStats({ totalEmployees, totalBudget });
    } catch (err) {
      console.error('Error fetching stats:', err);
    }
  };

  const handleAdd = () => {
    setSelectedDepartment(null);
    setIsModalOpen(true);
  };

  const handleEdit = (department) => {
    setSelectedDepartment(department);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu departmanı silmek istediğinizden emin misiniz?')) {
      try {
        await departmentAPI.delete(id);
        await fetchDepartments();
        await fetchStats();
        alert('Departman başarıyla silindi');
      } catch (err) {
        alert('Departman silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting department:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedDepartment) {
        await departmentAPI.update(selectedDepartment.id, formData);
      } else {
        await departmentAPI.create(formData);
      }

      await fetchDepartments();
      await fetchStats();
      setIsModalOpen(false);
      setSelectedDepartment(null);

      alert(selectedDepartment ? 'Departman başarıyla güncellendi' : 'Departman başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving department:', err);
    } finally {
      setFormLoading(false);
    }
  };

  const formatCurrency = (amount) => {
    if (!amount) return '0 TL';
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: 'TRY'
    }).format(amount);
  };

  if (loading) {
    return (
      <div className="container mt-4">
        <div className="text-center">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Yükleniyor...</span>
          </div>
          <p className="mt-2">Departmanlar yükleniyor...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="container mt-4">
        <div className="alert alert-danger" role="alert">
          {error}
        </div>
      </div>
    );
  }

  return (
    <div className="container-fluid mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <div>
          <h2>Departman Yönetimi</h2>
          <p className="text-muted">Şirket departmanlarını yönetin</p>
        </div>
        <button className="btn btn-primary" onClick={handleAdd}>
          <i className="bi bi-plus-circle me-2"></i>
          Yeni Departman
        </button>
      </div>

      {/* İstatistikler */}
      <div className="row mb-4">
        <div className="col-md-4">
          <div className="card">
            <div className="card-body">
              <h6 className="text-muted mb-2">Toplam Departman</h6>
              <h3 className="mb-0">{departments.length}</h3>
            </div>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card">
            <div className="card-body">
              <h6 className="text-muted mb-2">Toplam Çalışan</h6>
              <h3 className="mb-0">{stats.totalEmployees}</h3>
            </div>
          </div>
        </div>
        <div className="col-md-4">
          <div className="card">
            <div className="card-body">
              <h6 className="text-muted mb-2">Toplam Bütçe</h6>
              <h3 className="mb-0">{formatCurrency(stats.totalBudget)}</h3>
            </div>
          </div>
        </div>
      </div>

      {/* Departman Listesi */}
      <div className="card">
        <div className="card-header">
          <h5 className="mb-0">Departmanlar</h5>
        </div>
        <div className="card-body">
          {departments.length === 0 ? (
            <div className="text-center py-5">
              <i className="bi bi-building" style={{ fontSize: '3rem', color: '#ccc' }}></i>
              <p className="mt-3 text-muted">Henüz departman bulunmamaktadır.</p>
              <button className="btn btn-primary mt-2" onClick={handleAdd}>
                İlk Departmanı Ekle
              </button>
            </div>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Departman Adı</th>
                    <th>Kod</th>
                    <th>Müdür</th>
                    <th>Çalışan Sayısı</th>
                    <th>Bütçe</th>
                    <th>Durum</th>
                    <th>İşlemler</th>
                  </tr>
                </thead>
                <tbody>
                  {departments.map((dept) => (
                    <tr key={dept.id}>
                      <td>{dept.id}</td>
                      <td>
                        <strong>{dept.departmentName}</strong>
                        {dept.description && (
                          <div>
                            <small className="text-muted">{dept.description}</small>
                          </div>
                        )}
                      </td>
                      <td>
                        <span className="badge bg-secondary">{dept.departmentCode}</span>
                      </td>
                      <td>{dept.managerName || '-'}</td>
                      <td className="text-center">{dept.employeeCount || 0}</td>
                      <td>{formatCurrency(dept.budget)}</td>
                      <td>
                        {dept.isActive ? (
                          <span className="badge bg-success">Aktif</span>
                        ) : (
                          <span className="badge bg-secondary">Pasif</span>
                        )}
                      </td>
                      <td>
                        <div className="btn-group" role="group">
                          <button
                            className="btn btn-sm btn-outline-primary"
                            onClick={() => handleEdit(dept)}
                            title="Düzenle"
                          >
                            <i className="bi bi-pencil"></i>
                          </button>
                          <button
                            className="btn btn-sm btn-outline-danger"
                            onClick={() => handleDelete(dept.id)}
                            title="Sil"
                          >
                            <i className="bi bi-trash"></i>
                          </button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setSelectedDepartment(null);
        }}
        title={selectedDepartment ? 'Departman Düzenle' : 'Yeni Departman'}
      >
        <DepartmentForm
          department={selectedDepartment}
          onSubmit={handleSubmit}
          onCancel={() => {
            setIsModalOpen(false);
            setSelectedDepartment(null);
          }}
          loading={formLoading}
        />
      </Modal>
    </div>
  );
};

export default DepartmentList;
