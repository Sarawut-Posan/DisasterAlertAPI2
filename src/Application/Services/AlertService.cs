using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;
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
                // บันทึกลง database ก่อน
                var savedAlert = await _alertRepository.AddAsync(alert);
            
                // ส่ง notification
                var notificationSent = await _notificationService.SendNotificationAsync(alert.AlertMessage);
            
                if (!notificationSent)
                {
                    _logger.LogWarning(
                        "Notification failed for AlertId: {AlertId}, Region: {RegionId}", 
                        savedAlert.Id, 
                        savedAlert.RegionId);
                }
                else
                {
                    _logger.LogInformation(
                        "Alert sent and logged. AlertId: {AlertId}, Region: {RegionId}, Type: {DisasterType}", 
                        savedAlert.Id, 
                        savedAlert.RegionId, 
                        savedAlert.DisasterType);
                }

                return savedAlert;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Failed to send/log alert for Region: {RegionId}, Type: {DisasterType}", 
                    alert.RegionId, 
                    alert.DisasterType);
                throw;
            }
        }
        public string CreateAlertMessage(DisasterRisk risk)
        {
            return $"🚨 High-risk alert 🚨" +
                   $"\nRegion: {risk.RegionId}" +
                   $"\nDisaster Type: {risk.DisasterType}" +
                   $"\nRisk Level: {risk.RiskLevel}" +
                   $"\nRisk Score: {risk.RiskScore:F1}" +
                   $"\nTime: {DateTime.UtcNow:g}" +
                   $"\n\n⚠️ Please follow local authority instructions and stay safe!";
        }
    }



        // private string FormatAlertMessage(Alert alert)
        // {
        //     return $"\n🚨 DISASTER ALERT 🚨" +
        //            $"\nType: {alert.DisasterType}" +
        //            $"\nRisk Level: {alert.RiskLevel}" +
        //            $"\nRegion: {alert.RegionId}" +
        //            $"\nMessage: {alert.AlertMessage}" +
        //            $"\nTime: {alert.Timestamp:g}";
        // }
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
