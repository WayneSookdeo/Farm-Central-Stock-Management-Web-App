using System.ComponentModel.DataAnnotations;

namespace FarmerCentralWebsite.Models
{
    public class FilterViewModel
    {
        public string FarmerEmail { get; set; }

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        
        public string ProductType { get; set; }
    }

}
