// Sidebar toggle script
document.addEventListener('DOMContentLoaded', function () {
    var toggleBtn = document.getElementById('sidebarToggle');
    var sidebar = document.getElementById('sidebar');
    var mainContent = document.getElementById('mainContent');

    toggleBtn?.addEventListener('click', function () {
        sidebar.classList.toggle('sidebar-collapsed');
        mainContent.classList.toggle('collapsed');
    });
});
