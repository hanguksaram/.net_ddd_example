using System.Security.Principal;

namespace Entity.ApplicationServices
{
    public static class PredefinedUsers
    {
        public static IPrincipal ProcessUser => new Principal("Processs");
        public static IPrincipal MockUser => new Principal("Mock");
        public static IPrincipal ToUser(string name) => new Principal(name);

        private class Principal : IPrincipal
        {
            public Principal(string name)
            {
                Identity = new Identity(name);
            }

            public IIdentity Identity { get; }

            public bool IsInRole(string role)
            {
                return false;
            }
        }

        private class Identity : IIdentity
        {
            public Identity(string name)
            {
                Name = name;
            }

            public string AuthenticationType => null;

            public bool IsAuthenticated => false;

            public string Name { get; }
        }
    }
}
