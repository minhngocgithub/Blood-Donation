import React from 'react';

const About: React.FC = () => {
  return (
    <main className="main-content" style={{ marginTop: '80px', minHeight: '100vh' }}>
      <div className="container py-5">
        {/* Hero Section */}
        <div className="text-center mb-5">
          <h1 className="display-4 fw-bold" style={{ color: '#dc3545' }}>Về BloodLife</h1>
          <p className="lead text-muted">
            Kết nối những trái tim nhân ái, cứu sống những sinh mạng quý giá
          </p>
        </div>

        {/* Mission & Vision */}
        <div className="row mb-5">
          <div className="col-lg-6 mb-4">
            <div className="card h-100 border-0 shadow-sm">
              <div className="card-body p-4">
                <div className="text-center mb-3">
                  <i className="fas fa-bullseye fa-3x" style={{ color: '#dc3545' }}></i>
                </div>
                <h3 className="card-title text-center mb-3">Sứ Mệnh</h3>
                <p className="card-text text-muted">
                  BloodLife cam kết xây dựng một cộng đồng hiến máu nhân đạo mạnh mẽ, 
                  kết nối người hiến máu với những người cần máu một cách nhanh chóng, 
                  an toàn và hiệu quả. Chúng tôi tin rằng mỗi giọt máu đều có thể cứu sống một sinh mạng.
                </p>
              </div>
            </div>
          </div>
          <div className="col-lg-6 mb-4">
            <div className="card h-100 border-0 shadow-sm">
              <div className="card-body p-4">
                <div className="text-center mb-3">
                  <i className="fas fa-eye fa-3x" style={{ color: '#dc3545' }}></i>
                </div>
                <h3 className="card-title text-center mb-3">Tầm Nhìn</h3>
                <p className="card-text text-muted">
                  Trở thành nền tảng hàng đầu trong việc kết nối và quản lý hoạt động 
                  hiến máu nhân đạo tại Việt Nam, góp phần xây dựng một xã hội nhân ái, 
                  đoàn kết và sẵn sàng chia sẻ.
                </p>
              </div>
            </div>
          </div>
        </div>

        {/* Core Values */}
        <div className="mb-5">
          <h2 className="text-center mb-4">Giá Trị Cốt Lõi</h2>
          <div className="row">
            <div className="col-md-3 mb-4">
              <div className="text-center">
                <div className="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '80px', height: '80px', backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
                  <i className="fas fa-heart fa-2x" style={{ color: '#dc3545' }}></i>
                </div>
                <h5>Nhân Ái</h5>
                <p className="text-muted small">Tình yêu thương và sự quan tâm đến đồng loại</p>
              </div>
            </div>
            <div className="col-md-3 mb-4">
              <div className="text-center">
                <div className="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '80px', height: '80px', backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
                  <i className="fas fa-shield-alt fa-2x" style={{ color: '#dc3545' }}></i>
                </div>
                <h5>An Toàn</h5>
                <p className="text-muted small">Đảm bảo an toàn tuyệt đối cho người hiến máu</p>
              </div>
            </div>
            <div className="col-md-3 mb-4">
              <div className="text-center">
                <div className="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '80px', height: '80px', backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
                  <i className="fas fa-handshake fa-2x" style={{ color: '#dc3545' }}></i>
                </div>
                <h5>Tin Cậy</h5>
                <p className="text-muted small">Xây dựng niềm tin với cộng đồng</p>
              </div>
            </div>
            <div className="col-md-3 mb-4">
              <div className="text-center">
                <div className="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '80px', height: '80px', backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
                  <i className="fas fa-users fa-2x" style={{ color: '#dc3545' }}></i>
                </div>
                <h5>Đoàn Kết</h5>
                <p className="text-muted small">Sức mạnh của sự hợp tác và chia sẻ</p>
              </div>
            </div>
          </div>
        </div>

        {/* Story Section */}
        <div className="row mb-5">
          <div className="col-lg-6 mb-4">
            <h3 className="mb-4">Câu Chuyện Của Chúng Tôi</h3>
            <p className="text-muted">
              BloodLife được thành lập từ tình yêu thương và sự quan tâm đến cộng đồng. 
              Chúng tôi nhận ra rằng việc kết nối người hiến máu với người cần máu 
              thường gặp nhiều khó khăn và không hiệu quả.
            </p>
            <p className="text-muted">
              Với sự phát triển của công nghệ, chúng tôi quyết định xây dựng một nền tảng 
              số để giải quyết vấn đề này. BloodLife ra đời với mục tiêu tạo ra một 
              cộng đồng hiến máu nhân đạo mạnh mẽ và hiệu quả.
            </p>
          </div>
          <div className="col-lg-6 mb-4">
            <div className="rounded p-4" style={{ backgroundColor: 'rgba(220, 53, 69, 0.05)' }}>
              <h4 className="mb-3" style={{ color: '#dc3545' }}>Thống Kê Ấn Tượng</h4>
              <div className="row text-center">
                <div className="col-6 mb-3">
                  <div className="h3 fw-bold" style={{ color: '#dc3545' }}>10,000+</div>
                  <div className="text-muted small">Người hiến máu</div>
                </div>
                <div className="col-6 mb-3">
                  <div className="h3 fw-bold" style={{ color: '#dc3545' }}>5,000+</div>
                  <div className="text-muted small">Lượt hiến máu</div>
                </div>
                <div className="col-6 mb-3">
                  <div className="h3 fw-bold" style={{ color: '#dc3545' }}>50+</div>
                  <div className="text-muted small">Bệnh viện đối tác</div>
                </div>
                <div className="col-6 mb-3">
                  <div className="h3 fw-bold" style={{ color: '#dc3545' }}>15,000+</div>
                  <div className="text-muted small">Sinh mạng được cứu</div>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Team Section */}
        <div className="mb-5">
          <h2 className="text-center mb-4">Đội Ngũ Của Chúng Tôi</h2>
          <div className="row">
            <div className="col-lg-4 mb-4">
              <div className="card border-0 shadow-sm text-center">
                <div className="card-body p-4">
                  <div className="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '100px', height: '100px', backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
                    <i className="fas fa-user-md fa-3x" style={{ color: '#dc3545' }}></i>
                  </div>
                  <h5 className="card-title">Đội Ngũ Y Tế</h5>
                  <p className="card-text text-muted">
                    Các bác sĩ, y tá có kinh nghiệm trong lĩnh vực truyền máu và chăm sóc sức khỏe
                  </p>
                </div>
              </div>
            </div>
            <div className="col-lg-4 mb-4">
              <div className="card border-0 shadow-sm text-center">
                <div className="card-body p-4">
                  <div className="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '100px', height: '100px', backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
                    <i className="fas fa-laptop-code fa-3x" style={{ color: '#dc3545' }}></i>
                  </div>
                  <h5 className="card-title">Đội Ngũ Công Nghệ</h5>
                  <p className="card-text text-muted">
                    Các kỹ sư phần mềm chuyên nghiệp xây dựng nền tảng số hiện đại
                  </p>
                </div>
              </div>
            </div>
            <div className="col-lg-4 mb-4">
              <div className="card border-0 shadow-sm text-center">
                <div className="card-body p-4">
                  <div className="rounded-circle d-inline-flex align-items-center justify-content-center mb-3" style={{ width: '100px', height: '100px', backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
                    <i className="fas fa-hands-helping fa-3x" style={{ color: '#dc3545' }}></i>
                  </div>
                  <h5 className="card-title">Tình Nguyện Viên</h5>
                  <p className="card-text text-muted">
                    Những người tình nguyện nhiệt huyết hỗ trợ các hoạt động hiến máu
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* Call to Action */}
        <div className="text-center">
          <div className="rounded p-4" style={{ backgroundColor: 'rgba(220, 53, 69, 0.1)' }}>
            <h3 className="mb-3" style={{ color: '#dc3545' }}>Tham Gia Cùng Chúng Tôi</h3>
            <p className="text-muted mb-4">
              Mỗi giọt máu đều quý giá. Hãy cùng chúng tôi xây dựng một cộng đồng hiến máu nhân đạo mạnh mẽ!
            </p>
            <div className="d-flex justify-content-center gap-3 flex-wrap">
              <a href="/register" className="btn btn-lg" style={{ backgroundColor: '#dc3545', borderColor: '#dc3545', color: '#fff' }}>
                <i className="fas fa-user-plus me-2"></i>
                Đăng Ký Hiến Máu
              </a>
              <a href="/contact" className="btn btn-outline-danger btn-lg">
                <i className="fas fa-envelope me-2"></i>
                Liên Hệ Chúng Tôi
              </a>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
};

export default About; 