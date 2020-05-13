using System.Collections.Generic;
using Lynwood.Models;
using Lynwood.Models.Domain.Business;
using Lynwood.Models.Requests.BusinessVentures;
using Lynwood.Models.Domain;

namespace Lynwood.Services
{
    public interface IBusinessVenturesServices
    {
        #region CRUD
        int Add(BusinessVenturesAddRequest model);
        void Update(BusinessVenturesUpdateRequest model);
        Business Get(int id);
        Business GetByUserId(int userId);
        List<Business> Get();
        void Delete(int id);
        #endregion

        #region Pagination/Search
        Paged<Business> GetAllByPagination(int pageIndex, int pageSize);
        Paged<Business> SearchPagination(int pageIndex, int pageSize, string query);
        #endregion

        #region Business/Status/Industry Types
        List<BusinessType> GetBusinessTypes();
        List<Status> GetBusinessesStatus();
        List<IndustryType> GetIndustryType();
        #endregion

        #region Year Count
        List<BusinessTypesTotalCountByYear> GetBusinessTotalCountsByYear(int year);
        #endregion
    }
}