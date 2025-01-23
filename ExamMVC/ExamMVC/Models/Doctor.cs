using ExamMVC.Models.Base;

namespace ExamMVC.Models
{
    public class Doctor:BaseEntity
    {
        public string Name {  get; set; }
        public int PositionId {  get; set; }

        public string FBLink {  get; set; }
        public string TwitLink {  get; set; }   
        public string InsLink {  get; set; }
        public Position Position { get; set; }
        public string Image {  get; set; }
    }
}
