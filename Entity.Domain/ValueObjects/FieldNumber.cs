using System;
using System.Text.RegularExpressions;

namespace Entity.Domain
{
    public class EntityNumber:IEquatable<EntityNumber>
    {
        public static bool TryParse(string number, out EntityNumber Entity)
        {
            Entity = null;
            if (string.IsNullOrWhiteSpace(ParseEntity(number)))
                return false;

            Entity = new EntityNumber(number);
            return true;
        }

        private static Regex _Entity = new Regex("\\d{8}", RegexOptions.Compiled);

        public string Number { get; }
        public string NumberAsIs { get; }

        public EntityNumber(string number)
        {
            NumberAsIs = number;
            number = ParseEntity(number);
            if (string.IsNullOrWhiteSpace(number))
                throw new InvalidOperationException("Invalid Entity");

            Number = number;
        }

        private static string ParseEntity(string number)
        {
            number = number?.Replace("-", "").Trim();

            return _Entity.IsMatch(number) ? number : null;
        }

        public bool IsControlNumberValid()
        {
            return int.Parse(Number.Substring(0, 7)) % 7 == int.Parse(Number.Substring(7));
        }

        public override string ToString()
        {
            return Number.Substring(0, 3) + "-" + Number.Substring(3, 4) + "-" + Number.Substring(7);
        }

        public override bool Equals(object obj) => obj is EntityNumber group && Equals(group);
        public static bool operator ==(EntityNumber left, EntityNumber rigth) => !(left is null ^ rigth is null) && (left is null || left.Equals(rigth));
        public static bool operator !=(EntityNumber left, EntityNumber rigth) => left is null ^ rigth is null || !(left is null || left.Equals(rigth));
        public bool Equals(EntityNumber other) => !(other is null) && (ReferenceEquals(this, other) || Number == other.Number);
        public override int GetHashCode() => ToString().GetHashCode();
    }
}
