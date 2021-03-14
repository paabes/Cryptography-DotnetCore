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
using static Crypto.RsaLib;

namespace WebApp.Controllers
{
    [Authorize]
    public class RsaDecController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RsaDecController(ApplicationDbContext context)
        {
            _context = context;
        }
        public string GetUserId()
        {
            var calim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return calim?.Value ?? "";
        }

        // GET: RsaDec
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _context.RsaDecDbs.Where(c => c.UserId==userId).ToListAsync());
        }

        // GET: RsaDec/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsaDecDb = await _context.RsaDecDbs.Where(c => c.UserId==GetUserId())
                .FirstOrDefaultAsync(m => m.RsaDecDbId == id);
            if (rsaDecDb == null)
            {
                return NotFound();
            }

            return View(rsaDecDb);
        }

        // GET: RsaDec/Create
        public IActionResult Create()
        {
            return View();
        }
        public void CheckRsaDec(RsaDecDb rsaDecDb)
        {
            if (rsaDecDb.n<=0)
            {
                ModelState.AddModelError(nameof(rsaDecDb.n), errorMessage:"input correct public key");
            }
            if (rsaDecDb.d<=0)
            {
                ModelState.AddModelError(nameof(rsaDecDb.d), errorMessage:"input correct public key");
            }
            if (string.IsNullOrEmpty(rsaDecDb.Ciphertext))
            {
                ModelState.AddModelError(nameof(rsaDecDb.Ciphertext), errorMessage:"Ciphertext cannot be Null!");
            }
            if (!IsBase64Encoded(rsaDecDb.Ciphertext))
            {
                ModelState.AddModelError(nameof(rsaDecDb.Ciphertext), errorMessage:"correct Ciphertext should be in Base64");
            }
        }

        // POST: RsaDec/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RsaDecDbId,n,d,Plaintext,Ciphertext")] RsaDecDb rsaDecDb)
        {
            CheckRsaDec(rsaDecDb);
            
            if (ModelState.IsValid)
            {
                rsaDecDb.Plaintext = RsaDecText(rsaDecDb.n, rsaDecDb.d, rsaDecDb.Ciphertext);
                rsaDecDb.UserId = GetUserId();
                _context.Add(rsaDecDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rsaDecDb);
        }

        // GET: RsaDec/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsaDecDb = await _context.RsaDecDbs.Where(c => c.UserId==GetUserId()).FirstOrDefaultAsync(m=>m.RsaDecDbId == id);
            if (rsaDecDb == null)
            {
                return NotFound();
            }
            return View(rsaDecDb);
        }

        // POST: RsaDec/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RsaDecDbId,n,d,Plaintext,Ciphertext")] RsaDecDb rsaDecDb)
        {
            if (id != rsaDecDb.RsaDecDbId)
            {
                return NotFound();
            }

            CheckRsaDec(rsaDecDb);
            rsaDecDb.UserId = GetUserId();
            if (ModelState.IsValid)
            {
                try
                {
                    rsaDecDb.Plaintext = RsaDecText(rsaDecDb.n, rsaDecDb.d, rsaDecDb.Ciphertext);

                    if (rsaDecDb.UserId==GetUserId())
                    {
                        _context.Update(rsaDecDb);
                        await _context.SaveChangesAsync();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RsaDecDbExists(rsaDecDb.RsaDecDbId))
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
            return View(rsaDecDb);
        }

        // GET: RsaDec/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsaDecDb = await _context.RsaDecDbs.Where(c=>c.UserId==GetUserId())
                .FirstOrDefaultAsync(m => m.RsaDecDbId == id);
            if (rsaDecDb == null)
            {
                return NotFound();
            }

            return View(rsaDecDb);
        }

        // POST: RsaDec/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rsaDecDb = await _context.RsaDecDbs.Where(c=>c.RsaDecDbId==id && c.UserId==GetUserId()).FirstOrDefaultAsync(m=>m.RsaDecDbId==id);
            _context.RsaDecDbs.Remove(rsaDecDb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RsaDecDbExists(int id)
        {
            return _context.RsaDecDbs.Any(e => e.RsaDecDbId == id);
        }
    }
}
