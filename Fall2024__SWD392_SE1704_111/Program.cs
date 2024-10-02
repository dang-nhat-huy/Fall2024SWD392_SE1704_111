using BusinessObject.Mapper;
using BusinessObject.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.IRepository;
using Repository.Repository;
using Service;
using Service.IService;
using Service.Service;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            RoleClaimType = ClaimTypes.Role,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PRN231", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Please enter a valid token",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000/")
             .AllowAnyMethod()
             .AllowAnyHeader()
             .SetIsOriginAllowedToAllowWildcardSubdomains()
             .AllowCredentials()
             .SetIsOriginAllowed(_ => true);
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHairServiceRepository, HairServiceRepository>();
builder.Services.AddScoped<IScheduleRepository, ScheduleRepository>(); 
builder.Services.AddScoped<IScheduleUserRepository, ScheduleUserRepository>();

//Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IHairServiceService, HairServiceService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IScheduleUserService, ScheduleUserService>();

//Auto Mapper
builder.Services.AddAutoMapper(typeof(UserMapping));
builder.Services.AddAutoMapper(typeof(ServicesMapping));
builder.Services.AddAutoMapper(typeof(ScheduleMapping));
builder.Services.AddAutoMapper(typeof(ScheduleUserMapping));
builder.Services.AddAutoMapper(typeof(BookingMapping));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();

// Cấu hình Kestrel để lắng nghe trên toàn bộ các cổng
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(80); // HTTP
    serverOptions.ListenAnyIP(443, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});
