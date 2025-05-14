/**
 * sensrLIB JAVASCRIPT
 * Document Library Functionality
 */

document.addEventListener('DOMContentLoaded', function() {
    // Toggle view between grid and list
    initViewToggle();
    
    // Show/hide additional filter options
    initFilterShowMore();
    
    // Document card hover actions
    initDocumentCardActions();
    
    // Filter tag removal
    initFilterTagRemoval();
    
    // Clear all filters
    initClearFilters();
    
    // Load more functionality
    initLoadMore();
    
    // Init search functionality
    initSearch();
});

/**
 * Initialize view toggle between grid and list views
 */
function initViewToggle() {
    const gridViewBtn = document.querySelector('.sensr-grid-view');
    const listViewBtn = document.querySelector('.sensr-list-view');
    const gridLayout = document.querySelector('.sensr-grid-layout');
    const listLayout = document.querySelector('.sensr-list-layout');
    
    if (!gridViewBtn || !listViewBtn || !gridLayout || !listLayout) return;
    
    gridViewBtn.addEventListener('click', function() {
        gridLayout.style.display = 'grid';
        listLayout.style.display = 'none';
        gridViewBtn.classList.add('active');
        listViewBtn.classList.remove('active');
        
        // Store preference in localStorage
        localStorage.setItem('sensrlib-view-preference', 'grid');
    });
    
    listViewBtn.addEventListener('click', function() {
        gridLayout.style.display = 'none';
        listLayout.style.display = 'flex';
        listViewBtn.classList.add('active');
        gridViewBtn.classList.remove('active');
        
        // Store preference in localStorage
        localStorage.setItem('sensrlib-view-preference', 'list');
    });
    
    // Check for saved preference
    const savedViewPreference = localStorage.getItem('sensrlib-view-preference');
    if (savedViewPreference === 'list') {
        listViewBtn.click();
    }
}

/**
 * Initialize "Show More" functionality for filters
 */
function initFilterShowMore() {
    const showMoreButtons = document.querySelectorAll('.sensr-show-more');
    
    showMoreButtons.forEach(button => {
        let expanded = false;
        const filterOptions = button.closest('.sensr-filter-content').querySelector('.sensr-filter-options');
        const initialHeight = filterOptions.style.maxHeight;
        
        button.addEventListener('click', function() {
            if (!expanded) {
                filterOptions.style.maxHeight = 'none';
                button.querySelector('span').textContent = 'Thu gọn';
                button.querySelector('i').classList.remove('fa-chevron-down');
                button.querySelector('i').classList.add('fa-chevron-up');
            } else {
                filterOptions.style.maxHeight = initialHeight || '200px';
                button.querySelector('span').textContent = 'Xem thêm';
                button.querySelector('i').classList.remove('fa-chevron-up');
                button.querySelector('i').classList.add('fa-chevron-down');
            }
            expanded = !expanded;
        });
    });
}

/**
 * Initialize document card hover actions
 */
function initDocumentCardActions() {
    // Grid view actions
    const actionButtons = document.querySelectorAll('.sensr-action-btn');
    
    actionButtons.forEach(button => {
        button.addEventListener('click', function(event) {
            event.stopPropagation();
            
            const action = this.classList.contains('sensr-view-btn') ? 'view' : 
                         this.classList.contains('sensr-download-btn') ? 'download' : 'favorite';
            const documentCard = this.closest('.sensr-document-card');
            const documentTitle = documentCard.querySelector('.sensr-document-title').textContent;
            
            // In a real application, you would handle these actions differently
            // This is just a demo
            switch(action) {
                case 'view':
                    alert(`Đang mở tài liệu: ${documentTitle}`);
                    break;
                case 'download':
                    alert(`Đang tải tài liệu: ${documentTitle}`);
                    break;
                case 'favorite':
                    if (this.querySelector('i').classList.contains('far')) {
                        this.querySelector('i').classList.remove('far');
                        this.querySelector('i').classList.add('fas');
                        alert(`Đã thêm ${documentTitle} vào danh sách yêu thích`);
                    } else {
                        this.querySelector('i').classList.remove('fas');
                        this.querySelector('i').classList.add('far');
                        alert(`Đã xóa ${documentTitle} khỏi danh sách yêu thích`);
                    }
                    break;
            }
        });
    });
    
    // List view actions
    const rowButtons = document.querySelectorAll('.sensr-row-btn');
    
    rowButtons.forEach(button => {
        button.addEventListener('click', function() {
            const action = this.classList.contains('sensr-view-doc') ? 'view' : 
                         this.classList.contains('sensr-download-doc') ? 'download' : 'favorite';
            const documentRow = this.closest('.sensr-document-row');
            const documentTitle = documentRow.querySelector('.sensr-row-title').textContent;
            
            // In a real application, you would handle these actions differently
            switch(action) {
                case 'view':
                    alert(`Đang mở tài liệu: ${documentTitle}`);
                    break;
                case 'download':
                    alert(`Đang tải tài liệu: ${documentTitle}`);
                    break;
                case 'favorite':
                    if (this.querySelector('i').classList.contains('far')) {
                        this.querySelector('i').classList.remove('far');
                        this.querySelector('i').classList.add('fas');
                        alert(`Đã thêm ${documentTitle} vào danh sách yêu thích`);
                    } else {
                        this.querySelector('i').classList.remove('fas');
                        this.querySelector('i').classList.add('far');
                        alert(`Đã xóa ${documentTitle} khỏi danh sách yêu thích`);
                    }
                    break;
            }
        });
    });
}

/**
 * Initialize filter tag removal
 */
function initFilterTagRemoval() {
    const removeTags = document.querySelectorAll('.sensr-remove-tag');
    
    removeTags.forEach(tag => {
        tag.addEventListener('click', function() {
            const filterTag = this.closest('.sensr-filter-tag');
            const filterText = filterTag.textContent.trim().replace('×', '').trim();
            
            // Remove the tag from the UI
            filterTag.remove();
            
            // In a real application, you would also update the filter state
            console.log(`Removed filter: ${filterText}`);
            
            // Update the result count - in a real app this would come from your data
            updateResultCount();
        });
    });
}

/**
 * Initialize clear all filters button
 */
function initClearFilters() {
    const clearButton = document.querySelector('.sensr-clear-filters');
    
    if (!clearButton) return;
    
    clearButton.addEventListener('click', function() {
        // Clear all active filter tags
        const activeTags = document.querySelectorAll('.sensr-filter-tag');
        activeTags.forEach(tag => tag.remove());
        
        // Uncheck all filter checkboxes
        const checkboxes = document.querySelectorAll('.sensr-filter-checkbox');
        checkboxes.forEach(checkbox => checkbox.checked = false);
        
        // Clear range inputs
        const rangeInputs = document.querySelectorAll('.sensr-range-input');
        rangeInputs.forEach(input => input.value = '');
        
        // Reset result count - in a real app would come from your data
        updateResultCount(256);
    });
}

/**
 * Initialize load more functionality
 */
function initLoadMore() {
    const loadMoreBtn = document.querySelector('.sensr-load-more-btn');
    
    if (!loadMoreBtn) return;
    
    let currentPage = 1;
    const totalPages = 11;
    
    loadMoreBtn.addEventListener('click', function() {
        // Simulate loading more content
        currentPage++;
        
        // Update the page counter
        document.querySelector('.sensr-page-count').textContent = `Trang ${currentPage} / ${totalPages}`;
        
        // In a real application, you would fetch more documents and append them to the grid/list
        // For demo purposes, we'll just duplicate existing documents
        
        const gridContainer = document.querySelector('.sensr-grid-layout');
        const listContainer = document.querySelector('.sensr-list-layout');
        
        // Clone some document cards and append to grid
        const documentCards = document.querySelectorAll('.sensr-document-card');
        documentCards.forEach((card, index) => {
            if (index < 4) { // Just duplicate the first 4 cards
                const newCard = card.cloneNode(true);
                gridContainer.appendChild(newCard);
            }
        });
        
        // Clone some document rows and append to list
        const documentRows = document.querySelectorAll('.sensr-document-row');
        documentRows.forEach((row, index) => {
            if (index < 2) { // Just duplicate the first 2 rows
                const newRow = row.cloneNode(true);
                listContainer.appendChild(newRow);
            }
        });
        
        // Reinitialize document actions for the new elements
        initDocumentCardActions();
        
        // Update result count
        updateResultCount();
        
        // Hide load more button if we've reached the last page
        if (currentPage >= totalPages) {
            loadMoreBtn.style.display = 'none';
        }
    });
}

/**
 * Initialize search functionality
 */
function initSearch() {
    const searchForms = document.querySelectorAll('.sensr-search-bar');
    
    searchForms.forEach(form => {
        form.addEventListener('submit', function(event) {
            event.preventDefault();
            
            const searchInput = this.querySelector('.sensr-search-input');
            const searchTerm = searchInput.value.trim();
            
            if (searchTerm) {
                // In a real application, you would perform a search and update results
                alert(`Đang tìm kiếm: "${searchTerm}"`);
            }
        });
    });
    
    // Add submit on search button click
    const searchButtons = document.querySelectorAll('.sensr-search-button');
    searchButtons.forEach(button => {
        button.addEventListener('click', function() {
            const form = this.closest('.sensr-search-bar');
            form.dispatchEvent(new Event('submit'));
        });
    });
    
    // Filter search inputs (not main search)
    const filterSearchInputs = document.querySelectorAll('.sensr-filter-search-input');
    filterSearchInputs.forEach(input => {
        input.addEventListener('keyup', function() {
            const searchTerm = this.value.toLowerCase().trim();
            const filterOptions = this.closest('.sensr-filter-content').querySelectorAll('.sensr-filter-option');
            
            filterOptions.forEach(option => {
                const label = option.querySelector('label').textContent.toLowerCase();
                if (label.includes(searchTerm)) {
                    option.style.display = 'flex';
                } else {
                    option.style.display = 'none';
                }
            });
        });
    });
}

/**
 * Update result count - in a real app this would be more sophisticated
 */
function updateResultCount(totalCount) {
    const resultCountElement = document.querySelector('.sensr-result-count');
    if (!resultCountElement) return;
    
    // Count active filters
    const activeFilters = document.querySelectorAll('.sensr-filter-tag').length;
    
    // Current count of displayed documents
    const gridDocuments = document.querySelectorAll('.sensr-document-card').length;
    
    // Total count - if not provided, use what's in the UI or default
    const total = totalCount || parseInt(resultCountElement.querySelector('strong:last-child').textContent) || 256;
    
    // If we have active filters, show a reduced count
    const displayedCount = activeFilters > 0 ? Math.min(gridDocuments, 24) : gridDocuments;
    
    resultCountElement.innerHTML = `Hiển thị <strong>${displayedCount}</strong> trong số <strong>${total}</strong> tài liệu`;
}