
using AzureCustomerOPeration.Data;
using Microsoft.AspNetCore.Mvc;
using AzureCustomerOPeration.Enums;
using AzureCustomerOPeration.Models;
using Microsoft.AspNetCore.Authorization;

namespace AzureCustomerOPeration.Controllers
{
    public class AccountProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string AdminRole = "Admin";
        private const string SalesRepRole = "SalesRep";

        public AccountProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create
        [Authorize(Roles = AdminRole)]
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [Authorize(Roles = AdminRole)]
        [HttpPost]
        public IActionResult CreateCustomer(LeadEntity lead)
        {
            _context.Leads.Add(lead);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Read (Details)
        [Authorize(Roles = AdminRole)]
        [HttpGet]
        public IActionResult CustomerDetails(int id)
        {
            var lead = _context.Leads.Find(id);
            return View(lead);
        }

        // Update
        [Authorize(Roles = SalesRepRole)]
        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            var lead = _context.Leads.Find(id);
            return View(lead);
        }

        [Authorize(Roles = SalesRepRole)]
        [HttpPost]
        public IActionResult UpdateCustomer(LeadEntity lead)
        {
            _context.Leads.Update(lead);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Delete
        [Authorize(Roles = AdminRole)]
        [HttpGet]
        public IActionResult DeleteCustomer(int id)
        {
            var lead = _context.Leads.Find(id);
            return View(lead);
        }

        [Authorize(Roles = AdminRole)]
        [HttpPost]
        public IActionResult DeleteCustomerConfirmed(int id)
        {
            var lead = _context.Leads.Find(id);
            _context.Leads.Remove(lead);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
