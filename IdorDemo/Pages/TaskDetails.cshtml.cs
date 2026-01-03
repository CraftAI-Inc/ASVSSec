using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorIdorDemo.Data;
using RazorIdorDemo.Models;
using Microsoft.EntityFrameworkCore;

public class TaskDetailsModel : PageModel
{
    private readonly AppDbContext _context;
    public TaskDetailsModel(AppDbContext context) => _context = context;

    public UserTask? UserTask { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        // VULNERABILITY: We fetch by ID but never verify if 
        // the current user (e.g., "Alice") actually owns this task.
        // fix : && t.OwnerUsername == User.Identity.Name
        UserTask = await _context.UserTasks.FirstOrDefaultAsync(t => t.Id == id && t.OwnerUsername == User.Identity.Name);

        if (UserTask == null) return NotFound();
        return Page();
    }
}