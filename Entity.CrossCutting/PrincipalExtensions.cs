using System.Security.Principal;

namespace Entity.CrossCutting
{
    public static class PrincipalExtensions
    {
        private const string _adminRole = "Uestest_Admin";
        public static bool IsAdmin(this IPrincipal principal)
            => principal.IsInRole(_adminRole);

    }
}
