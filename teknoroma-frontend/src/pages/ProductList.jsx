import { useState, useEffect } from 'react';
import { productAPI } from '../services/api';
import Modal from '../components/Modal';
import ProductForm from '../components/ProductForm';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  // Component yüklendiğinde ürünleri API'den çek
  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await productAPI.getAll();
      setProducts(data || []);
    } catch (err) {
      setError('Ürünler yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching products:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="loading">Ürünler yükleniyor...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  const handleAdd = () => {
    setSelectedProduct(null);
    setIsModalOpen(true);
  };

  const handleEdit = (product) => {
    setSelectedProduct(product);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu ürünü silmek istediğinizden emin misiniz?')) {
      try {
        await productAPI.delete(id);
        await fetchProducts();
        alert('Ürün başarıyla silindi');
      } catch (err) {
        alert('Ürün silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting product:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedProduct) {
        // Update - include id in the request body
        await productAPI.update(selectedProduct.id, {
          ...formData,
          id: selectedProduct.id
        });
      } else {
        // Create
        await productAPI.create(formData);
      }

      // Önce verileri yenile
      await fetchProducts();

      // Sonra modal'ı kapat ve mesajı göster
      setIsModalOpen(false);
      setSelectedProduct(null);

      alert(selectedProduct ? 'Ürün başarıyla güncellendi' : 'Ürün başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving product:', err);
    } finally {
      setFormLoading(false);
    }
  };

  return (
    <>
      <div className="card">
        <div className="page-header">
          <h2>Ürünler ({products.length})</h2>
          <button className="btn btn-success" onClick={handleAdd}>
            + Yeni Ürün
          </button>
        </div>
        <table>
          <thead>
            <tr>
              <th>ID</th>
              <th>Ürün Adı</th>
              <th>Barkod</th>
              <th>Kategori</th>
              <th>Fiyat</th>
              <th>Stok</th>
              <th>Kritik Seviye</th>
              <th>Durum</th>
              <th>İşlemler</th>
            </tr>
          </thead>
          <tbody>
            {products.map((product) => {
              const isLowStock = product.stockQuantity <= product.criticalStockLevel;
              return (
                <tr key={product.id}>
                  <td>{product.id}</td>
                  <td><strong>{product.name}</strong></td>
                  <td>{product.barcode || '-'}</td>
                  <td>{product.categoryName}</td>
                  <td>{product.price.toFixed(2)} ₺</td>
                  <td>
                    <span className={`badge ${isLowStock ? 'badge-warning' : 'badge-success'}`}>
                      {product.stockQuantity}
                    </span>
                  </td>
                  <td>{product.criticalStockLevel}</td>
                  <td>
                    <span className={`badge ${product.isActive ? 'badge-success' : 'badge-danger'}`}>
                      {product.isActive ? 'Aktif' : 'Pasif'}
                    </span>
                  </td>
                  <td>
                    <div className="btn-group">
                      <button
                        className="btn btn-sm btn-warning"
                        onClick={() => handleEdit(product)}
                      >
                        Düzenle
                      </button>
                      <button
                        className="btn btn-sm btn-danger"
                        onClick={() => handleDelete(product.id)}
                      >
                        Sil
                      </button>
                    </div>
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title={selectedProduct ? 'Ürün Düzenle' : 'Yeni Ürün'}
      >
        <ProductForm
          product={selectedProduct}
          onSubmit={handleSubmit}
          onCancel={() => setIsModalOpen(false)}
          loading={formLoading}
        />
      </Modal>
    </>
  );
};

export default ProductList;
