using System;
using System.Threading.Tasks;

namespace Entity.ApplicationServices
{
    public class EntityRevocationProcessService : IEntityRevocationProcessService
    {
        private readonly IEntityRevocationProcessRepository _repo;
        public EntityRevocationProcessService(IEntityRevocationProcessRepository repo)
        {
            _repo = repo;
        }

        public async Task ProcessStartedNotificationHandler(ProcessStartedNotificationEvent @event)
        {
            var Process = await _repo.StartNew(@event.RequestNumber, @event.Author);

            foreach (var revokee in @event.PlacesToRevoke)
            {
                //todo
                //var pos = _repoPos.GetById(revokee.EntitySalesEntityGuid);
                //if (pos.IsEmpty)
                //    throw new Error();
                //
                //var EntityPlace  = pos.GetEntityPlace(revokee.EntityNumber, revokee.PlaceId);
                //
                //Process.AddRevokee(EntityPlace);
            }

            await _repo.Save(Process);
        }

        public async Task ProcessCancelledNotificationHandler(ProcessCancelledNotificationEvent @event)
        {
            var mayBeProcess = await _repo.GetByRequestNumber(@event.RequestNumber);

            if (mayBeProcess.IsEmpty) //todo
                throw new System.Exception("not found");
            var Process = mayBeProcess.Value;

            Process.Cancel(@event.Author);

            await _repo.Save(Process);
        }


        public async Task ProcessCompletedNotificationHandler(ProcessCompletedNotificationEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
