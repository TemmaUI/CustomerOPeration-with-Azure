using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureCustomerOPeration.Data;
using AzureCustomerOPeration.Models;
using Microsoft.AspNetCore.Authorization;

namespace AzureCustomerOPeration.Controllers
{
    [Authorize]
    public class CustomerDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LeadEntities
        public async Task<IActionResult> Index()
        {
            return _context.Leads != null ?
                View(await _context.Leads.ToListAsync()) :
                Problem("Entity set'ApplicationDbContext.Leads' is null.");
        }

        // GET: CustomerDetails/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Leads
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (leadEntity == null)
            {
                return NotFound();
            }

            return View(leadEntity);
        }

        // GET: CustomerDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomerDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Email,Address")] LeadEntity leadEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leadEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leadEntity);
        }

        // GET: CustomerDetails/Edit/5
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Leads.FindAsync(Id);
            if (leadEntity == null)
            {
                return NotFound();
            }
            return View(leadEntity);
        }

        // POST: CustomerDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Phone,Email,Address")] LeadEntity leadEntity)
        {
            if (Id != leadEntity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leadEntity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeadEntityExists(leadEntity.Id))
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
            return View(leadEntity);
        }

        // GET: CustomerDetails/Delete/5
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Leads
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (leadEntity == null)
            {
                return NotFound();
            }

            return View(leadEntity);
        }

        // POST: CustomerDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int Id)
        {
            var leadEntity = await _context.Leads.FindAsync(Id);
            if (leadEntity != null)
            {
                _context.Leads.Remove(leadEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeadEntityExists(int Id)
        {
            return _context.Leads.Any(e => e.Id == Id);
        }
    }
}
