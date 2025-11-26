// Events Page JavaScript
// =====================

document.addEventListener('DOMContentLoaded', function() {
    let currentPage = 1;
    let isLoading = false;
    let hasMoreEvents = true;
    let currentEventType = 'current';
    
    const currentEventsContainer = document.getElementById('currentEventsContainer');
    const pastEventsContainer = document.getElementById('pastEventsContainer');
    const loadingIndicator = document.getElementById('loadingIndicator');
    const noMoreEvents = document.getElementById('noMoreEvents');
    
    // Auto-submit form when filters change
    const filterForm = document.getElementById('filterForm');
    if (filterForm) {
        const filterInputs = filterForm.querySelectorAll('select, input[type="date"]');
        
        filterInputs.forEach(input => {
            input.addEventListener('change', function() {
                // Show loading before submit
                if (typeof showLoading === 'function') {
                    showLoading('Đang tìm kiếm...', 'Vui lòng chờ trong giây lát', 2000);
                }
                filterForm.submit();
            });
        });
        
        // Handle form submit
        filterForm.addEventListener('submit', function(e) {
            // Form will submit normally without toast
        });
    }
    
    // Clear filters function
    window.clearFilters = function() {
        if (!filterForm) return;
        
        // Show confirmation dialog
        if (typeof showConfirm === 'function') {
            showConfirm(
                'Xóa bộ lọc',
                'Bạn có chắc chắn muốn xóa tất cả bộ lọc và hiển thị lại tất cả sự kiện?',
                'Xóa bộ lọc',
                'Hủy bỏ',
                'question'
            ).then((result) => {
                if (result.isConfirmed) {
                    const inputs = filterForm.querySelectorAll('input, select');
                    inputs.forEach(input => {
                        if (input.type === 'text' || input.type === 'date') {
                            input.value = '';
                        } else if (input.tagName === 'SELECT') {
                            input.selectedIndex = 0;
                        }
                    });
                    filterForm.submit();
                }
            });
        } else {
            // Fallback if SweetAlert is not available
            const inputs = filterForm.querySelectorAll('input, select');
            inputs.forEach(input => {
                if (input.type === 'text' || input.type === 'date') {
                    input.value = '';
                } else if (input.tagName === 'SELECT') {
                    input.selectedIndex = 0;
                }
            });
            filterForm.submit();
        }
    };
    
    // Infinite scroll functionality
    function checkScroll() {
        if (isLoading || !hasMoreEvents) return;
        
        const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
        const windowHeight = window.innerHeight;
        const documentHeight = document.documentElement.scrollHeight;
        
        // Load more when user is 200px from bottom
        if (scrollTop + windowHeight >= documentHeight - 200) {
            loadMoreEvents();
        }
    }
    
    function loadMoreEvents() {
        if (isLoading || !hasMoreEvents) return;
        
        isLoading = true;
        currentPage++;
        
        if (loadingIndicator) {
            loadingIndicator.style.display = 'block';
        }
        
        // Determine which event type to load more of
        const currentEventsCount = currentEventsContainer ? currentEventsContainer.children.length : 0;
        const pastEventsCount = pastEventsContainer ? pastEventsContainer.children.length : 0;
        
        if (currentEventsCount < pastEventsCount || !pastEventsContainer) {
            currentEventType = 'current';
        } else {
            currentEventType = 'past';
        }
        
        fetch(`/Events/LoadMoreEvents?page=${currentPage}&pageSize=6&eventType=${currentEventType}`, {
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            }
        })
        .then(response => response.text())
        .then(html => {
            if (loadingIndicator) {
                loadingIndicator.style.display = 'none';
            }
            
            if (html.trim() === '' || html.includes('no-events')) {
                hasMoreEvents = false;
                if (noMoreEvents) {
                    noMoreEvents.style.display = 'block';
                }
                // Show no more events notification
                if (typeof showInfo === 'function') {
                    showInfo('Đã hiển thị tất cả', 'Không còn sự kiện nào để tải thêm');
                }
            } else {
                const targetContainer = currentEventType === 'current' ? currentEventsContainer : pastEventsContainer;
                if (targetContainer) {
                    targetContainer.insertAdjacentHTML('beforeend', html);
                }
            }
        })
        .catch(error => {
            console.error('Error loading more events:', error);
            if (typeof showError === 'function') {
                showError('Có lỗi xảy ra khi tải thêm sự kiện');
            }
        })
        .finally(() => {
            isLoading = false;
        });
    }
    
    // Event listeners
    window.addEventListener('scroll', checkScroll);
    
    // Event registration
    document.addEventListener('click', function(e) {
        if (e.target.classList.contains('btn-register') || e.target.closest('.btn-register')) {
            e.preventDefault();
            const button = e.target.classList.contains('btn-register') ? e.target : e.target.closest('.btn-register');
            const eventId = button.dataset.eventId;
            if (eventId) {
                registerForEvent(eventId);
            }
        }
    });
    
    function registerForEvent(eventId) {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value || '';
        
        fetch('/Events/Register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify({ eventId: eventId })
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                if (typeof showSuccess === 'function') {
                    showSuccess(data.message);
                }
            } else {
                if (typeof showWarning === 'function') {
                    showWarning('Thông báo', data.message);
                }
            }
        })
        .catch(error => {
            console.error('Error registering for event:', error);
            if (typeof showError === 'function') {
                showError('Có lỗi xảy ra khi đăng ký sự kiện');
            }
        });
    }
    
    // Load provinces for location filter
    function loadProvinces() {
        const locationFilter = document.getElementById('locationFilter');
        if (!locationFilter) return;
        
        fetch('https://provinces.open-api.vn/api/p/')
            .then(response => response.json())
            .then(provinces => {
                provinces.forEach(province => {
                    const option = new Option(province.name, province.name);
                    locationFilter.add(option);
                });
                
                // Set selected value if exists
                const currentLocation = locationFilter.dataset.currentLocation;
                if (currentLocation) {
                    locationFilter.value = currentLocation;
                }
            })
            .catch(error => {
                console.error('Error loading provinces:', error);
            });
    }
    
    // Initialize provinces loading
    loadProvinces();
    
    // Show search results notification
    function showSearchResults() {
        // Check if there are any active filters
        const searchTerm = document.querySelector('input[name="searchTerm"]')?.value;
        const location = document.querySelector('select[name="location"]')?.value;
        const fromDate = document.querySelector('input[name="fromDate"]')?.value;
        const toDate = document.querySelector('input[name="toDate"]')?.value;
        const bloodType = document.querySelector('select[name="bloodType"]')?.value;
        
        const hasFilters = searchTerm || location || fromDate || toDate || bloodType;
        
        if (hasFilters) {
            // Count total events
            const currentEventsCount = currentEventsContainer ? currentEventsContainer.children.length : 0;
            const pastEventsCount = pastEventsContainer ? pastEventsContainer.children.length : 0;
            const totalEvents = currentEventsCount + pastEventsCount;
            
            // Build filter description
            let filterDesc = [];
            if (searchTerm) filterDesc.push(`từ khóa "${searchTerm}"`);
            if (location) filterDesc.push(`địa điểm "${location}"`);
            if (fromDate && toDate) filterDesc.push(`từ ${fromDate} đến ${toDate}`);
            else if (fromDate) filterDesc.push(`từ ${fromDate}`);
            else if (toDate) filterDesc.push(`đến ${toDate}`);
            if (bloodType) filterDesc.push(`nhóm máu ${bloodType}`);
            
            const filterText = filterDesc.join(', ');
            
            // Show appropriate notification
            if (totalEvents === 0) {
                if (typeof showWarning === 'function') {
                    showWarning(
                        'Không tìm thấy sự kiện',
                        `Không có sự kiện nào phù hợp với bộ lọc: ${filterText}`
                    );
                }
            } else {
                if (typeof showSuccess === 'function') {
                    showSuccess(
                        'Tìm kiếm thành công',
                        `Tìm thấy ${totalEvents} sự kiện với bộ lọc: ${filterText}`,
                        4000
                    );
                }
            }
        }
    }
    
    // Show search results after page load
    setTimeout(showSearchResults, 500);
}); 