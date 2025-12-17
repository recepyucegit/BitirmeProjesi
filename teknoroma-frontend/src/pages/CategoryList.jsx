import React, { useState, useEffect } from 'react';
import { categoryAPI } from '../services/api';

const CategoryList = () => {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  // Component yüklendiğinde kategorileri API'den çek
  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      setLoading(true);
      setError(null);
      const response = await categoryAPI.getAll();
      setCategories(response.data);
    } catch (err) {
      setError('Kategoriler yüklenirken bir hata oluştu: ' + err.message);
      console.error('Error fetching categories:', err);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div className="loading">Kategoriler yükleniyor...</div>;
  }

  if (error) {
    return <div className="error">{error}</div>;
  }

  return (
    <div className="card">
      <h2>Kategoriler ({categories.length})</h2>
      <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Kategori Adı</th>
            <th>Açıklama</th>
            <th>Durum</th>
            <th>Oluşturulma Tarihi</th>
          </tr>
        </thead>
        <tbody>
          {categories.map((category) => (
            <tr key={category.id}>
              <td>{category.id}</td>
              <td><strong>{category.name}</strong></td>
              <td>{category.description || '-'}</td>
              <td>
                <span className={`badge ${category.isActive ? 'badge-success' : 'badge-danger'}`}>
                  {category.isActive ? 'Aktif' : 'Pasif'}
                </span>
              </td>
              <td>{new Date(category.createdDate).toLocaleDateString('tr-TR')}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CategoryList;
