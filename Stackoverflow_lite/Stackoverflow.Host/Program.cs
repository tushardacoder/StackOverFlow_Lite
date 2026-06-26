using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi;
using StackExchange.Redis;
using Stackoverflow.Application.Contracts;
using Stackoverflow.Application.Features.Auth.Command;
using Stackoverflow.Infrastructure.Data;
using Stackoverflow.Infrastructure.External;
using Stackoverflow.Infrastructure.Identity;
using Stackoverflow.Infrastructure.Repositories;
using Stackoverflow.Infrastructure.Security;
using Stackoverflow.Infrastructure.Services;
using System.Reflection;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); ;


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
            "secret_key_secret_key_12345_secret_key_12345_secret_key_12345_secret_key_12345"))
    };


    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            Console.WriteLine("JWT VALIDATED");
            return Task.CompletedTask;
        },

        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("JWT FAILED: " + context.Exception.Message);
            return Task.CompletedTask;
        },

        OnChallenge = context =>
        {
            Console.WriteLine("JWT CHALLENGE");
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddAuthorization();

builder.Services.AddApplicationDbContext(
    builder.Configuration.GetConnectionString("DefaultConnection")!,
    typeof(ApplicationDbContext).Assembly
);


builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(UserRegisterCommand).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(UserRegisterCommand).Assembly);

builder.Services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders(); ;


builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<
    IUserProfileRepository,
    UserProfileRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
builder.Services.AddScoped<IAnswerRepository,AnswerRepository>();
builder.Services.AddScoped<IVoteRepository,VoteRepository>();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration["Redis:ConnectionString"]!));

builder.Services.AddScoped<
    IRedisCacheService,
    RedisCacheService>();

builder.Services.AddScoped<
    IViewTrackingService,
    RedisViewTrackingService>();

builder.Services.AddValidatorsFromAssembly(
typeof(UserRegisterCommand).Assembly);






// JWT Authentication
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = [],
    });
});







//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters =
//        new TokenValidationParameters
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes("secret_key_secret_key_12345_secret_key_12345_secret_key_12345")),
            
//            ValidateIssuer = false,
//            ValidateAudience = false,
//            ValidateLifetime = true
//        };
//});



//builder.Services.AddApplicationDbContext(
//    builder.Configuration.GetConnectionString("DefaultConnection")!,
//    typeof(ApplicationDbContext).Assembly
//);


//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssembly(typeof(UserRegisterCommand).Assembly));

//builder.Services.AddValidatorsFromAssembly(typeof(UserRegisterCommand).Assembly);

//builder.Services.AddTransient(
//    typeof(IPipelineBehavior<,>),
//    typeof(ValidationBehavior<,>));

//builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
//.AddEntityFrameworkStores<ApplicationDbContext>();


//builder.Services.AddScoped<IIdentityService, IdentityService>();

//builder.Services.AddScoped<IJwtService, JwtService>();

//builder.Services.AddScoped<IUserRepository, UserRepository>();





//builder.Services.AddValidatorsFromAssembly(
//typeof(UserRegisterCommand).Assembly);

//builder.Services.AddScoped<IUserRepository, UserRepository>();

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
     


var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.Use(async (context, next) =>
{
    Console.WriteLine(
       "AUTH HEADER: " + context.Request.Headers["Authorization"]);
    await next();
});
app.UseAuthentication(); // Important


app.UseAuthentication(); // Important

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext =
        scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

    dbContext.Database.Migrate();
}

app.Run();




