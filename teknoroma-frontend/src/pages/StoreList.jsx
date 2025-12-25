import { useState, useEffect } from 'react';
import { storeAPI } from '../services/api';
import Modal from '../components/Modal';
import StoreForm from '../components/StoreForm';

const StoreList = () => {
  const [stores, setStores] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedStore, setSelectedStore] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  // Mağazaları yükle
  const fetchStores = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await storeAPI.getAll();
      setStores(data || []);
    } catch (err) {
      console.error('Error fetching stores:', err);
      setError('Mağazalar yüklenirken bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchStores();
  }, []);

  // Yeni mağaza ekle
  const handleAdd = () => {
    setSelectedStore(null);
    setIsModalOpen(true);
  };

  // Mağaza düzenle
  const handleEdit = (store) => {
    setSelectedStore(store);
    setIsModalOpen(true);
  };

  // Mağaza sil
  const handleDelete = async (id) => {
    if (!window.confirm('Bu mağazayı silmek istediğinizden emin misiniz?')) {
      return;
    }

    try {
      await storeAPI.delete(id);
      await fetchStores();
      alert('Mağaza başarıyla silindi!');
    } catch (err) {
      console.error('Error deleting store:', err);
      alert('Mağaza silinirken bir hata oluştu: ' + (err.response?.data?.message || err.message));
    }
  };

  // Form submit
  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);

      if (selectedStore) {
        // Güncelleme
        await storeAPI.update(selectedStore.id, formData);
        alert('Mağaza başarıyla güncellendi!');
      } else {
        // Yeni ekleme
        await storeAPI.create(formData);
        alert('Mağaza başarıyla eklendi!');
      }

      setIsModalOpen(false);
      await fetchStores();
    } catch (err) {
      console.error('Error saving store:', err);
      alert('Mağaza kaydedilirken bir hata oluştu: ' + (err.response?.data?.message || err.message));
    } finally {
      setFormLoading(false);
    }
  };

  // Tarih formatlama
  const formatDate = (dateString) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR');
  };

  // Para formatlama
  const formatCurrency = (amount) => {
    if (amount === null || amount === undefined) return '-';
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: 'TRY'
    }).format(amount);
  };

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: '3rem' }}>
        <div className="spinner"></div>
        <p>Mağazalar yükleniyor...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div style={{ textAlign: 'center', padding: '3rem' }}>
        <p style={{ color: '#dc3545' }}>{error}</p>
        <button className="btn btn-primary" onClick={fetchStores}>
          Tekrar Dene
        </button>
      </div>
    );
  }

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
        <h1>Mağazalar</h1>
        <button className="btn btn-primary" onClick={handleAdd}>
          + Yeni Mağaza
        </button>
      </div>

      {stores.length === 0 ? (
        <div style={{ textAlign: 'center', padding: '3rem', backgroundColor: '#f8f9fa', borderRadius: '8px' }}>
          <p style={{ color: '#666', marginBottom: '1rem' }}>Henüz mağaza bulunmamaktadır.</p>
          <button className="btn btn-primary" onClick={handleAdd}>
            İlk Mağazayı Ekle
          </button>
        </div>
      ) : (
        <div style={{ overflowX: 'auto' }}>
          <table className="table">
            <thead>
              <tr>
                <th>Mağaza Kodu</th>
                <th>Mağaza Adı</th>
                <th>Şehir</th>
                <th>İlçe</th>
                <th>Müdür</th>
                <th>Telefon</th>
                <th>Açılış Tarihi</th>
                <th>Aylık Hedef</th>
                <th>Çalışan Sayısı</th>
                <th>Durum</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              {stores.map((store) => (
                <tr key={store.id}>
                  <td>
                    <strong>{store.storeCode}</strong>
                  </td>
                  <td>{store.storeName}</td>
                  <td>{store.city || '-'}</td>
                  <td>{store.district || '-'}</td>
                  <td>{store.managerName || '-'}</td>
                  <td>{store.phone || '-'}</td>
                  <td>{formatDate(store.openingDate)}</td>
                  <td>{formatCurrency(store.monthlyTarget)}</td>
                  <td>
                    <span className="badge badge-info">
                      {store.employeeCount} kişi
                    </span>
                  </td>
                  <td>
                    <span className={`badge ${store.isActive ? 'badge-success' : 'badge-secondary'}`}>
                      {store.isActive ? 'Aktif' : 'Pasif'}
                    </span>
                  </td>
                  <td>
                    <div style={{ display: 'flex', gap: '0.5rem' }}>
                      <button
                        className="btn btn-sm btn-warning"
                        onClick={() => handleEdit(store)}
                        title="Düzenle"
                      >
                        ✏️
                      </button>
                      <button
                        className="btn btn-sm btn-danger"
                        onClick={() => handleDelete(store.id)}
                        title="Sil"
                      >
                        🗑️
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Mağaza Ekleme/Düzenleme Modal */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={selectedStore ? 'Mağaza Düzenle' : 'Yeni Mağaza Ekle'}
      >
        <StoreForm
          store={selectedStore}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
          loading={formLoading}
        />
      </Modal>

      {/* Mağaza Özeti */}
      <div style={{ marginTop: '2rem', padding: '1rem', backgroundColor: '#f8f9fa', borderRadius: '8px' }}>
        <h3 style={{ marginBottom: '1rem' }}>Özet</h3>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '1rem' }}>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Toplam Mağaza</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#007bff' }}>
              {stores.length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Aktif Mağaza</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#28a745' }}>
              {stores.filter(s => s.isActive).length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Pasif Mağaza</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#6c757d' }}>
              {stores.filter(s => !s.isActive).length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Toplam Çalışan</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#17a2b8' }}>
              {stores.reduce((sum, s) => sum + s.employeeCount, 0)}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default StoreList;
