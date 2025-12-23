import React, { useEffect, useState } from 'react';
import { supplierAPI } from '../services/api';

function SupplierList() {
  const [suppliers, setSuppliers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchSuppliers();
  }, []);

  const fetchSuppliers = async () => {
    try {
      setLoading(true);
      const response = await supplierAPI.getAll();
      setSuppliers(response.data);
      setError(null);
    } catch (err) {
      setError('Tedarikçiler yüklenirken bir hata oluştu: ' + err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id, companyName) => {
    if (!window.confirm(`${companyName} tedarikçisini silmek istediğinize emin misiniz?`)) {
      return;
    }

    try {
      await supplierAPI.delete(id);
      setSuppliers(suppliers.filter(s => s.id !== id));
    } catch (err) {
      alert('Tedarikçi silinirken bir hata oluştu: ' + err.message);
    }
  };

  if (loading) return <div className="loading">Yükleniyor...</div>;
  if (error) return <div className="error">{error}</div>;

  return (
    <div className="card">
      <h2>Tedarikçi Listesi</h2>

      <div style={{ marginBottom: '20px' }}>
        <button className="btn btn-primary">Yeni Tedarikçi Ekle</button>
      </div>

      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Şirket Adı</th>
            <th>İletişim Kişisi</th>
            <th>Ünvan</th>
            <th>Email</th>
            <th>Telefon</th>
            <th>Şehir</th>
            <th>Vergi No</th>
            <th>Durum</th>
            <th>İşlemler</th>
          </tr>
        </thead>
        <tbody>
          {suppliers.map((supplier) => (
            <tr key={supplier.id}>
              <td>{supplier.id}</td>
              <td>{supplier.companyName}</td>
              <td>{supplier.contactName}</td>
              <td>{supplier.contactTitle}</td>
              <td>{supplier.email}</td>
              <td>{supplier.phone}</td>
              <td>{supplier.city}</td>
              <td>{supplier.taxNumber}</td>
              <td>
                <span className={`badge ${supplier.isActive ? 'badge-success' : 'badge-danger'}`}>
                  {supplier.isActive ? 'Aktif' : 'Pasif'}
                </span>
              </td>
              <td>
                <button className="btn btn-sm btn-info" style={{ marginRight: '5px' }}>
                  Düzenle
                </button>
                <button
                  className="btn btn-sm btn-danger"
                  onClick={() => handleDelete(supplier.id, supplier.companyName)}
                >
                  Sil
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>

      {suppliers.length === 0 && (
        <p style={{ textAlign: 'center', padding: '20px', color: '#666' }}>
          Henüz tedarikçi bulunmamaktadır.
        </p>
      )}
    </div>
  );
}

export default SupplierList;
