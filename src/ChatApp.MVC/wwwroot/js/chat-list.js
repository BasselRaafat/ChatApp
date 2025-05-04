document.addEventListener('DOMContentLoaded', function() {
    // Search functionality
    const searchInput = document.querySelector('.search-input');
    if (searchInput) {
        searchInput.addEventListener('input', function() {
            const searchTerm = this.value.toLowerCase();
            const chatItems = document.querySelectorAll('.chat-item');

            chatItems.forEach(item => {
                const name = item.querySelector('.chat-name').textContent.toLowerCase();
                item.style.display = name.includes(searchTerm) ? 'flex' : 'none';
            });
        });
    }

    // NiceScroll initialization if needed
    if ($.fn.niceScroll) {
        $(".chat-items-container").niceScroll({
            cursorcolor: "#7c4dff",
            cursorwidth: "6px",
            cursorborderradius: "3px"
        });
    }
});