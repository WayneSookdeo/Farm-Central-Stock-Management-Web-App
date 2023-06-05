using System.ComponentModel.DataAnnotations;

namespace FarmerCentralWebsite.Models
{
    public class FarmerViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

}
