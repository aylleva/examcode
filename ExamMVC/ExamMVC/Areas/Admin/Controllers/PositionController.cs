using ExamMVC.Areas.Admin.ViewModels;
using ExamMVC.DAL;
using ExamMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PositionController : Controller
    {
        private readonly AppDBContext _context;

        public PositionController(AppDBContext context)
        {
           _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<GetPositionVM> vm = await _context.Positions.Include(p => p.Doctors)
                .Select(p => new GetPositionVM {
                
                Id= p.Id,
                Name= p.Name,
                DoctorCount=p.Doctors.Count
                }).ToListAsync();

            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);    
            }

            bool result=await _context.Positions.AnyAsync(p=>p.Name.Trim()==vm.Name.Trim());
            if (result)
            {
                ModelState.AddModelError(nameof(CreatePositionVM.Name), "Position is allready exist");
                return View(vm);
            }

            Position position = new Position() { 
            Name= vm.Name
            };

            await _context.Positions.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? Id)
        {
            if(Id == null || Id<1) return BadRequest(); 
            Position? position = await _context.Positions.FirstOrDefaultAsync(p=>p.Id==Id);
            if (position == null) return NotFound();

            UpdatePositionVM updatePositionVM = new UpdatePositionVM() { 
            Name=position.Name
            };
            return View(updatePositionVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? Id,UpdatePositionVM vm)
        {
            if (Id == null || Id < 1) return BadRequest();
            Position? existed = await _context.Positions.FirstOrDefaultAsync(p => p.Id == Id);
            if (existed == null) return NotFound();

            bool result = await _context.Positions.AnyAsync(p => p.Name.Trim() == vm.Name.Trim() && p.Id!=Id);
            if (result)
            {
                ModelState.AddModelError(nameof(UpdatePositionVM.Name), "Position is allready exist");
                return View(vm);
            }
            existed.Name=vm.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null || Id < 1) return BadRequest();
            Position? position = await _context.Positions.FirstOrDefaultAsync(p => p.Id == Id);
            if (position == null) return NotFound();

            _context.Positions.Remove(position);
            return RedirectToAction(nameof(Index));

        }
    }
}
