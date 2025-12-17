import React from 'react';
import { Link, useLocation } from 'react-router-dom';

const Navbar = () => {
  const location = useLocation();

  return (
    <div className="navbar">
      <h1>TeknoRoma Yönetim Paneli</h1>
      <nav>
        <Link
          to="/"
          className={location.pathname === '/' ? 'active' : ''}
        >
          Ana Sayfa
        </Link>
        <Link
          to="/categories"
          className={location.pathname === '/categories' ? 'active' : ''}
        >
          Kategoriler
        </Link>
        <Link
          to="/products"
          className={location.pathname === '/products' ? 'active' : ''}
        >
          Ürünler
        </Link>
      </nav>
    </div>
  );
};

export default Navbar;
