using ExamMVC.Models.Base;

namespace ExamMVC.Models
{
    public class Setting:BaseEntity
    {
        public string Key {  get; set; }
        public string Value { get; set; }
    }
}
