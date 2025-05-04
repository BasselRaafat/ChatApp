document.addEventListener('DOMContentLoaded', function() {
    // Tech Details Modal
    const techModal = document.getElementById('techModal');
    const techDetailsBtn = document.getElementById('techDetailsBtn');
    const closeTechModal = document.getElementById('closeTechModal');

    if (techDetailsBtn) {
        techDetailsBtn.addEventListener('click', function() {
            techModal.style.display = 'flex';
            document.body.style.overflow = 'hidden';
        });
    }

    if (closeTechModal) {
        closeTechModal.addEventListener('click', function() {
            techModal.style.display = 'none';
            document.body.style.overflow = 'auto';
        });
    }

    // Close modal when clicking outside content
    techModal.addEventListener('click', function(e) {
        if (e.target === techModal) {
            techModal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    });

    // Close modal with escape key
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape' && techModal.style.display === 'flex') {
            techModal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    });
});