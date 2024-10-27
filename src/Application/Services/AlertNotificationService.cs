using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class AlertNotificationService 
{
    private readonly IMessagingService _messagingService;
    private readonly ILogger<AlertNotificationService> _logger;

    public AlertNotificationService(
        IMessagingService messagingService,
        ILogger<AlertNotificationService> logger)
    {
        _messagingService = messagingService;
        _logger = logger;
    }

    
    // twilio service [cannot verify] 
    public async Task SendDisasterAlertAsync(Alert alert, IEnumerable<string> recipientPhoneNumbers)
    {
        var message = FormatAlertMessage(alert);
        
        try
        {
            await _messagingService.SendBatchSmsAsync(recipientPhoneNumbers, message);
            _logger.LogInformation(
                "Alert notifications sent successfully for Region: {RegionId}, DisasterType: {DisasterType}", 
                alert.RegionId, 
                alert.DisasterType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Failed to send alert notifications for Region: {RegionId}", 
                alert.RegionId);
            throw;
        }
    }

    private string FormatAlertMessage(Alert alert)
    {
        return $"DISASTER ALERT!\n" +
               $"Type: {alert.DisasterType}\n" +
               $"Risk Level: {alert.RiskLevel}\n" +
               $"Region: {alert.RegionId}\n" +
               $"Message: {alert.AlertMessage}\n" +
               $"Time: {alert.Timestamp:g}";
    }
    
    
}