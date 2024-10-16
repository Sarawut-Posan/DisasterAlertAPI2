using Domain.Entities;

namespace Application.Interfaces;

public interface IAlertService
{
    Task SendAlertAsync(Alert alert);
}