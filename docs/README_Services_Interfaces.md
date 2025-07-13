# Service Interfaces Documentation

This directory contains all the service interfaces for the Blood Donation Website application. These interfaces define the contract for business logic operations and provide a clean separation between the presentation layer and the business logic layer.

## Core Service Interfaces

### 1. **IUserService.cs**
Comprehensive user management operations including:
- **CRUD Operations**: Create, read, update, delete users
- **Status Management**: Activate, deactivate, lock, unlock users
- **Role Management**: Assign/remove roles, check role membership
- **Blood Type Operations**: Update and retrieve user blood type information
- **Statistics**: Donation history, eligibility tracking
- **Search & Filtering**: Advanced user search capabilities
- **Validation**: Email/username existence, donation eligibility

### 2. **IBloodTypeService.cs**
Blood type management operations including:
- **CRUD Operations**: Manage blood type entities
- **Statistics**: Donation counts, volume tracking, user counts
- **Validation**: Existence checks, name validation
- **Search**: Blood type search functionality

### 3. **IRoleService.cs**
Role-based access control operations including:
- **CRUD Operations**: Manage role entities
- **Role Assignment**: Assign/remove roles from users
- **User Management**: Get users by role, check role membership
- **Validation**: Role existence and name validation
- **Statistics**: User counts by role

### 4. **ILocationService.cs**
Location management operations including:
- **CRUD Operations**: Manage donation locations
- **Status Management**: Activate/deactivate locations
- **Capacity Management**: Track and update location capacity
- **Event Management**: Get events by location
- **Statistics**: Event and donation counts by location

## Event Management Interfaces

### 5. **IBloodDonationEventService.cs**
Comprehensive event management operations including:
- **CRUD Operations**: Create, manage, and delete donation events
- **Status Management**: Activate, deactivate, cancel, complete events
- **Capacity Management**: Track available slots, manage donor limits
- **Scheduling**: Upcoming, past, and date-range event queries
- **Statistics**: Registration and donation counts
- **Validation**: Date/time validation, existence checks
- **Notifications**: Event reminders and updates

### 6. **IDonationRegistrationService.cs**
Registration management operations including:
- **CRUD Operations**: Manage event registrations
- **Status Management**: Approve, reject, cancel, check-in registrations
- **Queries**: User and event-specific registrations
- **Validation**: Eligibility checks, event capacity validation
- **Statistics**: Registration counts and trends
- **Notifications**: Confirmation and reminder notifications

### 7. **IDonationHistoryService.cs**
Donation tracking operations including:
- **CRUD Operations**: Manage donation records
- **Status Management**: Complete, cancel, issue certificates
- **Queries**: User, event, and blood type specific donations
- **Statistics**: Total donations, volumes, trends
- **Eligibility**: Next donation dates, eligibility checks
- **Reporting**: Monthly/yearly reports, charts
- **Certificates**: Generate and send donation certificates

### 8. **IHealthScreeningService.cs**
Health screening management operations including:
- **CRUD Operations**: Manage screening records
- **Queries**: User, event, and screener specific screenings
- **Status Management**: Approve, disqualify screenings
- **Validation**: Vital signs validation, health checks
- **Statistics**: Screening results, disqualification reasons
- **Reporting**: Monthly results, average vitals
- **Workflow**: Start, complete, review screenings

## Content Management Interfaces

### 9. **INewsService.cs**
News article management operations including:
- **CRUD Operations**: Create, manage, and delete news articles
- **Status Management**: Publish, unpublish articles
- **Queries**: Published, unpublished, category, author specific
- **Statistics**: View counts, article counts
- **View Tracking**: Increment and manage view counts
- **Categories**: Assign and manage article categories
- **Authors**: Assign and manage article authors
- **Publishing Workflow**: Review, approve, reject, schedule
- **SEO**: Generate slugs, summaries, meta data

### 10. **INewsCategoryService.cs**
News category management operations including:
- **CRUD Operations**: Manage news categories
- **Status Management**: Activate/deactivate categories
- **Statistics**: News counts by category
- **News Management**: Move articles between categories
- **Hierarchy**: Parent-child category relationships (future)

## Communication Interfaces

### 11. **INotificationService.cs**
Notification system operations including:
- **CRUD Operations**: Manage notification records
- **Queries**: User-specific, unread, type-specific notifications
- **Status Management**: Mark as read/unread
- **Types**: System, event, donation, news notifications
- **Statistics**: Notification counts and trends
- **Sending**: Send notifications to users or groups
- **Templates**: Welcome, reminder, confirmation notifications
- **Cleanup**: Delete old notifications
- **Preferences**: User notification preferences (future)

### 12. **IContactMessageService.cs**
Contact form management operations including:
- **CRUD Operations**: Manage contact messages
- **Status Management**: New, in-progress, resolved, closed
- **Queries**: Status, email, subject, date-range specific
- **Statistics**: Message counts by status and resolver
- **Resolution**: Assign resolvers, track resolution dates
- **Response**: Send responses and notifications
- **Categories**: Message categorization (future)
- **Priority**: Set and manage message priorities
- **Reporting**: Status charts, resolver charts
- **Templates**: Welcome, thank you, follow-up messages

## System Interfaces

### 13. **IBloodCompatibilityService.cs**
Blood compatibility management operations including:
- **CRUD Operations**: Manage compatibility rules
- **Queries**: Compatible donors and recipients
- **Validation**: Compatibility checks
- **Matrix**: Full compatibility matrix
- **Statistics**: Compatibility usage
- **Recommendations**: Recommended donors/recipients
- **Emergency**: Emergency compatibility checks
- **Education**: Compatibility guidelines and information

### 14. **ISettingService.cs**
System settings management operations including:
- **CRUD Operations**: Manage system settings
- **Value Operations**: Get, set, update setting values
- **Categories**: Application, email, donation, event, notification, security, system settings
- **Statistics**: Setting counts and categories
- **Initialization**: Default settings, backup/restore
- **Validation**: Setting value validation
- **Cache**: Settings cache management

## Specialized Interfaces

### 15. **IStatisticsService.cs**
Comprehensive statistics and reporting operations including:
- **Dashboard**: Overall system statistics
- **Blood Types**: Blood type specific statistics
- **Events**: Event performance statistics
- **Users**: Top donors, recent donors
- **Donations**: Donation trends and volumes
- **Registrations**: Registration trends and status
- **Health Screenings**: Screening results and vitals
- **Locations**: Location performance statistics
- **Time-based**: Monthly, yearly, weekly, daily statistics
- **Comparative**: Period and location comparisons
- **Growth**: Growth rates and trends
- **Efficiency**: Conversion rates and utilization
- **Predictive**: Future predictions
- **Export**: Excel, PDF, CSV exports
- **Real-time**: Current statistics
- **Custom**: Parameterized statistics

## Existing Interfaces

### 16. **IAccountService.cs** (Existing)
Account management operations including:
- **Authentication**: Register, login, logout
- **Password Management**: Forgot password, reset password, change password
- **User Management**: Get users, update profiles
- **Email Verification**: Email verification and validation
- **Role Management**: Check user roles
- **User Status**: Lock/unlock users

### 17. **IProfileService.cs** (Existing)
Profile management operations including:
- **Profile Operations**: Get and update user profiles
- **Blood Types**: Get available blood types

### 18. **IEmailService.cs** (Existing)
Email service operations including:
- **General Email**: Send emails with HTML support
- **Templates**: Welcome, password reset, email verification
- **Event Emails**: Event reminders and confirmations
- **Donation Emails**: Donation confirmations

## Usage Guidelines

### Dependency Injection
Register all services in your DI container:

```csharp
// In Program.cs or Startup.cs
services.AddScoped<IUserService, UserService>();
services.AddScoped<IBloodTypeService, BloodTypeService>();
services.AddScoped<IRoleService, RoleService>();
// ... register all other services
```

### Controller Usage
Use interfaces in controllers for clean separation:

```csharp
public class UserController : Controller
{
    private readonly IUserService _userService;
    
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllUsersAsync();
        return View(users);
    }
}
```

### Service Implementation
Implement interfaces in the `Services/Implementations` directory:

```csharp
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    
    public UserService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    // Implement all interface methods
}
```

## Best Practices

### 1. **Async Operations**
All methods are async to support scalability and responsiveness.

### 2. **DTO Usage**
Services work with DTOs for clean data transfer and validation.

### 3. **Error Handling**
Implement proper exception handling in service implementations.

### 4. **Validation**
Use validation attributes and business logic validation in services.

### 5. **Logging**
Implement logging for important operations and errors.

### 6. **Caching**
Consider caching for frequently accessed data (statistics, settings).

### 7. **Security**
Implement proper authorization checks in service methods.

### 8. **Testing**
Create unit tests for all service implementations.

## Interface Design Principles

### 1. **Single Responsibility**
Each interface focuses on a specific domain area.

### 2. **Comprehensive Coverage**
Interfaces cover all CRUD operations plus business-specific methods.

### 3. **Consistent Naming**
All methods follow consistent naming conventions.

### 4. **Async-First**
All operations are async for better performance.

### 5. **DTO-Based**
All data transfer uses DTOs for clean separation.

### 6. **Validation-Ready**
Interfaces support validation and business rule enforcement.

### 7. **Extensible**
Interfaces are designed to be easily extended for future requirements.

This comprehensive interface design provides a solid foundation for implementing the business logic layer of your blood donation application. 