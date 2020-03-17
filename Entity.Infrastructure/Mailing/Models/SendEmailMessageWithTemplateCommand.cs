using ETR.test.Schemas.MailService.SendEmail.SendEmailTemplateMessageCommand;
using System;

namespace Entity.Infrastructure
{
    internal class SendEmailMessageWithTemplateCommand: ISendEmailMessageWithTemplateCommand
    {
        public SendEmailMessageWithTemplateCommand()
        {
            AssemblyName = null;
            Localization = "ru";
            CorrelationId = Guid.Empty;
        }
        public Guid CorrelationId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateBody { get; set; }
        public TemplateMessage[] Messages { get; set; }
        public string AssemblyName { get; set; }
        public string Localization { get; set; }
    }
}