using System.ComponentModel.DataAnnotations;

namespace ExamMVC.ViewModels
{
    public class LoginVM
    {
        [MaxLength(256)]
        public string UserNameorEmail {  get; set; }
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
