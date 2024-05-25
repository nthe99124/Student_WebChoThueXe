using WebChoThueXe.Repository.Validation;

namespace WebChoThueXe.Models.ViewModels
{
    public class RatingViewModel
    {
		public int Id { get; set; }
		public string Content { get; set; }
		public int Star { get; set; }
        public string LinkImage1 { get; set; }
        public string LinkImage2 { get; set; }
        public string LinkImage3 { get; set; }
        public int ProductId { get; set; }

        [FileExtension]
        public IFormFile LinkImageUpload1 { get; set; }

        [FileExtension]
        public IFormFile LinkImageUpload2 { get; set; }

        [FileExtension]
        public IFormFile LinkImageUpload3 { get; set; }
    }
}
