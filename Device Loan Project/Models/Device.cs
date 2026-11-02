using System.ComponentModel.DataAnnotations;

namespace Device_Loan_Project.Models
{
    public class Device
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Feild cannot be empty, idiot")]
        [StringLength(10, ErrorMessage = "must be 10 characters")]
        [Display(Name = "AssetTag")]
        public string AssetTag { get; set; } = "";

        [Required(ErrorMessage = "Feild cannot be empty, idiot")]
        [StringLength(50, ErrorMessage = "must be 50 characters")]
        [Display(Name = "Device Name")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Feild cannot be empty, idiot")]
        [StringLength(20, ErrorMessage = "must be 20 characters")]
        [Display(Name = "Category")]
        public string Category { get; set; } = "";


        [Range(2015, 2026, ErrorMessage = "must be between 2015 to 2026 , no way you a smartphone before 1999")]
        [Display(Name = "Year Bought")]
        public int PurchaseYear { get; set; } = 0;


        [Display(Name = "IsOnLoan")]
        public string IsOnLoan { get; set; } = "";


    }
}
