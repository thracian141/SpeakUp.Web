using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SpeakUp.Models;
using SpeakUp.Repository;
using SpeakUp.Services;
using SpeakUp.Utilities;

var SvelteSpecificOrigins = "_svelteSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SpeakUpWebContextConnection") ?? throw new InvalidOperationException("Connection string 'SpeakUpWebContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddCors(options => {
    options.AddPolicy(name: SvelteSpecificOrigins,
                      policy => {
                          policy.WithOrigins("http://localhost:5555",
											  "http://localhost:5555/authenticate",
                                              "91.139.215.122",
                                              "192.168.1.106",
                                              "http://localhost:5000").AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddTransient<IApplicationUserService, ApplicationUserService>();
builder.Services.AddTransient<IDeckService, DeckService>();
builder.Services.AddTransient<IWordService, WordService>();
builder.Services.AddTransient<IDailyPerformanceService, DailyPerformanceService>();
builder.Services.AddTransient<IGrammarService, GrammarService>();
builder.Services.AddTransient<ISentenceService, SentenceService>();
builder.Services.AddRazorPages();



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
DataSeeding();

app.UseRouting();

app.UseCors(SvelteSpecificOrigins);

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapAreaControllerRoute(
	  name: "Admin",
	  areaName: "Admin",
	  pattern: "Admin/{controller=Decks}/{action=Index}"
	);
	endpoints.MapControllerRoute(
	  name: "areaRoute",
	  pattern: "{area:exists}/{controller}/{action}"
	);

	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}"
	);
    endpoints.MapRazorPages();
});


app.Run();

void DataSeeding()
{
	using (var scope = app.Services.CreateScope())
	{
		var DbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
		DbInitializer.Initialize();
	}
}
