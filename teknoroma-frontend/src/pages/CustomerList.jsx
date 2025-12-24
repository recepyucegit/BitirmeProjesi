import { useState, useEffect } from 'react';
import { customerAPI } from '../services/api';
import Modal from '../components/Modal';
import CustomerForm from '../components/CustomerForm';

const CustomerList = () => {
  const [customers, setCustomers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCustomer, setSelectedCustomer] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  // Component yüklendiğinde müşterileri API'den çek
  useEffect(() => {
    fetchCustomers();
  }, []);

  const fetchCustomers = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await customerAPI.getAll();
      setCustomers(data || []);
    } catch (err) {
      setError('Müşteriler yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching customers:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="loading">Müşteriler yükleniyor...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  const handleAdd = () => {
    setSelectedCustomer(null);
    setIsModalOpen(true);
  };

  const handleEdit = (customer) => {
    setSelectedCustomer(customer);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu müşteriyi silmek istediğinizden emin misiniz?')) {
      try {
        await customerAPI.delete(id);
        await fetchCustomers();
        alert('Müşteri başarıyla silindi');
      } catch (err) {
        alert('Müşteri silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting customer:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedCustomer) {
        // Update - include id in the request body
        await customerAPI.update(selectedCustomer.id, {
          ...formData,
          id: selectedCustomer.id
        });
      } else {
        // Create
        await customerAPI.create(formData);
      }

      // Önce verileri yenile
      await fetchCustomers();

      // Sonra modal'ı kapat ve mesajı göster
      setIsModalOpen(false);
      setSelectedCustomer(null);

      alert(selectedCustomer ? 'Müşteri başarıyla güncellendi' : 'Müşteri başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving customer:', err);
    } finally {
      setFormLoading(false);
    }
  };

  return (
    <>
      <div className="card">
        <div className="page-header">
          <h2>Müşteriler ({customers.length})</h2>
          <button className="btn btn-success" onClick={handleAdd}>
            + Yeni Müşteri
          </button>
        </div>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Ad Soyad</th>
              <th>TC Kimlik No</th>
              <th>Müşteri Tipi</th>
              <th>E-posta</th>
              <th>Telefon</th>
              <th>Şehir</th>
              <th>Durum</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {customers.map((customer) => (
              <tr key={customer.id}>
                <td>{customer.id}</td>
                <td><strong>{customer.fullName}</strong></td>
                <td>{customer.identityNumber}</td>
                <td>
                  <span className={`badge ${customer.customerType === 'Corporate' ? 'badge-info' : 'badge-secondary'}`}>
                    {customer.customerType === 'Individual' ? 'Bireysel' : 'Kurumsal'}
                  </span>
                </td>
                <td>{customer.email || '-'}</td>
                <td>{customer.phone || '-'}</td>
                <td>{customer.city || '-'}</td>
                <td>
                  <span className={`badge ${customer.isActive ? 'badge-success' : 'badge-danger'}`}>
                    {customer.isActive ? 'Aktif' : 'Pasif'}
                  </span>
                </td>
                <td>
                  <div className="btn-group">
                    <button
                      className="btn btn-sm btn-warning"
                      onClick={() => handleEdit(customer)}
                    >
                      Düzenle
                    </button>
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() => handleDelete(customer.id)}
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
        title={selectedCustomer ? 'Müşteri Düzenle' : 'Yeni Müşteri'}
      >
        <CustomerForm
          customer={selectedCustomer}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
          loading={formLoading}
        />
      </Modal>
    </>
  );
};

export default CustomerList;
