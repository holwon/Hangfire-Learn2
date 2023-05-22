using Hangfire;
using Hangfire.SqlServer;
using IntroductHangfire.Data;
using IntroductHangfire.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IPeopleRepository, PeopleRepository>();

builder.Services.AddTransient<ITimeService, TimeService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var hangfireConnection =
    builder.Configuration.GetConnectionString("HangfireConnection")
    ?? throw new InvalidOperationException("Connection string 'HangfireConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(hangfireConnection)
);

builder.Services.AddHangfire(
    configuration =>
        configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(
                hangfireConnection,
                new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true
                }
            )
);

builder.Services.AddHangfireServer(
    options => options.SchedulePollingInterval = TimeSpan.FromSeconds(15)
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<ITimeService>(
    "print-time",
    service => service.PrintNow(),
    "* 0/2 * * * *"
);

app.MapControllers();

app.Run();
