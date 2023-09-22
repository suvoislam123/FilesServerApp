using Core.Entities.Identity;
using FileBankAPI.Extensions;
using FileBankAPI.Helpers;
using FileBankAPI.Middlewares;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo() { Title = "File Bank API", Version = "v11" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Description = "JWT Auth Bearer Scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    var securityRequirement = new OpenApiSecurityRequirement { { securityScheme, new[] { "Bearer" } } };
    c.AddSecurityRequirement(securityRequirement);
});
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddDbContext<FileBankDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase=false;
    opt.Password.RequireLowercase=false;
})
.AddEntityFrameworkStores<FileBankDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyHeader().WithOrigins("https://localhost:4200");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();
app.UseStaticFiles();
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<FileBankDbContext>();
        if (context.Database.GetPendingMigrations().Count() > 0)
        {
            await context.Database.MigrateAsync();
        }
        //await context.Database.MigrateAsync();
        
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An Error occured during migration");
    }
}


app.Run();
