namespace Entity.Infrastructure
{
    public class MailingConfigurationItem
    {
        public string Name { get; set; }
        public string From { get; set; }
        public MailingTemplate Template;
        public string Subject { get; set; }
        public string[] To { get; set; }
        public string[] CopyTo { get; set; }
        public string[] AttachPaths { get; set; }
    }

    public class MailingTemplate
    {
        public bool IsExternal { get; set; }
        public string Name { get; set; }
        public string TemplateContent { get; set; }
    }
}