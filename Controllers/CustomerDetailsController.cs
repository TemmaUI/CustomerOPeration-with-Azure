using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AzureCustomerOPeration.Data;
using AzureCustomerOPeration.Models;

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

        // Admin and SalesRep can view details
        [Authorize(Roles = "Admin,SalesRep")]
        public async Task<IActionResult> Index()
        {
            return _context.Leads != null ?
                View(await _context.Leads.ToListAsync()) :
                Problem("Entity set 'ApplicationDbContext.Leads' is null.");
        }

        // Admin and SalesRep can view details
        [Authorize(Roles = "Admin,SalesRep")]
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

        // Admin only can create customer details
        [Authorize(Policy = "CustomAuthorize")]
        public IActionResult Create()
        {
            return View();
        }

        // Admin only can create customer details
        [Authorize(Policy = "CustomAuthorize")]
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

        // Admin only can edit customer details
        [Authorize(Policy = "CustomAuthorize")]
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

        // Admin only can edit customer details
        [Authorize(Policy = "CustomAuthorize")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,Name,Phone,Email,Address")] LeadEntity leadEntity)
        {
            if (Id != null)
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

        // Admin only can delete customer details
        [Authorize(Policy = "CustomAuthorize")]
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

        // Admin only can delete customer details
        [Authorize(Policy = "CustomAuthorize")]
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