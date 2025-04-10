using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuizTask.Data;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Text;
using QuizTask.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ? Ensure Connection String is Set
var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// ? Add Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ? Use ONLY AddIdentity (Remove AddDefaultIdentity)
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ? Configure Cookie Authentication for Identity UI
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";  // Ensure login page is accessible
    options.LogoutPath = "/Identity/Account/Logout";
});

// ? Add JWT Authentication (if needed)
var jwtKey = configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new ArgumentNullException("Jwt:Key", "JWT secret key is missing from configuration.");
}

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidIssuer = configuration["Jwt:Issuer"]
        };
    });

// ? Add MVC & Razor Pages
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddScoped<EmailService>();


builder.Services.AddSingleton<SmtpClient>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var emailSettings = config.GetSection("EmailSettings");

    return new SmtpClient(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]))
    {
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(
            emailSettings["SenderEmail"],
            emailSettings["SenderPassword"])
    };
});

builder.Services.AddHostedService<TokenExpireService>();
var app = builder.Build();

// ? Configure Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();  // ? Ensure authentication middleware is used
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();  // ? Ensure Identity UI Pages are mapped
app.Run();
