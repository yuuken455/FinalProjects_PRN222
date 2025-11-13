using BLL.IService;

namespace RazorPage.Services
{
    public class BackgroundAppointmentUpdater : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<BackgroundAppointmentUpdater> logger;

        public BackgroundAppointmentUpdater(IServiceProvider serviceProvider, ILogger<BackgroundAppointmentUpdater> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // run every minute
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentService>();
                    var count = await appointmentService.AutoMarkInProgressAsync();
                    if (count > 0)
                    {
                        logger.LogInformation("Auto marked {Count} appointments InProgress", count);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error auto-marking appointments InProgress");
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
