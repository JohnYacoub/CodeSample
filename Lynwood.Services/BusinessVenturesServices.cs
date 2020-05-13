using Lynwood.Data;
using Lynwood.Data.Providers;
using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Business;
using Lynwood.Models.Requests.BusinessVentures;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Lynwood.Services
{
    public class BusinessVenturesServices : IBusinessVenturesServices
    {
        private IDataProvider _dataProvider;

        public BusinessVenturesServices(IDataProvider dataprovider)
        {
            _dataProvider = dataprovider;
        }

        #region CRUD
        public int Add(BusinessVenturesAddRequest model)
        {
            int id = 0;
            _dataProvider.ExecuteNonQuery("dbo.BusinessVentures_Insert", inputParamMapper: delegate (SqlParameterCollection parm)
            {
                SqlParameter retreiveSqlVal = new SqlParameter();
                retreiveSqlVal.DbType = DbType.Int32;
                retreiveSqlVal.Direction = ParameterDirection.Output;
                retreiveSqlVal.ParameterName = "@Id";
                parm.Add(retreiveSqlVal);
                parm.AddWithValue("@UserId", model.UserId);
                parm.AddWithValue("@Name", model.Name);
                parm.AddWithValue("@StatusId", model.StatusId);
                parm.AddWithValue("@BusinessTypeId", model.BusinessTypeId);
                parm.AddWithValue("@IndustryTypeId", model.IndustryTypeId);
                parm.AddWithValue("@ProjectedAnnualBusinessIncome", model.ProjectedAnnualBusinessIncome);
                parm.AddWithValue("@AnnualBusinessIncome", model.AnnualBusinessIncome);
                parm.AddWithValue("@YearsInBusiness", model.YearsInBusiness);
                parm.AddWithValue("@ImageUrl", model.ImageUrl);
                parm.AddWithValue("@AddressId", model.AddressId);

            }, returnParameters: delegate (SqlParameterCollection param)
            {
                Int32.TryParse(param["@Id"].Value.ToString(), out id);
            }
            );
            return id;
        }

        public void Update(BusinessVenturesUpdateRequest model)
        {
            _dataProvider.ExecuteNonQuery("dbo.BusinessVentures_Update", inputParamMapper: delegate (SqlParameterCollection parm)
            {
                parm.AddWithValue("@Id", model.Id);
                parm.AddWithValue("@Name", model.Name);
                parm.AddWithValue("@StatusId", model.StatusId);
                parm.AddWithValue("@BusinessTypeId", model.BusinessTypeId);
                parm.AddWithValue("@IndustryTypeId", model.IndustryTypeId);
                parm.AddWithValue("@AnnualBusinessIncome", model.AnnualBusinessIncome);
                parm.AddWithValue("@ProjectedAnnualBusinessIncome", model.ProjectedAnnualBusinessIncome);
                parm.AddWithValue("@YearsInBusiness", model.YearsInBusiness);
                parm.AddWithValue("@ImageUrl", model.ImageUrl);
            });
        }

        public Business Get(int id)
        {
            Business model = null;
            _dataProvider.ExecuteCmd("dbo.BusinessVentures_SelectById_JOINED", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = BusinessMapper(reader);
            }
            );
            return model;
        }
        
        public Business GetByUserId(int userId)
        {
            Business model = null;
            _dataProvider.ExecuteCmd("dbo.UserProfiles_SelectByUserId_Business", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@userId", userId);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = BusinessMapper(reader);
            }
            );
            return model;
        }

        public List<Business> Get()
        {
            List<Business> businessesList = null;
            _dataProvider.ExecuteCmd("dbo.BusinessVentures_SelectAll_JOINED", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {

                Business model = BusinessMapper(reader);
                if (businessesList == null)
                {
                    businessesList = new List<Business>();
                }
                businessesList.Add(model);

            });
            return businessesList;
        }

        public void Delete(int id)
        {

            _dataProvider.ExecuteNonQuery("dbo.BusinessVentures DeleteById",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Id", id);
                });
        }
        #endregion

        #region Pagination/Search
        public Paged<Business> GetAllByPagination(int pageIndex, int pageSize)
        {
            Paged<Business> pagedResult = null;

            List<Business> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd(
                "dbo.BusinessVentures_SelectandPaginate_JOINED",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Business model = new Business();
                    int index = 0;
                    model.Id = reader.GetSafeInt32(index++);
                    model.UserId = reader.GetSafeInt32(index++);
                    model.Name = reader.GetSafeString(index++);
                    model.Status = new Status();
                    model.Status.Id = reader.GetSafeInt32(index++);
                    model.Status.Name = reader.GetSafeString(index++);
                    model.BusinessType = new BusinessType();
                    model.BusinessType.Id = reader.GetSafeInt32(index++);
                    model.BusinessType.Name = reader.GetSafeString(index++);
                    model.IndustryType = new IndustryType();
                    model.IndustryType.Id = reader.GetSafeInt32(index++);
                    model.IndustryType.Name = reader.GetSafeString(index++);
                    model.AnnualBusinessIncome = reader.GetSafeInt32(index++);
                    model.ProjectedAnnualBusinessIncome = reader.GetSafeInt32(index++);
                    model.YearsInBusiness = reader.GetSafeInt32(index++);
                    model.ImageUrl = reader.GetSafeString(index++);
                    model.DateCreated = reader.GetDateTime(index++);
                    model.DateModified = reader.GetDateTime(index++);
                    totalCount = reader.GetSafeInt32(index++);

                    if (result == null)
                    {
                        result = new List<Business>();
                    }

                    result.Add(model);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<Business>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<Business> SearchPagination(int pageIndex, int pageSize, string query)
        {
            Paged<Business> pagedResult = null;

            List<Business> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd(
                "dbo.BusinessVentures_Search_JOINED",

                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Business model = new Business();
                    int index = 0;
                    model.Id = reader.GetSafeInt32(index++);
                    model.UserId = reader.GetSafeInt32(index++);
                    model.Name = reader.GetSafeString(index++);
                    model.Status = new Status();
                    model.Status.Id = reader.GetSafeInt32(index++);
                    model.Status.Name = reader.GetSafeString(index++);
                    model.BusinessType = new BusinessType();
                    model.BusinessType.Id = reader.GetSafeInt32(index++);
                    model.BusinessType.Name = reader.GetSafeString(index++);
                    model.IndustryType = new IndustryType();
                    model.IndustryType.Id = reader.GetSafeInt32(index++);
                    model.IndustryType.Name = reader.GetSafeString(index++);
                    model.AnnualBusinessIncome = reader.GetSafeInt32(index++);
                    model.ProjectedAnnualBusinessIncome = reader.GetSafeInt32(index++);
                    model.YearsInBusiness = reader.GetSafeInt32(index++);
                    model.ImageUrl = reader.GetSafeString(index++);
                    model.DateCreated = reader.GetDateTime(index++);
                    model.DateModified = reader.GetDateTime(index++);
                    totalCount = reader.GetSafeInt32(index++);

                    if (result == null)
                    {
                        result = new List<Business>();
                    }

                    result.Add(model);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<Business>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }

        #endregion

        #region Business/Status/Industry Types
        public List<BusinessType> GetBusinessTypes()
        {
            List<BusinessType> list = null;
            _dataProvider.ExecuteCmd("dbo.BusinessTypes_SelectAll", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                BusinessType model = new BusinessType();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);

                if(list == null)
                {
                    list = new List<BusinessType>();
                }
                list.Add(model);
            });
            return list;
        }

        public List<Status> GetBusinessesStatus()
        {
            List<Status> list = null;
            _dataProvider.ExecuteCmd("dbo.BusinessesStatus_SelectAll", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Status model = new Status();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);

                if (list == null)
                {
                    list = new List<Status>();
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
        #endregion

        #region Year Count
        public List<BusinessTypesTotalCountByYear> GetBusinessTotalCountsByYear(int year)
        {
            List<BusinessTypesTotalCountByYear> list = null;

            _dataProvider.ExecuteCmd("dbo.BusinessTypes_TotalCount",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Year", year);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    BusinessTypesTotalCountByYear model = new BusinessTypesTotalCountByYear();
                    int startingIndex = 0;

                    model.Name = reader.GetSafeString(startingIndex++);
                    model.Count = reader.GetSafeInt32(startingIndex++);

                    if (list == null)
                    {
                        list = new List<BusinessTypesTotalCountByYear>();
                    }
                    list.Add(model);
                });
            return list;
        }
        #endregion

        #region Mapper(s)
        private static Business BusinessMapper(IDataReader reader)
        {
            Business model = new Business();

            int startingIndex = 0;
            model.Id = reader.GetSafeInt32(startingIndex++);
            model.UserId = reader.GetSafeInt32(startingIndex++);
            model.Name = reader.GetSafeString(startingIndex++);
            model.Status = new Status();
            model.Status.Id = reader.GetSafeInt32(startingIndex++);
            model.Status.Name = reader.GetSafeString(startingIndex++);
            model.BusinessType = new BusinessType();
            model.BusinessType.Id = reader.GetSafeInt32(startingIndex++);
            model.BusinessType.Name = reader.GetSafeString(startingIndex++);
            model.IndustryType = new IndustryType();
            model.IndustryType.Id = reader.GetSafeInt32(startingIndex++);
            model.IndustryType.Name = reader.GetSafeString(startingIndex++);
            model.AnnualBusinessIncome = reader.GetSafeInt32(startingIndex++);
            model.ProjectedAnnualBusinessIncome = reader.GetSafeInt32(startingIndex++);
            model.YearsInBusiness = reader.GetSafeInt32(startingIndex++);
            model.ImageUrl = reader.GetSafeString(startingIndex++);
            model.DateCreated = reader.GetDateTime(startingIndex++);
            model.DateModified = reader.GetDateTime(startingIndex++);

            return model;
        }
        #endregion

    }
}
