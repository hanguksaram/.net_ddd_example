using System;
using System.Threading.Tasks;

namespace Entity.ApplicationServices.Services
{
    public interface IEntityPlaceApplicationService
    {
        Task<bool> CheckIfEntityPlaceExistsByIdentificator(string PlaceIdOrTap, Guid? existingPlace);
    }
}
