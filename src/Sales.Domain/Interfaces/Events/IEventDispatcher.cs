namespace Sales.Domain.Interfaces.Events
{
    public interface IEventDispatcher
    {
        Task DispatchAsync<TEvent>(TEvent evento) where TEvent : class;
    }
}
