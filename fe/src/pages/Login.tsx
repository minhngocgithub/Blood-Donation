import React, { useState } from 'react';
import { useNavigate, useLocation, Link } from 'react-router-dom';
import './Login.css';
import authService, { LoginData } from '../services/authService';
import { useAuth } from '../contexts/AuthContext';

interface LoginFormData {
  email: string;
  password: string;
  rememberMe: boolean;
}

const Login: React.FC = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { checkAuth } = useAuth();
  const [formData, setFormData] = useState<LoginFormData>({
    email: '',
    password: '',
    rememberMe: false
  });
  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);

  // Get return URL from query parameters
  const searchParams = new URLSearchParams(location.search);
  const returnUrl = searchParams.get('returnUrl') || '/';

  const validateForm = (): boolean => {
    const newErrors: { [key: string]: string } = {};

    if (!formData.email) {
      newErrors.email = 'Email là bắt buộc';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Email không hợp lệ';
    }

    if (!formData.password) {
      newErrors.password = 'Mật khẩu là bắt buộc';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }));

    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }

    setIsLoading(true);

    try {
      const loginData: LoginData = {
        email: formData.email,
        password: formData.password,
        rememberMe: formData.rememberMe
      };

      const result = await authService.login(loginData);

      if (result.success) {
        // Login successful
        if (result.user) {
          authService.setCurrentUser(result.user);
        }
        await checkAuth();
        navigate(returnUrl);
      } else {
        setErrors({ general: result.message || 'Email hoặc mật khẩu không chính xác.' });
      }
    } catch (error) {
      console.error('Login error:', error);
      setErrors({ general: 'Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại.' });
    } finally {
      setIsLoading(false);
    }
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className="auth-body">
      {/* Blood Drop Animation */}
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>
      <div className="blood-drop"></div>

      <main className="auth-main">
        {/* Home Navigation Link */}
        <div className="position-fixed top-0 start-0 p-3" style={{ zIndex: 1030 }}>
          <Link to="/" className="btn btn-outline-light btn-sm d-flex align-items-center">
            <i className="fas fa-arrow-left me-2"></i>
            <span className="d-none d-sm-inline">Về trang chủ</span>
          </Link>
        </div>

        <div className="auth-card-container">
          <div className="auth-card">
            <div className="auth-card-body">
              <div className="auth-card-header">
                <h1 className="auth-card-title">Chào Mừng Trở Lại!</h1>
                <p className="auth-card-subtitle">Đăng nhập để sử dụng tất cả tính năng của trang web</p>
              </div>

              {/* Social Login Options */}
              <div className="social-login">
                <a href="#" className="social-btn">
                  <i className="fab fa-facebook-f"></i>
                </a>
                <a href="#" className="social-btn">
                  <i className="fab fa-google"></i>
                </a>
                <a href="#" className="social-btn">
                  <i className="fab fa-linkedin-in"></i>
                </a>
                <a href="#" className="social-btn">
                  <i className="fab fa-github"></i>
                </a>
              </div>

              <div className="divider">
                <span>hoặc sử dụng email để đăng nhập</span>
              </div>

              <form onSubmit={handleSubmit} noValidate className="compact-form">
                {errors.general && (
                  <div className="alert alert-danger" role="alert">
                    {errors.general}
                  </div>
                )}

                <div className="mb-3">
                  <input
                    type="email"
                    className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                    placeholder="Địa chỉ email"
                    name="email"
                    value={formData.email}
                    onChange={handleInputChange}
                    autoComplete="username"
                  />
                  {errors.email && (
                    <span className="text-danger small">{errors.email}</span>
                  )}
                </div>

                <div className="mb-3">
                  <div className="input-group">
                    <input
                      type={showPassword ? 'text' : 'password'}
                      className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                      placeholder="Mật khẩu"
                      name="password"
                      value={formData.password}
                      onChange={handleInputChange}
                      autoComplete="current-password"
                    />
                    <button
                      type="button"
                      className="btn btn-outline-secondary toggle-password"
                      onClick={togglePasswordVisibility}
                      tabIndex={-1}
                    >
                      <i className={`fa fa-${showPassword ? 'eye-slash' : 'eye'}`}></i>
                    </button>
                  </div>
                  {errors.password && (
                    <span className="text-danger">{errors.password}</span>
                  )}
                </div>

                <div className="d-flex justify-content-between align-items-center mb-4">
                  <div className="form-check">
                    <input
                      type="checkbox"
                      className="form-check-input"
                      name="rememberMe"
                      checked={formData.rememberMe}
                      onChange={handleInputChange}
                    />
                    <label className="form-check-label small text-muted">
                      Ghi nhớ đăng nhập
                    </label>
                  </div>
                  <a href="#" className="text-decoration-none small" style={{ color: '#667eea' }}>
                    Quên mật khẩu?
                  </a>
                </div>

                <div className="d-grid">
                  <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={isLoading}
                  >
                    {isLoading ? (
                      <>
                        <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                        Đang đăng nhập...
                      </>
                    ) : (
                      'ĐĂNG NHẬP'
                    )}
                  </button>
                </div>
              </form>

              <div className="text-center mt-3">
                <p className="mb-0 text-muted">
                  Chưa có tài khoản?{' '}
                  <Link to="/register" className="text-decoration-none fw-bold" style={{ color: '#667eea' }}>
                    Đăng ký
                  </Link>
                </p>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  );
};

export default Login; 