window.LogoutUtils = {
    quickLogout: function() {
        const form = document.createElement('form');
        form.method = 'POST';
        form.action = '/account/quick-logout';
        
        const token = document.querySelector('input[name="__RequestVerificationToken"]');
        if (token) {
            const tokenInput = document.createElement('input');
            tokenInput.type = 'hidden';
            tokenInput.name = '__RequestVerificationToken';
            tokenInput.value = token.value;
            form.appendChild(tokenInput);
        }
        
        document.body.appendChild(form);
        
        this.clearClientSideData();
        
        form.submit();
    },
    
    clearClientSideData: function() {
        try {
            if (typeof(Storage) !== "undefined") {
                localStorage.removeItem('user');
                localStorage.removeItem('auth');
                localStorage.removeItem('session');
                localStorage.removeItem('bloodDonationUser');
                Object.keys(localStorage).forEach(key => {
                    if (key.toLowerCase().includes('auth') || 
                        key.toLowerCase().includes('user') || 
                        key.toLowerCase().includes('login') ||
                        key.toLowerCase().includes('blooddonation')) {
                        localStorage.removeItem(key);
                    }
                });
            }
            
            if (typeof(Storage) !== "undefined") {
                sessionStorage.clear();
            }
            
            if ('caches' in window) {
                caches.keys().then(names => {
                    names.forEach(name => {
                        caches.delete(name);
                    });
                });
            }
            
            this.clearAuthCookies();
            
        } catch (error) {
            console.warn('Error clearing client-side data:', error);
        }
    },
    
    clearAuthCookies: function() {
        try {
            const cookies = document.cookie.split(";");
            
            cookies.forEach(cookie => {
                const eqPos = cookie.indexOf("=");
                const name = eqPos > -1 ? cookie.substr(0, eqPos).trim() : cookie.trim();
                
                if (name.includes('AspNetCore') || 
                    name.includes('BloodDonation') || 
                    name.includes('Auth') ||
                    name.includes('.AspNetCore') ||
                    name === 'BloodDonationAuth') {
                    
                    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
                    
                    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; domain=${window.location.hostname};`;
                    
                    const domain = window.location.hostname;
                    if (domain.includes('.')) {
                        const parentDomain = '.' + domain.split('.').slice(-2).join('.');
                        document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/; domain=${parentDomain};`;
                    }
                }
            });
        } catch (error) {
            console.warn('Error clearing cookies:', error);
        }
    },
    
    confirmLogout: function() {
        if (confirm('Bạn có chắc chắn muốn đăng xuất không?')) {
            this.clearClientSideData();
            this.quickLogout();
        }
    },
    
    showLogoutPage: function() {
        window.location.href = '/account/logout';
    },
    
    ajaxLogout: function() {
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        
        this.clearClientSideData();
        
        fetch('/account/quick-logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: `__RequestVerificationToken=${encodeURIComponent(token || '')}`
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                this.showNotification(data.message, 'success');
                this.clearClientSideData();
                setTimeout(() => {
                    window.location.href = '/';
                }, 1000);
            } else {
                this.showNotification(data.message, 'error');
            }
        })
        .catch(error => {
            console.error('Logout error:', error);
            this.showNotification('Có lỗi xảy ra khi đăng xuất.', 'error');
            setTimeout(() => {
                window.location.href = '/';
            }, 2000);
        });
    },
    
    showNotification: function(message, type = 'info') {
        const notification = document.createElement('div');
        notification.className = `alert alert-${type === 'success' ? 'success' : type === 'error' ? 'danger' : 'info'} alert-dismissible fade show position-fixed`;
        notification.style.cssText = 'top: 20px; right: 20px; z-index: 9999; min-width: 300px;';
        notification.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        document.body.appendChild(notification);
        
        setTimeout(() => {
            if (notification.parentNode) {
                notification.remove();
            }
        }, 5000);
    }
};

document.addEventListener('DOMContentLoaded', function() {
    const logoutForms = document.querySelectorAll('form[action*="logout"]');
    logoutForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled = true;
                const originalText = submitBtn.innerHTML;
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang đăng xuất...';
                
                setTimeout(() => {
                    submitBtn.disabled = false;
                    submitBtn.innerHTML = originalText;
                }, 5000);
            }
        });
    });
    
    const logoutLinks = document.querySelectorAll('a[href*="logout"], .logout-link');
    logoutLinks.forEach(link => {
        if (link.classList.contains('no-confirm')) return;
        
        link.addEventListener('click', function(e) {
            e.preventDefault();
            LogoutUtils.confirmLogout();
        });
    });
});

// Keyboard shortcut for logout (Ctrl+Shift+Q)
document.addEventListener('keydown', function(e) {
    if (e.ctrlKey && e.shiftKey && e.key === 'Q') {
        e.preventDefault();
        const isAuthenticated = document.querySelector('.dropdown[id*="user"]') !== null;
        if (isAuthenticated) {
            LogoutUtils.confirmLogout();
        }
    }
});

let sessionTimeoutWarning = null;
let sessionTimeoutTimer = null;

function setupSessionTimeout() {
    if (sessionTimeoutWarning) clearTimeout(sessionTimeoutWarning);
    if (sessionTimeoutTimer) clearTimeout(sessionTimeoutTimer);
    
    const isAuthenticated = document.querySelector('.dropdown[id*="user"]') !== null;
    if (!isAuthenticated) return;
    
    sessionTimeoutWarning = setTimeout(() => {
        const continueSession = confirm(
            'Phiên đăng nhập của bạn sắp hết hạn.\n\n' +
            'Bấm "OK" để tiếp tục sử dụng hoặc "Cancel" để đăng xuất.'
        );
        
        if (continueSession) {
            setupSessionTimeout();
            fetch('/account/ping', { method: 'POST' }).catch(() => {});
        } else {
            LogoutUtils.quickLogout();
        }
    }, 25 * 60 * 1000);
    
    sessionTimeoutTimer = setTimeout(() => {
        LogoutUtils.showNotification('Phiên đăng nhập đã hết hạn. Bạn sẽ được đăng xuất tự động.', 'warning');
        setTimeout(() => {
            LogoutUtils.quickLogout();
        }, 3000);
    }, 30 * 60 * 1000);
}

document.addEventListener('DOMContentLoaded', setupSessionTimeout);

let activityTimeout;
function resetActivityTimeout() {
    clearTimeout(activityTimeout);
    activityTimeout = setTimeout(setupSessionTimeout, 1000);
}

['mousedown', 'mousemove', 'keypress', 'scroll', 'touchstart', 'click'].forEach(event => {
    document.addEventListener(event, resetActivityTimeout, { passive: true });
});
