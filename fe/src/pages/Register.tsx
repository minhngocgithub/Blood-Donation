import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './Login.css';
import authService, { RegisterData } from '../services/authService';

interface RegisterFormData {
  fullName: string;
  email: string;
  phone: string;
  password: string;
  confirmPassword: string;
  agreeToTerms: boolean;
}

const Register: React.FC = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState<RegisterFormData>({
    fullName: '',
    email: '',
    phone: '',
    password: '',
    confirmPassword: '',
    agreeToTerms: false
  });
  const [errors, setErrors] = useState<{ [key: string]: string }>({});
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const validateForm = (): boolean => {
    const newErrors: { [key: string]: string } = {};

    if (!formData.fullName.trim()) {
      newErrors.fullName = 'Họ và tên là bắt buộc';
    }

    if (!formData.email) {
      newErrors.email = 'Email là bắt buộc';
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = 'Email không hợp lệ';
    }

    if (!formData.phone) {
      newErrors.phone = 'Số điện thoại là bắt buộc';
    } else if (!/^[0-9]{10,11}$/.test(formData.phone.replace(/\s/g, ''))) {
      newErrors.phone = 'Số điện thoại không hợp lệ';
    }

    if (!formData.password) {
      newErrors.password = 'Mật khẩu là bắt buộc';
    } else if (formData.password.length < 6) {
      newErrors.password = 'Mật khẩu phải có ít nhất 6 ký tự';
    }

    if (!formData.confirmPassword) {
      newErrors.confirmPassword = 'Xác nhận mật khẩu là bắt buộc';
    } else if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = 'Mật khẩu xác nhận không khớp';
    }

    if (!formData.agreeToTerms) {
      newErrors.agreeToTerms = 'Bạn phải đồng ý với điều khoản dịch vụ';
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
      const registerData: RegisterData = {
        fullName: formData.fullName,
        email: formData.email,
        phone: formData.phone,
        password: formData.password,
        confirmPassword: formData.confirmPassword,
        agreeToTerms: formData.agreeToTerms
      };

      const result = await authService.register(registerData);

      if (result.success) {
        // Registration successful
        console.log('Registration successful:', result.message);
        navigate('/login', { 
          state: { message: result.message }
        });
      } else {
        setErrors({ general: result.message || 'Có lỗi xảy ra khi đăng ký. Vui lòng thử lại.' });
      }
    } catch (error) {
      console.error('Registration error:', error);
      setErrors({ general: 'Có lỗi xảy ra khi đăng ký. Vui lòng thử lại.' });
    } finally {
      setIsLoading(false);
    }
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const toggleConfirmPasswordVisibility = () => {
    setShowConfirmPassword(!showConfirmPassword);
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
                <h1 className="auth-card-title">Tham Gia Cùng Chúng Tôi</h1>
                <p className="auth-card-subtitle">Mỗi giọt máu đều quý giá - Chung tay cứu sống nhiều người</p>
                <p className="text-muted small mb-0">
                  <i className="fas fa-info-circle"></i>
                  Đăng ký nhanh - Hoàn thiện hồ sơ sau
                </p>
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
                <span>hoặc sử dụng email để đăng ký</span>
              </div>

              <form onSubmit={handleSubmit} noValidate className="compact-form">
                {errors.general && (
                  <div className="alert alert-danger" role="alert">
                    {errors.general}
                  </div>
                )}

                <div className="mb-3">
                  <input
                    type="text"
                    className={`form-control ${errors.fullName ? 'is-invalid' : ''}`}
                    placeholder="Họ và tên"
                    name="fullName"
                    value={formData.fullName}
                    onChange={handleInputChange}
                    autoComplete="name"
                  />
                  {errors.fullName && (
                    <span className="text-danger small">{errors.fullName}</span>
                  )}
                </div>

                <div className="mb-3">
                  <input
                    type="email"
                    className={`form-control ${errors.email ? 'is-invalid' : ''}`}
                    placeholder="Địa chỉ email"
                    name="email"
                    value={formData.email}
                    onChange={handleInputChange}
                    autoComplete="email"
                  />
                  {errors.email && (
                    <span className="text-danger small">{errors.email}</span>
                  )}
                </div>

                <div className="mb-3">
                  <input
                    type="tel"
                    className={`form-control ${errors.phone ? 'is-invalid' : ''}`}
                    placeholder="Số điện thoại"
                    name="phone"
                    value={formData.phone}
                    onChange={handleInputChange}
                    autoComplete="tel"
                  />
                  {errors.phone && (
                    <span className="text-danger small">{errors.phone}</span>
                  )}
                </div>

                <div className="mb-3">
                  <label className="form-label">Mật khẩu</label>
                  <div className="input-group">
                    <input
                      type={showPassword ? 'text' : 'password'}
                      className={`form-control ${errors.password ? 'is-invalid' : ''}`}
                      placeholder="Mật khẩu"
                      name="password"
                      value={formData.password}
                      onChange={handleInputChange}
                      autoComplete="new-password"
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

                <div className="mb-3">
                  <label className="form-label">Xác nhận mật khẩu</label>
                  <div className="input-group">
                    <input
                      type={showConfirmPassword ? 'text' : 'password'}
                      className={`form-control ${errors.confirmPassword ? 'is-invalid' : ''}`}
                      placeholder="Xác nhận mật khẩu"
                      name="confirmPassword"
                      value={formData.confirmPassword}
                      onChange={handleInputChange}
                      autoComplete="new-password"
                    />
                    <button
                      type="button"
                      className="btn btn-outline-secondary toggle-password"
                      onClick={toggleConfirmPasswordVisibility}
                      tabIndex={-1}
                    >
                      <i className={`fa fa-${showConfirmPassword ? 'eye-slash' : 'eye'}`}></i>
                    </button>
                  </div>
                  {errors.confirmPassword && (
                    <span className="text-danger">{errors.confirmPassword}</span>
                  )}
                </div>

                <div className="mb-3 form-check">
                  <input
                    type="checkbox"
                    className={`form-check-input ${errors.agreeToTerms ? 'is-invalid' : ''}`}
                    name="agreeToTerms"
                    checked={formData.agreeToTerms}
                    onChange={handleInputChange}
                  />
                  <label className="form-check-label small">
                    Tôi đồng ý với{' '}
                    <Link to="/terms" className="text-decoration-none" style={{ color: '#dc3545' }} target="_blank">
                      Điều khoản dịch vụ
                    </Link>{' '}
                    và{' '}
                    <Link to="/privacy" className="text-decoration-none" style={{ color: '#dc3545' }} target="_blank">
                      Chính sách bảo mật
                    </Link>
                  </label>
                  {errors.agreeToTerms && (
                    <span className="text-danger small d-block">{errors.agreeToTerms}</span>
                  )}
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
                        Đang đăng ký...
                      </>
                    ) : (
                      'ĐĂNG KÝ'
                    )}
                  </button>
                </div>
              </form>

              <div className="text-center mt-3">
                <p className="mb-2 text-muted small">
                  <i className="fas fa-heart text-danger me-1"></i>
                  "Một lần hiến máu có thể cứu sống 3 người"
                </p>
                <p className="mb-0 text-muted">
                  Đã có tài khoản?{' '}
                  <Link to="/login" className="text-decoration-none fw-bold" style={{ color: '#dc3545' }}>
                    Đăng nhập
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

export default Register; 