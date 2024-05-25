let tableElement = $('table');

tableElement.DataTable({
    pagingType: 'simple_numbers',
    language: {
        "decimal": "",
        "emptyTable": "Không có dữ liệu trong bảng",
        "info": "Hiển thị từ _START_ đến _END_ của _TOTAL_ mục",
        "infoEmpty": "Hiển thị từ 0 đến 0 của 0 mục",
        "infoFiltered": "(lọc từ tổng số _MAX_ mục)",
        "infoPostFix": "",
        "thousands": ",",
        "lengthMenu": "Hiển thị _MENU_ mục",
        "loadingRecords": "Đang tải...",
        "processing": "Đang xử lý...",
        "search": "Tìm kiếm:",
        "zeroRecords": "Không tìm thấy kết quả",
        "paginate": {
            "first": "Đầu tiên",
            "last": "Cuối cùng",
            "next": "Tiếp",
            "previous": "Trước"
        },
        "aria": {
            "sortAscending": ": sắp xếp tăng dần",
            "sortDescending": ": sắp xếp giảm dần"
        }
    }
});