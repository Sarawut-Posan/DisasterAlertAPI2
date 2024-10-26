using Domain.Entities;

namespace Application.Interfaces;

public interface IAlertService
{
    Task<Alert> SendAlertAsync(Alert alert);
}