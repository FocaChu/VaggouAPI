using System.ComponentModel.DataAnnotations;

namespace VaggouAPI
{
    public class CreateReviewRequestDto
    {
        [Required]
        public Guid ParkingLotId { get; set; }

        [Range(1, 5)]
        public int Score { get; set; }

        [MaxLength(500)]
        public string? Comment { get; set; }
    }
}
