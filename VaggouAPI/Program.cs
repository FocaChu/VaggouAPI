using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VaggouAP;
using VaggouAPI;
using VaggouAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to th

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped(typeof(IAdressService), typeof(AdressService));
builder.Services.AddScoped(typeof(IClientService), typeof(ClientService));
builder.Services.AddScoped(typeof(IFavoriteService), typeof(FavoriteService));
builder.Services.AddScoped(typeof(IMonthlyReportService), typeof(MonthlyReport));
builder.Services.AddScoped(typeof(IParkingLotService), typeof(ParkingLotService));
builder.Services.AddScoped(typeof(IParkingSpotService), typeof(ParkingSpotService));
builder.Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
builder.Services.AddScoped(typeof(IPaymentMethodService), typeof(PaymentMethodService));
builder.Services.AddScoped(typeof(IReservationService), typeof(ReservationService));
builder.Services.AddScoped(typeof(IUserService), typeof(UserService));
builder.Services.AddScoped(typeof(IVehicleService), typeof(VehicleService));
builder.Services.AddScoped(typeof(IVehicleModelService), typeof(VehicleModelService));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<Db>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseHttpsRedirection();

app.UseMiddleware<AppExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
