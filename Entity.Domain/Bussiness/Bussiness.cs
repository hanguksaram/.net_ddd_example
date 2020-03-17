namespace Entity.Domain.Process
{
    public abstract class Process
    {
        public Process(string requestNumber, Author author)
        {
            RequestNumber = requestNumber;
            Author = author;
        }

        public abstract string Code { get; }
        public string RequestNumber { get; }
        public Author Author { get; }
    }
}
