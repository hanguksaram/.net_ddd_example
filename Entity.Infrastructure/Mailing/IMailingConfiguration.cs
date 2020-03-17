namespace Entity.Infrastructure
{
    public interface IMailingConfiguration
    {
        MailingConfigurationItem GetFor(string v);
    }
}