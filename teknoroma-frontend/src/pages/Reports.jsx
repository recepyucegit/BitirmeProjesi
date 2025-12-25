import { useState, useEffect } from 'react';
import { reportAPI } from '../services/api';
import { Tab, Tabs, TabList, TabPanel } from 'react-tabs';
import 'react-tabs/style/react-tabs.css';

export default function Reports() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Sales Report State
  const [salesSummary, setSalesSummary] = useState(null);
  const [topProducts, setTopProducts] = useState([]);
  const [topCustomers, setTopCustomers] = useState([]);
  const [topEmployees, setTopEmployees] = useState([]);

  // Stock Report State
  const [stockSummary, setStockSummary] = useState(null);
  const [lowStockAlerts, setLowStockAlerts] = useState([]);

  // Expense Report State
  const [expenseSummary, setExpenseSummary] = useState(null);

  // Date filters
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState(new Date().toISOString().split('T')[0]);

  useEffect(() => {
    // Son 30 gün
    const thirtyDaysAgo = new Date();
    thirtyDaysAgo.setDate(thirtyDaysAgo.getDate() - 30);
    setStartDate(thirtyDaysAgo.toISOString().split('T')[0]);
  }, []);

  useEffect(() => {
    if (startDate && endDate) {
      loadAllReports();
    }
  }, [startDate, endDate]);

  const loadAllReports = async () => {
    setLoading(true);
    setError('');

    try {
      await Promise.all([
        loadSalesReports(),
        loadStockReports(),
        loadExpenseReports()
      ]);
    } catch (err) {
      console.error('Raporlar yüklenirken hata:', err);
      setError('Raporlar yüklenirken bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  const loadSalesReports = async () => {
    try {
      console.log('Loading sales reports...', { startDate, endDate });
      const [summary, products, customers, employees] = await Promise.all([
        reportAPI.getSalesSummary(startDate, endDate),
        reportAPI.getTopSellingProducts(5, startDate, endDate),
        reportAPI.getTopCustomers(5, startDate, endDate),
        reportAPI.getTopEmployees(5, startDate, endDate)
      ]);

      console.log('Sales data loaded:', { summary, products, customers, employees });
      setSalesSummary(summary);
      setTopProducts(products);
      setTopCustomers(customers);
      setTopEmployees(employees);
    } catch (err) {
      console.error('Satış raporları hatası:', err);
      console.error('Error details:', err.response?.data);
    }
  };

  const loadStockReports = async () => {
    try {
      const [summary, alerts] = await Promise.all([
        reportAPI.getStockSummary(),
        reportAPI.getLowStockAlerts()
      ]);

      setStockSummary(summary);
      setLowStockAlerts(alerts);
    } catch (err) {
      console.error('Stok raporları hatası:', err);
    }
  };

  const loadExpenseReports = async () => {
    try {
      const summary = await reportAPI.getExpenseSummary(startDate, endDate);
      setExpenseSummary(summary);
    } catch (err) {
      console.error('Gider raporları hatası:', err);
    }
  };

  const handleExportSales = async () => {
    try {
      const blob = await reportAPI.exportSalesReport({ startDate, endDate });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `SalesReport_${new Date().toISOString().split('T')[0]}.xlsx`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (err) {
      console.error('Export hatası:', err);
      alert('Rapor dışa aktarılırken bir hata oluştu');
    }
  };

  const handleExportStock = async () => {
    try {
      const blob = await reportAPI.exportStockReport({});
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `StockReport_${new Date().toISOString().split('T')[0]}.xlsx`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (err) {
      console.error('Export hatası:', err);
      alert('Rapor dışa aktarılırken bir hata oluştu');
    }
  };

  const handleExportExpenses = async () => {
    try {
      const blob = await reportAPI.exportExpenseReport({ startDate, endDate });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
      link.download = `ExpenseReport_${new Date().toISOString().split('T')[0]}.xlsx`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (err) {
      console.error('Export hatası:', err);
      alert('Rapor dışa aktarılırken bir hata oluştu');
    }
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: 'TRY',
    }).format(amount || 0);
  };

  if (loading && !salesSummary) {
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
      <h2 className="mb-4">Raporlar ve Analizler</h2>

      {error && <div className="alert alert-danger">{error}</div>}

      {/* Date Filter */}
      <div className="card mb-4">
        <div className="card-body">
          <div className="row">
            <div className="col-md-3">
              <label htmlFor="startDate" className="form-label">Başlangıç Tarihi</label>
              <input
                type="date"
                id="startDate"
                className="form-control"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
              />
            </div>
            <div className="col-md-3">
              <label htmlFor="endDate" className="form-label">Bitiş Tarihi</label>
              <input
                type="date"
                id="endDate"
                className="form-control"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
              />
            </div>
            <div className="col-md-3 d-flex align-items-end">
              <button
                className="btn btn-primary"
                onClick={loadAllReports}
                disabled={loading}
              >
                {loading ? 'Yükleniyor...' : '🔄 Raporları Yenile'}
              </button>
            </div>
          </div>
        </div>
      </div>

      <Tabs>
        <TabList>
          <Tab>📊 Satış Raporları</Tab>
          <Tab>📦 Stok Raporları</Tab>
          <Tab>💰 Gider Raporları</Tab>
        </TabList>

        {/* Sales Reports Tab */}
        <TabPanel>
          <div className="row mt-3">
            <div className="col-md-12 mb-3">
              <button className="btn btn-success" onClick={handleExportSales}>
                📥 Excel'e Aktar
              </button>
            </div>

            {/* Sales Summary */}
            {salesSummary && (
              <>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-primary">
                    <div className="card-body">
                      <h6 className="card-title">Toplam Satış</h6>
                      <h3 className="mb-0">{formatCurrency(salesSummary.totalSales)}</h3>
                      <small>{salesSummary.totalSalesCount} işlem</small>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-success">
                    <div className="card-body">
                      <h6 className="card-title">Tamamlanan Satışlar</h6>
                      <h3 className="mb-0">{formatCurrency(salesSummary.completedSales)}</h3>
                      <small>{salesSummary.completedSalesCount} işlem</small>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-info">
                    <div className="card-body">
                      <h6 className="card-title">Ortalama İşlem</h6>
                      <h3 className="mb-0">{formatCurrency(salesSummary.averageSaleAmount)}</h3>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-warning text-dark">
                    <div className="card-body">
                      <h6 className="card-title">Toplam Komisyon</h6>
                      <h3 className="mb-0">{formatCurrency(salesSummary.totalCommission)}</h3>
                    </div>
                  </div>
                </div>
              </>
            )}

            {/* Top Products */}
            <div className="col-md-4 mb-3">
              <div className="card">
                <div className="card-header">
                  <h5 className="mb-0">En Çok Satan Ürünler</h5>
                </div>
                <div className="card-body">
                  <table className="table table-sm">
                    <thead>
                      <tr>
                        <th>Ürün</th>
                        <th>Adet</th>
                        <th>Gelir</th>
                      </tr>
                    </thead>
                    <tbody>
                      {topProducts.map((product, index) => (
                        <tr key={product.productId}>
                          <td>{index + 1}. {product.productName}</td>
                          <td>{product.totalQuantitySold}</td>
                          <td>{formatCurrency(product.totalRevenue)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>

            {/* Top Customers */}
            <div className="col-md-4 mb-3">
              <div className="card">
                <div className="card-header">
                  <h5 className="mb-0">En İyi Müşteriler</h5>
                </div>
                <div className="card-body">
                  <table className="table table-sm">
                    <thead>
                      <tr>
                        <th>Müşteri</th>
                        <th>Alışveriş</th>
                        <th>Toplam</th>
                      </tr>
                    </thead>
                    <tbody>
                      {topCustomers.map((customer, index) => (
                        <tr key={customer.customerId}>
                          <td>{index + 1}. {customer.customerName}</td>
                          <td>{customer.totalPurchases}</td>
                          <td>{formatCurrency(customer.totalSpent)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>

            {/* Top Employees */}
            <div className="col-md-4 mb-3">
              <div className="card">
                <div className="card-header">
                  <h5 className="mb-0">En Başarılı Çalışanlar</h5>
                </div>
                <div className="card-body">
                  <table className="table table-sm">
                    <thead>
                      <tr>
                        <th>Çalışan</th>
                        <th>Satış</th>
                        <th>Gelir</th>
                      </tr>
                    </thead>
                    <tbody>
                      {topEmployees.map((employee, index) => (
                        <tr key={employee.employeeId}>
                          <td>{index + 1}. {employee.employeeName}</td>
                          <td>{employee.totalSales}</td>
                          <td>{formatCurrency(employee.totalRevenue)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            </div>
          </div>
        </TabPanel>

        {/* Stock Reports Tab */}
        <TabPanel>
          <div className="row mt-3">
            <div className="col-md-12 mb-3">
              <button className="btn btn-success" onClick={handleExportStock}>
                📥 Excel'e Aktar
              </button>
            </div>

            {/* Stock Summary */}
            {stockSummary && (
              <>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-primary">
                    <div className="card-body">
                      <h6 className="card-title">Toplam Ürün</h6>
                      <h3 className="mb-0">{stockSummary.totalProducts}</h3>
                      <small>{stockSummary.activeProducts} aktif</small>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-success">
                    <div className="card-body">
                      <h6 className="card-title">Toplam Stok Değeri</h6>
                      <h3 className="mb-0">{formatCurrency(stockSummary.totalStockValue)}</h3>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-warning text-dark">
                    <div className="card-body">
                      <h6 className="card-title">Düşük Stok</h6>
                      <h3 className="mb-0">{stockSummary.lowStockProducts}</h3>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-danger">
                    <div className="card-body">
                      <h6 className="card-title">Tükenen Ürünler</h6>
                      <h3 className="mb-0">{stockSummary.outOfStockProducts}</h3>
                    </div>
                  </div>
                </div>
              </>
            )}

            {/* Low Stock Alerts */}
            <div className="col-12">
              <div className="card">
                <div className="card-header">
                  <h5 className="mb-0">⚠️ Düşük Stok Uyarıları</h5>
                </div>
                <div className="card-body">
                  {lowStockAlerts.length === 0 ? (
                    <p className="text-muted">Tüm ürünler yeterli stokta!</p>
                  ) : (
                    <table className="table">
                      <thead>
                        <tr>
                          <th>Ürün</th>
                          <th>Mevcut Stok</th>
                          <th>Minimum Stok</th>
                          <th>Durum</th>
                        </tr>
                      </thead>
                      <tbody>
                        {lowStockAlerts.map((alert) => (
                          <tr key={alert.productId}>
                            <td>{alert.productName}</td>
                            <td>{alert.currentStock}</td>
                            <td>{alert.minimumStock}</td>
                            <td>
                              {alert.currentStock === 0 ? (
                                <span className="badge bg-danger">Tükendi</span>
                              ) : (
                                <span className="badge bg-warning text-dark">Kritik Seviye</span>
                              )}
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  )}
                </div>
              </div>
            </div>
          </div>
        </TabPanel>

        {/* Expense Reports Tab */}
        <TabPanel>
          <div className="row mt-3">
            <div className="col-md-12 mb-3">
              <button className="btn btn-success" onClick={handleExportExpenses}>
                📥 Excel'e Aktar
              </button>
            </div>

            {/* Expense Summary */}
            {expenseSummary && (
              <>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-primary">
                    <div className="card-body">
                      <h6 className="card-title">Toplam Gider</h6>
                      <h3 className="mb-0">{formatCurrency(expenseSummary.totalExpenses)}</h3>
                      <small>{expenseSummary.totalExpenseCount} kayıt</small>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-success">
                    <div className="card-body">
                      <h6 className="card-title">Onaylanan</h6>
                      <h3 className="mb-0">{formatCurrency(expenseSummary.approvedExpenses)}</h3>
                      <small>{expenseSummary.approvedExpenseCount} kayıt</small>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-warning text-dark">
                    <div className="card-body">
                      <h6 className="card-title">Bekleyen</h6>
                      <h3 className="mb-0">{formatCurrency(expenseSummary.pendingExpenses)}</h3>
                      <small>{expenseSummary.pendingExpenseCount} kayıt</small>
                    </div>
                  </div>
                </div>
                <div className="col-md-3 mb-3">
                  <div className="card text-bg-danger">
                    <div className="card-body">
                      <h6 className="card-title">Reddedilen</h6>
                      <h3 className="mb-0">{formatCurrency(expenseSummary.rejectedExpenses)}</h3>
                      <small>{expenseSummary.rejectedExpenseCount} kayıt</small>
                    </div>
                  </div>
                </div>

                {/* Category Breakdown */}
                <div className="col-12">
                  <div className="card">
                    <div className="card-header">
                      <h5 className="mb-0">Kategoriye Göre Giderler</h5>
                    </div>
                    <div className="card-body">
                      {expenseSummary.categoryBreakdown && expenseSummary.categoryBreakdown.length > 0 ? (
                        <table className="table">
                          <thead>
                            <tr>
                              <th>Kategori</th>
                              <th>Tutar</th>
                              <th>Kayıt Sayısı</th>
                            </tr>
                          </thead>
                          <tbody>
                            {expenseSummary.categoryBreakdown.map((cat) => (
                              <tr key={cat.category}>
                                <td>{cat.category}</td>
                                <td>{formatCurrency(cat.totalAmount)}</td>
                                <td>{cat.count}</td>
                              </tr>
                            ))}
                          </tbody>
                        </table>
                      ) : (
                        <p className="text-muted">Kategori verisi bulunamadı</p>
                      )}
                    </div>
                  </div>
                </div>
              </>
            )}
          </div>
        </TabPanel>
      </Tabs>
    </div>
  );
}
