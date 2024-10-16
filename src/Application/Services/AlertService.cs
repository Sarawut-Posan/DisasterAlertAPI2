using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly ILogger<AlertService> _logger;

        public AlertService(IAlertRepository alertRepository, ILogger<AlertService> logger)
        {
            _alertRepository = alertRepository;
            _logger = logger;
        }

        public async Task SendAlertAsync(Alert alert)
        {
            try
            {
                await _alertRepository.AddAsync(alert);
                _logger.LogInformation($"Alert sent for region {alert.RegionId} - {alert.DisasterType}");
                // Here you would typically integrate with a messaging service (e.g., SMS, email)
                // For now, we'll just log the alert
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending alert for region {alert.RegionId}");
                throw;
            }
        }
    }
}
