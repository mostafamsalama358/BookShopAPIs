using Domains;
using BookShopAPIs.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Bl.UnitOfWork;
using Bl.Repos.Account;
using Bl.Repos.Book;
using Bl.Repos.BorrowBook;
using Bl;
using BookShopAPIs.Helpers;
using Newtonsoft.Json;
using Bl.Repos.Category;
using Bl.Repos.Author;
using Domains.DTOS.ForLogin;
using Bl.ValidationService;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.   

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // Handle reference loops if necessary
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerAuth();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("https://localhost:7169;http://localhost:5256") // Adjust for your front-end URL and port
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;

    // Lockout settings
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<BookShopContextAPIs>()
.AddDefaultTokenProviders(); // Register default token providers


 builder.Services.AddDbContext<BookShopContextAPIs>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));

builder.Services.AddCustomAuth(builder.Configuration);



//// This registers the RoleManager for DI
//builder.Services.AddScoped<RoleManager<IdentityRole>>();


#region Custom Services
builder.Services.AddScoped<IAccount, Account>();
builder.Services.AddScoped<IBook, ClsBook>();
builder.Services.AddScoped<IBorrowBook, BorrowBook>();
builder.Services.AddScoped<ICategory,Category>();
builder.Services.AddScoped<IAuthor, Author>();
builder.Services.AddTransient<IEmailServices,EmailServices>();
builder.Services.Configure< EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
//builder.Services.AddScoped(typeof(IValidationService<>), typeof(ValidationService<>));

#endregion

#region Unit Of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
#endregion




var app = builder.Build();



// Enable CORS for the specified policy
app.UseCors("AllowLocalhost");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.Run();
