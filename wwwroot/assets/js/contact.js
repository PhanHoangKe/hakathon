/* Trang liên hệ / phản hồi JS */

document.addEventListener('DOMContentLoaded', function() {
    // Khởi tạo các biến
    const contactForm = document.getElementById('sensi-contact-form');
    const nameInput = document.getElementById('sensi-name');
    const emailInput = document.getElementById('sensi-email');
    const subjectInput = document.getElementById('sensi-subject');
    const messageInput = document.getElementById('sensi-message');
    const submitBtn = document.getElementById('sensi-submit-btn');
    const captchaInput = document.getElementById('sensi-captcha-input');
    const captchaDisplay = document.getElementById('sensi-captcha-code');
    const refreshCaptchaBtn = document.getElementById('sensi-refresh-captcha');
    
    // Popup elements
    const popup = document.getElementById('sensi-popup');
    const popupTitle = document.getElementById('sensi-popup-title');
    const popupMessage = document.getElementById('sensi-popup-message');
    const closePopupBtn = document.getElementById('sensi-close-popup');
    const popupBtn = document.getElementById('sensi-popup-btn');
    
    // Lưu trữ mã captcha
    let captchaCode = '';
    
    // Tạo mã captcha mới
    function generateCaptcha() {
        const characters = 'ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789';
        captchaCode = '';
        
        for (let i = 0; i < 6; i++) {
            captchaCode += characters.charAt(Math.floor(Math.random() * characters.length));
        }
        
        captchaDisplay.textContent = captchaCode;
    }
    
    // Hiển thị thông báo lỗi
    function showError(element, message) {
        const errorElement = document.getElementById(`${element.id}-error`);
        errorElement.textContent = message;
        element.classList.add('sensi-error');
    }
    
    // Xóa thông báo lỗi
    function clearError(element) {
        const errorElement = document.getElementById(`${element.id}-error`);
        errorElement.textContent = '';
        element.classList.remove('sensi-error');
    }
    
    // Kiểm tra định dạng email
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }
    
    // Kiểm tra form trước khi gửi
    function validateForm() {
        let isValid = true;
        
        // Kiểm tra tên
        if (nameInput.value.trim() === '') {
            showError(nameInput, 'Vui lòng nhập họ và tên');
            isValid = false;
        } else if (nameInput.value.trim().length < 2) {
            showError(nameInput, 'Họ và tên phải có ít nhất 2 ký tự');
            isValid = false;
        } else {
            clearError(nameInput);
        }
        
        // Kiểm tra email
        if (emailInput.value.trim() === '') {
            showError(emailInput, 'Vui lòng nhập địa chỉ email');
            isValid = false;
        } else if (!isValidEmail(emailInput.value.trim())) {
            showError(emailInput, 'Địa chỉ email không hợp lệ');
            isValid = false;
        } else {
            clearError(emailInput);
        }
        
        // Kiểm tra chủ đề
        if (subjectInput.value === '') {
            showError(subjectInput, 'Vui lòng chọn chủ đề');
            isValid = false;
        } else {
            clearError(subjectInput);
        }
        
        // Kiểm tra nội dung
        if (messageInput.value.trim() === '') {
            showError(messageInput, 'Vui lòng nhập nội dung');
            isValid = false;
        } else if (messageInput.value.trim().length < 10) {
            showError(messageInput, 'Nội dung phải có ít nhất 10 ký tự');
            isValid = false;
        } else {
            clearError(messageInput);
        }
        
        // Kiểm tra mã captcha
        if (captchaInput.value.trim() === '') {
            showError(captchaInput, 'Vui lòng nhập mã xác nhận');
            isValid = false;
        } else if (captchaInput.value.trim() !== captchaCode) {
            showError(captchaInput, 'Mã xác nhận không chính xác');
            isValid = false;
        } else {
            clearError(captchaInput);
        }
        
        return isValid;
    }
    
    // Xử lý sự kiện input để xóa thông báo lỗi khi người dùng bắt đầu nhập lại
    nameInput.addEventListener('input', () => clearError(nameInput));
    emailInput.addEventListener('input', () => clearError(emailInput));
    subjectInput.addEventListener('change', () => clearError(subjectInput));
    messageInput.addEventListener('input', () => clearError(messageInput));
    captchaInput.addEventListener('input', () => clearError(captchaInput));
    
    // Hiển thị popup thông báo
    function showPopup(title, message, isSuccess = true) {
        popupTitle.textContent = title;
        popupMessage.textContent = message;
        
        if (isSuccess) {
            popupTitle.style.backgroundColor = '#0bb9d7';
        } else {
            popupTitle.style.backgroundColor = '#e74c3c';
        }
        
        popup.style.display = 'flex';
    }
    
    // Đóng popup
    function closePopup() {
        popup.style.display = 'none';
    }
    
    // Mô phỏng gửi email đến admin (trong thực tế sẽ sử dụng API hoặc backend)
    function sendEmail() {
        // Trong thực tế, đây sẽ là một gọi API để gửi email
        return new Promise((resolve) => {
            // Mô phỏng thời gian gửi email
            setTimeout(() => {
                resolve(true);
            }, 1500);
        });
    }
    
    // Xử lý sự kiện submit form
    submitBtn.addEventListener('click', async function() {
        if (validateForm()) {
            // Thay đổi nút submit để hiển thị trạng thái đang gửi
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<span>Đang gửi...</span> <i class="fas fa-spinner fa-spin"></i>';
            
            try {
                // Mô phỏng gửi email
                const success = await sendEmail();
                
                if (success) {
                    // Hiển thị thông báo thành công
                    showPopup('Thành công', 'Cảm ơn bạn đã gửi phản hồi. Chúng tôi sẽ phản hồi lại trong thời gian sớm nhất!');
                    
                    // Reset form
                    contactForm.reset();
                    generateCaptcha();
                } else {
                    // Hiển thị thông báo lỗi
                    showPopup('Lỗi', 'Có lỗi xảy ra khi gửi phản hồi. Vui lòng thử lại sau!', false);
                }
            } catch (error) {
                // Hiển thị thông báo lỗi
                showPopup('Lỗi', 'Có lỗi xảy ra khi gửi phản hồi. Vui lòng thử lại sau!', false);
            } finally {
                // Khôi phục nút submit
                submitBtn.disabled = false;
                submitBtn.innerHTML = '<span>Gửi phản hồi</span> <i class="fas fa-paper-plane"></i>';
            }
        }
    });
    
    // Làm mới mã captcha khi nhấn nút refresh
    refreshCaptchaBtn.addEventListener('click', generateCaptcha);
    
    // Đóng popup khi nhấn nút đóng hoặc nút trong footer
    closePopupBtn.addEventListener('click', closePopup);
    popupBtn.addEventListener('click', closePopup);
    
    // Khởi tạo mã captcha khi trang được tải
    generateCaptcha();
});