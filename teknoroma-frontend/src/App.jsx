import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import Home from './pages/Home';
import CategoryList from './pages/CategoryList';
import ProductList from './pages/ProductList';

function App() {
  return (
    <Router>
      <div className="app">
        <Navbar />
        <div className="container">
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/categories" element={<CategoryList />} />
            <Route path="/products" element={<ProductList />} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;
