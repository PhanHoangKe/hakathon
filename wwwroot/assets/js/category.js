document.addEventListener('DOMContentLoaded', function() {
    initViewToggle();
    initFilterShowMore();
    initDocumentCardActions();
    initFilterTagRemoval();
    initClearFilters();
    initLoadMore();
    initSearch();
});

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

        localStorage.setItem('sensrlib-view-preference', 'grid');
    });
    
    listViewBtn.addEventListener('click', function() {
        gridLayout.style.display = 'none';
        listLayout.style.display = 'flex';
        listViewBtn.classList.add('active');
        gridViewBtn.classList.remove('active');
 
        localStorage.setItem('sensrlib-view-preference', 'list');
    });

    const savedViewPreference = localStorage.getItem('sensrlib-view-preference');
    if (savedViewPreference === 'list') {
        listViewBtn.click();
    }
}

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

function initDocumentCardActions() {
    const actionButtons = document.querySelectorAll('.sensr-action-btn');
    
    actionButtons.forEach(button => {
        button.addEventListener('click', function(event) {
            event.stopPropagation();
            
            const action = this.classList.contains('sensr-view-btn') ? 'view' : 
                         this.classList.contains('sensr-download-btn') ? 'download' : 'favorite';
            const documentCard = this.closest('.sensr-document-card');
            const documentId = this.getAttribute('data-id');
            const documentTitle = documentCard.querySelector('.sensr-document-title').textContent;

            switch(action) {
                case 'view':
                    // Redirect to document details page
                    window.location.href = `/Home/Document/${documentId}`;
                    break;
                case 'download':
                    // Call download function
                    downloadDocument(documentId);
                    break;
                case 'favorite':
                    // Toggle favorite
                    toggleFavorite(documentId, this);
                    break;
            }
        });
    });

    const rowButtons = document.querySelectorAll('.sensr-row-btn');
    
    rowButtons.forEach(button => {
        button.addEventListener('click', function() {
            const action = this.classList.contains('sensr-view-doc') ? 'view' : 
                         this.classList.contains('sensr-download-doc') ? 'download' : 'favorite';
            const documentRow = this.closest('.sensr-document-row');
            const documentId = this.getAttribute('data-id');
            const documentTitle = documentRow.querySelector('.sensr-row-title').textContent;

            switch(action) {
                case 'view':
                    // Redirect to document details page
                    window.location.href = `/Home/Document/${documentId}`;
                    break;
                case 'download':
                    // Call download function
                    downloadDocument(documentId);
                    break;
                case 'favorite':
                    // Toggle favorite
                    toggleFavorite(documentId, this);
                    break;
            }
        });
    });
}

function initFilterTagRemoval() {
    const removeTags = document.querySelectorAll('.sensr-remove-tag');
    
    removeTags.forEach(tag => {
        tag.addEventListener('click', function() {
            const filterTag = this.closest('.sensr-filter-tag');
            const filterText = filterTag.textContent.trim().replace('×', '').trim();
            
            filterTag.remove();
            
            console.log(`Removed filter: ${filterText}`);
            
            updateResultCount();
        });
    });
}

function initClearFilters() {
    const clearButton = document.querySelector('.sensr-clear-filters');
    
    if (!clearButton) return;
    
    clearButton.addEventListener('click', function() {
        const activeTags = document.querySelectorAll('.sensr-filter-tag');
        activeTags.forEach(tag => tag.remove());
 
        const checkboxes = document.querySelectorAll('.sensr-filter-checkbox');
        checkboxes.forEach(checkbox => checkbox.checked = false);

        const rangeInputs = document.querySelectorAll('.sensr-range-input');
        rangeInputs.forEach(input => input.value = '');

        updateResultCount(256);
    });
}

function initLoadMore() {
    const loadMoreBtn = document.querySelector('.sensr-load-more-btn');
    
    if (!loadMoreBtn) return;
    
    let currentPage = 1;
    const totalPages = 11;
    
    loadMoreBtn.addEventListener('click', function() {
        currentPage++;
        
        document.querySelector('.sensr-page-count').textContent = `Trang ${currentPage} / ${totalPages}`;
        
        const gridContainer = document.querySelector('.sensr-grid-layout');
        const listContainer = document.querySelector('.sensr-list-layout');

        const documentCards = document.querySelectorAll('.sensr-document-card');
        documentCards.forEach((card, index) => {
            if (index < 4) { 
                const newCard = card.cloneNode(true);
                gridContainer.appendChild(newCard);
            }
        });

        const documentRows = document.querySelectorAll('.sensr-document-row');
        documentRows.forEach((row, index) => {
            if (index < 2) { 
                const newRow = row.cloneNode(true);
                listContainer.appendChild(newRow);
            }
        });

        initDocumentCardActions();

        updateResultCount();

        if (currentPage >= totalPages) {
            loadMoreBtn.style.display = 'none';
        }
    });
}

function initSearch() {
    const searchForms = document.querySelectorAll('.sensr-search-bar');
    
    searchForms.forEach(form => {
        form.addEventListener('submit', function(event) {
            event.preventDefault();
            
            const searchInput = this.querySelector('.sensr-search-input');
            const searchTerm = searchInput.value.trim();
            
            if (searchTerm) {
                alert(`Đang tìm kiếm: "${searchTerm}"`);
            }
        });
    });

    const searchButtons = document.querySelectorAll('.sensr-search-button');
    searchButtons.forEach(button => {
        button.addEventListener('click', function() {
            const form = this.closest('.sensr-search-bar');
            form.dispatchEvent(new Event('submit'));
        });
    });

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

function updateResultCount(totalCount) {
    const resultCountElement = document.querySelector('.sensr-result-count');
    if (!resultCountElement) return;

    const activeFilters = document.querySelectorAll('.sensr-filter-tag').length;
  
    const gridDocuments = document.querySelectorAll('.sensr-document-card').length;

    const total = totalCount || parseInt(resultCountElement.querySelector('strong:last-child').textContent) || 256;

    const displayedCount = activeFilters > 0 ? Math.min(gridDocuments, 24) : gridDocuments;
    
    resultCountElement.innerHTML = `Hiển thị <strong>${displayedCount}</strong> trong số <strong>${total}</strong> tài liệu`;
}