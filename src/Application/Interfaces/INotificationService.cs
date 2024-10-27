namespace Application.Interfaces;

public interface INotificationService
{
    Task<bool> SendNotificationAsync(string message); 
    
}