using System.ComponentModel.DataAnnotations;

namespace NZWalks.Models.DTO
{
    public class UpdateRegionDto
    {
        [Required]
        [MinLength(2, ErrorMessage = "Code has to be a minimum of 2 characters")]
        [MaxLength(3, ErrorMessage = "Code has to be a maximum of 3 characters")]
        public string Code { get; set; }
        [Required]
        
        [MaxLength(100, ErrorMessage = "too long")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
