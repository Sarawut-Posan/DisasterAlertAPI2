using Application.Configuration;
using Application.Exceptions;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Services;

public class TwilioService : IMessagingService
{
    private readonly TwilioClient _client;
    private readonly string _fromNumber;
    private readonly ILogger<TwilioService> _logger;

    public TwilioService(
        IOptions<TwilioSettings> twilioSettings,
        ILogger<TwilioService> logger)
    {
        var settings = twilioSettings.Value;
        TwilioClient.Init(settings.AccountSid, settings.AuthToken);
        _fromNumber = settings.FromPhoneNumber;
        _logger = logger;
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            var messageResource = await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(_fromNumber),
                to: new PhoneNumber(phoneNumber)
            );

            _logger.LogInformation(
                "SMS sent successfully. MessageSid: {MessageSid}", 
                messageResource.Sid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "Failed to send SMS to {PhoneNumber}", 
                phoneNumber);
            throw new ExternalApiException("Failed to send SMS", ex);
        }
    }

    public async Task SendBatchSmsAsync(IEnumerable<string> phoneNumbers, string message)
    {
        var tasks = phoneNumbers.Select(phone => SendSmsAsync(phone, message));
        await Task.WhenAll(tasks);
    }
}