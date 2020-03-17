using System.Threading.Tasks;

namespace Entity.ApplicationServices
{
    public interface IEntityRevocationProcessService
    {
        Task ProcessCancelledNotificationHandler(ProcessCancelledNotificationEvent @event);
        Task ProcessCompletedNotificationHandler(ProcessCompletedNotificationEvent @event);
        Task ProcessStartedNotificationHandler(ProcessStartedNotificationEvent @event);
    }
}