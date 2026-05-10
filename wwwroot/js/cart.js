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
    // reload lại cart
    location.reload();
}