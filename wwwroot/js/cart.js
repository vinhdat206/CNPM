// File: wwwroot/js/cart.js
// Chức năng:
// Xử lý giỏ hàng realtime bằng AJAX
// Không cần reload lại trang khi:
// - tăng số lượng
// - giảm số lượng
// - xóa sản phẩm

// ================= DOM READY =================

// Chờ cho toàn bộ HTML load xong rồi mới chạy JavaScript
document.addEventListener("DOMContentLoaded", () => {

    // ================= INCREASE =================

    // Lấy tất cả button có class increase-btn
    // Ví dụ:
    // <button class="increase-btn">+</button>
    document.querySelectorAll(".increase-btn")
        .forEach(button => {

            // Gắn sự kiện click cho từng button
            button.addEventListener("click", async () => {

                // Tìm phần tử cha gần nhất có class cart-item
                // để biết user đang thao tác với sản phẩm nào
                const cartItem =
                    button.closest(".cart-item");

                // Lấy id sản phẩm từ data-id
                // Ví dụ:
                // <div class="cart-item" data-id="5">
                const id =
                    cartItem.dataset.id;

                // Gửi request AJAX đến Controller
                // URL:
                // /Cart/Increase?id=5
                const response =
                    await fetch(
                        `/Cart/Increase?id=${id}`,
                        {
                            // Gửi bằng phương thức POST
                            method: "POST"
                        });

                // Chuyển dữ liệu trả về sang JSON
                const data =
                    await response.json();

                // Cập nhật lại giao diện realtime
                updateCart(data);
            });
        });

    // ================= DECREASE =================

    // Lấy tất cả button giảm số lượng
    document.querySelectorAll(".decrease-btn")
        .forEach(button => {

            // Bắt sự kiện click
            button.addEventListener("click", async () => {

                // Lấy cart item cha
                const cartItem =
                    button.closest(".cart-item");

                // Lấy id sản phẩm
                const id =
                    cartItem.dataset.id;

                // Gửi AJAX đến action Decrease
                const response =
                    await fetch(
                        `/Cart/Decrease?id=${id}`,
                        {
                            method: "POST"
                        });

                // Nhận dữ liệu JSON trả về
                const data =
                    await response.json();

                // Update giao diện
                updateCart(data);
            });
        });

    // ================= REMOVE =================

    // Lấy tất cả button xóa sản phẩm
    document.querySelectorAll(".remove-btn")
        .forEach(button => {

            // Bắt sự kiện click
            button.addEventListener("click", async () => {

                // Lấy cart item cha
                const cartItem =
                    button.closest(".cart-item");

                // Lấy id sản phẩm
                const id =
                    cartItem.dataset.id;

                // ================= ANIMATION =================

                // Làm item mờ dần
                cartItem.style.opacity = "0";

                // Di chuyển item sang phải
                cartItem.style.transform =
                    "translateX(50px)";

                // Delay 300ms để animation chạy xong
                setTimeout(async () => {

                    // Gửi AJAX đến action Remove
                    const response =
                        await fetch(
                            `/Cart/Remove?id=${id}`,
                            {
                                method: "POST"
                            });

                    // Nhận JSON trả về
                    const data =
                        await response.json();

                    // Update lại giao diện
                    updateCart(data);

                }, 300);
            });
        });

});

// ================= UPDATE UI =================

// Hàm cập nhật giao diện giỏ hàng
function updateCart(data)
{
    // ================= TOTAL =================

    // Update tổng tiền hàng
    document.getElementById("cartTotal").innerText =
        data.total.toLocaleString("vi-VN") + " đ";

    // toLocaleString("vi-VN")
    // dùng để format tiền Việt Nam
    // Ví dụ:
    // 100000 -> 100.000

    // ================= SHIPPING =================

    // Update phí vận chuyển
    document.getElementById("shippingFee").innerText =
        data.shippingFee.toLocaleString("vi-VN") + " đ";

    // ================= GRAND TOTAL =================

    // Update tổng thanh toán cuối cùng
    document.getElementById("grandTotal").innerText =
        data.grandTotal.toLocaleString("vi-VN") + " đ";

    // ================= CART COUNT =================

    // Lấy element hiển thị số lượng cart
    // Ví dụ icon:
    // 🛒 5
    const cartCount =
        document.getElementById("cartCount");

    // Nếu tồn tại thì update số lượng
    if (cartCount) {
        cartCount.innerText = data.count;
    }

    // ================= UPDATE ITEMS =================

    // Lặp qua từng sản phẩm trong cart
    data.cart.forEach(item => {

        // Tìm cart-item tương ứng trong HTML
        const cartItem =
            document.querySelector(
                `.cart-item[data-id='${item.productId}']`
            );

        // Nếu không tìm thấy thì bỏ qua
        if (!cartItem) return;

        // ================= UPDATE QUANTITY =================

        // Tìm ô hiển thị số lượng
        const qtyBox =
            cartItem.querySelector(".qty-value");

        // Nếu tồn tại thì update số lượng mới
        if (qtyBox) {
            qtyBox.innerText = item.quantity;
        }

        // ================= UPDATE SUBTOTAL =================

        // Tìm ô hiển thị thành tiền
        const subtotalBox =
            cartItem.querySelector(".subtotal");

        // Nếu tồn tại thì update thành tiền mới
        if (subtotalBox) {

            // Thành tiền = giá × số lượng
            subtotalBox.innerText =
                (item.price * item.quantity)
                    .toLocaleString("vi-VN") + " đ";
        }

    });

    // ================= EMPTY CART =================

    // Nếu không còn sản phẩm nào trong cart
    if (data.cart.length === 0) {

        // Lấy container giỏ hàng
        const cartContainer =
            document.getElementById("cartContainer");

        // Nếu tồn tại thì hiển thị cart rỗng
        if (cartContainer) {
            cartContainer.innerHTML = `
                <div class="text-center py-5">

                    <!-- Thông báo giỏ hàng rỗng -->
                    <h4>Giỏ hàng trống</h4>

                </div>
            `;
        }
    }
}