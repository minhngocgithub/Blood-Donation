<<<<<<< HEAD
# Data Insert Order

When setting up the database or seeding initial data, it is important to insert records in the correct order to satisfy foreign key constraints and dependencies between tables. Use the following order and dependency notes:

## Insert Order & Dependency Details

1. **Roles**  
   - *No dependencies.*  
   - Other tables (Users) reference Roles.

2. **BloodTypes**  
   - *No dependencies.*  
   - Referenced by Users, BloodCompatibility, DonationHistory.

3. **BloodCompatibility**  
   - *Depends on BloodTypes.*  
   - Both FromBloodTypeId and ToBloodTypeId reference BloodTypes.

4. **Users**  
   - *Depends on Roles, BloodTypes.*  
   - RoleId references Roles. BloodTypeId references BloodTypes.
   - Referenced by BloodDonationEvents (CreatedBy), DonationRegistrations, HealthScreening (ScreenedBy), News (AuthorId), Notifications, ContactMessages (ResolvedBy).

5. **Locations**  
   - *No dependencies.*  
   - Referenced by BloodDonationEvents.

6. **BloodDonationEvents**  
   - *Depends on Locations, Users (CreatedBy).*  
   - LocationId references Locations. CreatedBy references Users.
   - Referenced by DonationRegistrations, DonationHistory.

7. **DonationRegistrations**  
   - *Depends on Users, BloodDonationEvents.*  
   - UserId references Users. EventId references BloodDonationEvents.
   - Referenced by HealthScreening, DonationHistory.

8. **HealthScreening**  
   - *Depends on DonationRegistrations, Users (ScreenedBy).*  
   - RegistrationId references DonationRegistrations. ScreenedBy references Users.

9. **DonationHistory**  
   - *Depends on Users, BloodDonationEvents, DonationRegistrations (optional), BloodTypes.*  
   - UserId references Users. EventId references BloodDonationEvents. RegistrationId references DonationRegistrations. BloodTypeId references BloodTypes.

10. **NewsCategories**  
    - *No dependencies.*  
    - Referenced by News.

11. **News**  
    - *Depends on NewsCategories, Users (AuthorId).*  
    - CategoryId references NewsCategories. AuthorId references Users.

12. **Notifications**  
    - *Depends on Users.*  
    - UserId references Users.

13. **Settings**  
    - *No dependencies.*  
    - Can be inserted at any time.

14. **ContactMessages**  
    - *Depends on Users (ResolvedBy, optional).*  
    - ResolvedBy references Users, but can be null for new messages.

---

## Dependency Graph

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
- **Settings** → (no dependencies)
- **ContactMessages** → Users (optional)

---

## Tables that can be inserted independently (no dependencies):
- Roles
- BloodTypes
- Locations
- NewsCategories
- Settings

## Notes:
- Always insert parent/master tables before child/dependent tables to avoid foreign key constraint errors.
- Some tables (e.g., ContactMessages, HealthScreening) have optional foreign keys and can be inserted with nulls for those fields if needed.
- Adjust the order if your schema has additional dependencies not reflected here. 
=======
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
>>>>>>> develop
