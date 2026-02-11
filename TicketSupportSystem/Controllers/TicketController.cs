using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSupportSystem.Models;

namespace TicketSupportSystem.Controllers
{
    public class TicketsController : Controller
    {
        private readonly AppDbContext _context;

        public TicketsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string searchString, string statusFilter, string priorityFilter)
        {
            var tickets = _context.Tickets.Include(t => t.Comments).AsQueryable();

            // Search functionality
            if (!string.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(t =>
                    t.Title.Contains(searchString) ||
                    t.Description.Contains(searchString) ||
                    t.Email.Contains(searchString));
            }

            // Filter by status
            if (!string.IsNullOrEmpty(statusFilter))
            {
                tickets = tickets.Where(t => t.Status == statusFilter);
            }

            // Filter by priority
            if (!string.IsNullOrEmpty(priorityFilter))
            {
                tickets = tickets.Where(t => t.Priority == priorityFilter);
            }

            // Pass filters to view
            ViewBag.StatusFilter = statusFilter;
            ViewBag.PriorityFilter = priorityFilter;
            ViewBag.SearchString = searchString;

            return View(await tickets.OrderByDescending(t => t.CreatedAt).ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.CreatedAt = DateTime.Now;
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ticket created successfully!";
                return RedirectToAction(nameof(Details), new { id = ticket.Id });
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            Console.WriteLine($"Edit POST called for ticket {id}");

            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ticket.UpdatedAt = DateTime.Now;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ticket updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
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
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Ticket deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Add Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int ticketId, string content, string author)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null)
            {
                return NotFound();
            }

            var comment = new Comment
            {
                TicketId = ticketId,
                Content = content,
                Author = author,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Comment added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Update Status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Status = status;
            ticket.UpdatedAt = DateTime.Now;
            _context.Update(ticket);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Status updated to {status}!";
            return RedirectToAction(nameof(Details), new { id });
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.Id == id);
        }
    }
}