using Entity.CrossCutting.Exceptions;
using System;
using System.Text.RegularExpressions;
using Entity.Domain.DataAccessModels;

namespace Entity.Domain
{
    //todo should be value object in next database/storage
    public sealed class SrnPool
    {
        private string _PoolNumber;
        private DateTime _authorizationDate;

        public SrnPool(string PoolNumber, DateTime authorizationDate)
        {
            Id = Guid.NewGuid();
            Tap = PoolNumber;
            AuthorizationDate = authorizationDate;
        }

        public SrnPool(SrnPoolDataAccessModel model)
        {
            Id = model.Id;
            _authorizationDate = model.AuthorizationDate;
            _PoolNumber = model.PoolNumber;
        }

        public Guid Id { get; }
        public string Tap
        {
            get => _PoolNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(Tap));

                value = value.Trim();
                if (!Regex.IsMatch(value, @"^[А-я]{4}\d{2}$"))
                    throw new DomainEntityInvariantException("Invalid TAP");

                _PoolNumber = value;
            }
        }

        public DateTime AuthorizationDate
        {
            get => _authorizationDate; 
            set
            {
                if(value == default)
                    throw new DomainEntityInvariantException("Invalid authorization date");

                _authorizationDate = value;
            }
        }
    }
}
