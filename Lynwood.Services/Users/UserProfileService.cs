using Lynwood.Data;
using Lynwood.Data.Providers;
using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Users;
using Lynwood.Models.Requests.Users;
using Lynwood.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace Lynwood.Services
{
    public class UserProfileService : IUserProfileService
    {
        private IDataProvider _dataProvider;


        public UserProfileService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public int AddProfile(UserProfileAddRequest model, int userId)
        {
            int id = 0;

            _dataProvider.ExecuteNonQuery("dbo.UserProfiles_Insert", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                SqlParameter parm = new SqlParameter();
                parm.ParameterName = "@Id";
                parm.SqlDbType = SqlDbType.Int;
                parm.Direction = ParameterDirection.Output;
                parms.Add(parm);

                parms.AddWithValue("@UserId", userId);
                parms.AddWithValue("@FirstName", model.FirstName);
                parms.AddWithValue("@LastName", model.LastName);
                parms.AddWithValue("@Bio", model.Bio ?? SqlString.Null);
                parms.AddWithValue("@AvatarUrl", model.AvatarUrl ?? SqlString.Null);
                parms.AddWithValue("@Phone", model.Phone);
                parms.AddWithValue("@RaceEthnicityId", model.RaceEthnicityId ?? SqlInt32.Null);
                parms.AddWithValue("@EducationLevelId", model.EducationLevelId ?? SqlInt32.Null);
                parms.AddWithValue("@HouseIncome", model.HouseIncome);
                parms.AddWithValue("@YearsInBusiness", model.YearsInBusiness ?? SqlInt32.Null);
                parms.AddWithValue("@Zip", model.Zip);

            }, returnParameters: delegate (SqlParameterCollection parms)
             {
                 Int32.TryParse(parms["@Id"].Value.ToString(), out id);
             });
            return id;
        }

        public void Update(UserProfileUpdateRequest model)
        {
            _dataProvider.ExecuteNonQuery("dbo.UserProfiles_Update", inputParamMapper: delegate (SqlParameterCollection parms)
            {

                parms.AddWithValue("@Id", model.Id);
                parms.AddWithValue("@Bio", model.Bio ?? SqlString.Null); ;
                parms.AddWithValue("@AvatarUrl", model.AvatarUrl ?? SqlString.Null);
                parms.AddWithValue("@Phone", model.Phone);
                parms.AddWithValue("@RaceEthnicityId", model.RaceEthnicityId ?? SqlInt32.Null);
                parms.AddWithValue("@EducationLevelId", model.EducationLevelId ?? SqlInt32.Null);
                parms.AddWithValue("@HouseIncome", model.HouseIncome);
                parms.AddWithValue("@YearsInBusiness", model.YearsInBusiness ?? SqlInt32.Null);
                parms.AddWithValue("@FirstName", model.FirstName);
                parms.AddWithValue("@Lastname", model.LastName);
                parms.AddWithValue("@Zip", model.Zip);
            });
        }

        public UserProfile GetById(int id)
        {
            UserProfile model = null;

            _dataProvider.ExecuteCmd("dbo.UserProfiles_SelectById_Join", 
                inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
           {
               model = UserProfileMapper(reader);

           });

            return model;
        }

        public UserProfile GetByUserId(int userId)
        {
            UserProfile model = null;

            _dataProvider.ExecuteCmd("dbo.UserProfiles_SelectByUserId_Join_V2",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@UserId", userId);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    model = UserProfileWithStatusMapper(reader);

                });

            return model;
        }


        private static UserProfile UserProfileMapper(IDataReader reader)
        {
            UserProfile model = new UserProfile();
            int startingIndex = 0;

            model.Id = reader.GetSafeInt32(startingIndex++);
            model.UserId = reader.GetSafeInt32(startingIndex++);
            model.Bio = reader.GetSafeString(startingIndex++);
            model.AvatarUrl = reader.GetSafeString(startingIndex++);
            model.Phone = reader.GetSafeString(startingIndex++);
            model.RaceEthnicity = new RaceEthnicity();
            model.RaceEthnicity.Id = reader.GetSafeInt32Nullable(startingIndex++);
            model.RaceEthnicity.Name = reader.GetSafeString(startingIndex++);
            model.EducationLevels = new EducationLevel();
            model.EducationLevels.Id = reader.GetSafeInt32Nullable(startingIndex++);
            model.EducationLevels.Name = reader.GetSafeString(startingIndex++);
            model.HouseIncome = reader.GetSafeInt32(startingIndex++);
            model.YearsInBusiness = reader.GetSafeInt32Nullable(startingIndex++);
            model.DateCreated = reader.GetSafeDateTime(startingIndex++);
            model.DateModified = reader.GetSafeDateTime(startingIndex++);
            model.FirstName = reader.GetSafeString(startingIndex++);
            model.LastName = reader.GetSafeString(startingIndex++);
            model.Zip = reader.GetSafeString(startingIndex++);
            model.Email = reader.GetSafeString(startingIndex++);
            return model;
        }

        private static UserProfile UserProfileWithStatusMapper(IDataReader reader)
        {
            UserProfile model = new UserProfile();
            int startingIndex = 0;

            model.Id = reader.GetSafeInt32(startingIndex++);
            model.UserId = reader.GetSafeInt32(startingIndex++);
            model.Bio = reader.GetSafeString(startingIndex++);
            model.AvatarUrl = reader.GetSafeString(startingIndex++);
            model.Phone = reader.GetSafeString(startingIndex++);
            model.RaceEthnicity = new RaceEthnicity();
            model.RaceEthnicity.Id = reader.GetSafeInt32Nullable(startingIndex++);
            model.RaceEthnicity.Name = reader.GetSafeString(startingIndex++);
            model.EducationLevels = new EducationLevel();
            model.EducationLevels.Id = reader.GetSafeInt32Nullable(startingIndex++);
            model.EducationLevels.Name = reader.GetSafeString(startingIndex++);
            model.HouseIncome = reader.GetSafeInt32(startingIndex++);
            model.YearsInBusiness = reader.GetSafeInt32Nullable(startingIndex++);
            model.DateCreated = reader.GetSafeDateTime(startingIndex++);
            model.DateModified = reader.GetSafeDateTime(startingIndex++);
            model.FirstName = reader.GetSafeString(startingIndex++);
            model.LastName = reader.GetSafeString(startingIndex++);
            model.Zip = reader.GetSafeString(startingIndex++);
            model.Email = reader.GetSafeString(startingIndex++);
            model.Status = reader.GetSafeInt32(startingIndex++);
            return model;
        }

        public void Delete(int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.UserProfiles_Delete", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            });
        }

        public List<UserProfile> Get(int pageIndex, int pageSize)
        {
            List<UserProfile> profileList = null;

            _dataProvider.ExecuteCmd("dbo.UserProfiles_SelectAllPaginated_V2", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                UserProfile model = UserProfileMapper(reader);
                if (profileList == null)
                {
                    profileList = new List<UserProfile>();
                }
                profileList.Add(model);

            });
            return profileList;
        }

        public Paged<UserProfile> GetAllPagination(int pageIndex, int pageSize)
        {
            Paged<UserProfile> pagedResult = null;

            List<UserProfile> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd("dbo.UserProfiles_SelectAllPaginated_V2", inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    UserProfile model = new UserProfile();
                    int startingIndex = 0;
                    model.Id = reader.GetSafeInt32(startingIndex++);
                    model.UserId = reader.GetSafeInt32(startingIndex++);
                    model.Bio = reader.GetSafeString(startingIndex++);
                    model.AvatarUrl = reader.GetSafeString(startingIndex++);
                    model.Phone = reader.GetSafeString(startingIndex++);
                    model.RaceEthnicity = new RaceEthnicity();
                    model.RaceEthnicity.Id = reader.GetSafeInt32Nullable(startingIndex++);
                    model.RaceEthnicity.Name = reader.GetSafeString(startingIndex++);
                    model.EducationLevels = new EducationLevel();
                    model.EducationLevels.Id = reader.GetSafeInt32Nullable(startingIndex++);
                    model.EducationLevels.Name = reader.GetSafeString(startingIndex++);
                    model.HouseIncome = reader.GetSafeInt32(startingIndex++);
                    model.YearsInBusiness = reader.GetSafeInt32Nullable(startingIndex++);
                    model.DateCreated = reader.GetSafeDateTime(startingIndex++);
                    model.DateModified = reader.GetSafeDateTime(startingIndex++);
                    model.FirstName = reader.GetSafeString(startingIndex++);
                    model.LastName = reader.GetSafeString(startingIndex++);
                    model.Zip = reader.GetSafeString(startingIndex++);
                    model.Email = reader.GetSafeString(startingIndex++);
                    model.Status = reader.GetSafeInt32(startingIndex++);
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (result == null)
                    {
                        result = new List<UserProfile>();
                    }

                    result.Add(model);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<UserProfile>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<UserProfile> SearchPagination(int pageIndex, int pageSize, string query)
        {
            Paged<UserProfile> pagedResult = null;

            List<UserProfile> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd("UserProfiles_SearchPaginated", inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);
                parameterCollection.AddWithValue("@Query", query);
            },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    UserProfile model = new UserProfile();
                    int startingIndex = 0;
                    model.Id = reader.GetSafeInt32(startingIndex++);
                    model.UserId = reader.GetSafeInt32(startingIndex++);
                    model.Bio = reader.GetSafeString(startingIndex++);
                    model.AvatarUrl = reader.GetSafeString(startingIndex++);
                    model.Phone = reader.GetSafeString(startingIndex++);
                    model.RaceEthnicity = new RaceEthnicity();
                    model.RaceEthnicity.Id = reader.GetSafeInt32Nullable(startingIndex++);
                    model.RaceEthnicity.Name = reader.GetSafeString(startingIndex++);
                    model.EducationLevels = new EducationLevel();
                    model.EducationLevels.Id = reader.GetSafeInt32Nullable(startingIndex++);
                    model.EducationLevels.Name = reader.GetSafeString(startingIndex++);
                    model.HouseIncome = reader.GetSafeInt32(startingIndex++);
                    model.YearsInBusiness = reader.GetSafeInt32Nullable(startingIndex++);
                    model.DateCreated = reader.GetSafeDateTime(startingIndex++);
                    model.DateModified = reader.GetSafeDateTime(startingIndex++);
                    model.FirstName = reader.GetSafeString(startingIndex++);
                    model.LastName = reader.GetSafeString(startingIndex++);
                    model.Zip = reader.GetSafeString(startingIndex++);
                    model.Email = reader.GetSafeString(startingIndex++);
                    model.Status = reader.GetSafeInt32(startingIndex++);
                    totalCount = reader.GetSafeInt32(startingIndex++);

                    if (result == null)
                    {
                        result = new List<UserProfile>();
                    }

                    result.Add(model);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<UserProfile>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public List<RaceEthnicity> GetRaceEthnicitys()
        {
            List<RaceEthnicity> list = null;
            _dataProvider.ExecuteCmd("dbo.RaceEthnicity_Get", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                RaceEthnicity model = new RaceEthnicity();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);
                if (list == null)
                {
                    list = new List<RaceEthnicity>();

                }
                list.Add(model);
            });
            return list;
        }

        public List<EducationLevel> GetEducationLevels()
        {
            List<EducationLevel> list = null;
            _dataProvider.ExecuteCmd("dbo.EducationLevel_Get", inputParamMapper: null, singleRecordMapper: delegate(IDataReader reader, short set)
            {
                EducationLevel model = new EducationLevel();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Name = reader.GetSafeString(startingIndex++);
                if(list == null){
                list = new List<EducationLevel>();
            }

            list.Add(model);
            });
        return list;
        }

        public List<EducationLevelTotalByYear> GetEducationLevelTotalByYears(int year)
        {
            List<EducationLevelTotalByYear> list = null;

            _dataProvider.ExecuteCmd("dbo.EducationLevel_TotalCount",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Year", year);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    EducationLevelTotalByYear model = new EducationLevelTotalByYear();
                    int startingIndex = 0;

                    model.Name = reader.GetSafeString(startingIndex++);
                    model.Count = reader.GetSafeInt32(startingIndex++);

                    if (list == null)
                    {
                        list = new List<EducationLevelTotalByYear>();
                    }
                    list.Add(model);
                });
            return list;
        }

        public List<RaceEthnicityTotalByYear> GetRaceEthnicityTotalByYears(int year)
        {
            List <RaceEthnicityTotalByYear> list = null;

            _dataProvider.ExecuteCmd("dbo.RaceEthnicity_TotalCount",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Year", year);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    RaceEthnicityTotalByYear model = new RaceEthnicityTotalByYear();
                    int startingIndex = 0;

                    model.Name = reader.GetSafeString(startingIndex++);
                    model.Count = reader.GetSafeInt32(startingIndex++);

                    if (list == null)
                    {
                        list = new List<RaceEthnicityTotalByYear>();
                    }
                    list.Add(model);
                });
            return list;    

        }
    }
}
