using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuizTask.Data;
using System.Data;

namespace QuizTask.Services
{
    public class TokenExpireService : BackgroundService
    {
        private readonly ILogger<TokenExpireService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public TokenExpireService(
            IServiceProvider serviceProvider,
            ILogger<TokenExpireService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateLinkStatusesAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Runs once every 24 hours
            }
        }

        private async Task UpdateLinkStatusesAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var currentDate = DateTime.Now;
                var testLinks = await dbContext.TestLinks
                    .Where(a => a.Status.Contains("EmailSend"))
                    .ToListAsync();

                foreach (var item in testLinks)
                {
                    if (item.EndDate < currentDate)
                    {
                        item.Status = "Expire";
                        dbContext.Update(item);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
