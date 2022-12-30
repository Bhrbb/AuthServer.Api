using AuthServer.Core.Configuration;
using AuthServer.Core.Models;
using AuthServer.Core.Reposityory;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data;
using AuthServer.Data.Repository;
using AuthServer.Data.UnitOfWork;
using AuthServer.Service;
using AuthServer.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Configuration;
using SharedLibrary.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOption"));

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Client"));

//DI Register
builder.Services.AddScoped<IAuthenticationService, AuthentictionService>();
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<ITokenService, TokenSevice>();
builder.Services.AddScoped(typeof(IGenericRepostory<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IServicesGeneric<,>), typeof(GenericService<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("AuthServer.Data");
    });
});
builder.Services.AddIdentity<UserApp, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;     


}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<CustomTokenOptions>(builder.Configuration.GetSection("TokenOption"));

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Client"));
builder.Services.AddAuthentication(option => {
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOptions>();

    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {

        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SingService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),
        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero//omrunu kontrol ederken senýn verdiðinin üstüne default
                                 //5 dk ekler farklý serverlardan istek yaparken geciklemeye tolerans tanýyor


    };
});


//ayný istekte býrden fazla internfacele karsýlasýrsa ayný nesne örneðinden devam edecek 
//Addtranslate olsa her seferinde birden yeniden nesne örneði üret demek 
//Singleton tek býr nesne örneðiyle devam etsin 

builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuthServer.Api v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
