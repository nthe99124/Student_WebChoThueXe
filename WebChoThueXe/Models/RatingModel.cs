using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebChoThueXe.Models
{
	public class RatingModel
	{
		[Key]
		public int Id { get; set; }
		public int? ProductId { get; set; }
		public string CreatedByName { get; set; }
		public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
		public string Content { get; set; }
		public int Star { get; set; }
		public string LinkImage1 { get; set; }
		public string LinkImage2 { get; set; }
		public string LinkImage3 { get; set; }

		[ForeignKey("ProductId")]

		public ProductModel Product { get; set; }
	}
}
