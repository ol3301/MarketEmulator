namespace ProjectsApi.Application.Domain.Interfaces;

public interface IEventHandler
{
    Task HandleAsync(string dto);
}
