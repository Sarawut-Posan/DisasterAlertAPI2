using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly INotificationService _notificationService;
        private readonly ILogger<AlertService> _logger;

        public AlertService(
            IAlertRepository alertRepository,
            INotificationService notificationService,
            ILogger<AlertService> logger)
        {
            _alertRepository = alertRepository;
            _notificationService = notificationService;
            _logger = logger;
        }
        
        public async Task<Alert> SendAlertAsync(Alert alert)
        {
            try
            {
                await _alertRepository.AddAsync(alert);

                var message = FormatAlertMessage(alert);
                await _notificationService.SendNotificationAsync(message);

                _logger.LogInformation($"Alert sent for region {alert.RegionId}");
                return alert;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending alert for region {alert.RegionId}");
                throw;
            }
        }

        private string FormatAlertMessage(Alert alert)
        {
            return $"\n🚨 DISASTER ALERT 🚨" +
                   $"\nType: {alert.DisasterType}" +
                   $"\nRisk Level: {alert.RiskLevel}" +
                   $"\nRegion: {alert.RegionId}" +
                   $"\nMessage: {alert.AlertMessage}" +
                   $"\nTime: {alert.Timestamp:g}";
        }
        // public async Task<Alert> SendAlertAsync(Alert alert)
        // {
        //     try
        //     {
        //         await _alertRepository.AddAsync(alert);
        //         _logger.LogInformation($"Alert created for region {alert.RegionId} - {alert.DisasterType}");
        //         return alert;  // return alert หลังจาก save
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, $"Error creating alert for region {alert.RegionId}");
        //         throw;
        //     }
        // }
    }
}
