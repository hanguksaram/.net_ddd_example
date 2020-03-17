namespace Entity.Domain.Process
{
    public class Author
    {
        public Author(string login):this(login, null) {}

        public Author(string login, string name)
        {
            Login = login;
            Name = name;
        }

        public string Login { get; }
        public string Name { get; }
    }
}