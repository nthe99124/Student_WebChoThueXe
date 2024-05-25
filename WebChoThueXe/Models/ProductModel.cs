using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebChoThueXe.Repository.Validation;

namespace WebChoThueXe.Models
{
    public class ProductModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Tên Sản Phẩm ")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Yêu Cầu Nhập Mô Tả Sản Phẩm ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Yêu Cầu Nhập Giá Sản Phẩm ")]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(8, 2)")]
        public decimal Price { get; set; }
        [Required, Range(1, int.MaxValue,ErrorMessage = "Chọn 1 Thương Hiệu ")]
        public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn 1 Danh Mục ")]
        public int CategoryId { get; set; }

        public CategoryModel Category { get; set; }

        public BrandModel Brand { get; set; }

        public string Image { get; set; } = "noimage.jpg";
        [NotMapped]
        [FileExtension]

        public IFormFile ImageUpload { get; set; }
        [NotMapped]
		public bool IsFavorite { get; set; }
		[NotMapped]
		public double Star { get; set; }
	}
}
