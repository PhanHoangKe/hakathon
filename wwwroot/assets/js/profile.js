document.addEventListener('DOMContentLoaded', function() {
    // ---------- Tab Navigation ----------
    initProfileTabNavigation();
    
    // ---------- Avatar Upload ----------
    initAvatarUpload();
    
    // ---------- Password Toggle ----------
    initPasswordToggle();
    
    // ---------- Password Validation ----------
    initPasswordValidation();
    
    // ---------- Activity History Tabs ----------
    initActivityTabs();
    
    // ---------- Form Submission ----------
    initFormSubmissions();
});

// ---------- Profile Tab Navigation ----------
function initProfileTabNavigation() {
    const navItems = document.querySelectorAll('.sensy-profile-nav-item');
    
    navItems.forEach(item => {
        item.addEventListener('click', function() {
            // Remove active class from all nav items
            navItems.forEach(navItem => navItem.classList.remove('active'));
            
            // Add active class to clicked item
            this.classList.add('active');
            
            // Hide all tabs
            const tabs = document.querySelectorAll('.sensy-profile-tab');
            tabs.forEach(tab => tab.classList.remove('active'));
            
            // Show the selected tab
            const targetTabId = this.getAttribute('data-tab');
            document.getElementById(targetTabId).classList.add('active');
        });
    });
}

// ---------- Avatar Upload ----------
function initAvatarUpload() {
    const avatarUpload = document.getElementById('sensy-avatar-upload');
    const userAvatar = document.getElementById('sensy-user-avatar');
    
    if (avatarUpload && userAvatar) {
        avatarUpload.addEventListener('change', function(e) {
            const file = e.target.files[0];
            
            if (file) {
                // Validate file is an image
                if (!file.type.match('image.*')) {
                    alert('Vui lòng chọn file hình ảnh.');
                    return;
                }
                
                // Validate file size (max 5MB)
                if (file.size > 5 * 1024 * 1024) {
                    alert('Dung lượng file tối đa là 5MB.');
                    return;
                }
                
                // Display the selected image
                const reader = new FileReader();
                reader.onload = function(e) {
                    userAvatar.src = e.target.result;
                };
                reader.readAsDataURL(file);
                
                // Here you would typically upload the image to the server
                // uploadAvatarToServer(file);
            }
        });
    }
}

// ---------- Password Toggle ----------
function initPasswordToggle() {
    const toggleButtons = document.querySelectorAll('.sensy-toggle-password');
    
    toggleButtons.forEach(button => {
        button.addEventListener('click', function() {
            const passwordField = this.previousElementSibling;
            const icon = this.querySelector('i');
            
            // Toggle password visibility
            if (passwordField.type === 'password') {
                passwordField.type = 'text';
                icon.classList.remove('sensy-icon-eye');
                icon.classList.add('sensy-icon-eye-off');
            } else {
                passwordField.type = 'password';
                icon.classList.remove('sensy-icon-eye-off');
                icon.classList.add('sensy-icon-eye');
            }
        });
    });
}

// ---------- Password Validation ----------
function initPasswordValidation() {
    const newPasswordInput = document.getElementById('sensy-new-password');
    const confirmPasswordInput = document.getElementById('sensy-confirm-password');
    
    if (newPasswordInput) {
        newPasswordInput.addEventListener('input', validatePassword);
    }
    
    if (confirmPasswordInput) {
        confirmPasswordInput.addEventListener('input', checkPasswordMatch);
    }
}

function validatePassword() {
    const password = document.getElementById('sensy-new-password').value;
    
    // Check length
    const lengthReq = document.getElementById('sensy-length');
    if (password.length >= 8) {
        lengthReq.classList.add('fulfilled');
    } else {
        lengthReq.classList.remove('fulfilled');
    }
    
    // Check uppercase
    const uppercaseReq = document.getElementById('sensy-uppercase');
    if (/[A-Z]/.test(password)) {
        uppercaseReq.classList.add('fulfilled');
    } else {
        uppercaseReq.classList.remove('fulfilled');
    }
    
    // Check lowercase
    const lowercaseReq = document.getElementById('sensy-lowercase');
    if (/[a-z]/.test(password)) {
        lowercaseReq.classList.add('fulfilled');
    } else {
        lowercaseReq.classList.remove('fulfilled');
    }
    
    // Check number
    const numberReq = document.getElementById('sensy-number');
    if (/[0-9]/.test(password)) {
        numberReq.classList.add('fulfilled');
    } else {
        numberReq.classList.remove('fulfilled');
    }
    
    // Check special character
    const specialReq = document.getElementById('sensy-special');
    if (/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(password)) {
        specialReq.classList.add('fulfilled');
    } else {
        specialReq.classList.remove('fulfilled');
    }
    
    // Also check password match if confirm field has a value
    if (document.getElementById('sensy-confirm-password').value) {
        checkPasswordMatch();
    }
}

function checkPasswordMatch() {
    const newPassword = document.getElementById('sensy-new-password').value;
    const confirmPassword = document.getElementById('sensy-confirm-password').value;
    
    if (confirmPassword) {
        if (newPassword === confirmPassword) {
            document.getElementById('sensy-confirm-password').classList.remove('sensy-error');
            // You might want to add a visual indicator for match
        } else {
            document.getElementById('sensy-confirm-password').classList.add('sensy-error');
        }
    }
}

// ---------- Activity History Tabs ----------
function initActivityTabs() {
    const activityTabs = document.querySelectorAll('.sensy-activity-tab');
    
    activityTabs.forEach(tab => {
        tab.addEventListener('click', function() {
            // Remove active class from all tabs
            activityTabs.forEach(actTab => actTab.classList.remove('active'));
            
            // Add active class to clicked tab
            this.classList.add('active');
            
            // Hide all content sections
            const contents = document.querySelectorAll('.sensy-activity-content');
            contents.forEach(content => content.classList.remove('active'));
            
            // Show the selected content
            const targetId = this.getAttribute('data-activity');
            document.getElementById(targetId).classList.add('active');
        });
    });
    
    // Initialize pagination
    initPagination();
}

// ---------- Pagination ----------
//function initPagination() {
//    const prevButtons = document.querySelectorAll('.sensy-pagination-prev');
//    const nextButtons = document.querySelectorAll('.sensy-pagination-next');
    
//    prevButtons.forEach(button => {
//        button.addEventListener('click', function() {
//            const paginationInfo = this.nextElementSibling;
//            const currentPage = parseInt(paginationInfo.textContent.split('/')[0].replace('Trang ', ''));
//            const totalPages = parseInt(paginationInfo.textContent.split('/')[1]);
            
//            if (currentPage > 1) {
//                const newPage = currentPage - 1;
//                paginationInfo.textContent = `Trang ${newPage}/${totalPages}`;
                
//                // Enable/disable buttons
//                this.disabled = newPage === 1;
//                this.nextElementSibling.nextElementSibling.disabled = false;
                
//                // Here you would fetch and display the appropriate data for the new page
//                // fetchPageData(currentActivityTab, newPage);
//            }
//        });
//    });
    
//    nextButtons.forEach(button => {
//        button.addEventListener('click', function() {
//            const paginationInfo = this.previousElementSibling;
//            const currentPage = parseInt(paginationInfo.textContent.split('/')[0].replace('Trang ', ''));
//            const totalPages = parseInt(paginationInfo.textContent.split('/')[1]);
            
//            if (currentPage < totalPages) {
//                const newPage = currentPage + 1;
//                paginationInfo.textContent = `Trang ${newPage}/${totalPages}`;
                
//                // Enable/disable buttons
//                this.disabled = newPage === totalPages;
//                this.previousElementSibling.previousElementSibling.disabled = false;
                
//                // Here you would fetch and display the appropriate data for the new page
//                // fetchPageData(currentActivityTab, newPage);
//            }
//        });
//    });
//}

// ---------- Form Submissions ----------
function initFormSubmissions() {
    // Personal Info Form
    const personalInfoForm = document.getElementById('sensy-personal-info-form');
    if (personalInfoForm) {
        personalInfoForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Get form data
            const formData = {
                fullName: document.getElementById('sensy-fullname').value,
                email: document.getElementById('sensy-email').value,
                phone: document.getElementById('sensy-phone').value,
                dob: document.getElementById('sensy-dob').value,
                gender: document.querySelector('input[name="gender"]:checked').value
            };
            
            // Validate data
            if (!formData.fullName || !formData.email || !formData.phone) {
                showNotification('Vui lòng điền đầy đủ thông tin bắt buộc', 'error');
                return;
            }
            
            // Email validation
            if (!validateEmail(formData.email)) {
                showNotification('Email không hợp lệ', 'error');
                return;
            }
            
            // Phone validation
            if (!validatePhone(formData.phone)) {
                showNotification('Số điện thoại không hợp lệ', 'error');
                return;
            }
            
            // Here you would typically send data to the server
            // updatePersonalInfo(formData);
            
            // Show success message
            showNotification('Thông tin cá nhân đã được cập nhật thành công', 'success');
            
            // Update display name in sidebar
            document.getElementById('sensy-user-name').textContent = formData.fullName;
            document.getElementById('sensy-user-email').textContent = formData.email;
        });
    }
    
    // Change Password Form
    const changePasswordForm = document.getElementById('sensy-change-password-form');
    if (changePasswordForm) {
        changePasswordForm.addEventListener('submit', function(e) {
            e.preventDefault();
            
            // Get form data
            const formData = {
                currentPassword: document.getElementById('sensy-current-password').value,
                newPassword: document.getElementById('sensy-new-password').value,
                confirmPassword: document.getElementById('sensy-confirm-password').value
            };
            
            // Validate data
            if (!formData.currentPassword || !formData.newPassword || !formData.confirmPassword) {
                showNotification('Vui lòng điền đầy đủ thông tin', 'error');
                return;
            }
            
            // Check if passwords match
            if (formData.newPassword !== formData.confirmPassword) {
                showNotification('Mật khẩu xác nhận không khớp', 'error');
                return;
            }
            
            // Check password requirements
            if (!meetsPasswordRequirements(formData.newPassword)) {
                showNotification('Mật khẩu mới không đáp ứng các yêu cầu', 'error');
                return;
            }
            
            // Here you would typically send data to the server
            // changePassword(formData);
            
            // Show success message
            showNotification('Mật khẩu đã được đổi thành công', 'success');
            
            // Reset form
            this.reset();
            document.querySelectorAll('.sensy-requirement').forEach(req => req.classList.remove('fulfilled'));
        });
    }
}

// ---------- Helper Functions ----------
function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(String(email).toLowerCase());
}

function validatePhone(phone) {
    const re = /^[0-9]{10,11}$/;
    return re.test(phone);
}

function meetsPasswordRequirements(password) {
    // At least 8 characters
    if (password.length < 8) return false;
    
    // At least 1 uppercase letter
    if (!/[A-Z]/.test(password)) return false;
    
    // At least 1 lowercase letter
    if (!/[a-z]/.test(password)) return false;
    
    // At least 1 number
    if (!/[0-9]/.test(password)) return false;
    
    // At least 1 special character
    if (!/[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(password)) return false;
    
    return true;
}

function showNotification(message, type = 'info') {
    // Check if notification container exists
    let container = document.querySelector('.sensy-notifications');
    
    // If not, create one
    if (!container) {
        container = document.createElement('div');
        container.className = 'sensy-notifications';
        document.body.appendChild(container);
    }
    
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `sensy-notification sensy-notification-${type}`;
    notification.innerHTML = `
        <div class="sensy-notification-content">
            <span class="sensy-notification-message">${message}</span>
        </div>
        <button class="sensy-notification-close">&times;</button>
    `;
    
    // Add to container
    container.appendChild(notification);
    
    // Add close button functionality
    const closeButton = notification.querySelector('.sensy-notification-close');
    closeButton.addEventListener('click', function() {
        notification.classList.add('sensy-notification-closing');
        setTimeout(() => {
            notification.remove();
        }, 300);
    });
    
    // Auto remove after 5 seconds
    setTimeout(() => {
        notification.classList.add('sensy-notification-closing');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }, 5000);
}