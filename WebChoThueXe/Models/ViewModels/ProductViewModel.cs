using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebChoThueXe.Repository.Validation;

namespace WebChoThueXe.Models
{
    public class ProductViewModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }

		public string BrandName { get; set; }
		public string CategoryName { get; set; }

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
