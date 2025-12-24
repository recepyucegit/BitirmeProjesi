import { useState, useEffect } from 'react';
import { productAPI, customerAPI, employeeAPI } from '../services/api';

const SaleForm = ({ sale, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    saleDate: new Date().toISOString().split('T')[0],
    customerId: '',
    employeeId: '',
    discountAmount: '0',
    paymentMethod: 'Cash',
    notes: ''
  });

  const [cart, setCart] = useState([]);
  const [products, setProducts] = useState([]);
  const [customers, setCustomers] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [selectedProduct, setSelectedProduct] = useState('');
  const [quantity, setQuantity] = useState('1');
  const [discountRate, setDiscountRate] = useState('0');
  const [barcodeSearch, setBarcodeSearch] = useState('');
  const [errors, setErrors] = useState({});

  // Load dropdown data
  useEffect(() => {
    loadProducts();
    loadCustomers();
    loadEmployees();
  }, []);

  const loadProducts = async () => {
    try {
      const data = await productAPI.getActive();
      setProducts(data || []);
    } catch (err) {
      console.error('Error loading products:', err);
    }
  };

  const loadCustomers = async () => {
    try {
      const data = await customerAPI.getActive();
      setCustomers(data || []);
    } catch (err) {
      console.error('Error loading customers:', err);
    }
  };

  const loadEmployees = async () => {
    try {
      const data = await employeeAPI.getActive();
      setEmployees(data || []);
    } catch (err) {
      console.error('Error loading employees:', err);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  // Barkod ile ürün ara
  const handleBarcodeSearch = async () => {
    if (!barcodeSearch.trim()) return;

    try {
      const product = await productAPI.getByBarcode(barcodeSearch);
      if (product) {
        setSelectedProduct(product.id.toString());
        setBarcodeSearch('');
        alert(`Ürün bulundu: ${product.name}`);
      } else {
        alert('Ürün bulunamadı!');
      }
    } catch (err) {
      alert('Barkod arama hatası: ' + err.message);
    }
  };

  // Sepete ürün ekle
  const handleAddToCart = () => {
    if (!selectedProduct) {
      alert('Lütfen bir ürün seçin!');
      return;
    }

    const product = products.find(p => p.id === parseInt(selectedProduct));
    if (!product) {
      alert('Ürün bulunamadı!');
      return;
    }

    const qty = parseInt(quantity) || 1;
    if (qty <= 0) {
      alert('Miktar 0\'dan büyük olmalıdır!');
      return;
    }

    if (qty > product.stockQuantity) {
      alert(`Stokta sadece ${product.stockQuantity} adet var!`);
      return;
    }

    const disc = parseFloat(discountRate) || 0;
    if (disc < 0 || disc > 100) {
      alert('İndirim oranı 0-100 arasında olmalıdır!');
      return;
    }

    // Sepette zaten var mı kontrol et
    const existingItemIndex = cart.findIndex(item => item.productId === product.id);

    if (existingItemIndex >= 0) {
      // Mevcut ürünü güncelle
      const newCart = [...cart];
      newCart[existingItemIndex].quantity += qty;
      newCart[existingItemIndex].discountRate = disc;
      calculateCartItemTotals(newCart[existingItemIndex]);
      setCart(newCart);
    } else {
      // Yeni ürün ekle
      const newItem = {
        productId: product.id,
        productName: product.name,
        quantity: qty,
        unitPrice: product.sellingPriceTL,
        discountRate: disc,
        discountAmount: 0,
        totalPrice: 0,
        netPrice: 0
      };
      calculateCartItemTotals(newItem);
      setCart([...cart, newItem]);
    }

    // Form alanlarını temizle
    setSelectedProduct('');
    setQuantity('1');
    setDiscountRate('0');
  };

  // Sepet item hesaplamaları
  const calculateCartItemTotals = (item) => {
    item.totalPrice = item.quantity * item.unitPrice;
    item.discountAmount = (item.totalPrice * item.discountRate) / 100;
    item.netPrice = item.totalPrice - item.discountAmount;
  };

  // Sepetten ürün çıkar
  const handleRemoveFromCart = (productId) => {
    setCart(cart.filter(item => item.productId !== productId));
  };

  // Sepet miktarını güncelle
  const handleUpdateQuantity = (productId, newQuantity) => {
    const qty = parseInt(newQuantity);
    if (isNaN(qty) || qty <= 0) return;

    const product = products.find(p => p.id === productId);
    if (product && qty > product.stockQuantity) {
      alert(`Stokta sadece ${product.stockQuantity} adet var!`);
      return;
    }

    const newCart = cart.map(item => {
      if (item.productId === productId) {
        item.quantity = qty;
        calculateCartItemTotals(item);
      }
      return item;
    });
    setCart(newCart);
  };

  // Toplam hesaplamaları
  const calculateTotals = () => {
    const totalAmount = cart.reduce((sum, item) => sum + item.totalPrice, 0);
    const cartDiscountTotal = cart.reduce((sum, item) => sum + item.discountAmount, 0);
    const extraDiscount = parseFloat(formData.discountAmount) || 0;
    const totalDiscount = cartDiscountTotal + extraDiscount;
    const netAmount = totalAmount - totalDiscount;
    const commissionAmount = netAmount * 0.1; // %10 komisyon

    return {
      totalAmount,
      cartDiscountTotal,
      extraDiscount,
      totalDiscount,
      netAmount,
      commissionAmount
    };
  };

  const validate = () => {
    const newErrors = {};

    if (!formData.customerId) {
      newErrors.customerId = 'Müşteri seçimi zorunludur';
    }

    if (!formData.employeeId) {
      newErrors.employeeId = 'Çalışan seçimi zorunludur';
    }

    if (cart.length === 0) {
      newErrors.cart = 'Sepete en az 1 ürün eklenmelidir';
    }

    const extraDiscount = parseFloat(formData.discountAmount) || 0;
    if (extraDiscount < 0) {
      newErrors.discountAmount = 'İndirim tutarı 0\'dan küçük olamaz';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!validate()) {
      return;
    }

    const totals = calculateTotals();

    const submitData = {
      saleDate: formData.saleDate,
      customerId: parseInt(formData.customerId),
      employeeId: parseInt(formData.employeeId),
      discountAmount: totals.extraDiscount,
      paymentMethod: formData.paymentMethod,
      notes: formData.notes || null,
      saleDetails: cart.map(item => ({
        productId: item.productId,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
        discountRate: item.discountRate
      }))
    };

    onSubmit(submitData);
  };

  const totals = calculateTotals();

  return (
    <form onSubmit={handleSubmit} style={{ maxHeight: '80vh', overflowY: 'auto' }}>
      {/* Müşteri ve Çalışan Seçimi */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="customerId">
            Müşteri <span style={{ color: 'red' }}>*</span>
          </label>
          <select
            id="customerId"
            name="customerId"
            value={formData.customerId}
            onChange={handleChange}
            disabled={loading}
          >
            <option value="">-- Müşteri Seçin --</option>
            {customers.map(customer => (
              <option key={customer.id} value={customer.id}>
                {customer.firstName} {customer.lastName} - {customer.identityNumber}
              </option>
            ))}
          </select>
          {errors.customerId && <span className="error-text">{errors.customerId}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="employeeId">
            Çalışan <span style={{ color: 'red' }}>*</span>
          </label>
          <select
            id="employeeId"
            name="employeeId"
            value={formData.employeeId}
            onChange={handleChange}
            disabled={loading}
          >
            <option value="">-- Çalışan Seçin --</option>
            {employees.map(employee => (
              <option key={employee.id} value={employee.id}>
                {employee.firstName} {employee.lastName}
              </option>
            ))}
          </select>
          {errors.employeeId && <span className="error-text">{errors.employeeId}</span>}
        </div>
      </div>

      {/* Barkod Arama */}
      <div className="form-group">
        <label htmlFor="barcodeSearch">Barkod ile Ara</label>
        <div style={{ display: 'flex', gap: '0.5rem' }}>
          <input
            type="text"
            id="barcodeSearch"
            value={barcodeSearch}
            onChange={(e) => setBarcodeSearch(e.target.value)}
            placeholder="Barkod numarasını girin"
            disabled={loading}
            onKeyPress={(e) => {
              if (e.key === 'Enter') {
                e.preventDefault();
                handleBarcodeSearch();
              }
            }}
          />
          <button
            type="button"
            className="btn btn-info"
            onClick={handleBarcodeSearch}
            disabled={loading}
          >
            Ara
          </button>
        </div>
      </div>

      {/* Ürün Ekleme */}
      <div style={{ border: '1px solid #dee2e6', borderRadius: '4px', padding: '1rem', marginBottom: '1rem', backgroundColor: '#f8f9fa' }}>
        <h4 style={{ marginTop: 0, marginBottom: '1rem', fontSize: '1rem' }}>Ürün Ekle</h4>
        <div style={{ display: 'grid', gridTemplateColumns: '2fr 1fr 1fr auto', gap: '0.5rem', alignItems: 'end' }}>
          <div className="form-group" style={{ margin: 0 }}>
            <label htmlFor="selectedProduct">Ürün</label>
            <select
              id="selectedProduct"
              value={selectedProduct}
              onChange={(e) => setSelectedProduct(e.target.value)}
              disabled={loading}
            >
              <option value="">-- Ürün Seçin --</option>
              {products.map(product => (
                <option key={product.id} value={product.id}>
                  {product.name} - ₺{product.sellingPriceTL} (Stok: {product.stockQuantity})
                </option>
              ))}
            </select>
          </div>

          <div className="form-group" style={{ margin: 0 }}>
            <label htmlFor="quantity">Miktar</label>
            <input
              type="number"
              id="quantity"
              value={quantity}
              onChange={(e) => setQuantity(e.target.value)}
              min="1"
              disabled={loading}
            />
          </div>

          <div className="form-group" style={{ margin: 0 }}>
            <label htmlFor="discountRate">İndirim %</label>
            <input
              type="number"
              id="discountRate"
              value={discountRate}
              onChange={(e) => setDiscountRate(e.target.value)}
              min="0"
              max="100"
              step="0.01"
              disabled={loading}
            />
          </div>

          <button
            type="button"
            className="btn btn-primary"
            onClick={handleAddToCart}
            disabled={loading}
            style={{ marginBottom: '0' }}
          >
            Ekle
          </button>
        </div>
      </div>

      {/* Sepet */}
      <div className="form-group">
        <label>Sepet {errors.cart && <span className="error-text">{errors.cart}</span>}</label>
        {cart.length > 0 ? (
          <table style={{ width: '100%', fontSize: '0.9rem' }}>
            <thead>
              <tr>
                <th>Ürün</th>
                <th>Birim Fiyat</th>
                <th>Miktar</th>
                <th>İndirim %</th>
                <th>Toplam</th>
                <th>İşlem</th>
              </tr>
            </thead>
            <tbody>
              {cart.map((item) => (
                <tr key={item.productId}>
                  <td>{item.productName}</td>
                  <td>₺{item.unitPrice.toFixed(2)}</td>
                  <td>
                    <input
                      type="number"
                      value={item.quantity}
                      onChange={(e) => handleUpdateQuantity(item.productId, e.target.value)}
                      min="1"
                      style={{ width: '60px' }}
                      disabled={loading}
                    />
                  </td>
                  <td>%{item.discountRate}</td>
                  <td>
                    <div>₺{item.totalPrice.toFixed(2)}</div>
                    {item.discountAmount > 0 && (
                      <div style={{ color: '#dc3545', fontSize: '0.85rem' }}>
                        -₺{item.discountAmount.toFixed(2)}
                      </div>
                    )}
                    <strong>₺{item.netPrice.toFixed(2)}</strong>
                  </td>
                  <td>
                    <button
                      type="button"
                      className="btn btn-sm btn-danger"
                      onClick={() => handleRemoveFromCart(item.productId)}
                      disabled={loading}
                    >
                      Sil
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <div style={{ textAlign: 'center', padding: '2rem', backgroundColor: '#f8f9fa', borderRadius: '4px', color: '#666' }}>
            Sepet boş
          </div>
        )}
      </div>

      {/* Toplam Bilgileri */}
      {cart.length > 0 && (
        <div style={{ backgroundColor: '#f8f9fa', padding: '1rem', borderRadius: '4px', marginBottom: '1rem' }}>
          <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem' }}>
            <span>Ara Toplam:</span>
            <span>₺{totals.totalAmount.toFixed(2)}</span>
          </div>
          {totals.cartDiscountTotal > 0 && (
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem', color: '#dc3545' }}>
              <span>Ürün İndirimleri:</span>
              <span>-₺{totals.cartDiscountTotal.toFixed(2)}</span>
            </div>
          )}
          {totals.extraDiscount > 0 && (
            <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '0.5rem', color: '#dc3545' }}>
              <span>Ekstra İndirim:</span>
              <span>-₺{totals.extraDiscount.toFixed(2)}</span>
            </div>
          )}
          <div style={{ borderTop: '2px solid #dee2e6', paddingTop: '0.5rem', marginTop: '0.5rem' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '1.2rem', fontWeight: 'bold' }}>
              <span>Net Tutar:</span>
              <span style={{ color: '#28a745' }}>₺{totals.netAmount.toFixed(2)}</span>
            </div>
            <div style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem', color: '#666', marginTop: '0.25rem' }}>
              <span>Komisyon (%10):</span>
              <span>₺{totals.commissionAmount.toFixed(2)}</span>
            </div>
          </div>
        </div>
      )}

      {/* Ödeme Detayları */}
      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="saleDate">Satış Tarihi</label>
          <input
            type="date"
            id="saleDate"
            name="saleDate"
            value={formData.saleDate}
            onChange={handleChange}
            disabled={loading}
          />
        </div>

        <div className="form-group">
          <label htmlFor="paymentMethod">Ödeme Yöntemi</label>
          <select
            id="paymentMethod"
            name="paymentMethod"
            value={formData.paymentMethod}
            onChange={handleChange}
            disabled={loading}
          >
            <option value="Cash">Nakit</option>
            <option value="CreditCard">Kredi Kartı</option>
            <option value="BankTransfer">Banka Havalesi</option>
          </select>
        </div>

        <div className="form-group">
          <label htmlFor="discountAmount">Ekstra İndirim (₺)</label>
          <input
            type="number"
            id="discountAmount"
            name="discountAmount"
            value={formData.discountAmount}
            onChange={handleChange}
            min="0"
            step="0.01"
            disabled={loading}
          />
          {errors.discountAmount && <span className="error-text">{errors.discountAmount}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="notes">Notlar</label>
        <textarea
          id="notes"
          name="notes"
          value={formData.notes}
          onChange={handleChange}
          placeholder="Opsiyonel notlar..."
          rows="2"
          disabled={loading}
        />
      </div>

      {/* Form Actions */}
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
          className="btn btn-success"
          disabled={loading || cart.length === 0}
        >
          {loading ? 'Kaydediliyor...' : 'Satışı Tamamla'}
        </button>
      </div>
    </form>
  );
};

export default SaleForm;
