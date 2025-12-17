import React, { useState, useEffect } from 'react';
import { productAPI } from '../services/api';

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Component yüklendiğinde ürünleri API'den çek
  useEffect(() => {
    fetchProducts();
  }, []);

  const fetchProducts = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await productAPI.getAll();
      setProducts(response.data);
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

  return (
    <div className="card">
      <h2>Ürünler ({products.length})</h2>
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
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
};

export default ProductList;
