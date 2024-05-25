using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebChoThueXe.Models
{
    public class ProductFavoriteModel
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string AccountId { get; set; }

        [ForeignKey("ProductId")]
        public ProductModel Product { get; set; }
    }
}
