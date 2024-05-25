using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace WebChoThueXe.Models.ViewModels
{
    namespace StoriesProject.Model.ViewModel
    {
        public class RestPagingOutput<T>
        {
            public int StatusCode { get; set; }    // Mã trạng thái HTTP
            public string Message { get; set; }    // Thông điệp mô tả kết quả
            public List<T> Data { get; set; }      // Dữ liệu trả về
            public int PageIndex { get; set; }     // Trang hiện tại
            public int PageSize { get; set; } = 5;     // Kích thước trang
            public int TotalPages { get; set; }    // Tổng số trang
            public int TotalItems { get; set; }    // Tổng số phần tử
            public bool IsSuccess { get; set; }

            // Constructor để tạo một đối tượng RestPagingOutput

            public void SuccessEventHandler(List<T> data = null, int totalCount = 0, int pageIndex = 0, int pageSize = 5, string message = null)
            {
                IsSuccess = true;
                if (data != null)
                {
                    Data = data;
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    PageIndex = pageIndex;
                    TotalItems = totalCount;
                }
                if (!string.IsNullOrEmpty(message))
                {
                    Message = message;
                }
            }

            public void ErrorEventHandler(List<T> data = null, string message = "Đã có lỗi xảy ra")
            {
                IsSuccess = false;
                if (data != null)
                {
                    Data = data;
                }
                if (!string.IsNullOrEmpty(message))
                {
                    Message = message;
                }
            }
        }
    }
}
