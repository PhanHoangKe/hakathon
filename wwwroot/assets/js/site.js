/**
 * SenseLib - Main JavaScript File
 * Chức năng:
 * - Điều khiển slider trên trang chủ
 * - Điều khiển slider testimonial
 * - Hiệu ứng menu khi cuộn trang
 */

document.addEventListener('DOMContentLoaded', function() {
    // Hero Slider functionality
    initializeSlider();
    
    // Testimonial Slider functionality
    initializeTestimonialSlider();
    
    // Sticky header effect
    initializeStickyHeader();
});

/**
 * Hàm khởi tạo và điều khiển slider chính
 */
function initializeSlider() {
    const slides = document.querySelectorAll('.sense-slide');
    const dots = document.querySelectorAll('.sense-slider-dots .sense-dot');
    const prevBtn = document.querySelector('.sense-slider-control.prev');
    const nextBtn = document.querySelector('.sense-slider-control.next');
    
    if (!slides.length) return;
    
    let currentSlide = 0;
    let slideInterval;
    
    // Bắt đầu slider tự động chuyển sau mỗi 5 giây
    startSlideInterval();
    
    // Hàm chuyển đến slide được chỉ định
    function goToSlide(index) {
        // Loại bỏ class active khỏi tất cả slides
        slides.forEach(slide => {
            slide.classList.remove('active');
        });
        
        // Loại bỏ class active khỏi tất cả các dots
        dots.forEach(dot => {
            dot.classList.remove('active');
        });
        
        // Thêm class active vào slide hiện tại
        slides[index].classList.add('active');
        dots[index].classList.add('active');
        
        // Cập nhật index của slide hiện tại
        currentSlide = index;
    }
    
    // Chuyển đến slide kế tiếp
    function nextSlide() {
        const next = (currentSlide + 1) % slides.length;
        goToSlide(next);
    }
    
    // Chuyển đến slide trước đó
    function prevSlide() {
        const prev = (currentSlide - 1 + slides.length) % slides.length;
        goToSlide(prev);
    }
    
    // Bắt đầu tự động chuyển slide
    function startSlideInterval() {
        // Clear bất kỳ interval nào đang chạy
        clearInterval(slideInterval);
        
        // Thiết lập interval mới
        slideInterval = setInterval(() => {
            nextSlide();
        }, 5000);
    }
    
    // Xử lý sự kiện click vào nút next
    if (nextBtn) {
        nextBtn.addEventListener('click', function() {
            nextSlide();
            startSlideInterval(); // Reset interval
        });
    }
    
    // Xử lý sự kiện click vào nút prev
    if (prevBtn) {
        prevBtn.addEventListener('click', function() {
            prevSlide();
            startSlideInterval(); // Reset interval
        });
    }
    
    // Xử lý sự kiện click vào dot
    dots.forEach((dot, index) => {
        dot.addEventListener('click', function() {
            goToSlide(index);
            startSlideInterval(); // Reset interval
        });
    });
}

/**
 * Hàm khởi tạo và điều khiển slider testimonial
 */
function initializeTestimonialSlider() {
    const testimonials = document.querySelectorAll('.sense-testimonial');
    const dots = document.querySelectorAll('.sense-testimonial-dots .sense-dot');
    const prevBtn = document.querySelector('.sense-testimonial-control.prev');
    const nextBtn = document.querySelector('.sense-testimonial-control.next');
    
    if (!testimonials.length) return;
    
    let currentTestimonial = 0;
    let testimonialInterval;
    
    // Bắt đầu slider tự động chuyển sau mỗi 6 giây
    startTestimonialInterval();
    
    // Hàm chuyển đến testimonial được chỉ định
    function goToTestimonial(index) {
        // Loại bỏ class active khỏi tất cả testimonials
        testimonials.forEach(testimonial => {
            testimonial.classList.remove('active');
        });
        
        // Loại bỏ class active khỏi tất cả các dots
        dots.forEach(dot => {
            dot.classList.remove('active');
        });
        
        // Thêm class active vào testimonial hiện tại
        testimonials[index].classList.add('active');
        dots[index].classList.add('active');
        
        // Cập nhật index của testimonial hiện tại
        currentTestimonial = index;
    }
    
    // Chuyển đến testimonial kế tiếp
    function nextTestimonial() {
        const next = (currentTestimonial + 1) % testimonials.length;
        goToTestimonial(next);
    }
    
    // Chuyển đến testimonial trước đó
    function prevTestimonial() {
        const prev = (currentTestimonial - 1 + testimonials.length) % testimonials.length;
        goToTestimonial(prev);
    }
    
    // Bắt đầu tự động chuyển testimonial
    function startTestimonialInterval() {
        // Clear bất kỳ interval nào đang chạy
        clearInterval(testimonialInterval);
        
        // Thiết lập interval mới
        testimonialInterval = setInterval(() => {
            nextTestimonial();
        }, 6000);
    }
    
    // Xử lý sự kiện click vào nút next
    if (nextBtn) {
        nextBtn.addEventListener('click', function() {
            nextTestimonial();
            startTestimonialInterval(); // Reset interval
        });
    }
    
    // Xử lý sự kiện click vào nút prev
    if (prevBtn) {
        prevBtn.addEventListener('click', function() {
            prevTestimonial();
            startTestimonialInterval(); // Reset interval
        });
    }
    
    // Xử lý sự kiện click vào dot
    dots.forEach((dot, index) => {
        dot.addEventListener('click', function() {
            goToTestimonial(index);
            startTestimonialInterval(); // Reset interval
        });
    });
}

