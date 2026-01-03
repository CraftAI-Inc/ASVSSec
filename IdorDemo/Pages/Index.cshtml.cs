using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorIdorDemo.Models;
using RazorIdorDemo.Data;

[Authorize]
public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    public IndexModel(AppDbContext context) => _context = context;

    public IList<UserTask> UserTasks { get; set; } = default!;

    public async Task OnGetAsync()
    {
        // For the demo, we show tasks that belong to the logged-in user.
        // This makes it "feel" secure until the user manually changes the URL.
        UserTasks = await _context.UserTasks
            .Where(t => t.OwnerUsername == User.Identity.Name)
            .ToListAsync();
    }
}