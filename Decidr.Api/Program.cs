using Decidr.Infrastructure;
using Decidr.Operations;
using FluentValidation;
using System.Text;

// TODO: Convert this to regular class syntax.

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();


// Add Swagger services
builder.Services.AddEndpointsApiExplorer(); // needed for minimal APIs
builder.Services.AddSwaggerGen();

// Add JWT auth
var key = Encoding.ASCII.GetBytes("super_secret_key_123!"); // use config/env in real apps

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddOperations();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // detailed error page
    app.UseSwagger();                // serve Swagger JSON
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
