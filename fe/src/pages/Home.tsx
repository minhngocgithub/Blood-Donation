import React, { useEffect, useState } from 'react';

interface EventDto {
  id: number;
  title: string;
  description: string;
  startDate: string;
  endDate: string;
  locationName: string;
  address: string;
  targetQuantity: number;
  currentQuantity: number;
  status: string;
}

const Home: React.FC = () => {
  const [events, setEvents] = useState<EventDto[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetch('/BloodDonationEvent/GetTop3')
      .then(res => res.json())
      .then(data => {
        setEvents(data);
        setLoading(false);
      })
      .catch(() => setLoading(false));
  }, []);

  return (
    <div className="container-fluid px-0 main-home-wrapper">
      {/* Hero Section */}
      <section className="hero-section">
        <div className="hero-background">
          <div className="container">
            <div className="row align-items-center min-vh-100">
              <div className="col-lg-6">
                <div className="hero-content">
                  <h1 className="hero-title">
                    Hiến máu cứu người
                    <span className="text-primary"> Chia sẻ yêu thương</span>
                  </h1>
                  <p className="hero-description">
                    Mỗi giọt máu bạn hiến tặng có thể cứu sống đến 3 người.
                    Hãy cùng chúng tôi lan tỏa tình yêu thương và cứu sống những sinh mạng quý giá.
                  </p>
                  <div className="hero-actions">
                    <a href="#" className="btn btn-primary btn-lg me-3">
                      <i className="fas fa-calendar-plus me-2"></i>
                      Đăng ký hiến máu
                    </a>
                    <a href="#" className="btn btn-outline-primary btn-lg">
                      <i className="fas fa-info-circle me-2"></i>
                      Tìm hiểu thêm
                    </a>
                  </div>
                </div>
              </div>
              <div className="col-lg-6">
                <div className="hero-image">
                  <img src="https://via.placeholder.com/600x400?text=Blood+Donation" alt="Blood Donation" className="img-fluid rounded-3 shadow-lg" />
                </div>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Stats Section */}
      <section className="stats-section py-5">
        <div className="container">
          <div className="row">
            <div className="col-lg-3 col-md-6 mb-4">
              <div className="stat-card text-center">
                <div className="stat-icon">
                  <i className="fas fa-users"></i>
                </div>
                <h3 className="stat-number">15,000+</h3>
                <p className="stat-label">Người hiến máu</p>
              </div>
            </div>
            <div className="col-lg-3 col-md-6 mb-4">
              <div className="stat-card text-center">
                <div className="stat-icon">
                  <i className="fas fa-tint"></i>
                </div>
                <h3 className="stat-number">45,000+</h3>
                <p className="stat-label">Đơn vị máu</p>
              </div>
            </div>
            <div className="col-lg-3 col-md-6 mb-4">
              <div className="stat-card text-center">
                <div className="stat-icon">
                  <i className="fas fa-heart"></i>
                </div>
                <h3 className="stat-number">135,000+</h3>
                <p className="stat-label">Sinh mạng cứu sống</p>
              </div>
            </div>
            <div className="col-lg-3 col-md-6 mb-4">
              <div className="stat-card text-center">
                <div className="stat-icon">
                  <i className="fas fa-calendar-check"></i>
                </div>
                <h3 className="stat-number">500+</h3>
                <p className="stat-label">Sự kiện tổ chức</p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="features-section py-5 bg-light">
        <div className="container">
          <div className="row mb-5">
            <div className="col-lg-8 mx-auto text-center">
              <h2 className="section-title">Tại sao nên hiến máu?</h2>
              <p className="section-description">
                Hiến máu không chỉ giúp cứu sống người khác mà còn mang lại nhiều lợi ích cho chính bạn
              </p>
            </div>
          </div>
          <div className="row">
            <div className="col-lg-4 col-md-6 mb-4">
              <div className="feature-card">
                <div className="feature-icon">
                  <i className="fas fa-heartbeat"></i>
                </div>
                <h4 className="feature-title">Tốt cho sức khỏe</h4>
                <p className="feature-description">
                  Hiến máu giúp kích thích sản xuất tế bào máu mới, giảm nguy cơ mắc bệnh tim mạch và ung thư.
                </p>
              </div>
            </div>
            <div className="col-lg-4 col-md-6 mb-4">
              <div className="feature-card">
                <div className="feature-icon">
                  <i className="fas fa-shield-alt"></i>
                </div>
                <h4 className="feature-title">Kiểm tra sức khỏe miễn phí</h4>
                <p className="feature-description">
                  Được kiểm tra sức khỏe tổng quát và xét nghiệm máu miễn phí trước khi hiến máu.
                </p>
              </div>
            </div>
            <div className="col-lg-4 col-md-6 mb-4">
              <div className="feature-card">
                <div className="feature-icon">
                  <i className="fas fa-hands-helping"></i>
                </div>
                <h4 className="feature-title">Cứu sống sinh mạng</h4>
                <p className="feature-description">
                  Mỗi lần hiến máu có thể cứu sống đến 3 người, mang lại ý nghĩa to lớn cho cộng đồng.
                </p>
              </div>
            </div>
          </div>
        </div>
      </section>

      {/* Upcoming Events Section */}
      <section className="events-section py-5">
        <div className="container">
          <div className="row mb-5">
            <div className="col-lg-8 mx-auto text-center">
              <h2 className="section-title">Sự kiện sắp tới</h2>
              <p className="section-description">
                Tham gia các sự kiện hiến máu gần bạn nhất
              </p>
            </div>
          </div>
          <div className="row" id="top3-events-container">
            {loading ? (
              <div className="text-center">Đang tải sự kiện...</div>
            ) : events.length === 0 ? (
              <div className="text-center">Không có sự kiện nào.</div>
            ) : (
              events.map(event => {
                const start = new Date(event.startDate);
                const end = new Date(event.endDate);
                const day = start.getDate();
                const month = `Th${start.getMonth() + 1}`;
                return (
                  <div className="col-lg-4 col-md-6 mb-4" key={event.id}>
                    <div className="event-card">
                      <div className="event-image">
                        <img src="https://via.placeholder.com/350x200?text=Event" alt="Event" className="img-fluid" />
                        <div className="event-date">
                          <span className="day">{day}</span>
                          <span className="month">{month}</span>
                        </div>
                      </div>
                      <div className="event-content">
                        <h5 className="event-title">{event.title}</h5>
                        <div className="event-meta">
                          <span className="event-location">
                            <i className="fas fa-map-marker-alt me-1"></i>
                            {event.locationName}
                          </span>
                          <span className="event-time">
                            <i className="fas fa-clock me-1"></i>
                            {start.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} - {end.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}
                          </span>
                        </div>
                        <p className="event-description">{event.description}</p>
                        <a href="#" className="btn btn-primary">Đăng ký ngay</a>
                      </div>
                    </div>
                  </div>
                );
              })
            )}
          </div>
          <div className="text-center mt-4">
            <a href="#" className="btn btn-outline-primary btn-lg">
              Xem tất cả sự kiện
              <i className="fas fa-arrow-right ms-2"></i>
            </a>
          </div>
        </div>
      </section>

      {/* CTA Section */}
      <section className="cta-section py-5 bg-primary text-white">
        <div className="container">
          <div className="row align-items-center">
            <div className="col-lg-8">
              <h3 className="cta-title mb-3">Sẵn sàng trở thành người hùng?</h3>
              <p className="cta-description mb-0">
                Đăng ký ngay hôm nay để tham gia vào cộng đồng những người hiến máu tình nguyện và cứu sống những sinh mạng quý giá.
              </p>
            </div>
            <div className="col-lg-4 text-lg-end">
              <a href="#" className="btn btn-light btn-lg">
                <i className="fas fa-user-plus me-2"></i>
                Đăng ký ngay
              </a>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
};

export default Home; 