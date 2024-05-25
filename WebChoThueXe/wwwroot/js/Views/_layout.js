$('.tab-menu').click(() => {
    $('.menu-mobile').removeClass('move-left');
});

$('.close-menu').click(() => {
    $('.menu-mobile').addClass('move-left');
});

$('.arrow-infor').hover(function () {
    $('.menu-icon-box .dropdown').css({
        'opacity': '1',
        'visibility': 'visible'
    });
});
$(window).click(function () {
    $('.menu-icon-box .dropdown').css({
        'opacity': '0',
        'visibility': 'hidden'
    });
});