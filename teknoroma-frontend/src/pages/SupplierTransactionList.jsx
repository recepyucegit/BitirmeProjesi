import { useState, useEffect } from 'react';
import { supplierTransactionAPI } from '../services/api';
import { useNavigate } from 'react-router-dom';

export default function SupplierTransactionList() {
  const navigate = useNavigate();
  const [transactions, setTransactions] = useState([]);
  const [filteredTransactions, setFilteredTransactions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const [statusFilter, setStatusFilter] = useState('All');
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    loadTransactions();
  }, []);

  useEffect(() => {
    filterTransactions();
  }, [transactions, statusFilter, searchTerm]);

  const loadTransactions = async () => {
    try {
      setLoading(true);
      const data = await supplierTransactionAPI.getAll();
      setTransactions(data);
    } catch (err) {
      console.error('Veri yükleme hatası:', err);
      setError('Tedarikçi işlemleri yüklenirken bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  const filterTransactions = () => {
    let filtered = [...transactions];

    // Durum filtresi
    if (statusFilter !== 'All') {
      filtered = filtered.filter(t => t.status === statusFilter);
    }

    // Arama filtresi (tedarikçi adı, çalışan adı)
    if (searchTerm) {
      const search = searchTerm.toLowerCase();
      filtered = filtered.filter(
        t =>
          t.supplierName?.toLowerCase().includes(search) ||
          t.employeeName?.toLowerCase().includes(search) ||
          t.id.toString().includes(search)
      );
    }

    setFilteredTransactions(filtered);
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Bu tedarikçi işlemini silmek istediğinize emin misiniz?')) {
      return;
    }

    try {
      await supplierTransactionAPI.delete(id);
      setTransactions(transactions.filter(t => t.id !== id));
    } catch (err) {
      console.error('Silme hatası:', err);
      alert('Tedarikçi işlemi silinirken bir hata oluştu');
    }
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('tr-TR', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    });
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: 'TRY',
    }).format(amount);
  };

  const getStatusBadge = (status) => {
    const statusMap = {
      Ordered: { label: 'Sipariş Verildi', class: 'bg-warning text-dark' },
      Delivered: { label: 'Teslim Edildi', class: 'bg-success' },
      Cancelled: { label: 'İptal Edildi', class: 'bg-danger' },
    };

    const statusInfo = statusMap[status] || { label: status, class: 'bg-secondary' };
    return <span className={`badge ${statusInfo.class}`}>{statusInfo.label}</span>;
  };

  // İstatistikler
  const stats = {
    total: transactions.length,
    ordered: transactions.filter(t => t.status === 'Ordered').length,
    delivered: transactions.filter(t => t.status === 'Delivered').length,
    cancelled: transactions.filter(t => t.status === 'Cancelled').length,
    totalAmount: transactions
      .filter(t => t.status === 'Delivered')
      .reduce((sum, t) => sum + t.totalAmount, 0),
  };

  if (loading) {
    return (
      <div className="container mt-4">
        <div className="text-center">
          <div className="spinner-border" role="status">
            <span className="visually-hidden">Yükleniyor...</span>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="container-fluid mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Tedarikçi Siparişleri</h2>
        <button
          className="btn btn-primary"
          onClick={() => navigate('/supplier-transactions/new')}
        >
          <i className="bi bi-plus-lg me-2"></i>
          Yeni Sipariş
        </button>
      </div>

      {error && <div className="alert alert-danger">{error}</div>}

      {/* İstatistik Kartları */}
      <div className="row mb-4">
        <div className="col-md-3">
          <div className="card text-bg-primary">
            <div className="card-body">
              <h6 className="card-title">Toplam Sipariş</h6>
              <h3 className="mb-0">{stats.total}</h3>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card text-bg-warning">
            <div className="card-body">
              <h6 className="card-title">Sipariş Verildi</h6>
              <h3 className="mb-0">{stats.ordered}</h3>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card text-bg-success">
            <div className="card-body">
              <h6 className="card-title">Teslim Edildi</h6>
              <h3 className="mb-0">{stats.delivered}</h3>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card text-bg-info">
            <div className="card-body">
              <h6 className="card-title">Toplam Tutar (Teslim Edilen)</h6>
              <h3 className="mb-0">{formatCurrency(stats.totalAmount)}</h3>
            </div>
          </div>
        </div>
      </div>

      {/* Filtreler */}
      <div className="card mb-4">
        <div className="card-body">
          <div className="row">
            <div className="col-md-4">
              <label htmlFor="statusFilter" className="form-label">
                Durum Filtresi
              </label>
              <select
                id="statusFilter"
                className="form-select"
                value={statusFilter}
                onChange={(e) => setStatusFilter(e.target.value)}
              >
                <option value="All">Tümü</option>
                <option value="Ordered">Sipariş Verildi</option>
                <option value="Delivered">Teslim Edildi</option>
                <option value="Cancelled">İptal Edildi</option>
              </select>
            </div>
            <div className="col-md-8">
              <label htmlFor="searchTerm" className="form-label">
                Ara
              </label>
              <input
                type="text"
                id="searchTerm"
                className="form-control"
                placeholder="Tedarikçi adı, çalışan adı veya sipariş numarası..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </div>
          </div>
        </div>
      </div>

      {/* Tedarikçi İşlemleri Tablosu */}
      <div className="card">
        <div className="card-body">
          {filteredTransactions.length === 0 ? (
            <p className="text-center text-muted">Tedarikçi işlemi bulunamadı</p>
          ) : (
            <div className="table-responsive">
              <table className="table table-hover">
                <thead>
                  <tr>
                    <th>Sipariş No</th>
                    <th>Tedarikçi</th>
                    <th>Sorumlu Çalışan</th>
                    <th>Sipariş Tarihi</th>
                    <th>Teslim Tarihi</th>
                    <th>Ürün Sayısı</th>
                    <th>Toplam Tutar</th>
                    <th>Durum</th>
                    <th>İşlemler</th>
                  </tr>
                </thead>
                <tbody>
                  {filteredTransactions.map((transaction) => (
                    <tr key={transaction.id}>
                      <td>#{transaction.orderNumber || transaction.id}</td>
                      <td>{transaction.supplierName || 'N/A'}</td>
                      <td>{transaction.employeeName || 'N/A'}</td>
                      <td>{formatDate(transaction.orderDate)}</td>
                      <td>{transaction.deliveryDate ? formatDate(transaction.deliveryDate) : '-'}</td>
                      <td>
                        <span className="badge bg-secondary">
                          {transaction.details?.length || 0} ürün
                        </span>
                      </td>
                      <td className="fw-bold">{formatCurrency(transaction.totalAmount)}</td>
                      <td>{getStatusBadge(transaction.status)}</td>
                      <td>
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => handleDelete(transaction.id)}
                          title="Sil"
                        >
                          🗑️ Sil
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>

      {/* Detay Modal için veya detay sayfası için alan bırakıldı */}
      <style>{`
        .table-responsive {
          max-height: 600px;
          overflow-y: auto;
        }
      `}</style>
    </div>
  );
}
