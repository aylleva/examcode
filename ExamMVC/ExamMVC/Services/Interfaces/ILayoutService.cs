namespace ExamMVC.Services.Interfaces
{
    public interface ILayoutService
    {
        public Task<Dictionary<String,string>> GetLayoutAsync();
    }
}
