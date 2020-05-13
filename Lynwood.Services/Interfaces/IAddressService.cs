using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Users;
using Lynwood.Models.Requests.Addresses;
using Lynwood.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lynwood.Services
{
    public interface IAddressService
    {
        int Insert(AddressAddRequest model, int userId);
        void Delete(int id);
        List<Address> Get();
        Address GetById(int id);
        void Update(AddressUpdateRequest model);
        List<StateId> GetStates();
        Address GetByResourceId(int id);
        Address GetByBusinessUserId(int userId);
        Address GetByBusinessId(int Id);

        Address GetByResourceUserId(int id);
    }
}