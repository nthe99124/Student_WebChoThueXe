﻿$('.fav-item-product').off('click').on('click', function (event) {
    event.preventDefault();
    event.stopPropagation();

    var current = $(event.target).parents('.fav-item-product'),
        productId = current.data('id');

    var formData = new FormData();
    formData.append("productId", productId);
    $.ajax({
        type: "POST",
        url: "/Product/AddOrRemoveFavourite",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response && response.success) {
                if (response.isCheckFavorite) {
                    current.empty();
                    current.append(`<svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
											<path d="M18.3644 2.45408C17.33 1.34216 15.8804 0.709493 14.3617 0.707148C12.8417 0.708907 11.3906 1.34124 10.3546 2.45337L10.0015 2.8265L9.64836 2.45337C7.59251 0.240709 4.13215 0.11364 1.91953 2.16949C1.82148 2.26063 1.72679 2.35528 1.63565 2.45337C-0.545218 4.80571 -0.545218 8.44113 1.63565 10.7935L9.48255 19.0685C9.62307 19.2169 9.81208 19.2917 10.0015 19.2918C10.178 19.2918 10.3548 19.2269 10.493 19.0959C10.5024 19.087 10.5115 19.0779 10.5204 19.0685L18.3645 10.7935C20.5452 8.44139 20.5452 4.80617 18.3644 2.45408Z" fill="#c93795" style="background: #c93795;"></path>
											<path fill-rule="evenodd" clip-rule="evenodd" d="M13.3999 4.00039C13.3999 3.66902 13.6685 3.40039 13.9999 3.40039C14.2037 3.40039 14.4039 3.41738 14.5992 3.45014C14.926 3.50497 15.1465 3.81434 15.0916 4.14114C15.0368 4.46795 14.7274 4.68843 14.4006 4.6336C14.2707 4.6118 14.1369 4.60039 13.9999 4.60039C13.6685 4.60039 13.3999 4.33176 13.3999 4.00039ZM15.6805 4.44389C15.9133 4.20808 16.2932 4.20565 16.529 4.43845C17.1895 5.09049 17.5999 5.99806 17.5999 7.00039C17.5999 7.42008 17.5278 7.82423 17.3949 8.20034C17.2845 8.51277 16.9417 8.67652 16.6292 8.56609C16.3168 8.45567 16.1531 8.11287 16.2635 7.80044C16.3517 7.55099 16.3999 7.28197 16.3999 7.00039C16.3999 6.33194 16.1274 5.72824 15.686 5.2924C15.4501 5.0596 15.4477 4.67971 15.6805 4.44389Z" fill="white"></path>
										</svg>`);
                }
                else {
                    current.empty();
                    current.append(`<svg width="20" height="20" viewBox="0 0 20 20" fill="none" xmlns="http://www.w3.org/2000/svg">
											<path d="M17.7083 7.26666C17.7083 8.23333 17.3417 9.20833 16.6 9.95L15.3667 11.1833L10.0583 16.4917C10.0333 16.5167 10.025 16.525 10 16.5417C9.97501 16.525 9.96667 16.5167 9.94167 16.4917L3.40001 9.95C2.65834 9.20833 2.29167 8.24166 2.29167 7.26666C2.29167 6.29166 2.65834 5.31667 3.40001 4.575C4.88334 3.1 7.28334 3.1 8.76667 4.575L9.99167 5.80833L11.225 4.575C12.7083 3.1 15.1 3.1 16.5833 4.575C17.3417 5.31667 17.7083 6.28333 17.7083 7.26666Z" stroke="white" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
										</svg>`);
                }
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(xhr.responseText);
        }
    });
})

$('.add-to-cart-product').off('click').on('click', function (event) {
    event.preventDefault();
    event.stopPropagation();

    var current = $(event.target).parents('.add-to-cart-product'),
        productId = current.data('id');

    var formData = new FormData();
    formData.append("Id", productId);
    $.ajax({
        type: "POST",
        url: "/Cart/AddToCardJson",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response && response.success) {
                window.location.reload()
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(xhr.responseText);
        }
    });
})