using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using RazorIdorDemo.Models;
using RazorIdorDemo.Data;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using HashidsNet;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Login"; // Where to go if not logged in
    });

// 1. Add Services
//builder.Services.AddRazorPages();
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Login"); // Critical: Don't lock yourself out of the login page!
});

builder.Services.AddSingleton<IHashids>(_ => new Hashids("this is a very secret salt phrase", 8));

// Using an In-Memory database for quick demonstration
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("DemoDb"));

var app = builder.Build();

// 2. Seed Data (Simulating Alice and Bob's private records)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.UserTasks.AddRange(
        new UserTask { Id = 1, Title = "Alice's Secret Plan", SecretNote = "Meet at the docks at midnight.", OwnerUsername = "Alice" },
        new UserTask { Id = 2, Title = "Bob's plan Results", SecretNote = "meeting with James bond tonight", OwnerUsername = "Bob" }
    );
    db.SaveChanges();
}

// 3. Configure Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();