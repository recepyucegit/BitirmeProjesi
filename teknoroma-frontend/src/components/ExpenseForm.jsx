import { useState, useEffect } from 'react';
import { employeeAPI, storeAPI } from '../services/api';

const ExpenseForm = ({ expense, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    expenseType: 'Operational',
    description: '',
    amount: '',
    currency: 'TL',
    exchangeRate: '1',
    expenseDate: new Date().toISOString().split('T')[0],
    employeeId: '',
    storeId: '',
    invoiceNumber: '',
    vendor: '',
    category: 'Utilities',
    paymentMethod: 'BankTransfer',
    notes: ''
  });

  const [employees, setEmployees] = useState([]);
  const [stores, setStores] = useState([]);
  const [errors, setErrors] = useState({});
  const [dataLoading, setDataLoading] = useState(true);

  // Gider Kategorileri
  const categories = [
    'Utilities', // Faturalar (Elektrik, Su, Doğalgaz)
    'Rent', // Kira
    'Salaries', // Maaşlar
    'Marketing', // Pazarlama
    'Supplies', // Malzemeler
    'Maintenance', // Bakım-Onarım
    'Transportation', // Ulaşım
    'Communication', // İletişim (Telefon, İnternet)
    'Insurance', // Sigorta
    'Tax', // Vergi
    'Legal', // Hukuki
    'Training', // Eğitim
    'Other' // Diğer
  ];

  // Gider Tipleri
  const expenseTypes = [
    'Operational', // İşletme Gideri
    'Capital', // Sermaye Gideri
    'Administrative', // İdari Gider
    'Sales', // Satış Gideri
    'Financial' // Finansal Gider
  ];

  // Ödeme Yöntemleri
  const paymentMethods = [
    'Cash', // Nakit
    'BankTransfer', // Banka Havalesi
    'CreditCard', // Kredi Kartı
    'Check', // Çek
    'Other' // Diğer
  ];

  // Para Birimleri
  const currencies = ['TL', 'USD', 'EUR'];

  // Çalışanları ve mağazaları yükle
  useEffect(() => {
    const fetchData = async () => {
      try {
        setDataLoading(true);
        const [employeesData, storesData] = await Promise.all([
          employeeAPI.getActive(),
          storeAPI.getActive()
        ]);
        setEmployees(employeesData || []);
        setStores(storesData || []);
      } catch (err) {
        console.error('Error fetching data:', err);
        setEmployees([]);
        setStores([]);
      } finally {
        setDataLoading(false);
      }
    };

    fetchData();
  }, []);

  // Gider düzenleme modunda formu doldur
  useEffect(() => {
    if (expense) {
      setFormData({
        expenseType: expense.expenseType || 'Operational',
        description: expense.description || '',
        amount: expense.amount?.toString() || '',
        currency: expense.currency || 'TL',
        exchangeRate: expense.exchangeRate?.toString() || '1',
        expenseDate: expense.expenseDate ? new Date(expense.expenseDate).toISOString().split('T')[0] : new Date().toISOString().split('T')[0],
        employeeId: expense.employeeId?.toString() || '',
        storeId: expense.storeId?.toString() || '',
        invoiceNumber: expense.invoiceNumber || '',
        vendor: expense.vendor || '',
        category: expense.category || 'Utilities',
        paymentMethod: expense.paymentMethod || 'BankTransfer',
        notes: expense.notes || ''
      });
    }
  }, [expense]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));

    // Döviz değiştikçe kuru güncelle
    if (name === 'currency') {
      if (value === 'TL') {
        setFormData(prev => ({ ...prev, exchangeRate: '1' }));
      }
    }

    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const validate = () => {
    const newErrors = {};

    // ExpenseType validation
    if (!formData.expenseType) {
      newErrors.expenseType = 'Gider tipi gereklidir';
    }

    // Description validation
    if (!formData.description.trim()) {
      newErrors.description = 'Açıklama gereklidir';
    } else if (formData.description.trim().length > 500) {
      newErrors.description = 'Açıklama en fazla 500 karakter olmalıdır';
    }

    // Amount validation
    if (!formData.amount) {
      newErrors.amount = 'Tutar gereklidir';
    } else if (isNaN(formData.amount) || parseFloat(formData.amount) <= 0) {
      newErrors.amount = 'Tutar 0\'dan büyük olmalıdır';
    }

    // ExchangeRate validation
    if (!formData.exchangeRate) {
      newErrors.exchangeRate = 'Kur gereklidir';
    } else if (isNaN(formData.exchangeRate) || parseFloat(formData.exchangeRate) <= 0) {
      newErrors.exchangeRate = 'Kur 0\'dan büyük olmalıdır';
    }

    // ExpenseDate validation
    if (!formData.expenseDate) {
      newErrors.expenseDate = 'Gider tarihi gereklidir';
    }

    // Category validation
    if (!formData.category) {
      newErrors.category = 'Kategori gereklidir';
    }

    // PaymentMethod validation
    if (!formData.paymentMethod) {
      newErrors.paymentMethod = 'Ödeme yöntemi gereklidir';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (validate()) {
      const submitData = {
        expenseType: formData.expenseType,
        description: formData.description.trim(),
        amount: parseFloat(formData.amount),
        currency: formData.currency,
        exchangeRate: parseFloat(formData.exchangeRate),
        expenseDate: formData.expenseDate,
        employeeId: formData.employeeId ? parseInt(formData.employeeId) : null,
        storeId: formData.storeId ? parseInt(formData.storeId) : null,
        invoiceNumber: formData.invoiceNumber.trim() || null,
        vendor: formData.vendor.trim() || null,
        category: formData.category,
        paymentMethod: formData.paymentMethod,
        notes: formData.notes.trim() || null
      };

      onSubmit(submitData);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="expenseType">
            Gider Tipi <span style={{ color: 'red' }}>*</span>
          </label>
          <select
            id="expenseType"
            name="expenseType"
            value={formData.expenseType}
            onChange={handleChange}
            disabled={loading}
          >
            {expenseTypes.map((type) => (
              <option key={type} value={type}>{type}</option>
            ))}
          </select>
          {errors.expenseType && <span className="error-text">{errors.expenseType}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="category">
            Kategori <span style={{ color: 'red' }}>*</span>
          </label>
          <select
            id="category"
            name="category"
            value={formData.category}
            onChange={handleChange}
            disabled={loading}
          >
            {categories.map((cat) => (
              <option key={cat} value={cat}>{cat}</option>
            ))}
          </select>
          {errors.category && <span className="error-text">{errors.category}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="description">
          Açıklama <span style={{ color: 'red' }}>*</span>
        </label>
        <textarea
          id="description"
          name="description"
          value={formData.description}
          onChange={handleChange}
          placeholder="Gider detayları"
          disabled={loading}
          rows="2"
        />
        {errors.description && <span className="error-text">{errors.description}</span>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '2fr 1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="amount">
            Tutar <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="amount"
            name="amount"
            value={formData.amount}
            onChange={handleChange}
            placeholder="0.00"
            step="0.01"
            min="0.01"
            disabled={loading}
          />
          {errors.amount && <span className="error-text">{errors.amount}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="currency">Para Birimi</label>
          <select
            id="currency"
            name="currency"
            value={formData.currency}
            onChange={handleChange}
            disabled={loading}
          >
            {currencies.map((curr) => (
              <option key={curr} value={curr}>{curr}</option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="exchangeRate">Kur</label>
          <input
            type="number"
            id="exchangeRate"
            name="exchangeRate"
            value={formData.exchangeRate}
            onChange={handleChange}
            placeholder="1.00"
            step="0.0001"
            min="0.0001"
            disabled={loading || formData.currency === 'TL'}
          />
          {errors.exchangeRate && <span className="error-text">{errors.exchangeRate}</span>}
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="vendor">Tedarikçi/Satıcı</label>
          <input
            type="text"
            id="vendor"
            name="vendor"
            value={formData.vendor}
            onChange={handleChange}
            placeholder="Tedarikçi adı"
            disabled={loading}
          />
        </div>

        <div className="form-group">
          <label htmlFor="invoiceNumber">Fatura/Fiş No</label>
          <input
            type="text"
            id="invoiceNumber"
            name="invoiceNumber"
            value={formData.invoiceNumber}
            onChange={handleChange}
            placeholder="Fatura numarası"
            disabled={loading}
          />
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="expenseDate">
            Gider Tarihi <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="date"
            id="expenseDate"
            name="expenseDate"
            value={formData.expenseDate}
            onChange={handleChange}
            disabled={loading}
          />
          {errors.expenseDate && <span className="error-text">{errors.expenseDate}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="storeId">Mağaza</label>
          <select
            id="storeId"
            name="storeId"
            value={formData.storeId}
            onChange={handleChange}
            disabled={loading || dataLoading}
          >
            <option value="">Mağaza Seçiniz (Opsiyonel)</option>
            {stores.map((store) => (
              <option key={store.id} value={store.id}>
                {store.storeName}
              </option>
            ))}
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="employeeId">Çalışan</label>
          <select
            id="employeeId"
            name="employeeId"
            value={formData.employeeId}
            onChange={handleChange}
            disabled={loading || dataLoading}
          >
            <option value="">Çalışan Seçiniz (Opsiyonel)</option>
            {employees.map((employee) => (
              <option key={employee.id} value={employee.id}>
                {employee.fullName}
              </option>
            ))}
          </select>
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="paymentMethod">
          Ödeme Yöntemi <span style={{ color: 'red' }}>*</span>
        </label>
        <select
          id="paymentMethod"
          name="paymentMethod"
          value={formData.paymentMethod}
          onChange={handleChange}
          disabled={loading}
        >
          {paymentMethods.map((method) => (
            <option key={method} value={method}>{method}</option>
          ))}
        </select>
        {errors.paymentMethod && <span className="error-text">{errors.paymentMethod}</span>}
      </div>

      <div className="form-group">
        <label htmlFor="notes">Notlar</label>
        <textarea
          id="notes"
          name="notes"
          value={formData.notes}
          onChange={handleChange}
          placeholder="Ek notlar"
          disabled={loading}
          rows="2"
        />
      </div>

      <div className="form-actions">
        <button
          type="button"
          className="btn btn-secondary"
          onClick={onCancel}
          disabled={loading}
        >
          İptal
        </button>
        <button
          type="submit"
          className="btn btn-primary"
          disabled={loading || dataLoading}
        >
          {loading ? 'Kaydediliyor...' : (expense ? 'Güncelle' : 'Kaydet')}
        </button>
      </div>
    </form>
  );
};

export default ExpenseForm;
