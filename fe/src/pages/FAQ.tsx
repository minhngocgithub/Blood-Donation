import React from 'react';

const FAQ: React.FC = () => {
  return (
    <main className="main-content py-5" style={{ paddingTop: '70px' }}>
      <div className="container my-5">
        <div className="row justify-content-center">
          <div className="col-lg-8">
            <div className="card shadow-sm">
              <div className="card-body p-4">
                <h1 className="text-center mb-5">Câu hỏi thường gặp</h1>
                
                <div className="accordion" id="faqAccordion">
                  <div className="accordion-item">
                    <h2 className="accordion-header" id="heading1">
                      <button className="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapse1" aria-expanded="true" aria-controls="collapse1">
                        Tôi cần chuẩn bị gì trước khi hiến máu?
                      </button>
                    </h2>
                    <div id="collapse1" className="accordion-collapse collapse show" aria-labelledby="heading1" data-bs-parent="#faqAccordion">
                      <div className="accordion-body">
                        Trước khi hiến máu, bạn nên ăn nhẹ, tránh đồ ăn nhiều dầu mỡ. Ngủ đủ giấc và mang theo giấy tờ tùy thân.
                      </div>
                    </div>
                  </div>

                  <div className="accordion-item">
                    <h2 className="accordion-header" id="heading2">
                      <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse2" aria-expanded="false" aria-controls="collapse2">
                        Tôi có thể hiến máu bao nhiêu lần trong năm?
                      </button>
                    </h2>
                    <div id="collapse2" className="accordion-collapse collapse" aria-labelledby="heading2" data-bs-parent="#faqAccordion">
                      <div className="accordion-body">
                        Nam giới có thể hiến máu mỗi 3 tháng, nữ giới mỗi 4 tháng. Tổng cộng khoảng 3-4 lần/năm.
                      </div>
                    </div>
                  </div>

                  <div className="accordion-item">
                    <h2 className="accordion-header" id="heading3">
                      <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse3" aria-expanded="false" aria-controls="collapse3">
                        Hiến máu có ảnh hưởng đến sức khỏe không?
                      </button>
                    </h2>
                    <div id="collapse3" className="accordion-collapse collapse" aria-labelledby="heading3" data-bs-parent="#faqAccordion">
                      <div className="accordion-body">
                        Không. Cơ thể sẽ nhanh chóng tái tạo lượng máu đã hiến. Hiến máu đều đặn còn tốt cho sức khỏe tim mạch.
                      </div>
                    </div>
                  </div>

                  <div className="accordion-item">
                    <h2 className="accordion-header" id="heading4">
                      <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse4" aria-expanded="false" aria-controls="collapse4">
                        Ai không nên hiến máu?
                      </button>
                    </h2>
                    <div id="collapse4" className="accordion-collapse collapse" aria-labelledby="heading4" data-bs-parent="#faqAccordion">
                      <div className="accordion-body">
                        Những người đang mắc bệnh, thiếu máu, huyết áp không ổn định, hoặc mới xăm hình/đi nước ngoài cần tạm hoãn.
                      </div>
                    </div>
                  </div>

                  <div className="accordion-item">
                    <h2 className="accordion-header" id="heading5">
                      <button className="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse5" aria-expanded="false" aria-controls="collapse5">
                        Tôi có được nhận giấy chứng nhận hay quà sau khi hiến máu không?
                      </button>
                    </h2>
                    <div id="collapse5" className="accordion-collapse collapse" aria-labelledby="heading5" data-bs-parent="#faqAccordion">
                      <div className="accordion-body">
                        Có. Bạn sẽ nhận được giấy chứng nhận hiến máu và có thể nhận quà tặng như áo, sữa, túi y tế nhỏ v.v.
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </main>
  );
};

export default FAQ; 