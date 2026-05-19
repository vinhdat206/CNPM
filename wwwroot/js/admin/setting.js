// =====================================
// FILE: wwwroot/js/admin/setting.js
// JS riêng cho trang Cài đặt Admin
// =====================================

// Lấy form đầu tiên trên trang
const settingForm = document.querySelector('form');

// Nếu tồn tại form thì gắn sự kiện submit
if (settingForm) {
    settingForm.addEventListener('submit', function () {

        // Hiện tại chỉ log để kiểm tra
        // Sau này có thể thêm validate trước khi gửi form
        console.log('Đang lưu cài đặt...');
    });
}