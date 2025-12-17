import React from 'react';

const Home = () => {
  return (
    <div>
      <div className="card">
        <h2>TeknoRoma Yönetim Sistemine Hoş Geldiniz</h2>
        <p>Bu sistem ile teknoloji mağazanızın tüm operasyonlarını yönetebilirsiniz.</p>
      </div>

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(250px, 1fr))', gap: '1rem' }}>
        <div className="card">
          <h3>Kategoriler</h3>
          <p>Ürün kategorilerini görüntüleyin ve yönetin.</p>
        </div>

        <div className="card">
          <h3>Ürünler</h3>
          <p>Ürün envanterini takip edin ve yönetin.</p>
        </div>

        <div className="card">
          <h3>Stok Uyarısı</h3>
          <p>Kritik stok seviyesindeki ürünleri görüntüleyin.</p>
        </div>
      </div>
    </div>
  );
};

export default Home;
