document.addEventListener('DOMContentLoaded', function() {
    // ===== QUẢN LÝ TAB =====
    initTabs();
    
    // Các chức năng khác sẽ được khởi tạo dựa vào sự sẵn sàng của PDF.js
    checkPdfJsAndInitialize();
    
    // ===== TÓM TẮT AI =====
    initAiSummary();
    
    // ===== HỎI ĐÁP AI =====
    initAiChat();
    
    // ===== ĐÁNH GIÁ & BÌNH LUẬN =====
    initReviews();
    
    // ===== CÁC NÚT HÀNH ĐỘNG =====
    initActionButtons();
});

// Kiểm tra PDF.js và khởi tạo trình xem PDF khi sẵn sàng
function checkPdfJsAndInitialize() {
    console.log("Checking if PDF.js is ready");
    
    // Nếu PDF.js đã sẵn sàng, khởi tạo ngay
    if (window.pdfjsLib) {
        console.log("PDF.js is ready, initializing viewer");
        initPdfViewer();
    } else {
        // Nếu chưa, đợi và kiểm tra lại sau mỗi 100ms
        console.log("PDF.js not ready yet, waiting");
        let checkCount = 0;
        const maxChecks = 50; // Tối đa 5 giây (50 * 100ms)
        
        const checkInterval = setInterval(function() {
            checkCount++;
            console.log(`Checking for PDF.js (attempt ${checkCount}/${maxChecks})`);
            
            if (window.pdfjsLib) {
                console.log("PDF.js is now ready, initializing viewer");
                clearInterval(checkInterval);
                initPdfViewer();
            } else if (checkCount >= maxChecks) {
                console.error("PDF.js failed to load after multiple attempts");
                clearInterval(checkInterval);
                
                // Hiển thị thông báo lỗi trong canvas
                const pdfContainer = document.getElementById('pdfRender');
                if (pdfContainer) {
                    const ctx = pdfContainer.getContext('2d');
                    if (ctx) {
                        pdfContainer.width = 600;
                        pdfContainer.height = 200;
                        ctx.font = '16px Arial';
                        ctx.fillText('Không thể tải thư viện PDF.js. Vui lòng làm mới trang.', 20, 50);
                    }
                }
            }
        }, 100);
    }
}

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
    console.log("Initializing PDF viewer");
    
    // Elements
    const pdfContainer = document.getElementById('pdfRender');
    const prevPageBtn = document.getElementById('prevPage');
    const nextPageBtn = document.getElementById('nextPage');
    const currentPageElem = document.getElementById('currentPage');
    const totalPagesElem = document.getElementById('totalPages');
    const zoomInBtn = document.getElementById('zoomIn');
    const zoomOutBtn = document.getElementById('zoomOut');
    
    // Kiểm tra canvas element
    if (!pdfContainer) {
        console.error("PDF container (canvas) element not found");
        return;
    }
    
    // Khởi tạo canvas context
    const ctx = pdfContainer.getContext('2d');
    
    // Kiểm tra thư viện PDF.js
    if (!window.pdfjsLib) {
        console.error("PDF.js library not found");
        
        // Hiển thị thông báo lỗi trên canvas
        if (ctx) {
            ctx.font = '16px Arial';
            ctx.fillText('Không thể tải thư viện PDF.js. Vui lòng làm mới trang.', 20, 50);
        }
        return;
    }
    
    console.log("PDF.js library found:", window.pdfjsLib);
    
    // Hiển thị "Đang tải" trên canvas
    if (ctx) {
        pdfContainer.width = 600;  // Đặt kích thước mặc định cho canvas
        pdfContainer.height = 200;
        ctx.clearRect(0, 0, pdfContainer.width, pdfContainer.height);
        ctx.font = '16px Arial';
        ctx.fillText('Đang tải tài liệu...', 20, 50);
    }
    
    // Variables
    let pdfDoc = null;
    let pageNum = 1;
    let pageRendering = false;
    let pageNumPending = null;
    let scale = 1.0;
    const MAX_PREVIEW_PAGES = 3; // Giới hạn xem trước 3 trang
    
    /**
     * Get page info from document, resize canvas accordingly, and render page.
     * @param num Page number.
     */
    function renderPage(num) {
        pageRendering = true;
        console.log(`Rendering page ${num}`);
        
        // Using promise to fetch the page
        pdfDoc.getPage(num).then(function(page) {
            console.log(`Page ${num} loaded`);
            
            const viewport = page.getViewport({ scale: scale });
            pdfContainer.height = viewport.height;
            pdfContainer.width = viewport.width;
            
            const renderContext = {
                canvasContext: ctx,
                viewport: viewport
            };
            
            console.log("Starting render task");
            const renderTask = page.render(renderContext);
            
            // Wait for rendering to finish
            renderTask.promise.then(function() {
                console.log(`Page ${num} rendered successfully`);
                pageRendering = false;
                
                if (pageNumPending !== null) {
                    // New page rendering is pending
                    renderPage(pageNumPending);
                    pageNumPending = null;
                }
            }).catch(function(error) {
                console.error('Error rendering PDF page:', error);
                pageRendering = false;
                
                if (ctx) {
                    ctx.font = '16px Arial';
                    ctx.fillText(`Lỗi khi hiển thị trang ${num}. ${error.message}`, 20, 50);
                }
            });
        }).catch(function(error) {
            console.error(`Error getting page ${num}:`, error);
            pageRendering = false;
            
            if (ctx) {
                ctx.font = '16px Arial';
                ctx.fillText(`Không thể tải trang ${num}. ${error.message}`, 20, 50);
            }
        });
        
        // Update page counters
        if (currentPageElem) {
            currentPageElem.textContent = num;
        }
    }
    
    /**
     * If another page rendering in progress, waits until the rendering is
     * finished. Otherwise, executes rendering immediately.
     */
    function queueRenderPage(num) {
        if (pageRendering) {
            pageNumPending = num;
        } else {
            renderPage(num);
        }
    }
    
    /**
     * Go to previous page.
     */
    function onPrevPage() {
        if (pageNum <= 1) {
            return;
        }
        pageNum--;
        queueRenderPage(pageNum);
    }
    
    /**
     * Go to next page.
     */
    function onNextPage() {
        if (pageNum >= Math.min(pdfDoc?.numPages || 1, MAX_PREVIEW_PAGES)) {
            return;
        }
        pageNum++;
        queueRenderPage(pageNum);
    }
    
    /**
     * Zoom in the PDF
     */
    function onZoomIn() {
        scale += 0.1;
        queueRenderPage(pageNum);
    }
    
    /**
     * Zoom out the PDF
     */
    function onZoomOut() {
        if (scale <= 0.2) return;
        scale -= 0.1;
        queueRenderPage(pageNum);
    }
    
    // Event listeners
    if (prevPageBtn) {
        console.log("Adding event listener to prevPageBtn");
        prevPageBtn.addEventListener('click', onPrevPage);
    }
    
    if (nextPageBtn) {
        console.log("Adding event listener to nextPageBtn");
        nextPageBtn.addEventListener('click', onNextPage);
    }
    
    if (zoomInBtn) {
        console.log("Adding event listener to zoomInBtn");
        zoomInBtn.addEventListener('click', onZoomIn);
    }
    
    if (zoomOutBtn) {
        console.log("Adding event listener to zoomOutBtn");
        zoomOutBtn.addEventListener('click', onZoomOut);
    }
    
    // Các cách khác nhau để tìm document ID
    let documentId;
    
    // Cách 1: Từ thuộc tính data-id
    const documentElement = document.querySelector('[data-id]');
    if (documentElement) {
        documentId = documentElement.getAttribute('data-id');
        console.log("Document ID found from data-id attribute:", documentId);
    }
    
    // Cách 2: Từ URL
    if (!documentId) {
        const urlParts = window.location.pathname.split('/');
        const potentialId = urlParts[urlParts.length - 1];
        if (!isNaN(potentialId)) {
            documentId = potentialId;
            console.log("Document ID found from URL:", documentId);
        }
    }
    
    // Cách 3: Từ ViewBag (nếu được truyền vào view)
    if (!documentId && window.documentId) {
        documentId = window.documentId;
        console.log("Document ID found from window variable:", documentId);
    }
    
    // Nếu tìm thấy document ID, tải PDF
    if (documentId) {
        loadPdf(`/Document/GetPdfPreview/${documentId}`);
    } else {
        // Nếu không tìm được ID, thử tải file cứng
        console.log("Document ID not found, trying hardcoded path");
        loadPdf('/uploads/documents/ke.pdf');
    }
    
    // Hàm tải PDF từ URL
    function loadPdf(url) {
        console.log("Loading PDF from:", url);
        
        // Sử dụng PDF.js để tải và hiển thị PDF
        const loadingTask = window.pdfjsLib.getDocument(url);
        
        loadingTask.promise.then(function(pdf) {
            console.log("PDF document loaded successfully");
            pdfDoc = pdf;
            
            // Show max 3 pages in preview
            const totalPages = Math.min(pdf.numPages, MAX_PREVIEW_PAGES);
            console.log(`Document has ${pdf.numPages} pages, showing max ${totalPages} pages`);
            
            if (totalPagesElem) {
                totalPagesElem.textContent = totalPages;
            }
            
            // Initial page render
            renderPage(pageNum);
        }).catch(function(error) {
            console.error('Error loading PDF document:', error);
            
            // Nếu không tải được từ URL đầu tiên, thử URL dự phòng
            if (url !== '/uploads/documents/ke.pdf') {
                console.log("Trying fallback PDF path");
                loadPdf('/uploads/documents/ke.pdf');
            } else {
                // Hiển thị thông báo lỗi trên canvas
                if (ctx) {
                    ctx.clearRect(0, 0, pdfContainer.width, pdfContainer.height);
                    ctx.font = '16px Arial';
                    ctx.fillText('Không thể tải tài liệu. Vui lòng thử lại sau.', 20, 50);
                    ctx.font = '14px Arial';
                    ctx.fillText(`Lỗi: ${error.message}`, 20, 75);
                }
            }
        });
    }
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