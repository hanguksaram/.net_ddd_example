using ETR.test.Schemas.RabbitMqRouting;
using MassTransit;
using System.Threading.Tasks;

namespace Entity.Infrastructure.Messaging
{
    public class MessageSender : IMessageSender
    {
        private readonly IBusControl _control;
        private const string _revocationQueueName = "smart.ticketing.Consumer_ETR.test.Schemas.SmartConfig.CashierAccess.RevocationCommand";

        public MessageSender(IBusControl control)
        {
            _control = control;
        }
        public async Task Send<T>(T message, string endpoint) where T : class
        {
            endpoint = GetEndpointAddressByName(endpoint) ?? endpoint;
            var se = await _control.GetSendEndpoint(new System.Uri(endpoint));
            await se.Send(message);
        }

        private string GetEndpointAddressByName(string endpointAddress)
        {
            string queue;
            switch (endpointAddress)
            {
                case "SendEmailsQueueName":
                    queue = RabbitMqRouting.MailService.QueueNames.SendEmailsQueueName;
                    break;
                case "TaskManagementQueueName":
                    queue = RabbitMqRouting.Planer.QueueNames.TaskManagementQueueName;
                    break;
                case "PartnerUserCreateQueueName":
                    queue = RabbitMqRouting.PartnerFile.QueueNames.PartnerUserCreateQueueName;
                    break;
                case "CreateSrnPoolQueueName":
                    queue = RabbitMqRouting.PartnerFile.QueueNames.CreateSrnPoolQueueName;
                    break;
                case "CreateSrnOperatorQueueName":
                    queue = RabbitMqRouting.PartnerFile.QueueNames.CreateSrnOperatorQueueName;
                    break;
                case "ConnectPlaceToUesMiddlewareQueueName":
                    queue = RabbitMqRouting.PartnerFile.QueueNames.ConnectPlaceToUesMiddlewareQueueName;
                    break;
                case "BsIntelligenceQueueName":
                    queue = RabbitMqRouting.BsIntelligence.QueueNames.BsIntelligenceQueueName;
                    break;
                case "RemoveSrnBasePointFromGRPQueueName":
                    queue = RabbitMqRouting.SrnPPR.QueueNames.RemoveSrnBasePointFromGRPQueueName;
                    break;
                case "NotifyCashiersWhenPlaceHasBeenRevokedQueueName":
                    queue = _revocationQueueName;
                    break;
                default:
                    queue = endpointAddress;
                    break;
            }
            return _control.Address.AbsoluteUri.Substring(0, _control.Address.AbsoluteUri.LastIndexOf('/')) + "/" + queue;
        }
    }
}
