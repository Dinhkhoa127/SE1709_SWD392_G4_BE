using BiologyRecognition.Application.Interface;
using BiologyRecognition.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiologyRecognition.Application.Implement
{
    public class RecognitionCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<RecognitionCleanupService> _logger;

        public RecognitionCleanupService(IServiceScopeFactory serviceScopeFactory, ILogger<RecognitionCleanupService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CleanupAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // chạy mỗi 24h

                await CleanupAsync(stoppingToken);
            }
        }

        private async Task CleanupAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var recognitionService = scope.ServiceProvider.GetRequiredService<IRecognitionService>();

            try
            {
                var deletedCount = await recognitionService.DeleteExpiredRecognitionsAsync(cancellationToken);
                if (deletedCount > 0)
                {
                    _logger.LogInformation($"Đã xóa {deletedCount} lịch sử nhận diện quá 90 ngày.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa Recognition quá hạn.");
            }
        }
    }
}
