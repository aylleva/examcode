using System.ComponentModel.DataAnnotations;

namespace ExamMVC.Areas.Admin.ViewModels
{
    public class CreatePositionVM
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
