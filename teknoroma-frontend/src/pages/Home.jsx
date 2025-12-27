import { useState, useEffect } from 'react';
import { reportAPI } from '../services/api';
import './Home.css';

export default function Home() {
  const [stats, setStats] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [autoRefresh, setAutoRefresh] = useState(false);
  const [refreshInterval, setRefreshInterval] = useState(30); // seconds
  const [lastRefreshTime, setLastRefreshTime] = useState(null);

  useEffect(() => {
    loadDashboardStats();
  }, []);

  // Auto-refresh effect
  useEffect(() => {
    if (!autoRefresh) return;

    const interval = setInterval(() => {
      loadDashboardStats();
    }, refreshInterval * 1000);

    return () => clearInterval(interval);
  }, [autoRefresh, refreshInterval]);

  const loadDashboardStats = async () => {
    try {
      setLoading(true);
      const data = await reportAPI.getDashboardStats();
      setStats(data);
      setLastRefreshTime(new Date());
      setError('');
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
    <div className="container-fluid mt-4 dashboard-container">
      <div className="dashboard-header d-flex justify-content-between align-items-center">
        <div>
          <h2>
            <i className="bi bi-speedometer2 me-3"></i>
            Dashboard
          </h2>
          <p className="mb-0 mt-2" style={{ opacity: 0.9 }}>
            Hoş geldiniz! İşte işletmenizin anlık görünümü
          </p>
          {lastRefreshTime && (
            <small className="text-muted">
              <i className="bi bi-clock me-1"></i>
              Son güncelleme: {formatDate(lastRefreshTime)}
            </small>
          )}
        </div>
        <div className="d-flex gap-2 align-items-center">
          {/* Auto-refresh toggle */}
          <div className="form-check form-switch">
            <input
              className="form-check-input"
              type="checkbox"
              id="autoRefreshToggle"
              checked={autoRefresh}
              onChange={(e) => setAutoRefresh(e.target.checked)}
            />
            <label className="form-check-label" htmlFor="autoRefreshToggle">
              Otomatik Yenileme
            </label>
          </div>

          {/* Refresh interval selector */}
          {autoRefresh && (
            <select
              className="form-select form-select-sm"
              value={refreshInterval}
              onChange={(e) => setRefreshInterval(Number(e.target.value))}
              style={{ width: 'auto' }}
            >
              <option value="10">10 saniye</option>
              <option value="30">30 saniye</option>
              <option value="60">1 dakika</option>
              <option value="120">2 dakika</option>
              <option value="300">5 dakika</option>
            </select>
          )}

          {/* Manual refresh button */}
          <button
            className="btn btn-primary"
            onClick={loadDashboardStats}
            disabled={loading}
          >
            <i className={`bi bi-arrow-clockwise me-2 ${loading ? 'spin' : ''}`}></i>
            Yenile
          </button>
        </div>
      </div>

      {/* Satış İstatistikleri */}
      <div className="row mb-4">
        <div className="col-md-3 mb-3">
          <div className="stat-card stat-card-primary">
            <i className="bi bi-calendar-day stat-icon"></i>
            <h6>Bugünkü Satışlar</h6>
            <h3>{formatCurrency(stats?.todaySales)}</h3>
            <small>Günlük toplam</small>
            <div className="progress-thin mt-3">
              <div className="progress-bar" style={{ width: '75%' }}></div>
            </div>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="stat-card stat-card-success">
            <i className="bi bi-calendar-week stat-icon"></i>
            <h6>Haftalık Satışlar</h6>
            <h3>{formatCurrency(stats?.weeklySales)}</h3>
            <small>Son 7 gün</small>
            <div className="progress-thin mt-3">
              <div className="progress-bar" style={{ width: '85%' }}></div>
            </div>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="stat-card stat-card-info">
            <i className="bi bi-calendar-month stat-icon"></i>
            <h6>Aylık Satışlar</h6>
            <h3>{formatCurrency(stats?.monthlySales)}</h3>
            <small>Son 30 gün</small>
            <div className="progress-thin mt-3">
              <div className="progress-bar" style={{ width: '92%' }}></div>
            </div>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="stat-card stat-card-warning">
            <i className="bi bi-calendar-range stat-icon"></i>
            <h6>Yıllık Satışlar</h6>
            <h3>{formatCurrency(stats?.yearlySales)}</h3>
            <small>Son 12 ay</small>
            <div className="progress-thin mt-3">
              <div className="progress-bar" style={{ width: '100%' }}></div>
            </div>
          </div>
        </div>
      </div>

      {/* Genel İstatistikler */}
      <div className="row mb-4">
        <div className="col-md-3 mb-3">
          <div className="info-card">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6>Toplam Ürün</h6>
                  <h4>{stats?.totalProducts || 0}</h4>
                </div>
                <div className="icon-wrapper icon-wrapper-primary">
                  <i className="bi bi-box-seam"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="info-card">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6>Düşük Stok</h6>
                  <h4 className="text-danger">
                    {stats?.lowStockProducts || 0}
                    {stats?.lowStockProducts > 0 && (
                      <span className="pulse-animation ms-2">
                        <i className="bi bi-exclamation-circle"></i>
                      </span>
                    )}
                  </h4>
                </div>
                <div className="icon-wrapper icon-wrapper-danger">
                  <i className="bi bi-exclamation-triangle"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="info-card">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6>Toplam Müşteri</h6>
                  <h4>{stats?.totalCustomers || 0}</h4>
                </div>
                <div className="icon-wrapper icon-wrapper-success">
                  <i className="bi bi-people"></i>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="col-md-3 mb-3">
          <div className="info-card">
            <div className="card-body">
              <div className="d-flex justify-content-between align-items-center">
                <div>
                  <h6>Stok Değeri</h6>
                  <h4>{formatCurrency(stats?.totalStockValue)}</h4>
                </div>
                <div className="icon-wrapper icon-wrapper-info">
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
          <div className="table-card">
            <div className="card-header">
              <h5>
                <i className="bi bi-trophy me-2"></i>
                En Çok Satan Ürünler
              </h5>
            </div>
            <div className="card-body">
              {stats?.topSellingProducts?.length === 0 ? (
                <div className="empty-state">
                  <i className="bi bi-cart-x"></i>
                  <p className="mb-0">Henüz satış yok</p>
                </div>
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
          <div className="table-card">
            <div className="card-header">
              <h5>
                <i className="bi bi-star me-2"></i>
                En İyi Müşteriler
              </h5>
            </div>
            <div className="card-body">
              {stats?.topCustomers?.length === 0 ? (
                <div className="empty-state">
                  <i className="bi bi-people"></i>
                  <p className="mb-0">Henüz müşteri yok</p>
                </div>
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
          <div className="table-card">
            <div className="card-header">
              <h5>
                <i className="bi bi-award me-2"></i>
                En Başarılı Çalışanlar
              </h5>
            </div>
            <div className="card-body">
              {stats?.topEmployees?.length === 0 ? (
                <div className="empty-state">
                  <i className="bi bi-person-badge"></i>
                  <p className="mb-0">Henüz satış yok</p>
                </div>
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
          <div className="alert-card">
            <div className="card-header">
              <h5>
                <i className="bi bi-exclamation-triangle me-2"></i>
                Düşük Stok Uyarıları
              </h5>
            </div>
            <div className="card-body">
              {stats?.lowStockAlerts?.length === 0 ? (
                <div className="empty-state">
                  <i className="bi bi-check-circle"></i>
                  <p className="mb-0">Stok uyarısı yok</p>
                </div>
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
          <div className="table-card">
            <div className="card-header">
              <h5>
                <i className="bi bi-clock-history me-2"></i>
                Son Satışlar
              </h5>
            </div>
            <div className="card-body">
              {stats?.recentSales?.length === 0 ? (
                <div className="empty-state">
                  <i className="bi bi-receipt"></i>
                  <p className="mb-0">Henüz satış yok</p>
                </div>
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
