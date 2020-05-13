using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Users;
using Lynwood.Models.Requests.Users;
using System.Collections.Generic;

namespace Lynwood.Services.Interfaces
{
    public interface IUserProfileService
    {
        void Delete(int id);
        Paged<UserProfile> GetAllPagination(int pageIndex, int pageSize);
        UserProfile GetById(int userId);
        UserProfile GetByUserId(int userId);
        int AddProfile(UserProfileAddRequest model, int userId);
        void Update(UserProfileUpdateRequest model);
        Paged<UserProfile> SearchPagination( int pageIndex, int pageSize, string query);
        List<RaceEthnicity> GetRaceEthnicitys();
        List<EducationLevel> GetEducationLevels();
        List<EducationLevelTotalByYear> GetEducationLevelTotalByYears(int year);
        List<RaceEthnicityTotalByYear> GetRaceEthnicityTotalByYears(int year);
    }
}