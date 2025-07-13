# Home Pages Structure

## Overview
This document describes the new Home pages structure created for the Blood Donation website. The pages are organized in a subdirectory structure for better organization and maintainability.

## Directory Structure

```
Controllers/
├── HomeController.cs (Original - now redirects to subdirectory)
└── Home/
    └── HomeController.cs (New - handles all home page actions)

Views/
└── Home/
    ├── Index.cshtml (Existing)
    ├── About.cshtml (New)
    ├── FAQ.cshtml (New)
    ├── Guide.cshtml (New)
    ├── Privacy.cshtml (New)
    ├── Terms.cshtml (New)
    └── SweetAlertDemo.cshtml (Existing)
```

## New Pages

### 1. About Page (`/Home/About`)
- **Purpose**: Information about BloodLife organization
- **Features**:
  - Mission and Vision statements
  - Core values with icons
  - Company story and statistics
  - Team information
  - Call-to-action buttons

### 2. FAQ Page (`/Home/FAQ`)
- **Purpose**: Frequently Asked Questions about blood donation
- **Features**:
  - Bootstrap accordion for questions
  - 8 common questions with detailed answers
  - Responsive design
  - Easy to expand with more questions

### 3. Guide Page (`/Home/Guide`)
- **Purpose**: Step-by-step guide for blood donation process
- **Features**:
  - 5-step donation process
  - Visual step indicators
  - Important notes before and after donation
  - Call-to-action for registration

### 4. Privacy Page (`/Home/Privacy`)
- **Purpose**: Privacy policy and data protection information
- **Features**:
  - Comprehensive privacy policy
  - User rights and data usage
  - Contact information
  - Cookie policy

### 5. Terms Page (`/Home/Terms`)
- **Purpose**: Terms of service and usage conditions
- **Features**:
  - Service terms and conditions
  - User responsibilities
  - Legal information
  - Contact details

## Navigation Updates

### Main Navigation
Added new menu items:
- **Hướng dẫn** (Guide) - Links to `/Home/Guide`
- **FAQ** - Links to `/Home/FAQ`

### Footer Links
Updated footer support section with proper links:
- Hướng dẫn hiến máu → `/Home/Guide`
- Chính sách bảo mật → `/Home/Privacy`
- Điều khoản sử dụng → `/Home/Terms`
- Câu hỏi thường gặp → `/Home/FAQ`

## Styling

### CSS Enhancements
Added new CSS classes in `wwwroot/css/site.css`:

#### About Page Styles
- `.about-hero` - Hero section styling
- `.about-stat-card` - Statistics card styling
- `.about-team-card` - Team member card styling

#### FAQ Page Styles
- `.faq-accordion` - Accordion styling
- Custom accordion button and body styles

#### Guide Page Styles
- `.guide-step-card` - Step card styling
- `.guide-step-number` - Step number styling

#### Legal Pages Styles
- `.legal-section` - Section styling for Privacy/Terms
- `.legal-contact-info` - Contact information styling

### Responsive Design
All pages are fully responsive with:
- Mobile-first approach
- Bootstrap grid system
- Custom responsive breakpoints
- Touch-friendly interactions

## Features

### Bootstrap Components Used
- **Cards**: For content sections
- **Accordion**: For FAQ page
- **Grid System**: For responsive layouts
- **Icons**: Font Awesome icons throughout
- **Buttons**: Call-to-action buttons
- **Alerts**: Information and warning messages

### Interactive Elements
- Hover effects on cards and buttons
- Smooth transitions and animations
- Collapsible FAQ sections
- Responsive navigation

## Content

### Vietnamese Language
All content is in Vietnamese, including:
- Page titles and headings
- Navigation labels
- Footer links
- Button text
- Form labels

### Medical Information
Content includes accurate medical information about:
- Blood donation process
- Health requirements
- Safety guidelines
- Post-donation care

## Technical Implementation

### Controller Structure
```csharp
namespace Blood_Donation_Website.Controllers.Home
{
    public class HomeController : Controller
    {
        // Actions for all home pages
        public IActionResult Index() { }
        public IActionResult About() { }
        public IActionResult FAQ() { }
        public IActionResult Guide() { }
        public IActionResult Privacy() { }
        public IActionResult Terms() { }
        public IActionResult SweetAlertDemo() { }
    }
}
```

### Routing
- Default route: `{controller=Home}/{action=Index}/{id?}`
- All pages accessible via `/Home/{ActionName}`
- Original HomeController redirects to new structure

### Dependencies
- Bootstrap 5.x
- Font Awesome 6.x
- jQuery (for Bootstrap components)
- Custom CSS for styling

## Maintenance

### Adding New Questions to FAQ
1. Add new accordion item in `Views/Home/FAQ.cshtml`
2. Follow existing structure with proper IDs
3. Test accordion functionality

### Updating Content
1. Edit respective `.cshtml` files
2. Maintain HTML structure and classes
3. Test responsive behavior

### Styling Changes
1. Modify `wwwroot/css/site.css`
2. Use existing CSS variables for consistency
3. Test across different screen sizes

## Future Enhancements

### Potential Improvements
- Add search functionality to FAQ
- Implement breadcrumb navigation
- Add print styles for legal pages
- Include more interactive elements
- Add analytics tracking
- Implement content management system

### SEO Considerations
- Proper meta tags for each page
- Semantic HTML structure
- Alt text for images
- Structured data markup
- Sitemap generation

## Testing

### Manual Testing Checklist
- [ ] All pages load correctly
- [ ] Navigation links work properly
- [ ] Responsive design on mobile devices
- [ ] Accordion functionality on FAQ page
- [ ] All buttons and links are functional
- [ ] Content is properly formatted
- [ ] Images and icons display correctly

### Browser Compatibility
- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)
- Mobile browsers (iOS Safari, Chrome Mobile)

## Support

For issues or questions regarding the Home pages:
1. Check the CSS classes and structure
2. Verify routing configuration
3. Test in different browsers
4. Review console for JavaScript errors
5. Check responsive behavior on different devices 