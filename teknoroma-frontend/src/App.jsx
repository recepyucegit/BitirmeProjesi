import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';
import Navbar from './components/Navbar';
import Login from './pages/Login';
import Home from './pages/Home';
import CategoryList from './pages/CategoryList';
import ProductList from './pages/ProductList';
import CustomerList from './pages/CustomerList';
import SupplierList from './pages/SupplierList';
import EmployeeList from './pages/EmployeeList';

function App() {
  return (
    <Router>
      <AuthProvider>
        <Routes>
          {/* Public route - Login */}
          <Route path="/login" element={<Login />} />

          {/* Protected routes - Require authentication */}
          <Route
            path="/*"
            element={
              <ProtectedRoute>
                <div className="app">
                  <Navbar />
                  <div className="container">
                    <Routes>
                      <Route path="/" element={<Home />} />
                      <Route path="/categories" element={<CategoryList />} />
                      <Route path="/products" element={<ProductList />} />
                      <Route path="/customers" element={<CustomerList />} />
                      <Route path="/suppliers" element={<SupplierList />} />
                      <Route path="/employees" element={<EmployeeList />} />
                      {/* Redirect unknown routes to home */}
                      <Route path="*" element={<Navigate to="/" replace />} />
                    </Routes>
                  </div>
                </div>
              </ProtectedRoute>
            }
          />
        </Routes>
      </AuthProvider>
    </Router>
  );
}

export default App;
