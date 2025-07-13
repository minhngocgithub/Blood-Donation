import React from 'react';

const Footer: React.FC = () => {
  return (
    <footer className="footer-section bg-dark text-light mt-5">
      <div className="container">
        <div className="row py-5">
          {/* Brand Column */}
          <div className="col-lg-4 col-md-6 mb-4">
            <div className="footer-brand mb-3">
              <img src="https://via.placeholder.com/40x40?text=Logo" alt="Blood Donation Logo" height="40" className="mb-2" />
              <h5 className="text-white fw-bold">BloodLife</h5>
            </div>
            <p className="text-muted mb-3">
              Kết nối những trái tim nhân ái, cứu sống những sinh mạng quý giá.
              Mỗi giọt máu hiến tặng là một cơ hội sống mới cho những người cần được giúp đỡ.
            </p>
            <div className="social-links">
              <a href="#" className="text-light me-3 social-link" title="Facebook">
                <i className="fab fa-facebook-f"></i>
              </a>
              <a href="#" className="text-light me-3 social-link" title="Twitter">
                <i className="fab fa-twitter"></i>
              </a>
              <a href="#" className="text-light me-3 social-link" title="Instagram">
                <i className="fab fa-instagram"></i>
              </a>
              <a href="#" className="text-light social-link" title="YouTube">
                <i className="fab fa-youtube"></i>
              </a>
            </div>
          </div>

          {/* Quick Links */}
          <div className="col-lg-2 col-md-6 mb-4">
            <h6 className="text-white mb-3 fw-bold">Liên kết nhanh</h6>
            <ul className="list-unstyled footer-links">
              <li><a href="/" className="text-muted text-decoration-none">Trang chủ</a></li>
              <li><a href="/events" className="text-muted text-decoration-none">Sự kiện hiến máu</a></li>
              <li><a href="/news" className="text-muted text-decoration-none">Tin tức</a></li>
              <li><a href="/about" className="text-muted text-decoration-none">Giới thiệu</a></li>
              <li><a href="/contact" className="text-muted text-decoration-none">Liên hệ</a></li>
            </ul>
          </div>

          {/* Support */}
          <div className="col-lg-3 col-md-6 mb-4">
            <h6 className="text-white mb-3 fw-bold">Hỗ trợ</h6>
            <ul className="list-unstyled footer-links">
              <li><a href="/guide" className="text-muted text-decoration-none">Hướng dẫn hiến máu</a></li>
              <li><a href="/privacy" className="text-muted text-decoration-none">Chính sách bảo mật</a></li>
              <li><a href="/terms" className="text-muted text-decoration-none">Điều khoản sử dụng</a></li>
              <li><a href="/faq" className="text-muted text-decoration-none">Câu hỏi thường gặp</a></li>
              <li><a href="tel:1900-1234" className="text-muted text-decoration-none">Hotline: 1900-1234</a></li>
            </ul>
          </div>

          {/* Contact Info */}
          <div className="col-lg-3 col-md-6 mb-4">
            <h6 className="text-white mb-3 fw-bold">Thông tin liên hệ</h6>
            <div className="contact-info">
              <div className="contact-item mb-2">
                <i className="fas fa-map-marker-alt me-2 text-danger"></i>
                <span className="text-muted">26 Nguyễn Thái Học, Ba Đình, Hà Nội</span>
              </div>
              <div className="contact-item mb-2">
                <i className="fas fa-phone me-2 text-danger"></i>
                <a href="tel:1900-1234" className="text-muted text-decoration-none">1900-1234</a>
              </div>
              <div className="contact-item mb-2">
                <i className="fas fa-envelope me-2 text-danger"></i>
                <a href="mailto:lienhe@hienmau.gov.vn" className="text-muted text-decoration-none">lienhe@hienmau.gov.vn</a>
              </div>
              <div className="contact-item">
                <i className="fas fa-clock me-2 text-danger"></i>
                <span className="text-muted">24/7 - Luôn sẵn sàng hỗ trợ</span>
              </div>
            </div>
          </div>
        </div>

        {/* Footer Bottom */}
        <hr className="border-secondary my-4" />
        <div className="row py-3 align-items-center">
          <div className="col-md-6">
            <p className="text-muted mb-0">
              &copy; {new Date().getFullYear()} BloodLife - Hệ thống Hiến Máu Nhân Đạo.
              Tất cả quyền được bảo lưu.
            </p>
          </div>
          <div className="col-md-6 text-md-end">
            <p className="text-muted mb-0">
              Được phát triển với <i className="fas fa-heart text-danger"></i> tại Việt Nam
            </p>
          </div>
        </div>
      </div>
    </footer>
  );
};

export default Footer; 