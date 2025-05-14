// Đợi tài liệu HTML tải xong trước khi thực thi JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // ===== QUẢN LÝ TAB =====
    initTabs();
    
    // ===== XEM TRƯỚC TÀI LIỆU =====
    initPdfViewer();
    
    // ===== TÓM TẮT AI =====
    initAiSummary();
    
    // ===== HỎI ĐÁP AI =====
    initAiChat();
    
    // ===== ĐÁNH GIÁ & BÌNH LUẬN =====
    initReviews();
    
    // ===== CÁC NÚT HÀNH ĐỘNG =====
    initActionButtons();
});

// ===== QUẢN LÝ TAB =====
function initTabs() {
    const tabItems = document.querySelectorAll('.senst-tab-item');
    const tabPanes = document.querySelectorAll('.senst-tab-pane');
    
    tabItems.forEach(tab => {
        tab.addEventListener('click', function() {
            // Xóa active class từ tất cả các tab
            tabItems.forEach(item => item.classList.remove('active'));
            tabPanes.forEach(pane => pane.classList.remove('active'));
            
            // Thêm active class cho tab được chọn
            this.classList.add('active');
            
            // Hiển thị nội dung tab tương ứng
            const tabId = this.getAttribute('data-tab');
            document.getElementById(tabId).classList.add('active');
        });
    });
}

// ===== XEM TRƯỚC TÀI LIỆU =====
function initPdfViewer() {
    // Mô phỏng PDF viewer (trong thực tế sẽ sử dụng thư viện như PDF.js)
    const currentPageEl = document.getElementById('currentPage');
    const totalPagesEl = document.getElementById('totalPages');
    const prevPageBtn = document.getElementById('prevPage');
    const nextPageBtn = document.getElementById('nextPage');
    const zoomInBtn = document.getElementById('zoomIn');
    const zoomOutBtn = document.getElementById('zoomOut');
    
    let currentPage = 1;
    const totalPages = 3; // Giả sử có 3 trang xem trước
    let currentZoom = 100;
    
    // Cập nhật hiển thị trang
    function updatePageInfo() {
        currentPageEl.textContent = currentPage;
        totalPagesEl.textContent = totalPages;
        
        // Vô hiệu hóa nút khi đạt giới hạn
        prevPageBtn.disabled = currentPage === 1;
        nextPageBtn.disabled = currentPage === totalPages;
    }
    
    // Xử lý chuyển trang
    prevPageBtn.addEventListener('click', function() {
        if (currentPage > 1) {
            currentPage--;
            updatePageInfo();
            renderPage(currentPage);
        }
    });
    
    nextPageBtn.addEventListener('click', function() {
        if (currentPage < totalPages) {
            currentPage++;
            updatePageInfo();
            renderPage(currentPage);
        }
    });
    
    // Xử lý phóng to/thu nhỏ
    zoomInBtn.addEventListener('click', function() {
        if (currentZoom < 200) {
            currentZoom += 25;
            applyZoom();
        }
    });
    
    zoomOutBtn.addEventListener('click', function() {
        if (currentZoom > 50) {
            currentZoom -= 25;
            applyZoom();
        }
    });
    
    function applyZoom() {
        const pdfContainer = document.querySelector('.senst-pdf-container');
        pdfContainer.style.transform = `scale(${currentZoom / 100})`;
        pdfContainer.style.transformOrigin = 'top left';
    }
    
    // Giả lập render trang PDF
    function renderPage(pageNum) {
        const canvas = document.getElementById('pdfRender');
        const ctx = canvas.getContext('2d');
        
        // Xóa nội dung hiện tại
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        
        // Thiết lập kích thước canvas
        canvas.width = 800;
        canvas.height = 1100;
        
        // Vẽ một khung giả lập trang PDF
        ctx.fillStyle = '#ffffff';
        ctx.fillRect(0, 0, canvas.width, canvas.height);
        
        ctx.strokeStyle = '#dddddd';
        ctx.strokeRect(5, 5, canvas.width - 10, canvas.height - 10);
        
        // Hiển thị số trang
        ctx.fillStyle = '#333333';
        ctx.font = '24px Arial';
        ctx.textAlign = 'center';
        ctx.fillText(`Trang xem trước ${pageNum}`, canvas.width / 2, 50);
        
        // Nội dung giả lập
        ctx.font = '16px Arial';
        ctx.textAlign = 'left';
        ctx.fillText(`Nội dung tài liệu "Lập trình Web với HTML, CSS và JavaScript"`, 50, 100);
        ctx.fillText(`Đây là nội dung xem trước cho trang ${pageNum}`, 50, 130);
        
        // Thêm văn bản theo trang
        switch(pageNum) {
            case 1:
                ctx.fillText("Chương 1: Giới thiệu về HTML", 50, 180);
                ctx.fillText("HTML (HyperText Markup Language) là ngôn ngữ đánh dấu siêu văn bản", 50, 210);
                ctx.fillText("được sử dụng để tạo và cấu trúc các trang web.", 50, 240);
                break;
            case 2:
                ctx.fillText("Chương 2: CSS và định dạng trang web", 50, 180);
                ctx.fillText("CSS (Cascading Style Sheets) là ngôn ngữ được sử dụng để định dạng", 50, 210);
                ctx.fillText("và trang trí nội dung được viết bằng HTML.", 50, 240);
                break;
            case 3:
                ctx.fillText("Chương 3: JavaScript và tương tác người dùng", 50, 180);
                ctx.fillText("JavaScript là ngôn ngữ lập trình được sử dụng để tạo các tính năng", 50, 210);
                ctx.fillText("tương tác trên trang web.", 50, 240);
                break;
        }
    }
    
    // Khởi tạo trạng thái ban đầu
    updatePageInfo();
    renderPage(currentPage);
}

// ===== TÓM TẮT AI =====
function initAiSummary() {
    const summaryType = document.getElementById('summaryType');
    const chapterSelection = document.getElementById('chapterSelection');
    const generateSummaryBtn = document.getElementById('generateSummary');
    const playTTSBtn = document.getElementById('playTTS');
    const audioPlayer = document.getElementById('audioPlayer');
    
    // Hiển thị/ẩn lựa chọn chương
    summaryType.addEventListener('change', function() {
        if (this.value === 'chapter') {
            chapterSelection.style.display = 'inline-block';
        } else {
            chapterSelection.style.display = 'none';
        }
    });
    
    // Xử lý tạo tóm tắt
    generateSummaryBtn.addEventListener('click', function() {
        const type = summaryType.value;
        let summaryTitle, summaryText;
        
        // Hiển thị hiệu ứng đang tải
        document.querySelector('.senst-summary-result').innerHTML = '<div class="senst-loading">Đang tạo tóm tắt...</div>';
        
        // Giả lập thời gian tạo tóm tắt
        setTimeout(function() {
            if (type === 'full') {
                summaryTitle = 'Tóm tắt toàn bộ tài liệu:';
                summaryText = 'Cuốn sách "Lập trình Web với HTML, CSS và JavaScript" là tài liệu toàn diện về phát triển web. Phần đầu giới thiệu về nền tảng web và cách hoạt động của trình duyệt. Các chương tiếp theo tập trung vào HTML để xây dựng cấu trúc, CSS để định dạng và tạo giao diện hấp dẫn. JavaScript được giới thiệu từ cơ bản đến nâng cao để tạo tương tác cho trang web. Phần cuối đề cập đến các framework hiện đại như React, Angular và Vue.js cùng các kỹ thuật tối ưu hiệu suất. Sách phù hợp cho cả người mới học và người muốn nâng cao kỹ năng lập trình web.';
            } else {
                const chapterNum = document.getElementById('chapterNumber').value;
                summaryTitle = `Tóm tắt Chương ${chapterNum}:`;
                
                switch(parseInt(chapterNum)) {
                    case 1:
                        summaryText = 'Chương 1 giới thiệu về HTML, cú pháp cơ bản và cấu trúc của một trang web. Chương này giải thích các thẻ HTML phổ biến, thuộc tính, và cách tạo liên kết giữa các trang. Phần cuối chương giới thiệu về HTML5 và các thẻ ngữ nghĩa mới.';
                        break;
                    case 2:
                        summaryText = 'Chương 2 tập trung vào CSS, bao gồm cách sử dụng CSS để định dạng văn bản, thêm màu sắc, tạo layout. Chương này cũng trình bày về CSS Box Model, đơn vị đo lường và responsive design với media queries.';
                        break;
                    case 3:
                        summaryText = 'Chương 3 đề cập đến JavaScript, bao gồm cú pháp, biến, hàm, và cấu trúc điều khiển. Chương này cũng trình bày cách thao tác với DOM, xử lý sự kiện và AJAX để tạo các trang web động.';
                        break;
                }
            }
            
            // Cập nhật kết quả tóm tắt
            document.querySelector('.senst-summary-result').innerHTML = `
                <h4>${summaryTitle}</h4>
                <p>${summaryText}</p>
            `;
        }, 1500);
    });
    
    // Xử lý chức năng Text-to-Speech
    playTTSBtn.addEventListener('click', function() {
        // Trong thực tế, đây sẽ là URL đến file audio được tạo ra
        // Ở đây chúng ta giả lập bằng cách hiển thị trình phát audio
        const btnIcon = this.querySelector('i');
        
        if (audioPlayer.style.display === 'none') {
            audioPlayer.style.display = 'block';
            btnIcon.className = 'fa fa-pause';
            this.querySelector('span') ? this.removeChild(this.querySelector('span')) : null;
            this.appendChild(document.createTextNode(' Dừng'));
        } else {
            audioPlayer.style.display = 'none';
            btnIcon.className = 'fa fa-play';
            this.querySelector('span') ? this.removeChild(this.querySelector('span')) : null;
            this.appendChild(document.createTextNode(' Phát giọng nói'));
        }
    });
}

// ===== HỎI ĐÁP AI =====
function initAiChat() {
    const chatMessages = document.getElementById('chatMessages');
    const userQuestion = document.getElementById('userQuestion');
    const sendQuestionBtn = document.getElementById('sendQuestion');
    const suggestionChips = document.querySelectorAll('.senst-chip');
    
    // Hàm thêm tin nhắn vào khung chat
    function addMessage(content, isUser = false) {
        const messageDiv = document.createElement('div');
        messageDiv.className = isUser ? 'senst-message senst-message-user' : 'senst-message senst-message-bot';
        
        if (!isUser) {
            const avatarDiv = document.createElement('div');
            avatarDiv.className = 'senst-message-avatar';
            const avatarImg = document.createElement('img');
            avatarImg.src = '/api/placeholder/40/40';
            avatarImg.alt = 'Bot Avatar';
            avatarDiv.appendChild(avatarImg);
            messageDiv.appendChild(avatarDiv);
        }
        
        const contentDiv = document.createElement('div');
        contentDiv.className = 'senst-message-content';
        contentDiv.innerHTML = `<p>${content}</p>`;
        messageDiv.appendChild(contentDiv);
        
        chatMessages.appendChild(messageDiv);
        
        // Cuộn xuống tin nhắn mới nhất
        chatMessages.scrollTop = chatMessages.scrollHeight;
    }
    
    // Xử lý gửi câu hỏi
    function sendQuestion() {
        const question = userQuestion.value.trim();
        
        if (question) {
            // Thêm câu hỏi của người dùng vào chat
            addMessage(question, true);
            
            // Xóa nội dung input
            userQuestion.value = '';
            
            // Hiển thị trạng thái đang nhập
            const typingDiv = document.createElement('div');
            typingDiv.className = 'senst-message senst-message-bot senst-typing';
            typingDiv.innerHTML = `
                <div class="senst-message-avatar">
                    <img src="/api/placeholder/40/40" alt="Bot Avatar" />
                </div>
                <div class="senst-message-content">
                    <div class="senst-typing-indicator">
                        <span></span>
                        <span></span>
                        <span></span>
                    </div>
                </div>
            `;
            chatMessages.appendChild(typingDiv);
            chatMessages.scrollTop = chatMessages.scrollHeight;
            
            // Giả lập thời gian trả lời
            setTimeout(function() {
                // Xóa trạng thái đang nhập
                chatMessages.removeChild(typingDiv);
                
                // Trả lời từ AI (giả lập)
                let answer = '';
                
                if (question.toLowerCase().includes('chương 1')) {
                    answer = 'Chương 1 của tài liệu nói về cơ bản của HTML. Nó bao gồm cách tạo cấu trúc trang web, các thẻ HTML cơ bản, và cách liên kết giữa các trang. Chương này cũng giới thiệu về HTML5 và các thẻ ngữ nghĩa mới.';
                } else if (question.toLowerCase().includes('bao nhiêu chương')) {
                    answer = 'Tài liệu "Lập trình Web với HTML, CSS và JavaScript" có tổng cộng 8 chương, bao gồm: 1) Giới thiệu về HTML, 2) CSS và định dạng trang web, 3) JavaScript cơ bản, 4) DOM và tương tác người dùng, 5) Responsive Web Design, 6) JavaScript nâng cao, 7) API và AJAX, 8) Các Framework hiện đại.';
                } else if (question.toLowerCase().includes('kết luận')) {
                    answer = 'Kết luận của tác giả nhấn mạnh tầm quan trọng của việc tiếp tục học hỏi và cập nhật kiến thức trong lĩnh vực phát triển web. Tác giả khuyến khích người đọc thực hành thường xuyên và tham gia vào các dự án thực tế để củng cố kiến thức đã học.';
                } else {
                    answer = 'Cảm ơn câu hỏi của bạn. Dựa trên nội dung của tài liệu "Lập trình Web với HTML, CSS và JavaScript", tôi có thể trả lời rằng sách này được thiết kế để giúp người đọc từ cơ bản đến nâng cao trong việc phát triển web. Mỗi chương được xây dựng trên kiến thức từ các chương trước và bao gồm nhiều ví dụ thực tế.';
                }
                
                // Thêm câu trả lời của AI vào chat
                addMessage(answer);
            }, 1500);
        }
    }
    
    // Xử lý sự kiện click nút gửi
    sendQuestionBtn.addEventListener('click', sendQuestion);
    
    // Xử lý sự kiện nhấn Enter trong ô input
    userQuestion.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            sendQuestion();
        }
    });
    
    // Xử lý sự kiện cho các gợi ý
    suggestionChips.forEach(chip => {
        chip.addEventListener('click', function() {
            userQuestion.value = this.textContent;
            sendQuestion();
        });
    });
}

// ===== ĐÁNH GIÁ & BÌNH LUẬN =====
function initReviews() {
    const starInputs = document.querySelectorAll('.senst-star-select i');
    const reviewTextArea = document.querySelector('.senst-review-text');
    const submitReviewBtn = document.querySelector('.senst-btn-submit');
    const moreReviewsBtn = document.querySelector('.senst-btn-more');
    const reviewsList = document.querySelector('.senst-reviews-list');
    
    // Xử lý chọn số sao
    let selectedRating = 0;
    
    starInputs.forEach(star => {
        // Hiệu ứng hover
        star.addEventListener('mouseover', function() {
            const rating = parseInt(this.getAttribute('data-rating'));
            highlightStars(rating);
        });
        
        // Trở về trạng thái đã chọn khi rời chuột
        star.addEventListener('mouseout', function() {
            highlightStars(selectedRating);
        });
        
        // Chọn đánh giá
        star.addEventListener('click', function() {
            selectedRating = parseInt(this.getAttribute('data-rating'));
            highlightStars(selectedRating);
        });
    });
    
    // Hàm làm nổi bật sao theo đánh giá
    function highlightStars(rating) {
        starInputs.forEach(star => {
            const starRating = parseInt(star.getAttribute('data-rating'));
            if (starRating <= rating) {
                star.className = 'fa fa-star';
            } else {
                star.className = 'fa fa-star-o';
            }
        });
    }
    
    // Xử lý gửi đánh giá
    submitReviewBtn.addEventListener('click', function() {
        const reviewText = reviewTextArea.value.trim();
        
        if (selectedRating === 0) {
            alert('Vui lòng chọn số sao đánh giá!');
            return;
        }
        
        if (!reviewText) {
            alert('Vui lòng viết đánh giá của bạn!');
            return;
        }
        
        // Tạo đánh giá mới (giả lập)
        const currentDate = new Date();
        const formattedDate = `${currentDate.getDate()}/${currentDate.getMonth() + 1}/${currentDate.getFullYear()}`;
        
        const newReview = document.createElement('div');
        newReview.className = 'senst-review-item';
        newReview.innerHTML = `
            <div class="senst-reviewer-info">
                <img src="/api/placeholder/50/50" alt="User Avatar" class="senst-reviewer-avatar" />
                <div class="senst-reviewer-details">
                    <div class="senst-reviewer-name">Bạn</div>
                    <div class="senst-review-date">${formattedDate}</div>
                    <div class="senst-reviewer-rating">
                        ${Array(5).fill().map((_, i) => 
                            `<i class="fa ${i < selectedRating ? 'fa-star' : 'fa-star-o'}"></i>`
                        ).join('')}
                    </div>
                </div>
            </div>
            <div class="senst-review-content">
                <p>${reviewText}</p>
            </div>
            <div class="senst-review-actions">
                <button class="senst-btn senst-btn-like">
                    <i class="fa fa-thumbs-up"></i> Hữu ích (0)
                </button>
                <button class="senst-btn senst-btn-reply">
                    <i class="fa fa-reply"></i> Trả lời
                </button>
            </div>
        `;
        
        // Thêm đánh giá mới vào đầu danh sách
        const firstReview = reviewsList.querySelector('.senst-review-item');
        reviewsList.insertBefore(newReview, firstReview);
        
        // Reset form
        reviewTextArea.value = '';
        selectedRating = 0;
        highlightStars(0);
        
        // Hiển thị thông báo thành công
        alert('Cảm ơn bạn đã đánh giá!');
    });
    
    // Xử lý nút xem thêm bình luận
    moreReviewsBtn.addEventListener('click', function() {
        // Giả lập tải thêm bình luận
        const loadingText = document.createElement('div');
        loadingText.className = 'senst-loading';
        loadingText.textContent = 'Đang tải thêm bình luận...';
        reviewsList.insertBefore(loadingText, moreReviewsBtn);
        
        setTimeout(function() {
            // Xóa thông báo đang tải
            reviewsList.removeChild(loadingText);
            
            // Thêm bình luận mới
            for (let i = 0; i < 2; i++) {
                const newReview = document.createElement('div');
                newReview.className = 'senst-review-item';
                newReview.innerHTML = `
                    <div class="senst-reviewer-info">
                        <img src="/api/placeholder/50/50" alt="User Avatar" class="senst-reviewer-avatar" />
                        <div class="senst-reviewer-details">
                            <div class="senst-reviewer-name">Người dùng Mới</div>
                            <div class="senst-review-date">0${i+1}/04/2025</div>
                            <div class="senst-reviewer-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa ${i % 2 === 0 ? 'fa-star' : 'fa-star-o'}"></i>
                            </div>
                        </div>
                    </div>
                    <div class="senst-review-content">
                        <p>Đây là một bình luận được tải thêm. Tài liệu rất có ích và dễ hiểu cho người mới bắt đầu học lập trình web.</p>
                    </div>
                    <div class="senst-review-actions">
                        <button class="senst-btn senst-btn-like">
                            <i class="fa fa-thumbs-up"></i> Hữu ích (${i})
                        </button>
                        <button class="senst-btn senst-btn-reply">
                            <i class="fa fa-reply"></i> Trả lời
                        </button>
                    </div>
                `;
                
                reviewsList.insertBefore(newReview, moreReviewsBtn);
            }
            
            // Ẩn nút nếu không còn bình luận để tải
            moreReviewsBtn.style.display = 'none';
        }, 1000);
    });
    
    // Xử lý nút "Hữu ích" cho các bình luận
    document.querySelectorAll('.senst-btn-like').forEach(likeBtn => {
        likeBtn.addEventListener('click', function() {
            const likeText = this.textContent.trim();
            const currentLikes = parseInt(likeText.match(/\d+/)[0]);
            this.innerHTML = `<i class="fa fa-thumbs-up"></i> Hữu ích (${currentLikes + 1})`;
            this.classList.add('senst-liked');
            this.disabled = true;
        });
    });
}

// ===== CÁC NÚT HÀNH ĐỘNG =====
function initActionButtons() {
    const favoriteBtn = document.getElementById('favoriteBtn');
    const readDocBtn = document.getElementById('readDocBtn');
    const downloadOptions = document.querySelectorAll('.senst-download-option');
    
    // Nút yêu thích
    favoriteBtn.addEventListener('click', function() {
        const icon = this.querySelector('i');
        
        if (icon.classList.contains('fa-heart-o')) {
            icon.className = 'fa fa-heart';
            this.classList.add('senst-favorited');
            alert('Đã thêm vào danh sách yêu thích!');
        } else {
            icon.className = 'fa fa-heart-o';
            this.classList.remove('senst-favorited');
            alert('Đã xóa khỏi danh sách yêu thích!');
        }
    });
    
    // Nút đọc tài liệu
    readDocBtn.addEventListener('click', function() {
        // Chuyển sang tab xem trước
        document.querySelector('.senst-tab-item[data-tab="preview"]').click();
        
        // Cuộn đến phần xem trước
        document.querySelector('.senst-pdf-viewer').scrollIntoView({
            behavior: 'smooth'
        });
    });
    
    // Các tùy chọn tải xuống
    downloadOptions.forEach(option => {
        option.addEventListener('click', function(e) {
            e.preventDefault();
            const format = this.textContent.includes('PDF') ? 'PDF' : 'DOCX';
            alert(`Đang tải xuống tài liệu ở định dạng ${format}...`);
        });
    });
}