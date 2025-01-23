using ExamMVC.Areas.Admin.ViewModels;
using ExamMVC.DAL;
using ExamMVC.Models;
using ExamMVC.Utilities.Extentitons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace ExamMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DoctorController : Controller
    {
        private readonly AppDBContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string roots = Path.Combine("assets", "images");

        public DoctorController(AppDBContext context,IWebHostEnvironment env)
        {
            _context = context;
           _env = env;
        }
        public async Task<IActionResult> Index(int page=1)
        {
            if (page < 1) return BadRequest();

            int count =await _context.Doctors.CountAsync();
            double total = Math.Ceiling((double)count / 2);

            if(page>total) return BadRequest();
            PaginatedItemVM<GetDoctorVM> vm = new()
            {
                CurrectPage = page,
                TotalPage = total,
                Items = await _context.Doctors.Include(d => d.Position)
                .Skip((page-1)*2)
                .Take(2)
                   .Select(d => new GetDoctorVM
                   {
                       Id = d.Id,
                       Name = d.Name,
                       Image = d.Image,
                       PositionName = d.Position.Name
                   }).ToListAsync()
            };
         
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            CreateDoctorVM vm = new CreateDoctorVM()
            {
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
        }
        [HttpPost]

        public async Task<IActionResult> Create(CreateDoctorVM doctvm)
        {
            doctvm.Positions=await _context.Positions.ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(doctvm);
            }

            if (!doctvm.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError(nameof(CreateDoctorVM.Photo), "Wrong Type");
                return View(doctvm);
            }
            if(!doctvm.Photo.ValidateSize(Utilities.Enums.FileSizes.MB,2))
            {
                ModelState.AddModelError(nameof(CreateDoctorVM.Photo), "Wrong Size");
                return View(doctvm);
            }
            bool result = await _context.Positions.AnyAsync(p => p.Id == doctvm.PositionId);
            if(!result)
            {
                ModelState.AddModelError(nameof(CreateDoctorVM.PositionId), "Position does not exists");
                return View(doctvm);
            }

            Doctor doctor = new Doctor()
            {
                Name = doctvm.Name,
                FBLink = doctvm.FbLink,
                TwitLink = doctvm.TwitLink,
                InsLink = doctvm.InstLink,
                PositionId = doctvm.PositionId.Value,
                Image = await doctvm.Photo.CreateFileAsync(_env.WebRootPath, roots)
            };

            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Doctor? doctor=await _context.Doctors.FirstOrDefaultAsync(p => p.Id == Id);
            if (doctor is null) return NotFound();

            UpdateDoctorVM vm = new UpdateDoctorVM()
            {
                Name = doctor.Name,
                FbLink = doctor.FBLink,
                InstLink = doctor.InsLink,
                TwitLink = doctor.TwitLink,
                PositionId = doctor.PositionId,
                Positions = await _context.Positions.ToListAsync()
            };
            return View(vm);
           
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? Id,UpdateDoctorVM vm)
        {
            if (Id is null || Id < 1) return BadRequest();
            Doctor? existed = await _context.Doctors.FirstOrDefaultAsync(p => p.Id == Id);
            if (existed is null) return NotFound();

            if (!ModelState.IsValid)
            {
               return View(vm);
            }

            if(vm.Photo is not null)
            {
                if (!vm.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError(nameof(UpdateDoctorVM.Photo), "Wrong Type");
                    return View(vm);
                }
                if (!vm.Photo.ValidateSize(Utilities.Enums.FileSizes.MB, 2))
                {
                    ModelState.AddModelError(nameof(UpdateDoctorVM.Photo), "Wrong Size");
                    return View(vm);
                }
            }
            if(existed.PositionId!=vm.PositionId)
            {
                bool result = await _context.Positions.AnyAsync(p => p.Id == vm.PositionId);
                if (!result)
                {
                    ModelState.AddModelError(nameof(UpdateDoctorVM.PositionId), "Position does not exists");
                    return View(vm);
                }
            }

            if(vm.Photo is not null)
            {
                existed.Image.DeleteFile(_env.WebRootPath,roots);
                existed.Image= await vm.Photo.CreateFileAsync(_env.WebRootPath, roots);
            }

            existed.Name=vm.Name;
            existed.FBLink = vm.FbLink;
            existed.InsLink = vm.InstLink;
            existed.TwitLink = vm.TwitLink;
            existed.PositionId = vm.PositionId.Value;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id is null || Id < 1) return BadRequest();
            Doctor? doctor = await _context.Doctors.FirstOrDefaultAsync(p => p.Id == Id);
            if (doctor is null) return NotFound();
           
            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
