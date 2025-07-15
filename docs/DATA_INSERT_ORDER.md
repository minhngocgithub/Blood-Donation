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