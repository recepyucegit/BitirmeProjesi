import { useState, useEffect } from 'react';
import { supplierTransactionAPI, supplierAPI, employeeAPI, productAPI } from '../services/api';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function SupplierTransactionForm() {
  const navigate = useNavigate();
  const { user } = useAuth();

  const [formData, setFormData] = useState({
    supplierId: '',
    employeeId: user?.employeeId || '',
    orderDate: new Date().toISOString().split('T')[0],
    deliveryDate: '',
    status: 'Ordered',
    orderNumber: '',
    notes: '',
  });

  const [suppliers, setSuppliers] = useState([]);
  const [employees, setEmployees] = useState([]);
  const [products, setProducts] = useState([]);

  // Sepet (işlem detayları)
  const [cart, setCart] = useState([]);

  // Ürün arama ve ekleme
  const [selectedProduct, setSelectedProduct] = useState('');
  const [quantity, setQuantity] = useState(1);
  const [unitPrice, setUnitPrice] = useState('');

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const statuses = [
    { value: 'Ordered', label: 'Sipariş Verildi' },
    { value: 'Delivered', label: 'Teslim Edildi' },
    { value: 'Cancelled', label: 'İptal Edildi' }
  ];

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    try {
      const [suppliersData, employeesData, productsData] = await Promise.all([
        supplierAPI.getActive(),
        employeeAPI.getActive(),
        productAPI.getActive(),
      ]);

      setSuppliers(suppliersData);
      setEmployees(employeesData);
      setProducts(productsData);
    } catch (err) {
      console.error('Veri yükleme hatası:', err);
      setError('Veriler yüklenirken bir hata oluştu');
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  // Ürün seçildiğinde fiyatı otomatik doldur
  const handleProductSelect = (e) => {
    const productId = e.target.value;
    setSelectedProduct(productId);

    if (productId) {
      const product = products.find(p => p.id === parseInt(productId));
      if (product) {
        // Ürünün alış fiyatını kullan (costPrice veya price)
        setUnitPrice(product.costPrice || product.price || '');
      }
    } else {
      setUnitPrice('');
    }
  };

  // Sepete ürün ekle
  const handleAddToCart = () => {
    if (!selectedProduct) {
      setError('Lütfen bir ürün seçin');
      return;
    }

    if (!quantity || quantity <= 0) {
      setError('Geçerli bir miktar girin');
      return;
    }

    if (!unitPrice || parseFloat(unitPrice) <= 0) {
      setError('Geçerli bir birim fiyat girin');
      return;
    }

    const product = products.find(p => p.id === parseInt(selectedProduct));
    if (!product) {
      setError('Ürün bulunamadı');
      return;
    }

    // Sepette zaten var mı kontrol et
    const existingItemIndex = cart.findIndex(item => item.productId === product.id);

    if (existingItemIndex >= 0) {
      // Varsa miktarı güncelle
      const updatedCart = [...cart];
      updatedCart[existingItemIndex].quantity = parseInt(quantity);
      updatedCart[existingItemIndex].unitPrice = parseFloat(unitPrice);
      updatedCart[existingItemIndex].totalPrice = parseInt(quantity) * parseFloat(unitPrice);
      setCart(updatedCart);
    } else {
      // Yoksa yeni ekle
      const newItem = {
        productId: product.id,
        productName: product.name,
        quantity: parseInt(quantity),
        unitPrice: parseFloat(unitPrice),
        totalPrice: parseInt(quantity) * parseFloat(unitPrice),
      };
      setCart([...cart, newItem]);
    }

    // Formu temizle
    setSelectedProduct('');
    setQuantity(1);
    setUnitPrice('');
    setError('');
  };

  // Sepetten ürün çıkar
  const handleRemoveFromCart = (productId) => {
    setCart(cart.filter(item => item.productId !== productId));
  };

  // Toplam tutar hesapla
  const calculateTotal = () => {
    return cart.reduce((sum, item) => sum + item.totalPrice, 0);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError('');

    // Validasyonlar
    if (!formData.supplierId) {
      setError('Lütfen tedarikçi seçin');
      return;
    }

    if (!formData.employeeId) {
      setError('Lütfen çalışan seçin');
      return;
    }

    if (cart.length === 0) {
      setError('Lütfen en az bir ürün ekleyin');
      return;
    }

    setLoading(true);

    try {
      // DTO'ya göre veriyi hazırla
      const submitData = {
        supplierId: parseInt(formData.supplierId),
        employeeId: parseInt(formData.employeeId),
        orderDate: formData.orderDate,
        deliveryDate: formData.deliveryDate || null,
        status: formData.status,
        orderNumber: formData.orderNumber.trim() || `ORD-${Date.now()}`,
        notes: formData.notes.trim() || null,
        details: cart.map(item => ({
          productId: item.productId,
          quantity: item.quantity,
          unitPrice: item.unitPrice,
        })),
      };

      await supplierTransactionAPI.create(submitData);
      navigate('/supplier-transactions');
    } catch (err) {
      console.error('Kayıt hatası:', err);
      setError(err.response?.data?.message || 'Tedarikçi işlemi kaydedilirken bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mt-4">
      <div className="row">
        <div className="col-md-8">
          <div className="card">
            <div className="card-header">
              <h5 className="mb-0">Yeni Tedarikçi Siparişi</h5>
            </div>
            <div className="card-body">
              {error && <div className="alert alert-danger">{error}</div>}

              <form onSubmit={handleSubmit}>
                <div className="row">
                  {/* Tedarikçi */}
                  <div className="col-md-6 mb-3">
                    <label htmlFor="supplierId" className="form-label">
                      Tedarikçi <span className="text-danger">*</span>
                    </label>
                    <select
                      id="supplierId"
                      name="supplierId"
                      className="form-select"
                      value={formData.supplierId}
                      onChange={handleChange}
                      required
                    >
                      <option value="">Tedarikçi seçin</option>
                      {suppliers.map((supplier) => (
                        <option key={supplier.id} value={supplier.id}>
                          {supplier.companyName}
                        </option>
                      ))}
                    </select>
                  </div>

                  {/* Çalışan */}
                  <div className="col-md-6 mb-3">
                    <label htmlFor="employeeId" className="form-label">
                      Sorumlu Çalışan <span className="text-danger">*</span>
                    </label>
                    <select
                      id="employeeId"
                      name="employeeId"
                      className="form-select"
                      value={formData.employeeId}
                      onChange={handleChange}
                      required
                    >
                      <option value="">Çalışan seçin</option>
                      {employees.map((employee) => (
                        <option key={employee.id} value={employee.id}>
                          {employee.firstName} {employee.lastName} - {employee.position}
                        </option>
                      ))}
                    </select>
                  </div>

                  {/* Sipariş Tarihi */}
                  <div className="col-md-3 mb-3">
                    <label htmlFor="orderDate" className="form-label">
                      Sipariş Tarihi <span className="text-danger">*</span>
                    </label>
                    <input
                      type="date"
                      id="orderDate"
                      name="orderDate"
                      className="form-control"
                      value={formData.orderDate}
                      onChange={handleChange}
                      required
                    />
                  </div>

                  {/* Teslim Tarihi */}
                  <div className="col-md-3 mb-3">
                    <label htmlFor="deliveryDate" className="form-label">
                      Teslim Tarihi
                    </label>
                    <input
                      type="date"
                      id="deliveryDate"
                      name="deliveryDate"
                      className="form-control"
                      value={formData.deliveryDate}
                      onChange={handleChange}
                    />
                  </div>

                  {/* Durum */}
                  <div className="col-md-3 mb-3">
                    <label htmlFor="status" className="form-label">
                      Durum <span className="text-danger">*</span>
                    </label>
                    <select
                      id="status"
                      name="status"
                      className="form-select"
                      value={formData.status}
                      onChange={handleChange}
                      required
                    >
                      {statuses.map((status) => (
                        <option key={status.value} value={status.value}>
                          {status.label}
                        </option>
                      ))}
                    </select>
                  </div>

                  {/* Sipariş Numarası */}
                  <div className="col-md-3 mb-3">
                    <label htmlFor="orderNumber" className="form-label">
                      Sipariş No
                    </label>
                    <input
                      type="text"
                      id="orderNumber"
                      name="orderNumber"
                      className="form-control"
                      value={formData.orderNumber}
                      onChange={handleChange}
                      placeholder="Otomatik oluşturulacak"
                    />
                  </div>

                  {/* Notlar */}
                  <div className="col-12 mb-3">
                    <label htmlFor="notes" className="form-label">
                      Notlar
                    </label>
                    <textarea
                      id="notes"
                      name="notes"
                      className="form-control"
                      rows="2"
                      value={formData.notes}
                      onChange={handleChange}
                      placeholder="Sipariş ile ilgili notlar..."
                    />
                  </div>
                </div>

                <hr />

                {/* Ürün Ekleme Bölümü */}
                <h6 className="mb-3">Ürün Ekle</h6>
                <div className="row">
                  <div className="col-md-5 mb-3">
                    <label htmlFor="productSelect" className="form-label">
                      Ürün
                    </label>
                    <select
                      id="productSelect"
                      className="form-select"
                      value={selectedProduct}
                      onChange={handleProductSelect}
                    >
                      <option value="">Ürün seçin</option>
                      {products.map((product) => (
                        <option key={product.id} value={product.id}>
                          {product.name} - Stok: {product.stockQuantity}
                        </option>
                      ))}
                    </select>
                  </div>

                  <div className="col-md-3 mb-3">
                    <label htmlFor="quantity" className="form-label">
                      Miktar
                    </label>
                    <input
                      type="number"
                      id="quantity"
                      className="form-control"
                      value={quantity}
                      onChange={(e) => setQuantity(e.target.value)}
                      min="1"
                    />
                  </div>

                  <div className="col-md-3 mb-3">
                    <label htmlFor="unitPrice" className="form-label">
                      Birim Fiyat (₺)
                    </label>
                    <input
                      type="number"
                      id="unitPrice"
                      className="form-control"
                      value={unitPrice}
                      onChange={(e) => setUnitPrice(e.target.value)}
                      min="0"
                      step="0.01"
                    />
                  </div>

                  <div className="col-md-1 mb-3 d-flex align-items-end">
                    <button
                      type="button"
                      className="btn btn-primary w-100"
                      onClick={handleAddToCart}
                    >
                      <i className="bi bi-plus-lg"></i>
                    </button>
                  </div>
                </div>

                <div className="d-flex gap-2 mt-3">
                  <button
                    type="submit"
                    className="btn btn-success"
                    disabled={loading || cart.length === 0}
                  >
                    {loading ? 'Kaydediliyor...' : 'Sipariş Oluştur'}
                  </button>
                  <button
                    type="button"
                    className="btn btn-secondary"
                    onClick={() => navigate('/supplier-transactions')}
                  >
                    İptal
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>

        {/* Sepet (Sipariş Özeti) */}
        <div className="col-md-4">
          <div className="card">
            <div className="card-header">
              <h5 className="mb-0">Sipariş Özeti</h5>
            </div>
            <div className="card-body">
              {cart.length === 0 ? (
                <p className="text-muted text-center">Henüz ürün eklenmedi</p>
              ) : (
                <>
                  <div className="list-group mb-3">
                    {cart.map((item) => (
                      <div
                        key={item.productId}
                        className="list-group-item d-flex justify-content-between align-items-start"
                      >
                        <div className="flex-grow-1">
                          <h6 className="mb-1">{item.productName}</h6>
                          <small className="text-muted">
                            {item.quantity} adet x {item.unitPrice.toFixed(2)} ₺
                          </small>
                          <div className="fw-bold">
                            {item.totalPrice.toFixed(2)} ₺
                          </div>
                        </div>
                        <button
                          type="button"
                          className="btn btn-sm btn-danger"
                          onClick={() => handleRemoveFromCart(item.productId)}
                        >
                          <i className="bi bi-trash"></i>
                        </button>
                      </div>
                    ))}
                  </div>

                  <div className="border-top pt-3">
                    <div className="d-flex justify-content-between mb-2">
                      <span>Ürün Sayısı:</span>
                      <strong>{cart.length}</strong>
                    </div>
                    <div className="d-flex justify-content-between mb-2">
                      <span>Toplam Adet:</span>
                      <strong>{cart.reduce((sum, item) => sum + item.quantity, 0)}</strong>
                    </div>
                    <div className="d-flex justify-content-between">
                      <h5>Genel Toplam:</h5>
                      <h5 className="text-primary">{calculateTotal().toFixed(2)} ₺</h5>
                    </div>
                  </div>
                </>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
