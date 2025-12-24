import { useState, useEffect } from 'react';
import { categoryAPI } from '../services/api';

const ProductForm = ({ product, onSubmit, onCancel, loading }) => {
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    barcode: '',
    price: '',
    costPrice: '',
    stockQuantity: '',
    criticalStockLevel: '10',
    isActive: true,
    imageUrl: '',
    categoryId: ''
  });

  const [categories, setCategories] = useState([]);
  const [errors, setErrors] = useState({});
  const [categoriesLoading, setCategoriesLoading] = useState(true);

  // Kategorileri yükle
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        setCategoriesLoading(true);
        const data = await categoryAPI.getActive();
        setCategories(data || []);
      } catch (err) {
        console.error('Error fetching categories:', err);
        setCategories([]);
      } finally {
        setCategoriesLoading(false);
      }
    };

    fetchCategories();
  }, []);

  // Ürün düzenleme modunda formu doldur
  useEffect(() => {
    if (product) {
      setFormData({
        name: product.name || '',
        description: product.description || '',
        barcode: product.barcode || '',
        price: product.price?.toString() || '',
        costPrice: product.costPrice?.toString() || '',
        stockQuantity: product.stockQuantity?.toString() || '',
        criticalStockLevel: product.criticalStockLevel?.toString() || '10',
        isActive: product.isActive !== undefined ? product.isActive : true,
        imageUrl: product.imageUrl || '',
        categoryId: product.categoryId?.toString() || ''
      });
    } else {
      // Yeni ürün modunda formu temizle
      setFormData({
        name: '',
        description: '',
        barcode: '',
        price: '',
        costPrice: '',
        stockQuantity: '',
        criticalStockLevel: '10',
        isActive: true,
        imageUrl: '',
        categoryId: ''
      });
    }
  }, [product]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const validate = () => {
    const newErrors = {};

    // Name validation
    if (!formData.name.trim()) {
      newErrors.name = 'Ürün adı gereklidir';
    } else if (formData.name.trim().length > 200) {
      newErrors.name = 'Ürün adı en fazla 200 karakter olmalıdır';
    }

    // Description validation
    if (formData.description && formData.description.length > 1000) {
      newErrors.description = 'Açıklama en fazla 1000 karakter olmalıdır';
    }

    // Barcode validation
    if (formData.barcode && formData.barcode.length > 50) {
      newErrors.barcode = 'Barkod en fazla 50 karakter olmalıdır';
    }

    // Price validation
    if (!formData.price) {
      newErrors.price = 'Satış fiyatı gereklidir';
    } else if (isNaN(formData.price) || parseFloat(formData.price) <= 0) {
      newErrors.price = 'Satış fiyatı 0\'dan büyük olmalıdır';
    }

    // CostPrice validation
    if (!formData.costPrice) {
      newErrors.costPrice = 'Alış fiyatı gereklidir';
    } else if (isNaN(formData.costPrice) || parseFloat(formData.costPrice) < 0) {
      newErrors.costPrice = 'Alış fiyatı 0 veya daha büyük olmalıdır';
    }

    // StockQuantity validation
    if (!formData.stockQuantity) {
      newErrors.stockQuantity = 'Stok miktarı gereklidir';
    } else if (isNaN(formData.stockQuantity) || parseInt(formData.stockQuantity) < 0) {
      newErrors.stockQuantity = 'Stok miktarı 0 veya daha büyük olmalıdır';
    }

    // CriticalStockLevel validation
    if (!formData.criticalStockLevel) {
      newErrors.criticalStockLevel = 'Kritik stok seviyesi gereklidir';
    } else if (isNaN(formData.criticalStockLevel) || parseInt(formData.criticalStockLevel) < 0) {
      newErrors.criticalStockLevel = 'Kritik stok seviyesi 0 veya daha büyük olmalıdır';
    }

    // CategoryId validation
    if (!formData.categoryId) {
      newErrors.categoryId = 'Kategori seçimi gereklidir';
    }

    // ImageUrl validation
    if (formData.imageUrl && formData.imageUrl.length > 500) {
      newErrors.imageUrl = 'Resim URL\'si en fazla 500 karakter olmalıdır';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (validate()) {
      // Convert string values to appropriate types
      const submitData = {
        name: formData.name.trim(),
        description: formData.description.trim() || null,
        barcode: formData.barcode.trim() || null,
        price: parseFloat(formData.price),
        costPrice: parseFloat(formData.costPrice),
        stockQuantity: parseInt(formData.stockQuantity),
        criticalStockLevel: parseInt(formData.criticalStockLevel),
        isActive: formData.isActive,
        imageUrl: formData.imageUrl.trim() || null,
        categoryId: parseInt(formData.categoryId)
      };

      onSubmit(submitData);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="name">
          Ürün Adı <span style={{ color: 'red' }}>*</span>
        </label>
        <input
          type="text"
          id="name"
          name="name"
          value={formData.name}
          onChange={handleChange}
          placeholder="Örn: Dell XPS 15"
          disabled={loading}
        />
        {errors.name && <span className="error-text">{errors.name}</span>}
      </div>

      <div className="form-group">
        <label htmlFor="description">Açıklama</label>
        <textarea
          id="description"
          name="description"
          value={formData.description}
          onChange={handleChange}
          placeholder="Ürün hakkında detaylı açıklama"
          disabled={loading}
          rows="3"
        />
        {errors.description && <span className="error-text">{errors.description}</span>}
      </div>

      <div className="form-group">
        <label htmlFor="categoryId">
          Kategori <span style={{ color: 'red' }}>*</span>
        </label>
        <select
          id="categoryId"
          name="categoryId"
          value={formData.categoryId}
          onChange={handleChange}
          disabled={loading || categoriesLoading}
        >
          <option value="">Kategori Seçiniz</option>
          {categories.map((category) => (
            <option key={category.id} value={category.id}>
              {category.name}
            </option>
          ))}
        </select>
        {errors.categoryId && <span className="error-text">{errors.categoryId}</span>}
      </div>

      <div className="form-group">
        <label htmlFor="barcode">Barkod</label>
        <input
          type="text"
          id="barcode"
          name="barcode"
          value={formData.barcode}
          onChange={handleChange}
          placeholder="Örn: 1234567890123"
          pattern="[0-9]*"
          inputMode="numeric"
          disabled={loading}
        />
        {errors.barcode && <span className="error-text">{errors.barcode}</span>}
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="costPrice">
            Alış Fiyatı (₺) <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="costPrice"
            name="costPrice"
            value={formData.costPrice}
            onChange={handleChange}
            placeholder="0.00"
            step="0.01"
            min="0"
            disabled={loading}
          />
          {errors.costPrice && <span className="error-text">{errors.costPrice}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="price">
            Satış Fiyatı (₺) <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="price"
            name="price"
            value={formData.price}
            onChange={handleChange}
            placeholder="0.00"
            step="0.01"
            min="0.01"
            disabled={loading}
          />
          {errors.price && <span className="error-text">{errors.price}</span>}
        </div>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '1rem' }}>
        <div className="form-group">
          <label htmlFor="stockQuantity">
            Stok Miktarı <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="stockQuantity"
            name="stockQuantity"
            value={formData.stockQuantity}
            onChange={handleChange}
            placeholder="0"
            min="0"
            disabled={loading}
          />
          {errors.stockQuantity && <span className="error-text">{errors.stockQuantity}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="criticalStockLevel">
            Kritik Stok Seviyesi <span style={{ color: 'red' }}>*</span>
          </label>
          <input
            type="number"
            id="criticalStockLevel"
            name="criticalStockLevel"
            value={formData.criticalStockLevel}
            onChange={handleChange}
            placeholder="10"
            min="0"
            disabled={loading}
          />
          {errors.criticalStockLevel && <span className="error-text">{errors.criticalStockLevel}</span>}
        </div>
      </div>

      <div className="form-group">
        <label htmlFor="imageUrl">Resim URL</label>
        <input
          type="text"
          id="imageUrl"
          name="imageUrl"
          value={formData.imageUrl}
          onChange={handleChange}
          placeholder="https://example.com/image.jpg"
          disabled={loading}
        />
        {errors.imageUrl && <span className="error-text">{errors.imageUrl}</span>}
      </div>

      <div className="form-group">
        <div className="checkbox-wrapper">
          <input
            type="checkbox"
            id="isActive"
            name="isActive"
            checked={formData.isActive}
            onChange={handleChange}
            disabled={loading}
          />
          <label htmlFor="isActive">Aktif</label>
        </div>
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
          disabled={loading || categoriesLoading}
        >
          {loading ? 'Kaydediliyor...' : (product ? 'Güncelle' : 'Kaydet')}
        </button>
      </div>
    </form>
  );
};

export default ProductForm;
