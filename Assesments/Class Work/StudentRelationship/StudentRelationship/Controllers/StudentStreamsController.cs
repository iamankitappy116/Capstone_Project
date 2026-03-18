using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentRelationship.Data;
using StudentRelationship.Models;

namespace StudentRelationship.Controllers
{
    public class StudentStreamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentStreamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StudentStreams
        public async Task<IActionResult> Index()
        {
            return View(await _context.Streams.ToListAsync());
        }

        // GET: StudentStreams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentStream = await _context.Streams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentStream == null)
            {
                return NotFound();
            }

            return View(studentStream);
        }

        // GET: StudentStreams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StudentStreams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Desc")] StudentStream studentStream)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentStream);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(studentStream);
        }

        // GET: StudentStreams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentStream = await _context.Streams.FindAsync(id);
            if (studentStream == null)
            {
                return NotFound();
            }
            return View(studentStream);
        }

        // POST: StudentStreams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Desc")] StudentStream studentStream)
        {
            if (id != studentStream.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentStream);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentStreamExists(studentStream.Id))
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
            return View(studentStream);
        }

        // GET: StudentStreams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentStream = await _context.Streams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentStream == null)
            {
                return NotFound();
            }

            return View(studentStream);
        }

        // POST: StudentStreams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentStream = await _context.Streams.FindAsync(id);
            if (studentStream != null)
            {
                _context.Streams.Remove(studentStream);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentStreamExists(int id)
        {
            return _context.Streams.Any(e => e.Id == id);
        }
    }
}
