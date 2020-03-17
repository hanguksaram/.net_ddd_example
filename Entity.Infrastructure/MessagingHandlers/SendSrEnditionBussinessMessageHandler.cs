using Entity.ApplicationServices.Services;
using Entity.Domain;
using Entity.Infrastructure.Messaging;
using System;
using System.Threading.Tasks;

namespace Entity.Infrastructure.MessagingHandlers
{
    public class SendRnRevocationProcessMessageHandler : ISendRnRevocationProcessMessageHandler
    {
        private const string Endpoint = "RemoveRnMainPointFromGRPQueueName";
        private readonly IProcessMessageSender _ProcessMessageSender;

        public SendRnRevocationProcessMessageHandler(IProcessMessageSender ProcessMessageSender)
        {
            _ProcessMessageSender = ProcessMessageSender;
        }

        public async Task Send(EntityEntity Entity, string user)
        {
            if (Entity?.RnGroup == null)
                throw new InvalidOperationException("Cannot send Rn revokation email on non-Rn Entity");

            var Rn = Entity.RnGroup;

            var command = new RemoveRnMainPointFromGRPCommand
            {
                AgencyCode = Entity.EntitySales.Agent.Code,
                Author = user,
                MainPointRnGroupCode = Rn.Grp,
                MainPointEntity = Entity.Number,
                TKPCode = Rn.Agn
            };


            await _ProcessMessageSender.Send(command, Endpoint);
        }
    }
}
