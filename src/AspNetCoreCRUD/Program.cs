using AspNetCoreCRUD.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;

var builder = WebApplication.CreateBuilder(args);

Host.CreateDefaultBuilder(args)
.UseSerilog();



var cmdLineConfig = new ConfigurationBuilder().AddCommandLine(args).Build();
var levelSwitch = new LoggingLevelSwitch();

levelSwitch.MinimumLevel = cmdLineConfig.GetValue<bool>("verbose")
    ? LogEventLevel.Verbose
    : LogEventLevel.Debug;

//Создание логгеров и разделение их на файлы по уровням логгирования, файлы создаются автоматически если в этом есть необходимость
var Logger = new LoggerConfiguration()
                   .MinimumLevel.ControlledBy(levelSwitch)
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information).WriteTo.File(
                       $@"C:/Logging/Info.txt", rollingInterval: RollingInterval.Day))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(
                       new CompactJsonFormatter(), $@"C:/Logging/warning.json", rollingInterval: RollingInterval.Day))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose).WriteTo.File(
                       new CompactJsonFormatter(), $@"C:/Logging/verbose.json", rollingInterval: RollingInterval.Day))
                   .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error || e.Level == LogEventLevel.Fatal).WriteTo.File(
                       new CompactJsonFormatter(), $@"C:/Logging/errfatal.err.json", rollingInterval: RollingInterval.Day))
                   .WriteTo.Console().CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Logger);


// Add services to the container.
builder.Services.AddControllersWithViews();

// Connecting the database
builder.Services.AddDbContext<StudentDbContext>(options =>
options.UseSqlServer(
builder.Configuration.GetConnectionString("CrudConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
