import { useState, useEffect } from 'react';
import { saleAPI } from '../services/api';
import Modal from '../components/Modal';
import SaleForm from '../components/SaleForm';

const SaleList = () => {
  const [sales, setSales] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedSale, setSelectedSale] = useState(null);
  const [formLoading, setFormLoading] = useState(false);
  const [viewDetailsModal, setViewDetailsModal] = useState(false);

  // Component yüklendiğinde satışları API'den çek
  useEffect(() => {
    fetchSales();
  }, []);

  const fetchSales = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await saleAPI.getAll();
      setSales(data || []);
    } catch (err) {
      setError('Satışlar yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching sales:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="loading">Satışlar yükleniyor...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  const handleAdd = () => {
    setSelectedSale(null);
    setIsModalOpen(true);
  };

  const handleViewDetails = (sale) => {
    setSelectedSale(sale);
    setViewDetailsModal(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu satışı silmek istediğinizden emin misiniz?')) {
      try {
        await saleAPI.delete(id);
        await fetchSales();
        alert('Satış başarıyla silindi');
      } catch (err) {
        alert('Satış silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting sale:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedSale) {
        // Update
        await saleAPI.update(selectedSale.id, {
          ...formData,
          id: selectedSale.id
        });
      } else {
        // Create
        await saleAPI.create(formData);
      }

      // Önce verileri yenile
      await fetchSales();

      // Sonra modal'ı kapat ve mesajı göster
      setIsModalOpen(false);
      setSelectedSale(null);

      alert(selectedSale ? 'Satış başarıyla güncellendi' : 'Satış başarıyla oluşturuldu');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving sale:', err);
    } finally {
      setFormLoading(false);
    }
  };

  // Tarihi formatla
  const formatDate = (dateString) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('tr-TR', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  // Para birimi formatla
  const formatCurrency = (amount) => {
    return `₺${amount.toLocaleString('tr-TR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
  };

  // Ödeme yöntemi Türkçeleştir
  const getPaymentMethodText = (method) => {
    const methods = {
      'Cash': 'Nakit',
      'CreditCard': 'Kredi Kartı',
      'BankTransfer': 'Banka Havalesi'
    };
    return methods[method] || method;
  };

  // Durum Türkçeleştir
  const getStatusText = (status) => {
    const statuses = {
      'Completed': 'Tamamlandı',
      'Cancelled': 'İptal Edildi',
      'Refunded': 'İade Edildi'
    };
    return statuses[status] || status;
  };

  // Durum badge rengi
  const getStatusBadgeClass = (status) => {
    const classes = {
      'Completed': 'badge-success',
      'Cancelled': 'badge-danger',
      'Refunded': 'badge-warning'
    };
    return classes[status] || 'badge-secondary';
  };

  return (
    <>
      <div className="card">
        <div className="page-header">
          <h2>Satışlar ({sales.length})</h2>
          <button className="btn btn-success" onClick={handleAdd}>
            + Yeni Satış
          </button>
        </div>

        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Fatura No</th>
              <th>Tarih</th>
              <th>Müşteri</th>
              <th>Çalışan</th>
              <th>Toplam Tutar</th>
              <th>İndirim</th>
              <th>Net Tutar</th>
              <th>Komisyon</th>
              <th>Ödeme</th>
              <th>Durum</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {sales.map((sale) => (
              <tr key={sale.id}>
                <td>{sale.id}</td>
                <td><strong>{sale.invoiceNumber || '-'}</strong></td>
                <td>{formatDate(sale.saleDate)}</td>
                <td>{sale.customerName || '-'}</td>
                <td>{sale.employeeName || '-'}</td>
                <td>{formatCurrency(sale.totalAmount)}</td>
                <td>{sale.discountAmount > 0 ? formatCurrency(sale.discountAmount) : '-'}</td>
                <td><strong>{formatCurrency(sale.netAmount)}</strong></td>
                <td>{formatCurrency(sale.commissionAmount)}</td>
                <td>
                  <span className="badge badge-info">
                    {getPaymentMethodText(sale.paymentMethod)}
                  </span>
                </td>
                <td>
                  <span className={`badge ${getStatusBadgeClass(sale.status)}`}>
                    {getStatusText(sale.status)}
                  </span>
                </td>
                <td>
                  <div className="btn-group">
                    <button
                      className="btn btn-sm btn-info"
                      onClick={() => handleViewDetails(sale)}
                    >
                      Detaylar
                    </button>
                    <button
                      className="btn btn-sm btn-danger"
                      onClick={() => handleDelete(sale.id)}
                      disabled={sale.status !== 'Completed'}
                    >
                      Sil
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {sales.length === 0 && (
          <div style={{ textAlign: 'center', padding: '2rem', color: '#666' }}>
            Henüz satış bulunmamaktadır.
          </div>
        )}
      </div>

      {/* Yeni Satış Modal */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={selectedSale ? 'Satış Düzenle' : 'Yeni Satış'}
      >
        <SaleForm
          sale={selectedSale}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
          loading={formLoading}
        />
      </Modal>

      {/* Satış Detayları Modal */}
      <Modal
        isOpen={viewDetailsModal}
        onClose={() => setViewDetailsModal(false)}
        title="Satış Detayları"
      >
        {selectedSale && (
          <div style={{ padding: '1rem' }}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem', marginBottom: '1.5rem' }}>
              <div>
                <strong>Fatura No:</strong> {selectedSale.invoiceNumber}
              </div>
              <div>
                <strong>Tarih:</strong> {formatDate(selectedSale.saleDate)}
              </div>
              <div>
                <strong>Müşteri:</strong> {selectedSale.customerName}
              </div>
              <div>
                <strong>Çalışan:</strong> {selectedSale.employeeName}
              </div>
              <div>
                <strong>Ödeme Yöntemi:</strong> {getPaymentMethodText(selectedSale.paymentMethod)}
              </div>
              <div>
                <strong>Durum:</strong>{' '}
                <span className={`badge ${getStatusBadgeClass(selectedSale.status)}`}>
                  {getStatusText(selectedSale.status)}
                </span>
              </div>
            </div>

            {selectedSale.notes && (
              <div style={{ marginBottom: '1.5rem', padding: '0.75rem', backgroundColor: '#f8f9fa', borderRadius: '4px' }}>
                <strong>Notlar:</strong>
                <p style={{ margin: '0.5rem 0 0 0' }}>{selectedSale.notes}</p>
              </div>
            )}

            <h3 style={{ marginBottom: '1rem', fontSize: '1.1rem' }}>Ürün Detayları</h3>
            <table style={{ width: '100%', marginBottom: '1.5rem' }}>
              <thead>
                <tr>
                  <th>Ürün</th>
                  <th>Miktar</th>
                  <th>Birim Fiyat</th>
                  <th>İndirim</th>
                  <th>Toplam</th>
                </tr>
              </thead>
              <tbody>
                {selectedSale.saleDetails && selectedSale.saleDetails.map((detail, index) => (
                  <tr key={index}>
                    <td>{detail.productName || `Ürün #${detail.productId}`}</td>
                    <td>{detail.quantity}</td>
                    <td>{formatCurrency(detail.unitPrice)}</td>
                    <td>
                      {detail.discountRate > 0 ? `%${detail.discountRate}` : '-'}
                      {detail.discountAmount > 0 && <div>{formatCurrency(detail.discountAmount)}</div>}
                    </td>
                    <td><strong>{formatCurrency(detail.netPrice)}</strong></td>
                  </tr>
                ))}
              </tbody>
            </table>

            <div style={{ borderTop: '2px solid #dee2e6', paddingTop: '1rem' }}>
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem' }}>
                <strong>Toplam Tutar:</strong>
                <span>{formatCurrency(selectedSale.totalAmount)}</span>
              </div>
              {selectedSale.discountAmount > 0 && (
                <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem', color: '#dc3545' }}>
                  <strong>İndirim:</strong>
                  <span>-{formatCurrency(selectedSale.discountAmount)}</span>
                </div>
              )}
              <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem', fontSize: '1.2rem' }}>
                <strong>Net Tutar:</strong>
                <strong style={{ color: '#28a745' }}>{formatCurrency(selectedSale.netAmount)}</strong>
              </div>
              <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem', color: '#666' }}>
                <span>Komisyon:</span>
                <span>{formatCurrency(selectedSale.commissionAmount)}</span>
              </div>
            </div>

            <div style={{ marginTop: '1.5rem', textAlign: 'right' }}>
              <button
                className="btn btn-secondary"
                onClick={() => setViewDetailsModal(false)}
              >
                Kapat
              </button>
            </div>
          </div>
        )}
      </Modal>
    </>
  );
};

export default SaleList;
