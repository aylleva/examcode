using ExamMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamMVC.Areas.Admin.ViewModels;
    public class UpdateDoctorVM
    {

    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; }

    public IFormFile? Photo { get; set; }
    public int? PositionId { get; set; }
    public List<Position>? Positions { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string? FbLink { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string? InstLink { get; set; }
    [MinLength(3)]
    [MaxLength(200)]
    public string? TwitLink { get; set; }


}
