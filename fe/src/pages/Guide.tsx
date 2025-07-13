import React from 'react';

const Guide: React.FC = () => {
  const steps = [
    {
      title: "1. Đăng ký hiến máu",
      description: "Bạn có thể đăng ký trực tiếp tại điểm hiến máu hoặc qua website/ứng dụng của trung tâm hiến máu.",
    },
    {
      title: "2. Khai báo y tế & kiểm tra sức khỏe",
      description: "Bạn sẽ điền phiếu thông tin và được nhân viên y tế kiểm tra các chỉ số cơ bản như huyết áp, cân nặng, nhịp tim.",
    },
    {
      title: "3. Thực hiện hiến máu",
      description: "Quá trình hiến máu thường kéo dài khoảng 5-10 phút. Dụng cụ đều vô trùng và chỉ dùng 1 lần.",
    },
    {
      title: "4. Nghỉ ngơi & nhận quà",
      description: "Sau khi hiến máu, bạn sẽ được nghỉ tại chỗ 10-15 phút, nhận sữa/bánh và quà tặng từ BTC.",
    },
    {
      title: "5. Nhận giấy chứng nhận",
      description: "Bạn sẽ được cấp Giấy chứng nhận hiến máu tình nguyện - có giá trị sử dụng khi cần truyền máu.",
    },
  ];

  return (
    <main className="main-content py-5" style={{ paddingTop: '70px' }}>
      <div className="container my-5">
        <div className="row justify-content-center">
          <div className="col-lg-10">
            <h1 className="text-center mb-5">Hướng dẫn hiến máu</h1>
            <div className="row g-4">
              {steps.map((step, index) => (
                <div key={index} className="col-md-6">
                  <div className="card shadow-lg h-100">
                    <div className="card-body p-4">
                      <h3 className="card-title h5 mb-3">{step.title}</h3>
                      <p className="card-text text-muted">{step.description}</p>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </div>
    </main>
  );
};

export default Guide; 