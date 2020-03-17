namespace Entity.Infrastructure.Messaging
{
    public class ProcessMessageSender : MessageSender, IProcessMessageSender
    {
        public ProcessMessageSender(IProcessBusControl control) : base(control.Control)
        {
        }
    }
}
