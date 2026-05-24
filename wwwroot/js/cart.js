// File: wwwroot/js/cart.js
// Mô tả: AJAX cart realtime

// ================= DOM READY =================

document.addEventListener("DOMContentLoaded", () => {

    // ================= INCREASE =================

    // lấy tất cả button tăng
    document.querySelectorAll(".increase-btn")
        .forEach(button => {

            // bắt sự kiện click
            button.addEventListener("click", async () => {

                // lấy cart item cha
                const cartItem =
                    button.closest(".cart-item");

                // lấy id sản phẩm
                const id =
                    cartItem.dataset.id;

                // gọi controller bằng AJAX
                const response =
                    await fetch(
                        `/Cart/Increase?id=${id}`,
                        {
                            method: "POST"
                        });

                // nhận json
                const data =
                    await response.json();

                // update UI
                updateCart(data);
            });
        });

    // ================= DECREASE =================

    document.querySelectorAll(".decrease-btn")
        .forEach(button => {

            button.addEventListener("click", async () => {

                const cartItem =
                    button.closest(".cart-item");

                const id =
                    cartItem.dataset.id;

                const response =
                    await fetch(
                        `/Cart/Decrease?id=${id}`,
                        {
                            method: "POST"
                        });

                const data =
                    await response.json();

                updateCart(data);
            });
        });

    // ================= REMOVE =================

    document.querySelectorAll(".remove-btn")
        .forEach(button => {

            button.addEventListener("click", async () => {

                const cartItem =
                    button.closest(".cart-item");

                const id =
                    cartItem.dataset.id;

                // animation fade out
                cartItem.style.opacity = "0";

                cartItem.style.transform =
                    "translateX(50px)";

                // delay animation
                setTimeout(async () => {

                    const response =
                        await fetch(
                            `/Cart/Remove?id=${id}`,
                            {
                                method: "POST"
                            });

                    const data =
                        await response.json();

                    updateCart(data);

                }, 300);
            });
        });

});

// ================= UPDATE UI =================

function updateCart(data)
{
    // update tổng tiền
    document.getElementById("cartTotal").innerText =
        data.total.toLocaleString("vi-VN") + " đ";

    // update phí ship
    document.getElementById("shippingFee").innerText =
        data.shippingFee.toLocaleString("vi-VN") + " đ";

    // update grand total
    document.getElementById("grandTotal").innerText =
        data.grandTotal.toLocaleString("vi-VN") + " đ";

    // update cart count
    const cartCount =
        document.getElementById("cartCount");

    if (cartCount) {
        cartCount.innerText = data.count;
    }

    // update từng item
    data.cart.forEach(item => {

        const cartItem =
            document.querySelector(
                `.cart-item[data-id='${item.productId}']`
            );

        if (!cartItem) return;

        // qty
        const qtyBox =
            cartItem.querySelector(".qty-value");

        if (qtyBox) {
            qtyBox.innerText = item.quantity;
        }

        // subtotal
        const subtotalBox =
            cartItem.querySelector(".subtotal");

        if (subtotalBox) {
            subtotalBox.innerText =
                (item.price * item.quantity)
                    .toLocaleString("vi-VN") + " đ";
        }

    });

    // nếu cart rỗng
    if (data.cart.length === 0) {

        const cartContainer =
            document.getElementById("cartContainer");

        if (cartContainer) {
            cartContainer.innerHTML = `
                <div class="text-center py-5">
                    <h4>Giỏ hàng trống</h4>
                </div>
            `;
        }
    }
}