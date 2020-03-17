using Entity.Domain.DataAccessModels;
using System;
using Entity.CrossCutting.Exceptions;
using System.Text.RegularExpressions;

namespace Entity.Domain
{
    //todo should be value object in next database/storage
    public class EntityPlace
    {
        private string _pcc;
        private DateTime _authorizationDate;

        /// <summary>
        /// Конструктор для восстановления доменной сущности из БД
        /// </summary>
        /// <param name="model"></param>
        public EntityPlace(EntityPlaceDataAccessModel model)
        {
            Id = model.Id;
            _authorizationDate = model.AuthorizationDate;
            _pcc = model.Pcc;
        }


        /// <summary>
        /// Конструктор для создания доменных сущностей в Application сервисах
        /// </summary>
        /// <param name="pcc">Pseudo Town code or Ues Place id</param>
        /// <param name="authorizationDate">Authorization date</param>
        public EntityPlace(string pcc, DateTime authorizationDate)
        {
            Id = Guid.NewGuid();
            AuthorizationDate = authorizationDate.Date;
            Pcc = pcc;
        }

        public Guid Id { get; }
        public string Pcc 
        { 
            get => _pcc;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException(nameof(Pcc));

                value = value.Trim();
                if (!Regex.IsMatch(value, @"^[A-Z0-9]{4}$", RegexOptions.Compiled)
                    && !Regex.IsMatch(value, @"^[A-Z0-9]{9}$", RegexOptions.Compiled))
                    throw new DomainEntityInvariantException("Invalid PCC");

                _pcc = value;
            }
        }

        public DateTime AuthorizationDate 
        { 
            get => _authorizationDate;
            set
            {
                if (value < new DateTime(1753, 1, 1))
                    throw new DomainEntityInvariantException("Invalid authorization date");

                _authorizationDate = value;
            }
        }
    }
}