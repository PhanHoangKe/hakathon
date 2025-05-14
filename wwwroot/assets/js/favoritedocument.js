/**
 * SenseLib - Trang tài liệu yêu thích
 * JavaScript xử lý các tương tác người dùng
 */

document.addEventListener('DOMContentLoaded', function() {
    // Khởi tạo các biến và elements
    const favoriteList = document.querySelector('.sensu-favorite-list');
    const emptyState = document.querySelector('.sensu-favorite-empty');
    const searchInput = document.querySelector('.sensu-favorite-search-input');
    const sortSelect = document.querySelector('.sensu-favorite-sort-select');
    const favoriteItems = document.querySelectorAll('.sensu-favorite-item');
    
    // Mảng lưu trữ thông tin tài liệu (ví dụ mẫu - trong thực tế sẽ lấy từ API)
    let favoriteDocuments = [
        {
            id: 1,
            title: "Lập trình web với React và Node.js",
            author: "Nguyễn Văn A",
            dateAdded: "10/05/2025",
            description: "Tài liệu hướng dẫn chi tiết về cách xây dựng ứng dụng web hiện đại sử dụng React cho front-end và Node.js cho back-end.",
            tags: ["Web", "Lập trình", "JavaScript"],
            image: "/api/placeholder/150/200" // Sử dụng placeholder
        },
        {
            id: 2,
            title: "Trí tuệ nhân tạo - Từ cơ bản đến nâng cao",
            author: "Trần Thị B",
            dateAdded: "05/05/2025",
            description: "Giới thiệu về các khái niệm cơ bản và kỹ thuật tiên tiến trong lĩnh vực trí tuệ nhân tạo, bao gồm machine learning và deep learning.",
            tags: ["AI", "Machine Learning", "Data Science"],
            image: "/api/placeholder/150/200" // Sử dụng placeholder
        }
    ];
    
    /**
     * Hiển thị trạng thái rỗng nếu không có tài liệu yêu thích
     */
    function checkEmptyState() {
        if (favoriteDocuments.length === 0) {
            favoriteList.style.display = 'none';
            emptyState.style.display = 'block';
            document.querySelector('.sensu-favorite-pagination').style.display = 'none';
        } else {
            favoriteList.style.display = 'flex';
            emptyState.style.display = 'none';
            document.querySelector('.sensu-favorite-pagination').style.display = 'flex';
        }
    }
    
    /**
     * Xử lý khi người dùng bỏ yêu thích tài liệu
     */
    function handleRemoveFavorite() {
        const unfavoriteButtons = document.querySelectorAll('.sensu-btn-unfavorite');
        
        unfavoriteButtons.forEach(button => {
            button.addEventListener('click', function(e) {
                e.preventDefault();
                const docId = parseInt(this.dataset.id);
                const confirmRemove = confirm('Bạn có chắc muốn bỏ yêu thích tài liệu này?');
                
                if (confirmRemove) {
                    // Tìm và xóa tài liệu khỏi danh sách yêu thích
                    favoriteDocuments = favoriteDocuments.filter(doc => doc.id !== docId);
                    
                    // Xóa item khỏi DOM
                    const itemToRemove = this.closest('.sensu-favorite-item');
                    itemToRemove.classList.add('sensu-item-removing');
                    
                    // Animation khi xóa
                    itemToRemove.style.opacity = '0';
                    itemToRemove.style.transform = 'translateX(50px)';
                    
                    setTimeout(() => {
                        itemToRemove.remove();
                        checkEmptyState();
                        
                        // Hiển thị thông báo
                        showNotification('Đã xóa khỏi danh sách yêu thích');
                    }, 300);
                }
            });
        });
    }
    
    /**
     * Hiển thị thông báo cho người dùng
     */
    function showNotification(message) {
        // Kiểm tra xem đã có notification container chưa
        let notifContainer = document.querySelector('.sensu-notification-container');
        
        if (!notifContainer) {
            notifContainer = document.createElement('div');
            notifContainer.className = 'sensu-notification-container';
            notifContainer.style.position = 'fixed';
            notifContainer.style.bottom = '20px';
            notifContainer.style.right = '20px';
            notifContainer.style.zIndex = '9999';
            document.body.appendChild(notifContainer);
        }
        
        // Tạo notification mới
        const notification = document.createElement('div');
        notification.className = 'sensu-notification';
        notification.innerHTML = `
            <div style="background-color: #fff; border-left: 4px solid #0bb9d7; padding: 15px 20px; 
                       box-shadow: 0 4px 12px rgba(0,0,0,0.15); margin-top: 10px; border-radius: 4px;
                       display: flex; align-items: center;">
                <i class="fas fa-check-circle" style="color: #0bb9d7; margin-right: 10px;"></i>
                <span>${message}</span>
            </div>
        `;
        
        notifContainer.appendChild(notification);
        
        // Xóa notification sau 3 giây
        setTimeout(() => {
            notification.style.opacity = '0';
            setTimeout(() => {
                notification.remove();
            }, 300);
        }, 3000);
    }
    
    /**
     * Xử lý tìm kiếm tài liệu
     */
    function handleSearch() {
        searchInput.addEventListener('input', function() {
            const searchTerm = this.value.toLowerCase().trim();
            
            // Lọc tất cả các mục hiện có
            const items = document.querySelectorAll('.sensu-favorite-item');
            
            items.forEach(item => {
                const title = item.querySelector('.sensu-favorite-item-title').textContent.toLowerCase();
                const author = item.querySelector('.sensu-favorite-item-author').textContent.toLowerCase();
                const description = item.querySelector('.sensu-favorite-item-desc').textContent.toLowerCase();
                
                // Kiểm tra nếu nội dung khớp với từ khóa tìm kiếm
                if (title.includes(searchTerm) || author.includes(searchTerm) || description.includes(searchTerm)) {
                    item.style.display = 'flex';
                } else {
                    item.style.display = 'none';
                }
            });
            
            // Kiểm tra nếu không có kết quả nào
            let hasVisibleItems = false;
            items.forEach(item => {
                if (item.style.display !== 'none') {
                    hasVisibleItems = true;
                }
            });
            
            // Hiển thị thông báo không có kết quả
            if (!hasVisibleItems && searchTerm !== '') {
                if (!document.querySelector('.sensu-no-results')) {
                    const noResults = document.createElement('div');
                    noResults.className = 'sensu-no-results';
                    noResults.innerHTML = `
                        <div style="text-align: center; padding: 30px; background: white; border-radius: 8px; margin-top: 20px;">
                            <i class="fas fa-search" style="font-size: 2rem; color: #ddd; margin-bottom: 15px;"></i>
                            <h3>Không tìm thấy kết quả</h3>
                            <p>Không có tài liệu nào khớp với từ khóa "${searchTerm}"</p>
                        </div>
                    `;
                    favoriteList.after(noResults);
                }
            } else {
                const noResults = document.querySelector('.sensu-no-results');
                if (noResults) {
                    noResults.remove();
                }
            }
        });
    }
    
    /**
     * Xử lý sắp xếp tài liệu
     */
    function handleSort() {
        sortSelect.addEventListener('change', function() {
            const sortOption = this.value;
            const items = Array.from(document.querySelectorAll('.sensu-favorite-item'));
            
            // Sắp xếp các mục dựa trên lựa chọn
            items.sort((a, b) => {
                if (sortOption === 'recent') {
                    // Sắp xếp theo ngày (giả định là ngày gần đây nhất trước)
                    const dateA = a.querySelector('.sensu-favorite-item-date').textContent.replace('Ngày thêm: ', '');
                    const dateB = b.querySelector('.sensu-favorite-item-date').textContent.replace('Ngày thêm: ', '');
                    return new Date(dateB.split('/').reverse().join('-')) - new Date(dateA.split('/').reverse().join('-'));
                } else if (sortOption === 'name') {
                    // Sắp xếp theo tên
                    const nameA = a.querySelector('.sensu-favorite-item-title').textContent;
                    const nameB = b.querySelector('.sensu-favorite-item-title').textContent;
                    return nameA.localeCompare(nameB);
                } else if (sortOption === 'author') {
                    // Sắp xếp theo tác giả
                    const authorA = a.querySelector('.sensu-favorite-item-author').textContent.replace('Tác giả: ', '');
                    const authorB = b.querySelector('.sensu-favorite-item-author').textContent.replace('Tác giả: ', '');
                    return authorA.localeCompare(authorB);
                }
                return 0;
            });
            
            // Cập nhật DOM với thứ tự mới
            const parent = favoriteList;
            items.forEach(item => {
                parent.appendChild(item);
            });
        });
    }
    
    /**
     * Xử lý chuyển hướng khi click vào nút xem chi tiết
     */
    function handleViewDetails() {
        const viewButtons = document.querySelectorAll('.sensu-btn-view');
        
        viewButtons.forEach(button => {
            button.addEventListener('click', function() {
                const docId = this.closest('.sensu-favorite-item').querySelector('.sensu-btn-unfavorite').dataset.id;
                // Trong thực tế, chuyển hướng đến trang chi tiết tài liệu
                window.location.href = `document-detail.html?id=${docId}`;
            });
        });
    }
    
    /**
     * Xử lý tải xuống tài liệu
     */
    function handleDownload() {
        const downloadButtons = document.querySelectorAll('.sensu-btn-download');
        
        downloadButtons.forEach(button => {
            button.addEventListener('click', function() {
                const docId = this.closest('.sensu-favorite-item').querySelector('.sensu-btn-unfavorite').dataset.id;
                
                // Thông báo tải xuống thành công (trong thực tế sẽ gọi API để tải file)
                showNotification('Đang tải xuống tài liệu...');
                
                // Giả lập quá trình tải xuống
                setTimeout(() => {
                    showNotification('Tải xuống hoàn tất!');
                }, 2000);
            });
        });
    }
    
    /**
     * Xử lý phân trang
     */
    function handlePagination() {
        const pagePrev = document.querySelector('.sensu-pagination-prev');
        const pageNext = document.querySelector('.sensu-pagination-next');
        const pageNumbers = document.querySelectorAll('.sensu-pagination-number');
        
        // Khi người dùng nhấp vào số trang
        pageNumbers.forEach(number => {
            number.addEventListener('click', function() {
                // Xóa trạng thái active hiện tại
                document.querySelector('.sensu-pagination-number.active').classList.remove('active');
                // Thiết lập trạng thái active mới
                this.classList.add('active');
                
                // Cập nhật nút trước/sau
                if (this.textContent === '1') {
                    pagePrev.setAttribute('disabled', 'disabled');
                } else {
                    pagePrev.removeAttribute('disabled');
                }
                
                if (this.textContent === '10') {
                    pageNext.setAttribute('disabled', 'disabled');
                } else {
                    pageNext.removeAttribute('disabled');
                }
                
                // Trong thực tế, tải dữ liệu trang mới từ API
                // Ví dụ giả lập việc tải dữ liệu mới
                simulateLoading();
            });
        });
        
        // Khi người dùng nhấp vào nút Trước
        pagePrev.addEventListener('click', function() {
            if (this.hasAttribute('disabled')) return;
            
            const activePage = document.querySelector('.sensu-pagination-number.active');
            const currentPage = parseInt(activePage.textContent);
            const prevPage = currentPage - 1;
            
            // Cập nhật UI
            activePage.classList.remove('active');
            
            const newActivePage = Array.from(pageNumbers).find(num => parseInt(num.textContent) === prevPage);
            if (newActivePage) {
                newActivePage.classList.add('active');
                
                if (prevPage === 1) {
                    this.setAttribute('disabled', 'disabled');
                }
                
                pageNext.removeAttribute('disabled');
                
                // Giả lập tải dữ liệu
                simulateLoading();
            }
        });
        
        // Khi người dùng nhấp vào nút Sau
        pageNext.addEventListener('click', function() {
            if (this.hasAttribute('disabled')) return;
            
            const activePage = document.querySelector('.sensu-pagination-number.active');
            const currentPage = parseInt(activePage.textContent);
            const nextPage = currentPage + 1;
            
            // Cập nhật UI
            activePage.classList.remove('active');
            
            const newActivePage = Array.from(pageNumbers).find(num => parseInt(num.textContent) === nextPage);
            if (newActivePage) {
                newActivePage.classList.add('active');
                
                if (nextPage === 10) {
                    this.setAttribute('disabled', 'disabled');
                }
                
                pagePrev.removeAttribute('disabled');
                
                // Giả lập tải dữ liệu
                simulateLoading();
            }
        });
    }
    
    /**
     * Giả lập tải dữ liệu khi chuyển trang
     */
    function simulateLoading() {
        // Hiển thị trạng thái đang tải
        favoriteList.innerHTML = `
            <div style="width: 100%; text-align: center; padding: 40px;">
                <div class="sensu-loading" style="display: inline-block; width: 40px; height: 40px; border: 3px solid rgba(11, 185, 215, 0.3); 
                                         border-radius: 50%; border-top-color: #0bb9d7; animation: sensu-spin 1s linear infinite;"></div>
                <style>
                    @keyframes sensu-spin {
                        to { transform: rotate(360deg); }
                    }
                </style>
                <p style="margin-top: 15px; color: #666;">Đang tải dữ liệu...</p>
            </div>
        `;
        
        // Giả lập việc tải dữ liệu (thời gian thực sẽ gọi API)
        setTimeout(() => {
            // Khôi phục nội dung ban đầu sau khi "tải xong"
            renderFavoriteItems();
            
            // Khởi tạo lại các event listener
            handleRemoveFavorite();
            handleViewDetails();
            handleDownload();
        }, 1000);
    }
    
    /**
     * Render lại danh sách tài liệu yêu thích
     */
    function renderFavoriteItems() {
        // Trong thực tế, dữ liệu sẽ được lấy từ API
        // Render giao diện dựa trên dữ liệu mẫu
        if (favoriteDocuments.length === 0) {
            checkEmptyState();
            return;
        }
        
        let html = '';
        favoriteDocuments.forEach(doc => {
            html += `
                <div class="sensu-favorite-item">
                    <div class="sensu-favorite-item-image">
                        <img src="${doc.image}" alt="Tài liệu">
                    </div>
                    <div class="sensu-favorite-item-content">
                        <h3 class="sensu-favorite-item-title">${doc.title}</h3>
                        <div class="sensu-favorite-item-meta">
                            <span class="sensu-favorite-item-author">Tác giả: ${doc.author}</span>
                            <span class="sensu-favorite-item-date">Ngày thêm: ${doc.dateAdded}</span>
                        </div>
                        <p class="sensu-favorite-item-desc">${doc.description}</p>
                        <div class="sensu-favorite-item-tags">
                            ${doc.tags.map(tag => `<span class="sensu-tag">${tag}</span>`).join('')}
                        </div>
                        <div class="sensu-favorite-item-actions">
                            <button class="sensu-btn sensu-btn-view">
                                <i class="fas fa-eye"></i> Xem chi tiết
                            </button>
                            <button class="sensu-btn sensu-btn-download">
                                <i class="fas fa-download"></i> Tải xuống
                            </button>
                            <button class="sensu-btn sensu-btn-unfavorite" data-id="${doc.id}">
                                <i class="fas fa-heart-broken"></i> Bỏ yêu thích
                            </button>
                        </div>
                    </div>
                </div>
            `;
        });
        
        favoriteList.innerHTML = html;
    }
    
    // Khởi tạo các chức năng trang
    function initPage() {
        checkEmptyState();
        handleRemoveFavorite();
        handleSearch();
        handleSort();
        handleViewDetails();
        handleDownload();
        handlePagination();
    }
    
    // Khởi chạy trang
    initPage();
});