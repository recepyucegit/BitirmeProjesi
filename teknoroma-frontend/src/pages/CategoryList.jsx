import { useState, useEffect } from 'react';
import { categoryAPI } from '../services/api';
import Modal from '../components/Modal';
import CategoryForm from '../components/CategoryForm';

const CategoryList = () => {
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [formLoading, setFormLoading] = useState(false);

  // Component yüklendiğinde kategorileri API'den çek
  useEffect(() => {
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await categoryAPI.getAll();
      setCategories(data || []);
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

  const handleAdd = () => {
    setSelectedCategory(null);
    setIsModalOpen(true);
  };

  const handleEdit = (category) => {
    setSelectedCategory(category);
    setIsModalOpen(true);
  };

  const handleDelete = async (id) => {
    if (window.confirm('Bu kategoriyi silmek istediğinizden emin misiniz?')) {
      try {
        await categoryAPI.delete(id);
        await fetchCategories();
        alert('Kategori başarıyla silindi');
      } catch (err) {
        alert('Kategori silinirken bir hata oluştu: ' + err.message);
        console.error('Error deleting category:', err);
      }
    }
  };

  const handleSubmit = async (formData) => {
    try {
      setFormLoading(true);
      if (selectedCategory) {
        // Update - include id in the request body
        await categoryAPI.update(selectedCategory.id, {
          ...formData,
          id: selectedCategory.id
        });
      } else {
        // Create
        await categoryAPI.create(formData);
      }

      // Önce verileri yenile
      await fetchCategories();

      // Sonra modal'ı kapat ve mesajı göster
      setIsModalOpen(false);
      setSelectedCategory(null);

      alert(selectedCategory ? 'Kategori başarıyla güncellendi' : 'Kategori başarıyla eklendi');
    } catch (err) {
      alert('İşlem sırasında bir hata oluştu: ' + err.message);
      console.error('Error saving category:', err);
    } finally {
      setFormLoading(false);
    }
  };

  return (
    <>
      <div className="card">
        <div className="page-header">
          <h2>Kategoriler ({categories.length})</h2>
          <button className="btn btn-success" onClick={handleAdd}>
            + Yeni Kategori
          </button>
        </div>
        <table>
        <thead>
          <tr>
            <th>ID</th>
            <th>Kategori Adı</th>
            <th>Açıklama</th>
            <th>Durum</th>
            <th>Oluşturulma Tarihi</th>
            <th>İşlemler</th>
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
              <td>
                <div className="btn-group">
                  <button
                    className="btn btn-sm btn-warning"
                    onClick={() => handleEdit(category)}
                  >
                    Düzenle
                  </button>
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleDelete(category.id)}
                  >
                    Sil
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>

    <Modal
      isOpen={isModalOpen}
      onClose={() => setIsModalOpen(false)}
      title={selectedCategory ? 'Kategori Düzenle' : 'Yeni Kategori'}
    >
      <CategoryForm
        category={selectedCategory}
        onSubmit={handleSubmit}
        onCancel={() => setIsModalOpen(false)}
        loading={formLoading}
      />
    </Modal>
  </>
  );
};

export default CategoryList;
