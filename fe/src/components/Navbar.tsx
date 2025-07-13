import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const Navbar: React.FC = () => {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    await logout();
    navigate('/'); // <-- chuyển về trang chủ
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
      <div className="container">
        <Link className="navbar-brand fw-bold text-danger" to="/">
          <i className="fas fa-tint me-2"></i>BloodLife
        </Link>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#mainNavbar"
          aria-controls="mainNavbar"
          aria-expanded="false"
          aria-label="Toggle navigation"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="mainNavbar">
          <ul className="navbar-nav me-auto mb-2 mb-lg-0">
            <li className="nav-item">
              <Link className="nav-link" to="/">Trang chủ</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/events">Sự kiện</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/news">Tin tức</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/about">Giới thiệu</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/contact">Liên hệ</Link>
            </li>
          </ul>
          <div className="d-flex align-items-center">
            {isAuthenticated && user ? (
              <div className="dropdown">
                <button className="btn btn-outline-danger dropdown-toggle d-flex align-items-center"
                  type="button" id="userDropdown" data-bs-toggle="dropdown" aria-expanded="false">
                  <i className="fas fa-user-circle me-2"></i>
                  <span className="d-none d-md-inline">{user.fullName}</span>
                  <span className="d-md-none">Menu</span>
                </button>
                <ul className="dropdown-menu dropdown-menu-end shadow" aria-labelledby="userDropdown">
                  <li>
                    <h6 className="dropdown-header">
                      <i className="fas fa-user me-2"></i>{user.fullName}
                    </h6>
                  </li>
                  <li><hr className="dropdown-divider" /></li>
                  <li>
                    <Link className="dropdown-item" to="/profile">
                      <i className="fas fa-user me-2 text-primary"></i>Thông tin cá nhân
                    </Link>
                  </li>
                  <li>
                    <Link className="dropdown-item" to="/change-password">
                      <i className="fas fa-key me-2 text-warning"></i>Đổi mật khẩu
                    </Link>
                  </li>
                  <li>
                    <Link className="dropdown-item" to="/my-registrations">
                      <i className="fas fa-calendar-check me-2 text-success"></i>Đăng ký của tôi
                    </Link>
                  </li>
                  <li>
                    <Link className="dropdown-item" to="/donation-history">
                      <i className="fas fa-history me-2 text-info"></i>Lịch sử hiến máu
                    </Link>
                  </li>
                  {(user.role === 'Admin' || user.role === 'SuperAdmin') && (
                    <>
                      <li><hr className="dropdown-divider" /></li>
                      <li>
                        <Link className="dropdown-item" to="/admin/dashboard">
                          <i className="fas fa-cog me-2 text-warning"></i>Quản trị hệ thống
                        </Link>
                      </li>
                    </>
                  )}
                  {(user.role === 'Doctor' || user.role === 'Nurse') && (
                    <li>
                      <Link className="dropdown-item" to="/health-screening">
                        <i className="fas fa-stethoscope me-2 text-success"></i>Sàng lọc sức khỏe
                      </Link>
                    </li>
                  )}
                  <li><hr className="dropdown-divider" /></li>
                  <li>
                    <button className="dropdown-item text-danger" onClick={handleLogout}>
                      <i className="fas fa-sign-out-alt me-2"></i>Đăng xuất
                    </button>
                  </li>
                </ul>
              </div>
            ) : (
              <div className="d-flex gap-2">
                <Link className="btn btn-outline-danger" to="/login">
                  <i className="fas fa-sign-in-alt me-1"></i>
                  <span className="d-none d-sm-inline">Đăng nhập</span>
                </Link>
                <Link className="btn btn-danger" to="/register">
                  <i className="fas fa-user-plus me-1"></i>
                  <span className="d-none d-sm-inline">Đăng ký</span>
                </Link>
              </div>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
};

export default Navbar; 