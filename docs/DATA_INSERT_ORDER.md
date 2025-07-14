# Data Insert Order

When setting up the database or seeding initial data, it is important to insert records in the correct order to satisfy foreign key constraints and dependencies between tables. Use the following order:

1. **Roles**
2. **BloodTypes**
3. **BloodCompatibility**
4. **Users**
5. **Locations**
6. **BloodDonationEvents**
7. **DonationRegistrations**
8. **HealthScreening**
9. **DonationHistory**
10. **NewsCategories**
11. **News**
12. **Notifications**
13. **Settings**
14. **ContactMessages**

> **Note:**
> - This order helps prevent issues with foreign key constraints and ensures data integrity.
> - Adjust the order if your schema has additional dependencies not reflected here. 