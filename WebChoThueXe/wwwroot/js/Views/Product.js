$('.upload__img-wrap').hide();

$('#hdFileRatingUpload').off('change').on('change', function (event) {
    var input = event.target;
    var files = input.files;

    $('.upload__img-wrap').empty();

    if (Math.min(files.length, 3) > 0) {
        $('.upload__img-wrap').show();
        for (var i = 0; i < Math.min(files.length, 3); i++) {
            var file = files[i];
            var reader = new FileReader();

            reader.onload = function (e) {
                var imageUrl = e.target.result;
                $('.upload__img-wrap').append('<div class="upload__img-box"><div style="background-image: url(' + imageUrl + ')" class="img-bg"></div></div>');
            }

            reader.readAsDataURL(file);
        }
    }
    else {
        $('.upload__img-wrap').hide();
    }
});

var ratingItems = document.querySelectorAll("#modalRate .rating-topzonecr-star li");

// Loop through each list item
ratingItems.forEach(function (item) {
    // Add click event listener to each item
    item.addEventListener("click", function () {
        // Remove 'active' class from all items
        ratingItems.forEach(function (item) {
            item.querySelector("i").classList.remove("active");
            item.querySelector("p").classList.remove("active-slt");
        });
        // Add 'active' class to clicked item and all preceding items
        var val = parseInt(this.getAttribute("data-val"));
        for (var i = 0; i < val; i++) {
            ratingItems[i].querySelector("i").classList.add("active");
            ratingItems[i].querySelector("p").classList.add("active-slt");
        }
        $('.star-real').val(val)
    });
});

$('#modalRate #submitrt').on('click', function () {
    var rating = parseInt($('.star-real').val());
    var productId = $("#modalRate .product-id").val();
    var content = $(".fRContent").val();

    var file1 = $("#hdFileRatingUpload").prop("files")[0];
    var file2 = $("#hdFileRatingUpload").prop("files")[1];
    var file3 = $("#hdFileRatingUpload").prop("files")[2]; 

    // Create RatingViewModel object
    //var ratingViewModel = {
    //    ProductId: productId,
    //    Rating: rating,
    //    Content: content
    //};
    var formData = new FormData();
    formData.append("productId", productId);
    formData.append("star", rating);
    formData.append("content", content);

    if (file1) {
        formData.append("file1", file1);
    }
    if (file2) {
        formData.append("file2", file2);
    }
    if (file3) {
        formData.append("file3", file3);
    }

    // AJAX request to send rating and content to the action
    $.ajax({
        type: "POST",
        url: "/Rating/Rating",
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            // Handle successful response
            window.location.reload()
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error(xhr.responseText);
        }
    });
})

// xử lý paging cho trang đánh giá
function getPagingDataRating(pageIndex = 1) {
    var star = $('.filter-list li.current').data('star') ? $('.filter-list li.current').data('star') : null;
    pageIndex = pageIndex ? pageIndex: $('.page-Index').text();
    var productId = $("#modalRate .product-id").val();
    var url = `/Rating/GetPagingRating?pageIndex=${pageIndex}&productId=${productId}`;
    if (star != null) {
        url += `&star=${star}`
    }
    $.ajax({
        type: "GET",
        url: url,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response && response.isSuccess) {
                bindPagingFooter(response.pageIndex, response.totalPages);
                bindPagingData(response.data);
            }
        },
        error: function (xhr, status, error) {
            console.error("Error fetching data:", error);
        }
    });
}

function bindPagingFooter(pageIndex, totalPages) {
    const paginationContainer = $('.pagcomment');
    paginationContainer.empty();

    if (totalPages === 1) {
        paginationContainer.append('<a>1</a>');
    } else if (totalPages > 1) {
        if (pageIndex > 1) {
            paginationContainer.append('<a class="prev-page">‹</a>');
        }

        for (let i = 1; i <= totalPages; i++) {
            debugger
            if (i === pageIndex) {
                paginationContainer.append(`<span class="page-index active">${i}</span>`);
            } else {
                paginationContainer.append(`<a class="page-number">${i}</a>`);
            }
        }

        if (pageIndex < totalPages) {
            paginationContainer.append('<a class="next-page">›</a>');
        }
    }

    // Attach click event listeners for pagination buttons
    $('.page-number').click(function () {
        getPagingDataRating(parseInt($(this).text()));
    });

    $('.prev-page').click(function () {
        if (pageIndex > 1) {
            getPagingDataRating(pageIndex - 1);
        }
    });

    $('.next-page').click(function () {
        if (pageIndex < totalPages) {
            getPagingDataRating(pageIndex + 1);
        }
    });
}

function bindPagingData(data) {
    const commentList = $('.comment-list');
    commentList.empty();

    data.forEach(item => {
        // Khởi tạo một đối tượng Date
        const currentDate = new Date(item.createdDate);

        // Lấy thông tin về ngày, tháng, năm, giờ, phút
        const day = String(currentDate.getDate()).padStart(2, '0');
        const month = String(currentDate.getMonth() + 1).padStart(2, '0'); // Tháng bắt đầu từ 0
        const year = currentDate.getFullYear();
        const hours = String(currentDate.getHours()).padStart(2, '0');
        const minutes = String(currentDate.getMinutes()).padStart(2, '0');

        // Tạo chuỗi định dạng "dd/mm/yyyy hh:mm"
        const formattedDate = `${day}/${month}/${year} ${hours}:${minutes}`;

        const commentItem = `
            <li class="par">
                <div class="cmt-top">
                    <p class="cmt-top-name">${item.createdByName}</p>
                </div>
                <div class="cmt-intro">
                    <div class="cmt-top-star">
                        <i class="${item.star >= 1 ? 'iconcmt-starbuy' : 'iconcmt-unstarbuy'}"></i>
                        <i class="${item.star >= 2 ? 'iconcmt-starbuy' : 'iconcmt-unstarbuy'}"></i>
                        <i class="${item.star >= 3 ? 'iconcmt-starbuy' : 'iconcmt-unstarbuy'}"></i>
                        <i class="${item.star >= 4 ? 'iconcmt-starbuy' : 'iconcmt-unstarbuy'}"></i>
                        <i class="${item.star >= 5 ? 'iconcmt-starbuy' : 'iconcmt-unstarbuy'}"></i>
                    </div>
                </div>
                <div class="cmt-content">
                        <p class="cmt-txt">${item.content}</p>
                        <div class="cmt-img">
                            ${item.linkImage1 ? `<p class="it-img"><img class="lazyloaded" src="/media/products/${item.linkImage1}" alt="img"></p>` : ''}
                            ${item.linkImage2 ? `<p class="it-img"><img class="lazyloaded" src="/media/products/${item.linkImage2}" alt="img"></p>` : ''}
                            ${item.linkImage3 ? `<p class="it-img"><img class="lazyloaded" src="/media/products/${item.linkImage3}" alt="img"></p>` : ''}
                        </div>
                    </div>
                    <div class="cmt-command">
                        <span class="cmtd">${formattedDate}</span>
                    </div>
            </li>`;
        commentList.append(commentItem);
    });
}

// Gọi hàm lần đầu với trang mặc định và số sao
getPagingDataRating(1);


// xử lý sự kiện lọc
$('.filter-list li').off('click').on('click', function (event) {
    var current = $(event.target);
    $('.filter-list li').removeClass('current');
    current.addClass('current');
    getPagingDataRating();
})