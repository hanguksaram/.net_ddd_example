using Entity.Domain.DataAccessModels;
using System;
using System.Text.RegularExpressions;

namespace Entity.Domain
{
    public sealed class SrnGroup : IEquatable<SrnGroup>
    {
        public SrnGroup(string agn, string grp)
        {
            if (!Regex.IsMatch(agn, @"^\d{2}[А-я]{3}$"))
                throw new InvalidOperationException("Invalid AGN"); 
            
            if (!Regex.IsMatch(grp, @"^421\d{7}$"))
                throw new InvalidOperationException("Invalid GRP");

            Agn = agn.ToUpper();
            Grp = grp.ToUpper();
        }


        public SrnGroup(SrnEntityDataAccessModel model)
        {
            Agn = model.Agn;
            Grp = model.Grp;
        }

        /// <summary>
        /// Код ТКП
        /// </summary>
        public string Agn { get; }
        /// <summary>
        /// Грп
        /// </summary>
        public string Grp { get; }

        public override string ToString()
        {
            return Agn + " / " + Grp;
        }


        public override bool Equals(object obj) => obj is SrnGroup group && Equals(group);
        public static bool operator ==(SrnGroup left, SrnGroup rigth) => !(left is null ^ rigth is null) && (left is null || left.Equals(rigth));
        public static bool operator !=(SrnGroup left, SrnGroup rigth) => left is null ^ rigth is null || !(left is null || left.Equals(rigth));
        public bool Equals(SrnGroup other) => !(other is null) && (ReferenceEquals(this, other) || (Agn == other.Agn && Grp == other.Grp));
        public override int GetHashCode() => ToString().GetHashCode();
    }
}
