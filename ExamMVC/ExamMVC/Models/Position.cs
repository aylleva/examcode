using ExamMVC.Models.Base;

namespace ExamMVC.Models
{
    public class Position:BaseEntity
    {
        public string Name {  get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
