import { Link, useLocation, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

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
        <Link
          to="/customers"
          className={location.pathname === '/customers' ? 'active' : ''}
        >
          Müşteriler
        </Link>
        <Link
          to="/suppliers"
          className={location.pathname === '/suppliers' ? 'active' : ''}
        >
          Tedarikçiler
        </Link>
        <Link
          to="/employees"
          className={location.pathname === '/employees' ? 'active' : ''}
        >
          Çalışanlar
        </Link>
        <Link
          to="/sales"
          className={location.pathname === '/sales' ? 'active' : ''}
        >
          Satışlar
        </Link>
      </nav>
      <div className="navbar-user">
        {user && <span className="user-name">{user.username || 'Kullanıcı'}</span>}
        <button onClick={handleLogout} className="logout-button">
          Çıkış Yap
        </button>
      </div>
    </div>
  );
};

export default Navbar;
