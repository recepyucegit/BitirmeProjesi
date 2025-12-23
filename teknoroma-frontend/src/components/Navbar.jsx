import React from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Navbar = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const { user, logout } = useAuth();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

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
      <div className="navbar-user">
        <span className="user-info">
          {user?.username || 'Kullanıcı'}
        </span>
        <button onClick={handleLogout} className="logout-button">
          Çıkış Yap
        </button>
      </div>
    </div>
  );
};

export default Navbar;
