namespace RazorIdorDemo.Models;

public class UserTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string SecretNote { get; set; } = string.Empty;
    public string OwnerUsername { get; set; } = string.Empty; // The "Context"
}