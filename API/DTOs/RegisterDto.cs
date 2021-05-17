using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        public string CustomerId { get; set; }
        [Required]
        public string  ContactName { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string  City { get; set; }
        [Required]
        public string Country { get; set; }

    }
}