namespace Application.Interfaces;

public interface IMessagingService
{
    Task SendSmsAsync(string phoneNumber, string message);
    Task SendBatchSmsAsync(IEnumerable<string> phoneNumbers, string message);
}