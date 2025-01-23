using ExamMVC.DAL;
using ExamMVC.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExamMVC.Services.Implementations
{
    public class LayoutService : ILayoutService
    {
        private readonly AppDBContext _context;

        public LayoutService(AppDBContext context)
        {
           _context = context;
        }
        public async Task<Dictionary<string, string>> GetLayoutAsync()
        {
           return await _context.Settings.ToDictionaryAsync(k=>k.Key, k=>k.Value);
        }
    }
}
