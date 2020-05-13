using Lynwood.Models.Domain;
using Lynwood.Models.Requests.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lynwood.Services
{
    public interface IUserService
    {
        int Add(UserAddRequest model);

        void Update(UserUpdateRequest model);

        User Get(int id);

        List<User> Get();

        int Create(object userModel);

        Task<bool> LogInAsync(string email, string password);

        Task<bool> LogInTest(string email, string password, int id, string[] roles = null);

        UserAuth GetByEmail(string email);

        Task<bool> LogIn(string email, int id, IEnumerable<string> roles = null);

        Task<bool> AuthByEmail(string email, string password);

        AdminTotals GetAdminTotals();

        UserDetails Get(string email);

        User GetWithRolesById(int id);

        UserAuth GetWithRolesByEmail(string email);
    
        List<UsersTotalByMonth> GetUsersTotalByMonths(int year);

        void AddUserRoleId(int id, int userTypeId);

        int GetByToken(string token);

        void Confirm(int userId);

        void UpdateStatus(int id);

    };
}