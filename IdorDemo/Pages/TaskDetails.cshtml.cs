using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorIdorDemo.Data;
using RazorIdorDemo.Models;
using Microsoft.EntityFrameworkCore;
using HashidsNet;

public class TaskDetailsModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IHashids _hashids;

    public TaskDetailsModel(AppDbContext db, IHashids hashids)
    {
        _context = db;
        _hashids = hashids;
    }
    public UserTask? UserTask { get; set; }

    public async Task<IActionResult> OnGetAsync(string hashedId)
    {
        // VULNERABILITY: We fetch by ID but never verify if 
        // the current user (e.g., "Alice") actually owns this task.

        // 1. Decode the Hashid
        var decoded = _hashids.Decode(hashedId);
        if (decoded.Length == 0) return NotFound();

        int internalId = decoded[0];
        
        // fix : && t.OwnerUsername == User.Identity.Name
        UserTask = await _context.UserTasks.FirstOrDefaultAsync(t => t.Id == internalId 
        //&& t.OwnerUsername == User.Identity.Name
        );

        if (UserTask == null) return NotFound();
        return Page();
    }
}