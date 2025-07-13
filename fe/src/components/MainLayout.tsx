import React from 'react';
import { Outlet } from 'react-router-dom';
import Navbar from './Navbar';
import Footer from './Footer';

const MainLayout: React.FC = () => {
  return (
    <div className="main-layout">
      <Navbar />
      <main role="main" className="main-content">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
};

export default MainLayout; 