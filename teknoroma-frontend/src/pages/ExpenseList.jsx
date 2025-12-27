import { useState, useEffect } from 'react';
import { expenseAPI } from '../services/api';
import { useAuth } from '../context/AuthContext';
import Modal from '../components/Modal';
import ExpenseForm from '../components/ExpenseForm';
import ApprovalModal from '../components/ApprovalModal';

const ExpenseList = () => {
  const { user } = useAuth();
  const [expenses, setExpenses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedExpense, setSelectedExpense] = useState(null);
  const [formLoading, setFormLoading] = useState(false);
  const [filterStatus, setFilterStatus] = useState('All');

  // Approval modal states
  const [isApprovalModalOpen, setIsApprovalModalOpen] = useState(false);
  const [approvalExpense, setApprovalExpense] = useState(null);
  const [isApproving, setIsApproving] = useState(true); // true = approve, false = reject

  // Giderleri yükle
  const fetchExpenses = async () => {
    try {
      setLoading(true);
      setError(null);

      let data;
      if (filterStatus === 'All') {
        data = await expenseAPI.getAll();
      } else if (filterStatus === 'Pending') {
        data = await expenseAPI.getPending();
      } else {
        data = await expenseAPI.getByStatus(filterStatus);
      }

      setExpenses(data || []);
    } catch (err) {
      console.error('Error fetching expenses:', err);
      setError('Giderler yüklenirken bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchExpenses();
  }, [filterStatus]);

  // Yeni gider ekle
  const handleAdd = () => {
    setSelectedExpense(null);
    setIsModalOpen(true);
  };

  // Gider düzenle
  const handleEdit = (expense) => {
    // Sadece Pending durumdaki giderler düzenlenebilir
    if (expense.status !== 'Pending') {
      alert('Sadece onay bekleyen giderler düzenlenebilir!');
      return;
    }
    setSelectedExpense(expense);
    setIsModalOpen(true);
  };

  // Gider sil
  const handleDelete = async (id, expense) => {
    // Sadece Admin veya Accounting rolü silebilir
    if (!user?.roles?.includes('Admin') && !user?.roles?.includes('Accounting')) {
      alert('Bu işlem için yetkiniz yok!');
      return;
    }

    if (!window.confirm('Bu gideri silmek istediğinizden emin misiniz?')) {
      return;
    }

    try {
      await expenseAPI.delete(id);
      await fetchExpenses();
      alert('Gider başarıyla silindi!');
    } catch (err) {
      console.error('Error deleting expense:', err);
      alert('Gider silinirken bir hata oluştu: ' + (err.response?.data?.message || err.message));
    }
  };

  // Onay/Red modalını aç
  const openApprovalModal = (expense, isApproval) => {
    setApprovalExpense(expense);
    setIsApproving(isApproval);
    setIsApprovalModalOpen(true);
  };

  // Gideri onayla veya reddet
  const handleApprovalSubmit = async (notes) => {
    try {
      const approveData = {
        approvedBy: user.employeeId || 1, // Employee ID kullanıcıdan gelecek
        isApproved: isApproving,
        notes: notes
      };

      await expenseAPI.approve(approvalExpense.id, approveData);
      await fetchExpenses();
      alert(isApproving ? 'Gider başarıyla onaylandı!' : 'Gider reddedildi!');
    } catch (err) {
      console.error('Error approving expense:', err);
      alert('İşlem sırasında bir hata oluştu: ' + (err.response?.data?.message || err.message));
      throw err; // Re-throw to stop modal from closing
    }
  };

  // Form submit
  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);

      if (selectedExpense) {
        // Güncelleme
        await expenseAPI.update(selectedExpense.id, formData);
        alert('Gider başarıyla güncellendi!');
      } else {
        // Yeni ekleme
        await expenseAPI.create(formData);
        alert('Gider başarıyla eklendi!');
      }

      setIsModalOpen(false);
      await fetchExpenses();
    } catch (err) {
      console.error('Error saving expense:', err);
      alert('Gider kaydedilirken bir hata oluştu: ' + (err.response?.data?.message || err.message));
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
  const formatCurrency = (amount, currency = 'TL') => {
    if (amount === null || amount === undefined) return '-';
    return new Intl.NumberFormat('tr-TR', {
      style: 'currency',
      currency: currency === 'TL' ? 'TRY' : currency
    }).format(amount);
  };

  // Durum badge rengi
  const getStatusBadge = (status) => {
    const statusColors = {
      'Pending': 'badge-warning',
      'Approved': 'badge-success',
      'Rejected': 'badge-danger'
    };
    return statusColors[status] || 'badge-secondary';
  };

  // Durum çevirisi
  const translateStatus = (status) => {
    const statusTranslations = {
      'Pending': 'Onay Bekliyor',
      'Approved': 'Onaylandı',
      'Rejected': 'Reddedildi'
    };
    return statusTranslations[status] || status;
  };

  if (loading) {
    return (
      <div style={{ textAlign: 'center', padding: '3rem' }}>
        <div className="spinner"></div>
        <p>Giderler yükleniyor...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div style={{ textAlign: 'center', padding: '3rem' }}>
        <p style={{ color: '#dc3545' }}>{error}</p>
        <button className="btn btn-primary" onClick={fetchExpenses}>
          Tekrar Dene
        </button>
      </div>
    );
  }

  return (
    <div>
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
        <h1>Giderler</h1>
        <div style={{ display: 'flex', gap: '1rem', alignItems: 'center' }}>
          <select
            value={filterStatus}
            onChange={(e) => setFilterStatus(e.target.value)}
            className="form-select"
            style={{ width: 'auto' }}
          >
            <option value="All">Tümü</option>
            <option value="Pending">Onay Bekliyor</option>
            <option value="Approved">Onaylandı</option>
            <option value="Rejected">Reddedildi</option>
          </select>
          <button className="btn btn-primary" onClick={handleAdd}>
            + Yeni Gider
          </button>
        </div>
      </div>

      {expenses.length === 0 ? (
        <div style={{ textAlign: 'center', padding: '3rem', backgroundColor: '#f8f9fa', borderRadius: '8px' }}>
          <p style={{ color: '#666', marginBottom: '1rem' }}>
            {filterStatus === 'All' ? 'Henüz gider bulunmamaktadır.' : `${translateStatus(filterStatus)} gider bulunmamaktadır.`}
          </p>
          <button className="btn btn-primary" onClick={handleAdd}>
            İlk Gideri Ekle
          </button>
        </div>
      ) : (
        <div style={{ overflowX: 'auto' }}>
          <table className="table">
            <thead>
              <tr>
                <th>Tarih</th>
                <th>Kategori</th>
                <th>Açıklama</th>
                <th>Tutar</th>
                <th>Mağaza</th>
                <th>Çalışan</th>
                <th>Durum</th>
                <th>Onaylayan</th>
                <th>İşlemler</th>
              </tr>
            </thead>
            <tbody>
              {expenses.map((expense) => (
                <tr key={expense.id}>
                  <td>{formatDate(expense.expenseDate)}</td>
                  <td>
                    <span className="badge badge-info">{expense.category}</span>
                  </td>
                  <td>
                    <div style={{ maxWidth: '200px' }}>
                      <strong>{expense.expenseType}</strong>
                      <div style={{ fontSize: '0.85rem', color: '#666' }}>
                        {expense.description}
                      </div>
                      {expense.vendor && (
                        <div style={{ fontSize: '0.8rem', color: '#999' }}>
                          {expense.vendor}
                        </div>
                      )}
                    </div>
                  </td>
                  <td>
                    <div>
                      <strong>{formatCurrency(expense.amount, expense.currency)}</strong>
                      {expense.currency !== 'TL' && (
                        <div style={{ fontSize: '0.85rem', color: '#666' }}>
                          ≈ {formatCurrency(expense.amountInTL, 'TL')}
                        </div>
                      )}
                    </div>
                  </td>
                  <td>{expense.storeName || '-'}</td>
                  <td>{expense.employeeName || '-'}</td>
                  <td>
                    <span className={`badge ${getStatusBadge(expense.status)}`}>
                      {translateStatus(expense.status)}
                    </span>
                    {expense.approvalDate && (
                      <div style={{ fontSize: '0.75rem', color: '#666' }}>
                        {formatDate(expense.approvalDate)}
                      </div>
                    )}
                  </td>
                  <td>{expense.approverName || '-'}</td>
                  <td>
                    <div style={{ display: 'flex', gap: '0.5rem', flexWrap: 'wrap' }}>
                      {expense.status === 'Pending' && (
                        <>
                          <button
                            className="btn btn-sm btn-success"
                            onClick={() => openApprovalModal(expense, true)}
                            title="Onayla"
                          >
                            ✓
                          </button>
                          <button
                            className="btn btn-sm btn-danger"
                            onClick={() => openApprovalModal(expense, false)}
                            title="Reddet"
                          >
                            ✗
                          </button>
                          <button
                            className="btn btn-sm btn-warning"
                            onClick={() => handleEdit(expense)}
                            title="Düzenle"
                          >
                            ✏️
                          </button>
                        </>
                      )}
                      {(user?.roles?.includes('Admin') || user?.roles?.includes('Accounting')) && (
                        <button
                          className="btn btn-sm btn-danger"
                          onClick={() => handleDelete(expense.id, expense)}
                          title="Sil"
                        >
                          🗑️
                        </button>
                      )}
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Gider Ekleme/Düzenleme Modal */}
      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={selectedExpense ? 'Gider Düzenle' : 'Yeni Gider Ekle'}
      >
        <ExpenseForm
          expense={selectedExpense}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
          loading={formLoading}
        />
      </Modal>

      {/* Onay/Red Modal */}
      <ApprovalModal
        isOpen={isApprovalModalOpen}
        onClose={() => {
          setIsApprovalModalOpen(false);
          setApprovalExpense(null);
        }}
        onSubmit={handleApprovalSubmit}
        expense={approvalExpense}
        isApproval={isApproving}
      />

      {/* Gider Özeti */}
      <div style={{ marginTop: '2rem', padding: '1rem', backgroundColor: '#f8f9fa', borderRadius: '8px' }}>
        <h3 style={{ marginBottom: '1rem' }}>Özet</h3>
        <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '1rem' }}>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Toplam Gider</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#007bff' }}>
              {expenses.length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Onay Bekliyor</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#ffc107' }}>
              {expenses.filter(e => e.status === 'Pending').length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Onaylandı</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#28a745' }}>
              {expenses.filter(e => e.status === 'Approved').length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Reddedildi</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#dc3545' }}>
              {expenses.filter(e => e.status === 'Rejected').length}
            </div>
          </div>
          <div>
            <div style={{ fontSize: '0.875rem', color: '#666' }}>Toplam Tutar (TL)</div>
            <div style={{ fontSize: '1.5rem', fontWeight: 'bold', color: '#17a2b8' }}>
              {formatCurrency(
                expenses
                  .filter(e => e.status === 'Approved')
                  .reduce((sum, e) => sum + (e.amountInTL || 0), 0),
                'TL'
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ExpenseList;
