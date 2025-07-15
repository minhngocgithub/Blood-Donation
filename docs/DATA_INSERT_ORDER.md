# Thứ Tự Chèn Dữ Liệu

Khi thiết lập cơ sở dữ liệu hoặc tạo dữ liệu ban đầu, điều quan trọng là phải chèn bản ghi theo đúng thứ tự để đảm bảo ràng buộc khóa ngoại và sự phụ thuộc giữa các bảng. Hãy sử dụng thứ tự và ghi chú phụ thuộc sau:

## Thứ Tự Chèn & Chi Tiết Phụ Thuộc

1. **Roles (Vai trò)**  
   - *Không có phụ thuộc.*  
   - Các bảng khác (Users) tham chiếu tới Roles.

2. **BloodTypes (Nhóm máu)**  
   - *Không có phụ thuộc.*  
   - Được tham chiếu bởi Users, BloodCompatibility, DonationHistory.

3. **BloodCompatibility (Tương thích nhóm máu)**  
   - *Phụ thuộc vào BloodTypes.*  
   - Cả FromBloodTypeId và ToBloodTypeId đều tham chiếu đến BloodTypes.

4. **Users (Người dùng)**  
   - *Phụ thuộc vào Roles, BloodTypes.*  
   - RoleId tham chiếu tới Roles. BloodTypeId tham chiếu tới BloodTypes.  
   - Được tham chiếu bởi BloodDonationEvents (CreatedBy), DonationRegistrations, HealthScreening (ScreenedBy), News (AuthorId), Notifications, ContactMessages (ResolvedBy).

5. **Locations (Địa điểm)**  
   - *Không có phụ thuộc.*  
   - Được tham chiếu bởi BloodDonationEvents.

6. **BloodDonationEvents (Sự kiện hiến máu)**  
   - *Phụ thuộc vào Locations, Users (CreatedBy).*  
   - LocationId tham chiếu tới Locations. CreatedBy tham chiếu tới Users.  
   - Được tham chiếu bởi DonationRegistrations, DonationHistory.

7. **DonationRegistrations (Đăng ký hiến máu)**  
   - *Phụ thuộc vào Users, BloodDonationEvents.*  
   - UserId tham chiếu tới Users. EventId tham chiếu tới BloodDonationEvents.  
   - Được tham chiếu bởi HealthScreening, DonationHistory.

8. **HealthScreening (Khám sức khỏe)**  
   - *Phụ thuộc vào DonationRegistrations, Users (ScreenedBy).*  
   - RegistrationId tham chiếu tới DonationRegistrations. ScreenedBy tham chiếu tới Users.

9. **DonationHistory (Lịch sử hiến máu)**  
   - *Phụ thuộc vào Users, BloodDonationEvents, DonationRegistrations (tùy chọn), BloodTypes.*  
   - UserId tham chiếu tới Users. EventId tham chiếu tới BloodDonationEvents. RegistrationId tham chiếu tới DonationRegistrations. BloodTypeId tham chiếu tới BloodTypes.

10. **NewsCategories (Danh mục tin tức)**  
    - *Không có phụ thuộc.*  
    - Được tham chiếu bởi News.

11. **News (Tin tức)**  
    - *Phụ thuộc vào NewsCategories, Users (AuthorId).*  
    - CategoryId tham chiếu tới NewsCategories. AuthorId tham chiếu tới Users.

12. **Notifications (Thông báo)**  
    - *Phụ thuộc vào Users.*  
    - UserId tham chiếu tới Users.

13. **Settings (Cài đặt)**  
    - *Không có phụ thuộc.*  
    - Có thể chèn bất cứ lúc nào.

14. **ContactMessages (Tin nhắn liên hệ)**  
    - *Phụ thuộc vào Users (ResolvedBy, tùy chọn).*  
    - ResolvedBy tham chiếu tới Users, nhưng có thể là null đối với tin nhắn mới.

---

## Biểu đồ Phụ Thuộc

- **Roles** → Users  
- **BloodTypes** → Users, BloodCompatibility, DonationHistory  
- **BloodCompatibility** → BloodTypes  
- **Users** → BloodDonationEvents, DonationRegistrations, HealthScreening, News, Notifications, ContactMessages  
- **Locations** → BloodDonationEvents  
- **BloodDonationEvents** → DonationRegistrations, DonationHistory  
- **DonationRegistrations** → HealthScreening, DonationHistory  
- **HealthScreening** → Users, DonationRegistrations  
- **DonationHistory** → Users, BloodDonationEvents, DonationRegistrations, BloodTypes  
- **NewsCategories** → News  
- **News** → NewsCategories, Users  
- **Notifications** → Users  
- **Settings** → (không phụ thuộc)  
- **ContactMessages** → Users (tùy chọn)

---

## Các Bảng Có Thể Chèn Độc Lập (Không Phụ Thuộc):
- Roles  
- BloodTypes  
- Locations  
- NewsCategories  
- Settings  

## Ghi Chú:
- Luôn chèn các bảng cha/trên trước bảng con/phụ để tránh lỗi ràng buộc khóa ngoại.
- Một số bảng (vd: ContactMessages, HealthScreening) có khóa ngoại tùy chọn và có thể chèn với giá trị null nếu cần.
- Điều chỉnh thứ tự nếu sơ đồ của bạn có thêm các phụ thuộc không được phản ánh ở đây.
