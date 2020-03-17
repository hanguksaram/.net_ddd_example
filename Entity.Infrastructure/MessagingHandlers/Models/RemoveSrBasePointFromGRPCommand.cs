using ETR.Test.Schemas.RnPPR.RemoveRnMainPoint.RemoveRnMainPointFromGRPCommand;

namespace Entity.Infrastructure.MessagingHandlers
{
    internal class RemoveRnMainPointFromGRPCommand : IRemoveRnMainPointFromGRPCommand
    {
        public string Author { get; set; }
        public string AgencyCode { get; set; }
        public string TKPCode { get; set; }
        public string MainPointRnGroupCode { get; set; }
        public string MainPointEntity { get; set; }
    }
}