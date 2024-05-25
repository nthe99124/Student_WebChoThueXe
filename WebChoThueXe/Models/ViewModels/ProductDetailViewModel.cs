using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebChoThueXe.Repository.Validation;

namespace WebChoThueXe.Models
{
	public class ProductDetailViewModel
	{
		public ProductDetailVM ProductDetail { get; set; }
		public RatingVM RatingInfor { get; set; }
	}

	public class ProductDetailVM
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string BrandName { get; set; }
		public string CategoryName { get; set; }
		public string Image { get; set; }
	}

	public class RatingVM
	{
		public int TotalRate { get; set; }
		public float Star { get; set; }
		public float PercentStar1 { get; set; }
		public float PercentStar2 { get; set; }
		public float PercentStar3 { get; set; }
		public float PercentStar4 { get; set; }
		public float PercentStar5 { get; set; }
		public bool HasEnableRate { get; set; }
        public List<string> ListImg { get; set; }
		public List<RatingModel> Rating { get; set; }
	}
}
