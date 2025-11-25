// Registration Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Add fade-in animation to registration cards
    const registrationCards = document.querySelectorAll('.card');
    registrationCards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';
        
        setTimeout(() => {
            card.style.transition = 'all 0.5s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, index * 100);
    });

    // Handle cancel registration form submission
    const cancelForm = document.getElementById('cancelForm');
    if (cancelForm) {
        cancelForm.addEventListener('submit', function(e) {
            const reason = document.getElementById('cancelReason').value;
            const eventName = document.getElementById('eventName').textContent;
            
            // Show confirmation dialog
            if (!confirm(`Bạn có chắc chắn muốn hủy đăng ký cho sự kiện "${eventName}"?`)) {
                e.preventDefault();
                return false;
            }
            
            // Show loading state
            const submitBtn = this.querySelector('button[type="submit"]');
            const originalText = submitBtn.innerHTML;
            submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Đang xử lý...';
            submitBtn.disabled = true;
            
            // Re-enable button after a delay (in case of error)
            setTimeout(() => {
                submitBtn.innerHTML = originalText;
                submitBtn.disabled = false;
            }, 5000);
        });
    }

    // Handle status badge hover effects
    const statusBadges = document.querySelectorAll('.badge');
    statusBadges.forEach(badge => {
        badge.addEventListener('mouseenter', function() {
            this.style.transform = 'scale(1.1)';
            this.style.transition = 'transform 0.2s ease';
        });
        
        badge.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });

    // Handle card hover effects
    const cards = document.querySelectorAll('.card');
    cards.forEach(card => {
        card.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-5px)';
            this.style.boxShadow = '0 10px 25px rgba(0, 0, 0, 0.15)';
        });
        
        card.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0)';
            this.style.boxShadow = '0 0.125rem 0.25rem rgba(0, 0, 0, 0.075)';
        });
    });

    // Handle search functionality (if implemented)
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        let searchTimeout;
        searchInput.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                performSearch(this.value);
            }, 300);
        });
    }

    // Handle filter functionality (if implemented)
    const statusFilter = document.getElementById('statusFilter');
    if (statusFilter) {
        statusFilter.addEventListener('change', function() {
            filterRegistrations(this.value);
        });
    }

    // Handle sort functionality (if implemented)
    const sortSelect = document.getElementById('sortSelect');
    if (sortSelect) {
        sortSelect.addEventListener('change', function() {
            sortRegistrations(this.value);
        });
    }

    // Add loading states to buttons
    const actionButtons = document.querySelectorAll('.btn');
    actionButtons.forEach(button => {
        button.addEventListener('click', function() {
            if (this.type === 'submit' || this.classList.contains('action-btn')) {
                const originalText = this.innerHTML;
                this.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Đang xử lý...';
                this.disabled = true;
                
                // Re-enable after a delay
                setTimeout(() => {
                    this.innerHTML = originalText;
                    this.disabled = false;
                }, 3000);
            }
        });
    });

    // Handle responsive behavior
    function handleResponsive() {
        const cards = document.querySelectorAll('.col-lg-6, .col-xl-4');
        if (window.innerWidth < 768) {
            cards.forEach(card => {
                card.classList.remove('col-lg-6', 'col-xl-4');
                card.classList.add('col-12');
            });
        } else if (window.innerWidth < 1200) {
            cards.forEach(card => {
                card.classList.remove('col-xl-4');
                card.classList.add('col-lg-6');
            });
        }
    }

    // Call on load and resize
    handleResponsive();
    window.addEventListener('resize', handleResponsive);

    // Add smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                target.scrollIntoView({
                    behavior: 'smooth',
                    block: 'start'
                });
            }
        });
    });

    // Handle empty state animation
    const emptyState = document.querySelector('.text-center.py-5');
    if (emptyState) {
        emptyState.style.opacity = '0';
        setTimeout(() => {
            emptyState.style.transition = 'opacity 1s ease';
            emptyState.style.opacity = '1';
        }, 500);
    }

    // Add keyboard shortcuts
    document.addEventListener('keydown', function(e) {
        // Ctrl/Cmd + K for search
        if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
            e.preventDefault();
            const searchInput = document.getElementById('searchInput');
            if (searchInput) {
                searchInput.focus();
            }
        }
        
        // Escape to close modals
        if (e.key === 'Escape') {
            const modals = document.querySelectorAll('.modal.show');
            modals.forEach(modal => {
                const modalInstance = bootstrap.Modal.getInstance(modal);
                if (modalInstance) {
                    modalInstance.hide();
                }
            });
        }
    });
});

// Search functionality
function performSearch(searchTerm) {
    const cards = document.querySelectorAll('.col-lg-6, .col-xl-4, .col-12');
    const searchLower = searchTerm.toLowerCase();
    
    cards.forEach(card => {
        const text = card.textContent.toLowerCase();
        const isVisible = text.includes(searchLower);
        
        if (isVisible) {
            card.style.display = 'block';
            card.style.animation = 'fadeIn 0.3s ease';
        } else {
            card.style.display = 'none';
        }
    });
    
    // Show/hide empty state
    const visibleCards = document.querySelectorAll('.col-lg-6:not([style*="display: none"]), .col-xl-4:not([style*="display: none"]), .col-12:not([style*="display: none"])');
    const emptyState = document.querySelector('.text-center.py-5');
    
    if (visibleCards.length === 0 && emptyState) {
        emptyState.style.display = 'block';
    } else if (emptyState) {
        emptyState.style.display = 'none';
    }
}

// Filter functionality
function filterRegistrations(status) {
    const cards = document.querySelectorAll('.col-lg-6, .col-xl-4, .col-12');
    
    cards.forEach(card => {
        const statusBadge = card.querySelector('.badge');
        if (status === 'all' || !statusBadge) {
            card.style.display = 'block';
        } else {
            const cardStatus = statusBadge.textContent.toLowerCase();
            const filterStatus = status.toLowerCase();
            
            if (cardStatus.includes(filterStatus)) {
                card.style.display = 'block';
            } else {
                card.style.display = 'none';
            }
        }
    });
}

// Sort functionality
function sortRegistrations(sortBy) {
    const container = document.querySelector('.row');
    const cards = Array.from(container.querySelectorAll('.col-lg-6, .col-xl-4, .col-12'));
    
    cards.sort((a, b) => {
        let aValue, bValue;
        
        switch (sortBy) {
            case 'date':
                const aDate = a.querySelector('[data-date]')?.getAttribute('data-date');
                const bDate = b.querySelector('[data-date]')?.getAttribute('data-date');
                aValue = new Date(aDate || 0);
                bValue = new Date(bDate || 0);
                break;
            case 'status':
                const aStatus = a.querySelector('.badge')?.textContent || '';
                const bStatus = b.querySelector('.badge')?.textContent || '';
                aValue = aStatus;
                bValue = bStatus;
                break;
            case 'event':
                const aEvent = a.querySelector('.card-title')?.textContent || '';
                const bEvent = b.querySelector('.card-title')?.textContent || '';
                aValue = aEvent;
                bValue = bEvent;
                break;
            default:
                return 0;
        }
        
        if (aValue < bValue) return -1;
        if (aValue > bValue) return 1;
        return 0;
    });
    
    // Re-append sorted cards
    cards.forEach(card => {
        container.appendChild(card);
    });
}

// Export functionality (if needed)
function exportRegistrations(format) {
    // Implementation for exporting registration data
    console.log(`Exporting registrations in ${format} format`);
    
    // Show loading state
    Swal.fire({
        title: 'Đang xuất dữ liệu...',
        text: 'Vui lòng chờ trong giây lát',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    
    // Simulate export process
    setTimeout(() => {
        Swal.fire({
            icon: 'success',
            title: 'Xuất dữ liệu thành công!',
            text: `Dữ liệu đã được xuất ra file ${format.toUpperCase()}`,
            confirmButtonText: 'OK'
        });
    }, 2000);
}

// Print functionality
function printRegistrations() {
    window.print();
}

// Share functionality
function shareRegistration(registrationId) {
    if (navigator.share) {
        navigator.share({
            title: 'Đăng ký hiến máu của tôi',
            text: 'Xem chi tiết đăng ký hiến máu',
            url: window.location.origin + '/DonationRegistration/Details/' + registrationId
        });
    } else {
        // Fallback for browsers that don't support Web Share API
        const url = window.location.origin + '/DonationRegistration/Details/' + registrationId;
        navigator.clipboard.writeText(url).then(() => {
            Swal.fire({
                icon: 'success',
                title: 'Đã sao chép link!',
                text: 'Link đã được sao chép vào clipboard',
                confirmButtonText: 'OK'
            });
        });
    }
} 