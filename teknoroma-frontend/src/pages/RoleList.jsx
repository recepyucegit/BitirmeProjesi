import { useState, useEffect } from 'react';
import { roleAPI } from '../services/api';
import Modal from '../components/Modal';
import RoleForm from '../components/RoleForm';

const RoleList = () => {
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedRole, setSelectedRole] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  useEffect(() => {
    fetchRoles();
  }, []);

  const fetchRoles = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await roleAPI.getAll();
      setRoles(data || []);
    } catch (err) {
      setError('Roller yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching roles:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = () => {
    setSelectedRole(null);
    setIsModalOpen(true);
  };

  const handleEdit = (role) => {
    setSelectedRole(role);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu rolü silmek istediğinizden emin misiniz? Bu role atanmış kullanıcılar etkilenebilir.')) {
      try {
        await roleAPI.delete(id);
        await fetchRoles();
        alert('Rol başarıyla silindi');
      } catch (err) {
        alert('Rol silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting role:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedRole) {
        // Update
        await roleAPI.update(selectedRole.id, formData);
      } else {
        // Create
        await roleAPI.create(formData);
      }

      await fetchRoles();
      setIsModalOpen(false);
      setSelectedRole(null);

      alert(selectedRole ? 'Rol başarıyla güncellendi' : 'Rol başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving role:', err);
    } finally {
      setFormLoading(false);
    }
  };

  const formatDate = (dateString) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('tr-TR', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  if (loading) {
    return (
      <div className="container mt-4">
        <div className="text-center">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Yükleniyor...</span>
          </div>
          <p className="mt-2">Roller yükleniyor...</p>
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
          <h2>Rol Yönetimi</h2>
          <p className="text-muted">Kullanıcı rollerini tanımlayın ve yönetin</p>
        </div>
        <button className="btn btn-primary" onClick={handleAdd}>
          <i className="bi bi-plus-circle me-2"></i>
          Yeni Rol
        </button>
      </div>

      <div className="row">
        {roles.length === 0 ? (
          <div className="col-12">
            <div className="card">
              <div className="card-body text-center py-5">
                <i className="bi bi-shield-check" style={{ fontSize: '3rem', color: '#ccc' }}></i>
                <p className="mt-3 text-muted">Henüz rol bulunmamaktadır.</p>
                <button className="btn btn-primary mt-2" onClick={handleAdd}>
                  İlk Rolü Ekle
                </button>
              </div>
            </div>
          </div>
        ) : (
          roles.map((role) => (
            <div key={role.id} className="col-md-6 col-lg-4 mb-4">
              <div className="card h-100 shadow-sm">
                <div className="card-header d-flex justify-content-between align-items-center">
                  <h5 className="mb-0">
                    <i className="bi bi-shield-fill text-primary me-2"></i>
                    {role.name}
                  </h5>
                  {role.isActive ? (
                    <span className="badge bg-success">Aktif</span>
                  ) : (
                    <span className="badge bg-secondary">Pasif</span>
                  )}
                </div>
                <div className="card-body">
                  <p className="card-text text-muted">
                    {role.description || 'Açıklama bulunmamaktadır.'}
                  </p>
                  <div className="mt-3">
                    <small className="text-muted">
                      <i className="bi bi-calendar me-1"></i>
                      Oluşturulma: {formatDate(role.createdDate)}
                    </small>
                  </div>
                </div>
                <div className="card-footer bg-transparent">
                  <div className="d-flex justify-content-end gap-2">
                    <button
                      className="btn btn-sm btn-outline-primary"
                      onClick={() => handleEdit(role)}
                      title="Düzenle"
                    >
                      <i className="bi bi-pencil me-1"></i>
                      Düzenle
                    </button>
                    <button
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(role.id)}
                      title="Sil"
                    >
                      <i className="bi bi-trash me-1"></i>
                      Sil
                    </button>
                  </div>
                </div>
              </div>
            </div>
          ))
        )}
      </div>

      {/* Sistem Rolleri Bilgilendirmesi */}
      <div className="card mt-4">
        <div className="card-header">
          <h5 className="mb-0">
            <i className="bi bi-info-circle me-2"></i>
            Sistem Rolleri
          </h5>
        </div>
        <div className="card-body">
          <div className="row">
            <div className="col-md-6 mb-3">
              <h6><strong>Admin</strong></h6>
              <p className="text-muted small">Sistem yöneticisi - Tüm yetkilere sahiptir</p>
            </div>
            <div className="col-md-6 mb-3">
              <h6><strong>BranchManager</strong></h6>
              <p className="text-muted small">Şube müdürü - Şube yönetimi ve raporlama</p>
            </div>
            <div className="col-md-6 mb-3">
              <h6><strong>Cashier</strong></h6>
              <p className="text-muted small">Kasiyer - Satış işlemleri</p>
            </div>
            <div className="col-md-6 mb-3">
              <h6><strong>Warehouse</strong></h6>
              <p className="text-muted small">Depo sorumlusu - Stok yönetimi</p>
            </div>
            <div className="col-md-6 mb-3">
              <h6><strong>Accounting</strong></h6>
              <p className="text-muted small">Muhasebe - Mali işlemler ve gider yönetimi</p>
            </div>
            <div className="col-md-6 mb-3">
              <h6><strong>TechnicalService</strong></h6>
              <p className="text-muted small">Teknik servis - Teknik destek ve onarım</p>
            </div>
          </div>
        </div>
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => {
          setIsModalOpen(false);
          setSelectedRole(null);
        }}
        title={selectedRole ? 'Rol Düzenle' : 'Yeni Rol'}
      >
        <RoleForm
          role={selectedRole}
          onSubmit={handleSubmit}
          onCancel={() => {
            setIsModalOpen(false);
            setSelectedRole(null);
          }}
          loading={formLoading}
        />
      </Modal>
    </div>
  );
};

export default RoleList;
