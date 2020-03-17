using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.CrossCutting
{
    public interface IFeatureChecker
    {
        bool Check(string name);
    }
}
