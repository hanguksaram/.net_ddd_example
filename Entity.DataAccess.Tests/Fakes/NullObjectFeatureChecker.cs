using Entity.CrossCutting;

namespace Entity.DataAccess.Tests
{
    internal class NullObjectFeatureChecker : IFeatureChecker
    {
        public bool Check(string name)
        {
            return false;
        }
    }
}