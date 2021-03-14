using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.AspNetCore.Authorization;
using WebApp.Data;
using static Crypto.Diffe;


namespace WebApp.Controllers
{
    [Authorize]
    public class DiffeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiffeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetUserId()
        {
            var calim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return calim?.Value ?? "";
        }

        // GET: Diffe
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _context.DiffeDbs.Where(c => c.UserId==userId).ToListAsync());
        }

        // GET: Diffe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diffeDB = await _context.DiffeDbs.Where(c => c.UserId==GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diffeDB == null)
            {
                return NotFound();
            }

            return View(diffeDB);
        }

        // GET: Diffe/Create
        public IActionResult Create()
        {
            return View();
        }
       

        // POST: Diffe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiffeDB diffeDB)
        {
            CheckDiffe(diffeDB);
            if (ModelState.IsValid)
            {
                var X_secret = ExpMod(diffeDB.g, diffeDB.a, diffeDB.p);
                var Y_secret = ExpMod(diffeDB.g, diffeDB.b, diffeDB.p);
            
                var x_key = ExpMod(Y_secret, diffeDB.a, diffeDB.p);
                var y_key = ExpMod(X_secret, diffeDB.b, diffeDB.p);

                diffeDB.UserId = GetUserId();
                
                if (x_key == y_key)
                {
                    diffeDB.HelKey = x_key;
                }
                _context.Add(diffeDB);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(diffeDB);
        }
        public void CheckDiffe(DiffeDB diffeDb)
        {
            if (TestPrime(diffeDb.g)==false)
            {
                ModelState.AddModelError(nameof(diffeDb.g), errorMessage:"input should be PRIME");
            }
            if (TestPrime(diffeDb.p)==false)
            {
                ModelState.AddModelError(nameof(diffeDb.p), errorMessage:"input should be PRIME");
            }
            if (diffeDb.a==0)
            {
                ModelState.AddModelError(nameof(diffeDb.a), errorMessage:"input should be Non-Zero Integer!");
            }
            if (diffeDb.b==0)
            {
                ModelState.AddModelError(nameof(diffeDb.b), errorMessage:"input should be Non-Zero Integer!");
            }
        }
        public bool CheckUser(DiffeDB diffeDb)
        {
            if (diffeDb.UserId == GetUserId())
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }

        // GET: Diffe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diffeDB = await _context.DiffeDbs.Where(c => c.UserId==GetUserId()).FirstOrDefaultAsync(m=>m.Id == id);
            if (diffeDB == null)
            {
                return NotFound();
            }

            if (diffeDB.UserId==GetUserId())
            {
                return View(diffeDB);
            }
            else
            {
                return NotFound();
            }
            
        }

        // POST: Diffe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,g,p,a,b,HelKey")] DiffeDB diffeDB)
        {
            if (id != diffeDB.Id)
            {
                return NotFound();
            }

            CheckDiffe(diffeDB);
            diffeDB.UserId = GetUserId();
            if (ModelState.IsValid)
            {
                try
                {
                    var X_secret = ExpMod(diffeDB.g, diffeDB.a, diffeDB.p);
                    var Y_secret = ExpMod(diffeDB.g, diffeDB.b, diffeDB.p);
            
                    var x_key = ExpMod(Y_secret, diffeDB.a, diffeDB.p);
                    var y_key = ExpMod(X_secret, diffeDB.b, diffeDB.p);
                
                    if (x_key == y_key)
                    {
                        diffeDB.HelKey = x_key;
                    }

                    if (diffeDB.UserId==GetUserId())
                    {
                        _context.Update(diffeDB);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiffeDBExists(diffeDB.Id))
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
            return View(diffeDB);
        }

        // GET: Diffe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // .Where(c => c.UserId==GetUserId()
            var diffeDB = await _context.DiffeDbs.Where(c=>c.UserId==GetUserId())
                .FirstOrDefaultAsync(m => m.Id == id);
            if (diffeDB == null)
            {
                return NotFound();
            }

            return View(diffeDB);
        }

        // POST: Diffe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diffeDB = await _context.DiffeDbs.Where(c=>c.Id==id && c.UserId==GetUserId()).FirstOrDefaultAsync(m=>m.Id==id);
            _context.DiffeDbs.Remove(diffeDB);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiffeDBExists(int id)
        {
            return _context.DiffeDbs.Any(e => e.Id == id);
        }
    }
}