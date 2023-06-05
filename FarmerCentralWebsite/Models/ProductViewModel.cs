using System.ComponentModel.DataAnnotations;

namespace FarmerCentralWebsite.Models
{
    public class ProductViewModel
    {
        [Required]
        public string ProductName { get; set; }

        [Required]
        public string ProductType { get; set; }

        [Required]
        public float StockPrice { get; set; }
    }

}
