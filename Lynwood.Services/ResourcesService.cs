using Newtonsoft.Json;
using Lynwood.Data;
using Lynwood.Data.Providers;
using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Business;
using Lynwood.Models.Domain.Resource;
using Lynwood.Models.Domain.Resources;
using Lynwood.Models.Requests.Resources;
using Lynwood.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Lynwood.Services
{
    public class ResourcesService : IResourcesService
    {
        private IDataProvider _dataProvider;

        public ResourcesService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public int Add(ResourcesAddRequest model)
        {
            int id = 0;

            _dataProvider.ExecuteNonQuery("dbo.Resources_Insert_V3",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = "@Id";
                    parm.SqlDbType = SqlDbType.Int;
                    parm.Direction = ParameterDirection.Output;
                    parms.Add(parm);

                    parms.AddWithValue("@UserId", model.UserId);
                    parms.AddWithValue("@CompanyName", model.CompanyName);
                    parms.AddWithValue("@Description", model.Description);
                    parms.AddWithValue("@ContactName", model.ContactName);
                    parms.AddWithValue("@ContactEmail", model.ContactEmail);
                    parms.AddWithValue("@BusinessTypeId", model.BusinessTypeId);
                    parms.AddWithValue("@IndustryTypeId", model.IndustryTypeId);
                    parms.AddWithValue("@ImageUrl", model.ImageUrl);
                    parms.AddWithValue("@SiteUrl", model.SiteUrl);
                    parms.AddWithValue("@Phone", model.Phone);
                    parms.AddWithValue("@AddressId", model.AddressId);

                }, returnParameters: delegate (SqlParameterCollection parms)
                {
                    Int32.TryParse(parms["@Id"].Value.ToString(), out id);
                });
            return id;
        }

        public void Update(ResourcesUpdateRequest model)
        {
            _dataProvider.ExecuteNonQuery("dbo.Resources_Update",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Id", model.Id);
                    parms.AddWithValue("@CompanyName", model.CompanyName);
                    parms.AddWithValue("@Description", model.Description);
                    parms.AddWithValue("@ContactName", model.ContactName);
                    parms.AddWithValue("@ContactEmail", model.ContactEmail);
                    parms.AddWithValue("@BusinessTypeId", model.BusinessTypeId);
                    parms.AddWithValue("@IndustryTypeId", model.IndustryTypeId);
                    parms.AddWithValue("@ImageUrl", model.ImageUrl);
                    parms.AddWithValue("@SiteUrl", model.SiteUrl);
                    parms.AddWithValue("@Phone", model.Phone);
                });
        }

        public Resource Get(int id)
        {
            Resource model = null;

            _dataProvider.ExecuteCmd("dbo.Resources_SelectById_V3",
                inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = ResourceMapper(reader);
            });

            return model;
        }

        public Resource GetByUserId(int userId)
        {
            Resource model = null;

            _dataProvider.ExecuteCmd("dbo.Resources_SelectByUserId",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@UserId", userId);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    model = ResourceMapper(reader);
                });

            return model;
        }

        public List<Resource> Get()
        {
            List<Resource> list = null;

            _dataProvider.ExecuteCmd("dbo.Resources_SelectAll",
                inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Resource model = ResourceMapper(reader);
                if (list == null)
                {
                    list = new List<Resource>();
                }
                list.Add(model);
            });

            return list;
        }

        public void Delete(int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.Resources_Delete",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Id", id);
                });
        }

        public Paged<Resource> Get(int pageIndex, int pageSize)
        {
            Paged<Resource> pagedResult = null;

            List<Resource> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd("dbo.Resources_SelectPaginated_V2",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Resource model = new Resource();
                    int startingIndex = 0;

                    model.Id = reader.GetSafeInt32(startingIndex++);
                    model.CompanyName = reader.GetSafeString(startingIndex++);
                    model.Description = reader.GetSafeString(startingIndex++);
                    model.ContactName = reader.GetSafeString(startingIndex++);
                    model.ContactEmail = reader.GetSafeString(startingIndex++);
                    model.BusinessType = new BusinessType();
                    model.BusinessType.Id = reader.GetSafeInt32(startingIndex++);
                    model.BusinessType.Name = reader.GetSafeString(startingIndex++);
                    model.ImageUrl = reader.GetSafeString(startingIndex++);
                    model.SiteUrl = reader.GetSafeString(startingIndex++);
                    model.Phone = reader.GetSafeString(startingIndex++);
                    model.DateCreated = reader.GetDateTime(startingIndex++);
                    model.DateModified = reader.GetDateTime(startingIndex++);

                    string categories = reader.GetSafeString(startingIndex++);
                    if (categories != null)
                    {
                        List<CategoryType> categoryType = JsonConvert.DeserializeObject<List<CategoryType>>(categories);
                        model.Categories = categoryType;
                    }

                    totalCount = reader.GetSafeInt32(startingIndex++); if (result == null)
                    {
                        result = new List<Resource>();
                    }
                    result.Add(model);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<Resource>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }

        public Paged<Resource> Get(int pageIndex, int pageSize, string query)
        {
            Paged<Resource> pagedResult = null;

            List<Resource> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd("dbo.Resources_SearchPaginated_V3",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Resource model = new Resource();
                    int startingIndex = 0;

                    model.Id = reader.GetSafeInt32(startingIndex++);
                    model.CompanyName = reader.GetSafeString(startingIndex++);
                    model.Description = reader.GetSafeString(startingIndex++);
                    model.ContactName = reader.GetSafeString(startingIndex++);
                    model.ContactEmail = reader.GetSafeString(startingIndex++);
                    model.BusinessType = new BusinessType();
                    model.BusinessType.Id = reader.GetSafeInt32(startingIndex++);
                    model.BusinessType.Name = reader.GetSafeString(startingIndex++);
                    model.IndustryType = new IndustryType();
                    model.IndustryType.Id = reader.GetSafeInt32(startingIndex++);
                    model.IndustryType.Name = reader.GetSafeString(startingIndex++);
                    model.ImageUrl = reader.GetSafeString(startingIndex++);
                    model.SiteUrl = reader.GetSafeString(startingIndex++);
                    model.Phone = reader.GetSafeString(startingIndex++);
                    model.DateCreated = reader.GetDateTime(startingIndex++);
                    model.DateModified = reader.GetDateTime(startingIndex++);
                    string categories = reader.GetSafeString(startingIndex++);
                    List<CategoryType> categoryType = JsonConvert.DeserializeObject<List<CategoryType>>(categories);
                    model.Categories = categoryType;
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (result == null)
                    {
                        result = new List<Resource>();
                    }
                    result.Add(model);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<Resource>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }

        public List<CategoryType> GetCategoryTypes()
        {
            List<CategoryType> list = null;
            _dataProvider.ExecuteCmd("dbo.CategoryTypes_SelectAll", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                CategoryType model = new CategoryType();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);

                if (list == null)
                {
                    list = new List<CategoryType>();
                }
                list.Add(model);
            });
            return list;
        }

        public static Resource ResourceMapper(IDataReader reader)
        {
            Resource model = new Resource();
            int startingIndex = 0;

            model.Id = reader.GetSafeInt32(startingIndex++);
            model.CompanyName = reader.GetSafeString(startingIndex++);
            model.Description = reader.GetSafeString(startingIndex++);
            model.ContactName = reader.GetSafeString(startingIndex++);
            model.ContactEmail = reader.GetSafeString(startingIndex++);
            model.BusinessType = new BusinessType();
            model.BusinessType.Id = reader.GetSafeInt32(startingIndex++);
            model.BusinessType.Name = reader.GetSafeString(startingIndex++);
            model.IndustryType = new IndustryType();
            model.IndustryType.Id = reader.GetSafeInt32(startingIndex++);
            model.IndustryType.Name = reader.GetSafeString(startingIndex++);
            model.ImageUrl = reader.GetSafeString(startingIndex++);
            model.SiteUrl = reader.GetSafeString(startingIndex++);
            model.Phone = reader.GetSafeString(startingIndex++);
            model.DateCreated = reader.GetDateTime(startingIndex++);
            model.DateModified = reader.GetDateTime(startingIndex++);
            string categories = reader.GetSafeString(startingIndex++);
            List<CategoryType> categoryType = JsonConvert.DeserializeObject<List<CategoryType>>(categories);
            model.Categories = categoryType;

            return model;
        }

        public static ResourceAndCategory ResourceAndCategoryMapper(IDataReader reader)
        {
            ResourceAndCategory model = new ResourceAndCategory();
            int startingIndex = 0;

            model.ResourceId = reader.GetSafeInt32(startingIndex++);
            model.ResourceCategoryId = reader.GetSafeInt32(startingIndex++);

            return model;
        }

        public void Insert_Multiple(List<int> models, int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.ResourcesAndCategories_Insert_Multiple", inputParamMapper: delegate (SqlParameterCollection parms)
            {

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ResourceId", typeof(Int32));
                dataTable.Columns.Add("ResourceCategoryId", typeof(Int32));

                foreach (int index in models)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = id;
                    dataRow[1] = index;
                    dataTable.Rows.Add(dataRow);
                }
                parms.AddWithValue("@ResourcesAndCategoriesList", dataTable);

            });
        }

        public void Update_Multiple(List<int> models, int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.ResourcesAndCategories_Update_Multiple", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("@_ResourceId", typeof(Int32));
                dataTable.Columns.Add("@_ResourceCategoryId", typeof(Int32));

                foreach (int index in models)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = id;
                    dataRow[1] = index;
                    dataTable.Rows.Add(dataRow);
                }
                parms.AddWithValue("@_ResourcesAndCategoriesList", dataTable);
                parms.AddWithValue("@_ResourceId", id);

            });
        }

        public List<ResourceAndCategory> GetByResourceId(int resourceId)
        {
            List<ResourceAndCategory> models = null;

            foreach (var model in models)
            {
                _dataProvider.ExecuteCmd("dbo.Resources_SelectById",
                    inputParamMapper: delegate (SqlParameterCollection parms)
                    {
                        parms.AddWithValue("@ResourceId", resourceId);
                    }, singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        ResourceAndCategory record = new ResourceAndCategory();
                        record = ResourceAndCategoryMapper(reader);
                        models.Add(record);
                    });
            }

            return models;
        }

        public List<BusinessType> GetBusinessTypes()
        {
            List<BusinessType> list = null;
            _dataProvider.ExecuteCmd("dbo.BusinessTypes_SelectAll", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                BusinessType model = new BusinessType();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);

                if (list == null)
                {
                    list = new List<BusinessType>();
                }
                list.Add(model);
            });
            return list;
        }
        public List<IndustryType> GetIndustryType()
        {
            List<IndustryType> list = null;
            _dataProvider.ExecuteCmd("dbo.IndustryTypes_SelectAll", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                IndustryType model = new IndustryType();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);

                if (list == null)
                {
                    list = new List<IndustryType>();
                }
                list.Add(model);
            });
            return list;
        }

        public ResourcesTypes GetResources()
        {

            ResourcesTypes data = new ResourcesTypes();
            _dataProvider.ExecuteCmd("dbo.ResourcesTypes_SelectAll", inputParamMapper: null, singleRecordMapper:
                delegate (IDataReader reader, short set)
            {

                switch (set)
                {
                    case 0:
                        CategoryType model = new CategoryType();
                        int startingIndex = 0;
                        model.Id = reader.GetSafeInt32(startingIndex++);
                        model.Name = reader.GetSafeString(startingIndex++);

                        if (data.Categories == null)
                        {
                            data.Categories = new List<CategoryType>();
                        }
                        data.Categories.Add(model);
                        break;
                }
                switch (set)
                {
                    case 1:
                        BusinessType model = new BusinessType();
                        int startingIndex = 0;
                        model.Id = reader.GetSafeInt32(startingIndex++);
                        model.Name = reader.GetSafeString(startingIndex++);

                        if (data.BusinessTypes == null)
                        {
                            data.BusinessTypes = new List<BusinessType>();
                        }
                        data.BusinessTypes.Add(model);
                        break;
                }
                switch (set)
                {
                    case 2:
                        IndustryType model = new IndustryType();
                        int startingIndex = 0;
                        model.Id = reader.GetSafeInt32(startingIndex++);
                        model.Name = reader.GetSafeString(startingIndex++);
                        if (data.IndustryTypes == null)
                        {
                            data.IndustryTypes = new List<IndustryType>();
                        }
                        data.IndustryTypes.Add(model);
                        break;
                }
            });
            return data;
        }

        public List<IndustryTypesTotalCountByYear> GetIndustryTypesTotalCountByYear(int year)
        {
            List<IndustryTypesTotalCountByYear> list = null;

            _dataProvider.ExecuteCmd("dbo.IndustryTypes_TotalCount",
                inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Year", year);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
             {
                 IndustryTypesTotalCountByYear model = new IndustryTypesTotalCountByYear();
                 int startingIndex = 0;

                 model.Name = reader.GetSafeString(startingIndex++);
                 model.Count = reader.GetSafeInt32(startingIndex++);

                 if (list == null)
                 {
                     list = new List<IndustryTypesTotalCountByYear>();
                 }
                 list.Add(model);
             });
            return list;
        }

        public List<Recommendations> GetByInstanceId(int surveyInstanceId)
        {
            List<Recommendations> list = null;
            _dataProvider.ExecuteCmd("dbo.recommendation", inputParamMapper: delegate (SqlParameterCollection parms)
             {
                 parms.AddWithValue("@SurveyInstanceId", surveyInstanceId);
             }, singleRecordMapper: delegate (IDataReader reader, short set)
             {
                 Recommendations model = RecommendationMapper(reader);
                 if (list == null)
                 {
                     list = new List<Recommendations>();
                 }
                 list.Add(model);
             });
            return list;
        }

        private static Recommendations RecommendationMapper(IDataReader reader)
        {
            Recommendations model = new Recommendations();
            int index = 0;
            model.Id = reader.GetSafeInt32(index++);
            model.CompanyName = reader.GetSafeString(index++);
            model.Description = reader.GetSafeString(index++);
            model.ContactName = reader.GetSafeString(index++);
            model.ContactEmail = reader.GetSafeString(index++);
            model.BusinessTypeName = reader.GetSafeString(index++);
            model.IndustryTypeName = reader.GetSafeString(index++);
            model.ImageUrl = reader.GetSafeString(index++);
            model.SiteUrl = reader.GetSafeString(index++);
            model.Phone = reader.GetSafeString(index++);
            string Catagories = reader.GetSafeString(index++);
            List<CategoryType> categoryType = JsonConvert.DeserializeObject<List<CategoryType>>(Catagories);
            model.Categories = categoryType;
            return model;
        }



        public Paged<Recommendations> GetRecommendation(int id, int pageIndex, int pageSize)
        {
            List<Recommendations> list = null;
            Paged<Recommendations> pageResult = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd("dbo.Recommendation_Paged", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@SurveyInstanceId", id);
                parms.AddWithValue("@pageIndex", pageIndex);
                parms.AddWithValue("@pageSize", pageSize);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Recommendations model = new Recommendations();
                int index = 0;
                model.Id = reader.GetSafeInt32(index++);
                model.CompanyName = reader.GetSafeString(index++);
                model.Description = reader.GetSafeString(index++);
                model.ContactName = reader.GetSafeString(index++);
                model.ContactEmail = reader.GetSafeString(index++);
                model.BusinessTypeName = reader.GetSafeString(index++);
                model.IndustryTypeName = reader.GetSafeString(index++);
                model.ImageUrl = reader.GetSafeString(index++);
                model.SiteUrl = reader.GetSafeString(index++);
                model.Phone = reader.GetSafeString(index++);
                string Catagories = reader.GetSafeString(index++);
                List<CategoryType> categoryType = JsonConvert.DeserializeObject<List<CategoryType>>(Catagories);
                model.Categories = categoryType;
                totalCount = reader.GetSafeInt32(index++);
                if (list == null)
                {
                    list = new List<Recommendations>();
                }
                list.Add(model);
            });
            if (list != null)
            {
                pageResult = new Paged<Recommendations>(list, pageIndex, pageSize, totalCount);
            }
            return pageResult;
        }
    }
}