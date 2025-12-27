import { useState, useEffect } from 'react';
import { userAPI } from '../services/api';
import Modal from '../components/Modal';
import UserForm from '../components/UserForm';

const UserList = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedUser, setSelectedUser] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await userAPI.getAll();
      setUsers(data || []);
    } catch (err) {
      setError('Kullanıcılar yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching users:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleAdd = () => {
    setSelectedUser(null);
    setIsModalOpen(true);
  };

  const handleEdit = (user) => {
    setSelectedUser(user);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu kullanıcıyı silmek istediğinizden emin misiniz?')) {
      try {
        await userAPI.delete(id);
        await fetchUsers();
        alert('Kullanıcı başarıyla silindi');
      } catch (err) {
        alert('Kullanıcı silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting user:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedUser) {
        // Update
        await userAPI.update(selectedUser.id, formData);
      } else {
        // Create
        await userAPI.create(formData);
      }

      await fetchUsers();
      setIsModalOpen(false);
      setSelectedUser(null);

      alert(selectedUser ? 'Kullanıcı başarıyla güncellendi' : 'Kullanıcı başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving user:', err);
    } finally {
      setFormLoading(false);
    }
  };

  const formatDate = (dateString) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('tr-TR', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  if (loading) {
    return (
      <div className="container mt-4">
        <div className="text-center">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Yükleniyor...</span>
          </div>
          <p className="mt-2">Kullanıcılar yükleniyor...</p>
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
        <h2>Kullanıcı Yönetimi</h2>
        <button className="btn btn-primary" onClick={handleAdd}>
          <i className="bi bi-plus-circle me-2"></i>
          Yeni Kullanıcı
        </button>
      </div>

      <div className="card">
        <div className="card-header">
          <h5 className="mb-0">Kullanıcı Listesi ({users.length})</h5>
        </div>
        <div className="card-body">
          {users.length === 0 ? (
            <div className="text-center py-5">
              <i className="bi bi-people" style={{ fontSize: '3rem', color: '#ccc' }}></i>
              <p className="mt-3 text-muted">Henüz kullanıcı bulunmamaktadır.</p>
              <button className="btn btn-primary mt-2" onClick={handleAdd}>
                İlk Kullanıcıyı Ekle
              </button>
            </div>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover">
                <thead>
                  <tr>
                    <th>ID</th>
                    <th>Kullanıcı Adı</th>
                    <th>Email</th>
                    <th>Telefon</th>
                    <th>Çalışan</th>
                    <th>Roller</th>
                    <th>Durum</th>
                    <th>Son Giriş</th>
                    <th>Kayıt Tarihi</th>
                    <th>İşlemler</th>
                  </tr>
                </thead>
                <tbody>
                  {users.map((user) => (
                    <tr key={user.id}>
                      <td>{user.id}</td>
                      <td>
                        <strong>{user.username}</strong>
                      </td>
                      <td>{user.email}</td>
                      <td>{user.phoneNumber || '-'}</td>
                      <td>
                        {user.employeeFullName ? (
                          <span className="badge bg-info">
                            {user.employeeFullName}
                          </span>
                        ) : (
                          <span className="text-muted">-</span>
                        )}
                      </td>
                      <td>
                        {user.roles && user.roles.length > 0 ? (
                          <div className="d-flex flex-wrap gap-1">
                            {user.roles.map((role, index) => (
                              <span key={index} className="badge bg-primary">
                                {role}
                              </span>
                            ))}
                          </div>
                        ) : (
                          <span className="text-muted">Rol yok</span>
                        )}
                      </td>
                      <td>
                        {user.isActive ? (
                          <span className="badge bg-success">Aktif</span>
                        ) : (
                          <span className="badge bg-secondary">Pasif</span>
                        )}
                      </td>
                      <td>
                        <small>{formatDate(user.lastLoginDate)}</small>
                      </td>
                      <td>
                        <small>{formatDate(user.createdDate)}</small>
                      </td>
                      <td>
                        <div className="btn-group" role="group">
                          <button
                            className="btn btn-sm btn-outline-primary"
                            onClick={() => handleEdit(user)}
                            title="Düzenle"
                          >
                            <i className="bi bi-pencil"></i>
                          </button>
                          <button
                            className="btn btn-sm btn-outline-danger"
                            onClick={() => handleDelete(user.id)}
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
          setSelectedUser(null);
        }}
        title={selectedUser ? 'Kullanıcı Düzenle' : 'Yeni Kullanıcı'}
      >
        <UserForm
          user={selectedUser}
          onSubmit={handleSubmit}
          onCancel={() => {
            setIsModalOpen(false);
            setSelectedUser(null);
          }}
          loading={formLoading}
        />
      </Modal>
    </div>
  );
};

export default UserList;
