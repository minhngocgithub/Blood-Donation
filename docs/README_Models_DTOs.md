# DTOs (Data Transfer Objects) Documentation

This directory contains all the Data Transfer Objects (DTOs) for the Blood Donation Website application. DTOs are used to transfer data between different layers of the application, providing a clean separation between the data layer and the presentation layer.

## Entity DTOs

### Core Entities

1. **UserDto.cs**
   - `UserDto` - Complete user information with navigation properties
   - `UserCreateDto` - For creating new users
   - `UserUpdateDto` - For updating existing users

2. **BloodTypeDto.cs**
   - `BloodTypeDto` - Blood type information
   - `BloodTypeCreateDto` - For creating new blood types
   - `BloodTypeUpdateDto` - For updating blood types

3. **RoleDto.cs**
   - `RoleDto` - Role information
   - `RoleCreateDto` - For creating new roles
   - `RoleUpdateDto` - For updating roles

4. **LocationDto.cs**
   - `LocationDto` - Location information
   - `LocationCreateDto` - For creating new locations
   - `LocationUpdateDto` - For updating locations

### Event Management

5. **BloodDonationEventDto.cs**
   - `BloodDonationEventDto` - Complete event information with navigation properties
   - `BloodDonationEventCreateDto` - For creating new events
   - `BloodDonationEventUpdateDto` - For updating events

6. **DonationRegistrationDto.cs**
   - `DonationRegistrationDto` - Registration information with user and event details
   - `DonationRegistrationCreateDto` - For creating new registrations
   - `DonationRegistrationUpdateDto` - For updating registrations

7. **DonationHistoryDto.cs**
   - `DonationHistoryDto` - Donation history with complete details
   - `DonationHistoryCreateDto` - For creating new donation records
   - `DonationHistoryUpdateDto` - For updating donation records

8. **HealthScreeningDto.cs**
   - `HealthScreeningDto` - Health screening information
   - `HealthScreeningCreateDto` - For creating new screenings
   - `HealthScreeningUpdateDto` - For updating screenings

### Content Management

9. **NewsDto.cs**
   - `NewsDto` - News article information
   - `NewsCreateDto` - For creating new articles
   - `NewsUpdateDto` - For updating articles

10. **NewsCategoryDto.cs**
    - `NewsCategoryDto` - News category information
    - `NewsCategoryCreateDto` - For creating new categories
    - `NewsCategoryUpdateDto` - For updating categories

### Communication

11. **NotificationDto.cs**
    - `NotificationDto` - Notification information
    - `NotificationCreateDto` - For creating new notifications
    - `NotificationUpdateDto` - For updating notifications

12. **ContactMessageDto.cs**
    - `ContactMessageDto` - Contact message information
    - `ContactMessageCreateDto` - For creating new messages
    - `ContactMessageUpdateDto` - For updating message status

### System

13. **BloodCompatibilityDto.cs**
    - `BloodCompatibilityDto` - Blood compatibility rules
    - `BloodCompatibilityCreateDto` - For creating compatibility rules
    - `BloodCompatibilityUpdateDto` - For updating compatibility rules

14. **SettingDto.cs**
    - `SettingDto` - System settings
    - `SettingCreateDto` - For creating new settings
    - `SettingUpdateDto` - For updating settings

## Specialized DTOs

### Statistics and Reporting

15. **StatisticsDto.cs**
    - `DashboardStatisticsDto` - Dashboard overview statistics
    - `BloodTypeStatisticsDto` - Blood type specific statistics
    - `EventStatisticsDto` - Event performance statistics
    - `UserDonationHistoryDto` - User donation summary

### Search and Filtering

16. **SearchDto.cs**
    - `SearchParametersDto` - Base search parameters
    - `EventSearchDto` - Event-specific search
    - `UserSearchDto` - User-specific search
    - `DonationSearchDto` - Donation-specific search
    - `NewsSearchDto` - News-specific search

### API Responses

17. **ResponseDto.cs**
    - `ApiResponseDto<T>` - Generic API response wrapper
    - `PagedResponseDto<T>` - Paginated response wrapper
    - `LoginResponseDto` - Authentication response
    - `EmailResponseDto` - Email operation response

## Usage Guidelines

### Naming Convention
- **Dto** - Complete entity information (for reading)
- **CreateDto** - For creating new entities (excludes auto-generated fields)
- **UpdateDto** - For updating existing entities (excludes immutable fields)

### Navigation Properties
DTOs include navigation properties (e.g., `UserName`, `BloodTypeName`) to provide related entity information without requiring additional queries.

### Validation
Create and Update DTOs should be used with validation attributes in controllers or services to ensure data integrity.

### Security
- Never include sensitive information like passwords in DTOs
- Use specific DTOs for different user roles (admin vs regular user)
- Validate all input data before processing

## Example Usage

```csharp
// In a controller
[HttpPost]
public async Task<ActionResult<ApiResponseDto<UserDto>>> CreateUser([FromBody] UserCreateDto createDto)
{
    // Validation and processing
    var user = await _userService.CreateUserAsync(createDto);
    var userDto = _mapper.Map<UserDto>(user);
    
    return Ok(new ApiResponseDto<UserDto>
    {
        Success = true,
        Message = "User created successfully",
        Data = userDto
    });
}
```

## Mapping

Consider using AutoMapper or similar mapping libraries to convert between entities and DTOs:

```csharp
// AutoMapper configuration
CreateMap<User, UserDto>()
    .ForMember(dest => dest.BloodTypeName, opt => opt.MapFrom(src => src.BloodType.BloodTypeName))
    .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

CreateMap<UserCreateDto, User>();
CreateMap<UserUpdateDto, User>();
``` 