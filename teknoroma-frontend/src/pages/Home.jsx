import { useState, useEffect } from 'react';
import { reportAPI } from '../services/api';

export default function Home() {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    loadDashboardStats();
  }, []);

  const loadDashboardStats = async () => {
    try {
      setLoading(true);
      const data = await reportAPI.getDashboardStats();
      setStats(data);
    } catch (err) {
      console.error('Dashboard yükleme hatası:', err);
      setError('Dashboard verileri yüklenirken bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: 'TRY',
    }).format(amount || 0);
  };

  const formatDate = (dateString) => {
    if (!dateString) return '-';
    return new Date(dateString).toLocaleDateString('tr-TR', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
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

  if (error) {
    return (
      <div className="container mt-4">
        <div className="alert alert-danger">{error}</div>
      </div>
    );
  }

  return (
    <div className="container-fluid mt-4">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h2>Dashboard</h2>
        <button className="btn btn-outline-primary btn-sm" onClick={loadDashboardStats}>
          <i className="bi bi-arrow-clockwise me-2"></i>
          Yenile
        </button>
      </div>

      {/* Satış İstatistikleri */}
      <div className="row mb-4">
        <div className="col-md-3">
          <div className="card text-bg-primary">
            <div className="card-body">
              <h6 className="card-title">Bugünkü Satışlar</h6>
              <h3 className="mb-0">{formatCurrency(stats?.todaySales)}</h3>
              <small>Günlük toplam</small>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card text-bg-success">
            <div className="card-body">
              <h6 className="card-title">Haftalık Satışlar</h6>
              <h3 className="mb-0">{formatCurrency(stats?.weeklySales)}</h3>
              <small>Son 7 gün</small>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card text-bg-info">
            <div className="card-body">
              <h6 className="card-title">Aylık Satışlar</h6>
              <h3 className="mb-0">{formatCurrency(stats?.monthlySales)}</h3>
              <small>Son 30 gün</small>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card text-bg-warning text-dark">
            <div className="card-body">
              <h6 className="card-title">Yıllık Satışlar</h6>
              <h3 className="mb-0">{formatCurrency(stats?.yearlySales)}</h3>
              <small>Son 12 ay</small>
            </div>
          </div>
        </div>
      </div>

      {/* Genel İstatistikler */}
      <div className="row mb-4">
        <div className="col-md-3">
          <div className="card border-primary">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6 className="text-muted mb-1">Toplam Ürün</h6>
                  <h4 className="mb-0">{stats?.totalProducts || 0}</h4>
                </div>
                <div className="text-primary" style={{ fontSize: '2rem' }}>
                  <i className="bi bi-box-seam"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card border-danger">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6 className="text-muted mb-1">Düşük Stok</h6>
                  <h4 className="mb-0 text-danger">{stats?.lowStockProducts || 0}</h4>
                </div>
                <div className="text-danger" style={{ fontSize: '2rem' }}>
                  <i className="bi bi-exclamation-triangle"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card border-success">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6 className="text-muted mb-1">Toplam Müşteri</h6>
                  <h4 className="mb-0">{stats?.totalCustomers || 0}</h4>
                </div>
                <div className="text-success" style={{ fontSize: '2rem' }}>
                  <i className="bi bi-people"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3">
          <div className="card border-info">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6 className="text-muted mb-1">Stok Değeri</h6>
                  <h4 className="mb-0">{formatCurrency(stats?.totalStockValue)}</h4>
                </div>
                <div className="text-info" style={{ fontSize: '2rem' }}>
                  <i className="bi bi-currency-dollar"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="row">
        {/* En Çok Satan Ürünler */}
        <div className="col-md-6 mb-4">
          <div className="card">
            <div className="card-header">
              <h5 className="mb-0">En Çok Satan Ürünler</h5>
            </div>
            <div className="card-body">
              {stats?.topSellingProducts?.length === 0 ? (
                <p className="text-muted text-center">Henüz satış yok</p>
              ) : (
                <div className="table-responsive">
                  <table className="table table-sm table-hover">
                    <thead>
                      <tr>
                        <th>Ürün</th>
                        <th className="text-end">Satış Adedi</th>
                        <th className="text-end">Toplam</th>
                      </tr>
                    </thead>
                    <tbody>
                      {stats?.topSellingProducts?.slice(0, 5).map((product, index) => (
                        <tr key={index}>
                          <td>
                            <strong>{product.productName}</strong>
                            <br />
                            <small className="text-muted">{product.categoryName}</small>
                          </td>
                          <td className="text-end">
                            <span className="badge bg-primary">{product.totalQuantity}</span>
                          </td>
                          <td className="text-end fw-bold">{formatCurrency(product.totalRevenue)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          </div>
        </div>

        {/* En İyi Müşteriler */}
        <div className="col-md-6 mb-4">
          <div className="card">
            <div className="card-header">
              <h5 className="mb-0">En İyi Müşteriler</h5>
            </div>
            <div className="card-body">
              {stats?.topCustomers?.length === 0 ? (
                <p className="text-muted text-center">Henüz müşteri yok</p>
              ) : (
                <div className="table-responsive">
                  <table className="table table-sm table-hover">
                    <thead>
                      <tr>
                        <th>Müşteri</th>
                        <th className="text-end">Alışveriş</th>
                        <th className="text-end">Toplam</th>
                      </tr>
                    </thead>
                    <tbody>
                      {stats?.topCustomers?.slice(0, 5).map((customer, index) => (
                        <tr key={index}>
                          <td>
                            <strong>{customer.customerName}</strong>
                            <br />
                            <small className="text-muted">{customer.email}</small>
                          </td>
                          <td className="text-end">
                            <span className="badge bg-info">{customer.totalPurchases}</span>
                          </td>
                          <td className="text-end fw-bold">{formatCurrency(customer.totalSpent)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>

      <div className="row">
        {/* En Başarılı Çalışanlar */}
        <div className="col-md-6 mb-4">
          <div className="card">
            <div className="card-header">
              <h5 className="mb-0">En Başarılı Çalışanlar</h5>
            </div>
            <div className="card-body">
              {stats?.topEmployees?.length === 0 ? (
                <p className="text-muted text-center">Henüz satış yok</p>
              ) : (
                <div className="table-responsive">
                  <table className="table table-sm table-hover">
                    <thead>
                      <tr>
                        <th>Çalışan</th>
                        <th className="text-end">Satış</th>
                        <th className="text-end">Toplam</th>
                      </tr>
                    </thead>
                    <tbody>
                      {stats?.topEmployees?.slice(0, 5).map((employee, index) => (
                        <tr key={index}>
                          <td>
                            <strong>{employee.employeeName}</strong>
                            <br />
                            <small className="text-muted">{employee.position}</small>
                          </td>
                          <td className="text-end">
                            <span className="badge bg-success">{employee.totalSales}</span>
                          </td>
                          <td className="text-end fw-bold">{formatCurrency(employee.totalRevenue)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          </div>
        </div>

        {/* Düşük Stok Uyarıları */}
        <div className="col-md-6 mb-4">
          <div className="card border-danger">
            <div className="card-header bg-danger text-white">
              <h5 className="mb-0">
                <i className="bi bi-exclamation-triangle me-2"></i>
                Düşük Stok Uyarıları
              </h5>
            </div>
            <div className="card-body">
              {stats?.lowStockAlerts?.length === 0 ? (
                <p className="text-muted text-center">Stok uyarısı yok</p>
              ) : (
                <div className="table-responsive">
                  <table className="table table-sm table-hover">
                    <thead>
                      <tr>
                        <th>Ürün</th>
                        <th className="text-end">Stok</th>
                        <th className="text-end">Min. Stok</th>
                      </tr>
                    </thead>
                    <tbody>
                      {stats?.lowStockAlerts?.slice(0, 5).map((alert, index) => (
                        <tr key={index}>
                          <td>
                            <strong>{alert.productName}</strong>
                            <br />
                            <small className="text-muted">{alert.categoryName}</small>
                          </td>
                          <td className="text-end">
                            <span className="badge bg-danger">{alert.currentStock}</span>
                          </td>
                          <td className="text-end">
                            <span className="badge bg-warning text-dark">{alert.minStockLevel}</span>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
              {stats?.lowStockAlerts?.length > 5 && (
                <div className="text-center mt-2">
                  <small className="text-muted">
                    +{stats.lowStockAlerts.length - 5} ürün daha düşük stokta
                  </small>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>

      {/* Son Satışlar */}
      <div className="row">
        <div className="col-12 mb-4">
          <div className="card">
            <div className="card-header">
              <h5 className="mb-0">Son Satışlar</h5>
            </div>
            <div className="card-body">
              {stats?.recentSales?.length === 0 ? (
                <p className="text-muted text-center">Henüz satış yok</p>
              ) : (
                <div className="table-responsive">
                  <table className="table table-sm table-hover">
                    <thead>
                      <tr>
                        <th>Tarih</th>
                        <th>Müşteri</th>
                        <th>Çalışan</th>
                        <th>Mağaza</th>
                        <th className="text-end">Ürün Sayısı</th>
                        <th className="text-end">Toplam</th>
                        <th className="text-end">Ödeme</th>
                      </tr>
                    </thead>
                    <tbody>
                      {stats?.recentSales?.slice(0, 10).map((sale, index) => (
                        <tr key={index}>
                          <td>
                            <small>{formatDate(sale.saleDate)}</small>
                          </td>
                          <td>{sale.customerName}</td>
                          <td>{sale.employeeName}</td>
                          <td>{sale.storeName}</td>
                          <td className="text-end">
                            <span className="badge bg-secondary">{sale.totalItems}</span>
                          </td>
                          <td className="text-end fw-bold">{formatCurrency(sale.totalAmount)}</td>
                          <td className="text-end">
                            <span className={`badge ${
                              sale.paymentMethod === 'Cash' ? 'bg-success' :
                              sale.paymentMethod === 'CreditCard' ? 'bg-primary' :
                              sale.paymentMethod === 'BankTransfer' ? 'bg-info' : 'bg-secondary'
                            }`}>
                              {sale.paymentMethod === 'Cash' ? 'Nakit' :
                               sale.paymentMethod === 'CreditCard' ? 'Kredi Kartı' :
                               sale.paymentMethod === 'BankTransfer' ? 'Havale' : sale.paymentMethod}
                            </span>
                          </td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
