using e_commerce.Core;
using e_commerce.data;
using e_commerce.Helpers;
using e_commerce.Interfaces;
using e_commerce.Middleware;
using e_commerce.Profiles;
using e_commerce.Services;
using e_commerce.Services.Account;
using e_commerce.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using e_commerce.Services.FilesManagement;
using e_commerce.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers to DI
builder.Services.AddControllers();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<FilestService>();
builder.Services.AddScoped<UserTokenContext>();
builder.Services.AddHttpClient<FilesManagementHelper>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAutoMapper(typeof(UserLoginProfile));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.Configure<ApiBehaviorOptions>(Options => {
    Options.InvalidModelStateResponseFactory = context => {
        var errors = context.ModelState
                .Where(e => e.Value != null && e.Value.Errors.Count > 0)
                .SelectMany(e => e.Value?.Errors != null ? e.Value.Errors.Select(x => x.ErrorMessage) : new List<string>()).ToList();

        return new BadRequestObjectResult(ApiResponse<Object>.ErrorResponse(errors, 400, "Validation error"));
    };
});

//cloudinary register
var cloudinaryAccount = new Account(
    builder.Configuration["CLOUDINARY:CloudName"],
    builder.Configuration["CLOUDINARY:ApiKey"],
    builder.Configuration["CLOUDINARY:ApiSecret"]
);
Cloudinary cloudinary = new Cloudinary(cloudinaryAccount);
cloudinary.Api.Secure = true;
builder.Services.AddSingleton(cloudinary);



// Swagger Configuration for JWT Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Pappa´s API", Version = "v1" });

    // Define the OAuth2.0 scheme that's in use (i.e., Implicit Flow)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});




//configuration JWT Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Token invalid: " + context.Exception.Message);
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});




// Add Swagger for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Build the application
var app = builder.Build();

/*================================ Middleware Configuration ================================*/

// JwtMiddleware register
app.UseMiddleware<JwtMiddleware>();

// Serve static files from "wwwroot"
app.UseStaticFiles();


// Enable Swagger UI in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Enable HTTPS redirection
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Enable Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Default test route
app.MapGet("/", () => Results.Json(new { status = "success", message = "API is working fine" }));

// Map Controllers (Make sure controllers are mapped after middleware setup)
app.MapControllers();

// Run the application
app.Run();
