using Microsoft.AspNetCore.Identity;

namespace ExamMVC.Models
{
    public class AppUser:IdentityUser
    {
        public string Name {  get; set; }
        public string Surname {  get; set; }

    }
}
