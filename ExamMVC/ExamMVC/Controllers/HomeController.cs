using ExamMVC.DAL;
using ExamMVC.Models;
using ExamMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDBContext _context;

        public HomeController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM= new HomeVM() { 
            Doctors=await _context.Doctors.Include(D=>D.Position).Take(8).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
