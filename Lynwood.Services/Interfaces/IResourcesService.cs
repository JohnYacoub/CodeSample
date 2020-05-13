using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests.Resources;
using System.Collections.Generic;
using Lynwood.Models.Domain.Business;
using Lynwood.Models.Domain.Resources;
using Lynwood.Models.Domain.Resource;

namespace Lynwood.Services.Interfaces
{
    public interface IResourcesService
    {
        int Add(ResourcesAddRequest model);
        void Update(ResourcesUpdateRequest model);
        Resource Get(int id);
        Resource GetByUserId(int userId);
        List<Resource> Get();
        void Delete(int id);
        Paged<Resource> Get(int pageIndex, int pageSize);
        Paged<Resource> Get(int pageIndex, int pageSize, string query);
        List<CategoryType> GetCategoryTypes();
        List<BusinessType> GetBusinessTypes();
        List<IndustryType> GetIndustryType();
        List<IndustryTypesTotalCountByYear> GetIndustryTypesTotalCountByYear(int year);
        void Insert_Multiple(List<int> models, int id);
        void Update_Multiple(List<int> models, int id);
        List<ResourceAndCategory> GetByResourceId(int resourceId);
        ResourcesTypes GetResources();
        List<Recommendations> GetByInstanceId(int surveyInstanceId);
        Paged<Recommendations> GetRecommendation(int id, int pageIndex, int pageSize);
    }
}