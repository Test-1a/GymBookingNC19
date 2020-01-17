using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBookingNC19.Core.Models;
using GymBookingNC19.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using GymBookingNC19.Core.ViewModels;
using GymBookingNC19.Data.Repositories;

namespace GymBookingNC19.Controllers
{
    [Authorize]
    public class GymClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private GymClassesRepository gymClassesRepository;
        private ApplicationUserGymClassRepository applicationUserGymClassRepository;
        private UnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> userManager;

        public GymClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            gymClassesRepository = new GymClassesRepository(_context);
            applicationUserGymClassRepository = new ApplicationUserGymClassRepository(_context);
            unitOfWork = new UnitOfWork(_context);
            this.userManager = userManager;
        }

        // GET: GymClasses
        [AllowAnonymous]
        public async Task<IActionResult> Index(IndexViewModel vm = null)
        {
            var model = new IndexViewModel();

            if (vm.History)
            {
                List<GymClass> gym = await gymClassesRepository.GetHistoryAsync();
                model = new IndexViewModel { GymClasses = gym };
                return View(model);
            }

            List<GymClass> gymclasses = await gymClassesRepository.GetAllWithUsersAsync();

            var model2 = new IndexViewModel { GymClasses = gymclasses };

            return View(model2);
        }

      

        [Authorize(Roles ="Member")]
        public async Task<IActionResult> GetBookings()
        {
            var userId = userManager.GetUserId(User);
            List<GymClass> model = await applicationUserGymClassRepository.GetAllBookingsAsync(userId);

            return View(model);
        }


        [Authorize(Roles ="Member")]
        public async Task<IActionResult> BookingToogle(int? id)
        {
            if (id == null) return NotFound();

            //Hämta den inloggade användarens id
            // var userId = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var userId = userManager.GetUserId(User);

            //Hämta aktuellt gympass
            //Todo: Remove button in ui if pass is history!!!
            GymClass currentGymClass = await gymClassesRepository.GetWithAttendingMembersAsync(id);

            //Är den aktuella inloggade användaren bokad på passet?
            var attending = currentGymClass.AttendingMembers
                .FirstOrDefault(u => u.ApplicationUserId == userId);

            //Om inte, boka användaren på passet
            if (attending == null)
            {
                var book = new ApplicationUserGymClass
                {
                    ApplicationUserId = userId,
                    GymClassId = currentGymClass.Id
                };

                applicationUserGymClassRepository.Add(book);
                await unitOfWork.CompleteAsync();
            }

            //Annars avboka
            else
            {
                applicationUserGymClassRepository.Remove(attending);
                await unitOfWork.CompleteAsync();
            }

            return RedirectToAction(nameof(Index));

        }

       

        // GET: GymClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await gymClassesRepository.GetAsync(id);

            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

        // GET: GymClasses/Create
        [Authorize(Roles ="Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: GymClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                gymClassesRepository.Add(gymClass);
                await unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gymClass = await gymClassesRepository.GetAsync(id);
            if (gymClass == null)
            {
                return NotFound();
            }
            return View(gymClass);
        }

        // POST: GymClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,Duration,Description")] GymClass gymClass)
        {
            if (id != gymClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    gymClassesRepository.Update(gymClass);
                    await unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GymClassExists(gymClass.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gymClass);
        }

        // GET: GymClasses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GymClass gymClass = await gymClassesRepository.GetAsync(id);

            if (gymClass == null)
            {
                return NotFound();
            }

            return View(gymClass);
        }

      

        // POST: GymClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gymClass = await gymClassesRepository.GetAsync(id);

            gymClassesRepository.Remove(gymClass);
            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GymClassExists(int id)
        {
            return gymClassesRepository.GetAny(id);
        }

       
    }
}
