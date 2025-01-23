using System.ComponentModel.DataAnnotations;

namespace ExamMVC.Areas.Admin.ViewModels
{
    public class UpdatePositionVM
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
