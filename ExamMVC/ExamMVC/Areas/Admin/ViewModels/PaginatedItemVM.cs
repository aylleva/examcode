namespace ExamMVC.Areas.Admin.ViewModels
{
    public class PaginatedItemVM<T>
    {
        public int CurrectPage {  get; set; }
        public double TotalPage {  get; set; }
        public List<T> Items { get; set; }
    }
}
