using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Adressar_TestApp.Data;
using Adressar_TestApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Adressar_TestApp.Controllers
{
    public class AdresarController : Controller
    {
        private readonly AddresarDbContext _context;
        private readonly ILogger<AdresarController> logger;

        public AdresarController(AddresarDbContext context , ILogger<AdresarController> logger)
        {
            this._context = context;
            this.logger = logger;
        }

         public async Task<IActionResult> Index(string name, string lastName, string address, string phone)
        {
            var person = from p in _context.Person select p;

            if (!string.IsNullOrEmpty(name))
                person = person.Where(p => p.Name.Contains(name));

            if (!string.IsNullOrEmpty(lastName))
                person = person.Where(p => p.LastName.Contains(lastName));

            if (!string.IsNullOrEmpty(address))
                person = person.Where(p => p.Address.Contains(address));

            if (!string.IsNullOrEmpty(phone))
                person = person.Where(p => p.PhoneNumber.Contains(phone));

            return View(await person.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var person = await _context.Person.FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return NotFound();

            return View(person);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Address,PhoneNumber")] Person person)
        {
            if (ModelState.IsValid)
            {
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(person);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var person = await _context.Person.FindAsync(id);

            if (person == null)
                return NotFound();

            return View(person);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,LastName,Address,PhoneNumber")] Person person)
        {
            if (id != person.Id)
                return NotFound();

            if (ModelState.IsValid)
            {

                try
                {
                    if (!_context.Person.Any(p => p.Id == person.Id))
                        return NotFound();

                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    logger.LogError($"Something went wrong. Exception info: {ex}");
                }

                return RedirectToAction(nameof(Index));
            }
            return View(person);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var person = await _context.Person.FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                return NotFound();

            return View(person);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var person = await _context.Person.FindAsync(id);
            _context.Person.Remove(person);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}