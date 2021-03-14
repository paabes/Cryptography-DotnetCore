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
    public class RsaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RsaController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public string GetUserId()
        {
            var calim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return calim?.Value ?? "";
        }

        // GET: Rsa
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            return View(await _context.RsaDbs.Where(c => c.UserId==userId).ToListAsync());
        }

        // GET: Rsa/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsaDb = await _context.RsaDbs.Where(c => c.UserId==GetUserId())
                .FirstOrDefaultAsync(m => m.RsaDbId == id);
            if (rsaDb == null)
            {
                return NotFound();
            }

            return View(rsaDb);
        }

        // GET: Rsa/Create
        public IActionResult Create()
        {
            return View();
        }
        
        public void CheckRsa(RsaDb rsaDb)
        {
            if (TestPrime(rsaDb.p)==false)
            {
                ModelState.AddModelError(nameof(rsaDb.p), errorMessage:"input should be PRIME");
            }
            if (Crypto.RsaLib.TestPrime(rsaDb.q)==false)
            {
                ModelState.AddModelError(nameof(rsaDb.q), errorMessage:"input should be PRIME");
            }
            if ((ulong.MaxValue / rsaDb.p) < rsaDb.q)
            {
                ModelState.AddModelError(nameof(rsaDb.q), errorMessage:"p*q will overflow, choose SMALLER Primes!");
            }
            if (string.IsNullOrEmpty(rsaDb.Plaintext))
            {
                ModelState.AddModelError(nameof(rsaDb.Plaintext), errorMessage:"Plaintext cannot be Null!");
            }
        }

        public void setRsaKeys(RsaDb rsaDb)
        {
            var n = rsaDb.p * rsaDb.q;
            var m = (rsaDb.p - 1)*(rsaDb.q - 1);
            var e = Get_e(m);
            rsaDb.d = Get_d(m,e);
            rsaDb.e = e;
            rsaDb.n = n;
        }

        // POST: Rsa/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RsaDbId,p,q,n,e,d,Plaintext,Ciphertext")] RsaDb rsaDb)
        {
            // from here
            CheckRsa(rsaDb);
            
            if (ModelState.IsValid)
            {
                rsaDb.Ciphertext = RsaEncText(rsaDb.p, rsaDb.q, rsaDb.Plaintext);
                setRsaKeys(rsaDb);
                rsaDb.UserId = GetUserId();
                _context.Add(rsaDb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rsaDb);
        }

        // GET: Rsa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsaDb = await _context.RsaDbs.Where(c => c.UserId==GetUserId()).FirstOrDefaultAsync(m=> m.RsaDbId == id);
            if (rsaDb == null)
            {
                return NotFound();
            }

            if (rsaDb.UserId==GetUserId())
            {
                return View(rsaDb);
            }
            else
            {
                return NotFound();
            }
            
        }

        // POST: Rsa/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RsaDbId,p,q,n,e,d,Plaintext,Ciphertext")] RsaDb rsaDb)
        {
            if (id != rsaDb.RsaDbId)
            {
                return NotFound();
            }
            CheckRsa(rsaDb);
            rsaDb.UserId = GetUserId();
            if (ModelState.IsValid)
            {
                try
                {
                    rsaDb.Ciphertext = RsaEncText(rsaDb.p, rsaDb.q, rsaDb.Plaintext);
                    setRsaKeys(rsaDb);

                    if (rsaDb.UserId==GetUserId())
                    {
                        _context.Update(rsaDb);
                        await _context.SaveChangesAsync();
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RsaDbExists(rsaDb.RsaDbId))
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
            return View(rsaDb);
        }

        // GET: Rsa/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rsaDb = await _context.RsaDbs.Where(c=>c.UserId==GetUserId())
                .FirstOrDefaultAsync(m => m.RsaDbId == id);
            if (rsaDb == null)
            {
                return NotFound();
            }

            return View(rsaDb);
        }

        // POST: Rsa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rsaDb = await _context.RsaDbs.Where(c=>c.RsaDbId==id && c.UserId==GetUserId()).FirstOrDefaultAsync(m=>m.RsaDbId==id);
            _context.RsaDbs.Remove(rsaDb);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RsaDbExists(int id)
        {
            return _context.RsaDbs.Any(e => e.RsaDbId == id);
        }
    }
}
