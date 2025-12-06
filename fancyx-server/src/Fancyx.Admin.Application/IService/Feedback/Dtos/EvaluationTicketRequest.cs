using System.ComponentModel.DataAnnotations;

namespace Fancyx.Admin.Application.IService.Feedback.Dtos
{
    public class EvaluationTicketRequest
    {
        [Required]
        public long Id { get; set; }

        [Required]
        public int Rating { get; set; }

        public string? RatingComment { get; set; }
    }
}