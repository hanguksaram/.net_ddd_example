using ETR.Test.Schemas.MailService.SendEmail.SendEmailTemplateMessageCommand;
using Entity.ApplicationServices.Notifications;
using Entity.Domain;
using Entity.Infrastructure.Messaging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Entity.Infrastructure
{
    public class SendRnRevocationMailHandler : ISendRnRevocationMailHandler
    {
        private readonly IMessageSender _messageSender;
        private readonly IMailingConfiguration _configuration;
        const string MailEndpointAddress = "SendEmailsQueueName";

        public SendRnRevocationMailHandler(IMessageSender messageSender, IMailingConfiguration configuration)
        {
            _messageSender = messageSender;
            _configuration = configuration;
        }

        public async Task Send(EntityEntity EntityEntity) {

            if (EntityEntity?.RnGroup == null)
                throw new InvalidOperationException("Cannot send Rn revokation email on non-Rn Entity");

            var Rn = EntityEntity.RnGroup;
            var model = new AnnulLastRnTerminalNotificationModel(Rn.Agn, EntityEntity.EntitySales.Address, EntityEntity.EntitySales.Deal.Name,  EntityEntity.Number);

            var jsModel = JsonConvert.SerializeObject(model);

            var notificationConfig = _configuration.GetFor("AnnulLastRnTerminalNotification");
            

            var command = new SendEmailMessageWithTemplateCommand
            {
                TemplateName = notificationConfig.Template.Name,
                Messages = new[] 
                {
                    new TemplateMessage
                    {
                        From = notificationConfig.From,
                        Subject = notificationConfig.Subject,
                        JsonModel = jsModel,
                        To = notificationConfig.To,
                        Cc = notificationConfig.CopyTo
                    }
                }
            };

            await _messageSender.Send(command, MailEndpointAddress);
        }

        private class AnnulLastRnTerminalNotificationModel
        {
            public AnnulLastRnTerminalNotificationModel(string tkpCode, string address, string DealName, string EntityNumber)
            {
                TkpCode = tkpCode;
                Address = address;
                DealName = DealName;
                EntityNumber = EntityNumber;
            }

            public string TkpCode { get; }
            public string Address { get; }
            public string DealName { get;  }
            public string EntityNumber { get; }
        }
    }
}
