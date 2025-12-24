import { useState, useEffect } from 'react';
import { supplierAPI } from '../services/api';
import Modal from '../components/Modal';
import SupplierForm from '../components/SupplierForm';

const SupplierList = () => {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedSupplier, setSelectedSupplier] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  // Component yüklendiğinde tedarikçileri API'den çek
  useEffect(() => {
    fetchSuppliers();
  }, []);

  const fetchSuppliers = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await supplierAPI.getAll();
      setSuppliers(data || []);
    } catch (err) {
      setError('Tedarikçiler yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching suppliers:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="loading">Tedarikçiler yükleniyor...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  const handleAdd = () => {
    setSelectedSupplier(null);
    setIsModalOpen(true);
  };

  const handleEdit = (supplier) => {
    setSelectedSupplier(supplier);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu tedarikçiyi silmek istediğinizden emin misiniz?')) {
      try {
        await supplierAPI.delete(id);
        await fetchSuppliers();
        alert('Tedarikçi başarıyla silindi');
      } catch (err) {
        alert('Tedarikçi silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting supplier:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedSupplier) {
        // Update - include id in the request body
        await supplierAPI.update(selectedSupplier.id, {
          ...formData,
          id: selectedSupplier.id
        });
      } else {
        // Create
        await supplierAPI.create(formData);
      }

      // Önce verileri yenile
      await fetchSuppliers();

      // Sonra modal'ı kapat ve mesajı göster
      setIsModalOpen(false);
      setSelectedSupplier(null);

      alert(selectedSupplier ? 'Tedarikçi başarıyla güncellendi' : 'Tedarikçi başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving supplier:', err);
    } finally {
      setFormLoading(false);
    }
  };

  return (
    <>
      <div className="card">
        <div className="page-header">
          <h2>Tedarikçiler ({suppliers.length})</h2>
          <button className="btn btn-success" onClick={handleAdd}>
            + Yeni Tedarikçi
          </button>
        </div>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Firma Adı</th>
              <th>İlgili Kişi</th>
              <th>E-posta</th>
              <th>Telefon</th>
              <th>Şehir</th>
              <th>Ülke</th>
              <th>Durum</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {suppliers.map((supplier) => (
              <tr key={supplier.id}>
                <td>{supplier.id}</td>
                <td><strong>{supplier.companyName}</strong></td>
                <td>
                  {supplier.contactName ? (
                    <div>
                      <div>{supplier.contactName}</div>
                      {supplier.contactTitle && (
                        <small style={{ color: '#666' }}>{supplier.contactTitle}</small>
                      )}
                    </div>
                  ) : '-'}
                </td>
                <td>{supplier.email || '-'}</td>
                <td>{supplier.phone || '-'}</td>
                <td>{supplier.city || '-'}</td>
                <td>{supplier.country || '-'}</td>
                <td>
                  <span className={`badge ${supplier.isActive ? 'badge-success' : 'badge-danger'}`}>
                    {supplier.isActive ? 'Aktif' : 'Pasif'}
                  </span>
                </td>
                <td>
                  <div className="btn-group">
                    <button
                      className="btn btn-sm btn-warning"
                      onClick={() => handleEdit(supplier)}
                    >
                      Düzenle
                    </button>
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() => handleDelete(supplier.id)}
                    >
                      Sil
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={selectedSupplier ? 'Tedarikçi Düzenle' : 'Yeni Tedarikçi'}
      >
        <SupplierForm
          supplier={selectedSupplier}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
          loading={formLoading}
        />
      </Modal>
    </>
  );
};

export default SupplierList;
