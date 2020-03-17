namespace Entity.Common
{
    public interface ISortable
    {
        int Skip { get; set; }
        int Take { get; set; }

        string OrderBy { get; set; }
        bool OrderDesc { get; set; }
    }
}
