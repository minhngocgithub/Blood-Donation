# CSS Structure Documentation

## Overview
CSS đã được tách thành các file riêng biệt để dễ quản lý và bảo trì. Mỗi file có trách nhiệm cụ thể và được tổ chức theo chức năng.

## File Structure

```
wwwroot/css/
├── site.css              # Global styles và utility classes
├── components.css        # Common components (navbar, footer, buttons, cards)
├── main-sections.css     # Main page sections (hero, features, events, stats)
├── home-pages.css        # Specific styles cho Home pages (About, FAQ, Guide, Privacy, Terms)
├── index-page.css        # Specific styles cho trang Index (Home page)
├── auth.css             # Authentication pages styles
├── profile.css          # Profile page styles
└── sweetalert-custom.css # SweetAlert customizations
```

## File Descriptions

### 1. `site.css` - Global Styles
**Chức năng**: Chứa các style cơ bản và global cho toàn bộ website
**Nội dung**:
- CSS Variables (color scheme, fonts)
- Typography (h1-h6, p, a, lists)
- Layout utilities (container, row, col)
- Utility classes (text-center, bg-primary, m-1, p-1, etc.)
- Responsive breakpoints
- Print styles
- Accessibility styles

### 2. `components.css` - Common Components
**Chức năng**: Style cho các component được sử dụng chung
**Nội dung**:
- Navbar styles
- Footer styles
- Button styles (primary, danger, outline variants)
- Card styles
- Alert styles
- Form styles
- Dropdown styles
- Modal styles
- Breadcrumb styles
- Pagination styles
- Progress bar styles
- Tooltip styles
- Back to top button

### 3. `main-sections.css` - Main Page Sections
**Chức năng**: Style cho các section chính của trang chủ
**Nội dung**:
- Hero section
- Stats section
- Features section
- Events section
- CTA section
- News section
- Testimonials section
- Contact section
- Background variations
- Section spacing utilities

### 4. `home-pages.css` - Home Pages Specific
**Chức năng**: Style riêng cho các trang Home (About, FAQ, Guide, Privacy, Terms)
**Nội dung**:
- About page styles (hero, stats, team, values)
- FAQ page styles (accordion customization)
- Guide page styles (step cards, numbers)
- Privacy & Terms page styles (legal sections)
- Page-specific overrides
- Call to action sections
- Statistics display
- Team section
- Values section

### 5. `index-page.css` - Index Page Specific
**Chức năng**: Style riêng cho trang chủ (Index)
**Nội dung**:
- Hero section optimization
- Stats section positioning
- Responsive design cho trang chủ
- Mobile navigation optimization
- Button layout improvements

### 6. `auth.css` - Authentication Pages
**Chức năng**: Style cho trang đăng nhập, đăng ký
**Nội dung**:
- Login form styles
- Register form styles
- Password reset styles
- Authentication layout

### 7. `profile.css` - Profile Page
**Chức năng**: Style cho trang thông tin cá nhân
**Nội dung**:
- Profile layout
- User information display
- Edit forms
- Avatar styles

### 8. `sweetalert-custom.css` - SweetAlert Customizations
**Chức năng**: Tùy chỉnh SweetAlert notifications
**Nội dung**:
- Custom alert styles
- Animation overrides
- Color scheme integration

## CSS Variables

### Color Scheme
```css
:root {
    --primary-color: #dc3545;    /* Red - main brand color */
    --secondary-color: #6c757d;  /* Gray - secondary text */
    --success-color: #28a745;    /* Green - success states */
    --info-color: #17a2b8;       /* Blue - info states */
    --warning-color: #ffc107;    /* Yellow - warning states */
    --danger-color: #dc3545;     /* Red - danger states */
    --light-color: #f8f9fa;      /* Light gray - backgrounds */
    --dark-color: #343a40;       /* Dark gray - text */
    --font-family: "Inter", sans-serif;
}
```

## Responsive Design

### Breakpoints
- **Mobile**: `max-width: 576px`
- **Tablet**: `max-width: 768px`
- **Desktop**: `max-width: 992px`
- **Large Desktop**: `max-width: 1200px`

### Mobile-First Approach
Tất cả styles được viết theo mobile-first approach, sau đó sử dụng media queries để tối ưu cho màn hình lớn hơn.

## Utility Classes

### Spacing
- `m-0` to `m-5` (margin)
- `mt-0` to `mt-5` (margin-top)
- `mb-0` to `mb-5` (margin-bottom)
- `p-0` to `p-5` (padding)
- `pt-0` to `pt-5` (padding-top)
- `pb-0` to `pb-5` (padding-bottom)

### Text Alignment
- `text-center`
- `text-left`
- `text-right`

### Colors
- `text-primary`, `bg-primary`
- `text-secondary`, `bg-secondary`
- `text-success`, `bg-success`
- `text-danger`, `bg-danger`
- `text-warning`, `bg-warning`
- `text-info`, `bg-info`

### Display
- `d-none`, `d-block`, `d-inline`, `d-flex`
- `d-inline-block`, `d-inline-flex`

### Flexbox
- `flex-row`, `flex-column`
- `justify-content-start`, `justify-content-center`, `justify-content-between`
- `align-items-start`, `align-items-center`, `align-items-end`

## Best Practices

### 1. CSS Organization
- Sử dụng CSS variables cho colors và fonts
- Group related styles together
- Comment rõ ràng cho từng section
- Sử dụng consistent naming convention

### 2. Performance
- Minimize CSS file size
- Use efficient selectors
- Avoid deep nesting
- Use CSS Grid và Flexbox thay vì floats

### 3. Maintainability
- Tách CSS theo chức năng
- Sử dụng semantic class names
- Avoid inline styles
- Keep specificity low

### 4. Accessibility
- Ensure sufficient color contrast
- Provide focus indicators
- Support keyboard navigation
- Use semantic HTML

## Adding New Styles

### 1. Global Styles
Thêm vào `site.css` nếu là utility classes hoặc global styles.

### 2. Component Styles
Thêm vào `components.css` nếu là component được sử dụng chung.

### 3. Page-Specific Styles
Thêm vào file CSS tương ứng:
- Home pages → `home-pages.css`
- Auth pages → `auth.css`
- Profile page → `profile.css`

### 4. Section Styles
Thêm vào `main-sections.css` nếu là section chính của trang.

## Troubleshooting

### Common Issues

1. **Styles not applying**
   - Check if CSS file is included in layout
   - Verify CSS specificity
   - Check for typos in class names

2. **Responsive issues**
   - Ensure mobile-first approach
   - Check breakpoint values
   - Test on actual devices

3. **Performance issues**
   - Minimize CSS file size
   - Remove unused styles
   - Optimize selectors

### Debugging Tips

1. **Browser DevTools**
   - Use element inspector
   - Check computed styles
   - Verify CSS loading

2. **CSS Validation**
   - Validate CSS syntax
   - Check for missing semicolons
   - Verify property values

3. **Cross-browser Testing**
   - Test on different browsers
   - Check vendor prefixes
   - Verify fallbacks

## Future Enhancements

### Planned Improvements
1. **CSS Modules**: Implement CSS modules for better scoping
2. **PostCSS**: Add PostCSS for advanced features
3. **CSS-in-JS**: Consider CSS-in-JS for dynamic styles
4. **Design System**: Create comprehensive design system
5. **Dark Mode**: Add dark mode support
6. **Animation Library**: Add animation library integration

### Performance Optimizations
1. **Critical CSS**: Extract critical CSS for above-the-fold content
2. **CSS Minification**: Implement CSS minification
3. **CSS Compression**: Enable gzip compression
4. **CSS Caching**: Implement proper caching strategies
5. **CSS Loading**: Optimize CSS loading order

## Support

For CSS-related issues:
1. Check this documentation first
2. Review browser DevTools
3. Validate CSS syntax
4. Test on different devices
5. Check for conflicts with Bootstrap 