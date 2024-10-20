using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureCustomerOPeration.Data;
using AzureCustomerOPeration.Models;

namespace AzureCustomerOPeration.Controllers
{
    public class LeadEntitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LeadEntitiesController(ApplicationDbContext context)
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

        // GET: LeadEntities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Leads
                .FirstOrDefaultAsync(m => m.id == id);
            if (leadEntity == null)
            {
                return NotFound();
            }

            return View(leadEntity);
        }

        // GET: LeadEntities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LeadEntities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,Name,Phone,Email,Address")] LeadEntity leadEntity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leadEntity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leadEntity);
        }

        // GET: LeadEntities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Leads.FindAsync(id);
            if (leadEntity == null)
            {
                return NotFound();
            }
            return View(leadEntity);
        }

        // POST: LeadEntities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,Name,Phone,Email,Address")] LeadEntity leadEntity)
        {
            if (id != leadEntity.id)
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
                    if (!LeadEntityExists(leadEntity.id))
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

        // GET: LeadEntities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leadEntity = await _context.Leads
                .FirstOrDefaultAsync(m => m.id == id);
            if (leadEntity == null)
            {
                return NotFound();
            }

            return View(leadEntity);
        }

        // POST: LeadEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leadEntity = await _context.Leads.FindAsync(id);
            if (leadEntity != null)
            {
                _context.Leads.Remove(leadEntity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeadEntityExists(int id)
        {
            return _context.Leads.Any(e => e.id == id);
        }
    }
}
