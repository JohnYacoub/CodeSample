using Lynwood.Data;
using Lynwood.Data.Providers;
using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests.Entrepreneurs;
using Lynwood.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Lynwood.Services
{
    public class EntrepreneursService : IEntrepreneursService
    {
        private IDataProvider _dataProvider;
      
        public EntrepreneursService(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public int Insert(EntrepreneursAddRequest model, int userId)
        {
            int id = 0;

            _dataProvider.ExecuteNonQuery("dbo.Entrepreneurs_Insert",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    SqlParameter parm = new SqlParameter();
                    parm.ParameterName = "@Id";
                    parm.SqlDbType = SqlDbType.Int;
                    parm.Direction = ParameterDirection.Output;
                    parms.Add(parm);

                    parms.AddWithValue("@UserId", userId);
                    parms.AddWithValue("@IndustryTypeId", model.IndustryTypeId);
                    parms.AddWithValue("@CompanyStatusId", model.CompanyStatusId);
                    parms.AddWithValue("@HasSecurityClearance", model.HasSecurityClearance);
                    parms.AddWithValue("@HasInsurance", model.HasInsurance);
                    parms.AddWithValue("@HasBonds", model.HasBonds);
                    parms.AddWithValue("@SpecializedEquipment", model.SpecializedEquipment);
                    parms.AddWithValue("@ImageUrl", model.ImageUrl);

                }, returnParameters: delegate (SqlParameterCollection parms)
                {
                    Int32.TryParse(parms["@Id"].Value.ToString(), out id);
                });
            return id;
        }

        public void Update(EntrepreneursUpdateRequest model, int userId)
        {
            _dataProvider.ExecuteNonQuery("dbo.Entrepreneurs_Update",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Id", model.Id);
                    parms.AddWithValue("@UserId", userId);
                    parms.AddWithValue("@IndustryTypeId", model.IndustryTypeId);
                    parms.AddWithValue("@CompanyStatusId", model.CompanyStatusId);
                    parms.AddWithValue("@HasSecurityClearance", model.HasSecurityClearance);
                    parms.AddWithValue("@HasInsurance", model.HasInsurance);
                    parms.AddWithValue("@HasBonds", model.HasBonds);
                    parms.AddWithValue("@SpecializedEquipment", model.SpecializedEquipment);
                    parms.AddWithValue("@ImageUrl", model.ImageUrl);
                });                
        }

        public Entrepreneur GetById(int id)
        {
            Entrepreneur model = null;

            _dataProvider.ExecuteCmd("dbo.Entrepreneurs_GetById_Details", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = EntrepreneurMapper(reader);
            });
            return model;
        }

        private static Entrepreneur EntrepreneurMapper(IDataReader reader)
        {
            Entrepreneur model = new Entrepreneur();
            int startingIndex = 0;
         
            model.Id = reader.GetSafeInt32(startingIndex++);
            model.FirstName = reader.GetSafeString(startingIndex++);
            model.LastName = reader.GetSafeString(startingIndex++);
            model.Email = reader.GetSafeString(startingIndex++);
            model.UserId = reader.GetSafeInt32(startingIndex++);
            model.IndustryType = new IndustryType();
            model.IndustryType.Id = reader.GetSafeInt32(startingIndex++);
            model.IndustryType.Name = reader.GetSafeString(startingIndex++);
            model.CompanyStatus = new CompanyStatus();
            model.CompanyStatus.Id = reader.GetSafeInt32(startingIndex++);
            model.CompanyStatus.Name = reader.GetSafeString(startingIndex++);
            model.HasSecurityClearance = reader.GetSafeBool(startingIndex++);
            model.HasInsurance = reader.GetSafeBool(startingIndex++);
            model.HasBonds = reader.GetSafeBool(startingIndex++);
            model.SpecializedEquipment = reader.GetSafeString(startingIndex++);
            model.ImageUrl = reader.GetSafeString(startingIndex++);
            return model;
        }

        public List<Entrepreneur> GetAll()
        {
            List<Entrepreneur> entrepreneursList = null; 

            _dataProvider.ExecuteCmd("dbo.Entrepreneurs_GetAll_Details", inputParamMapper: null,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Entrepreneur model = EntrepreneurMapper(reader);
                    if(entrepreneursList == null)
                    {
                        entrepreneursList = new List<Entrepreneur>();
                    }
                    entrepreneursList.Add(model);
                });
            return entrepreneursList;
        }

        public void Delete(int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.Entrepreneurs_Delete", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            });
        }

        public Paged<Entrepreneur> GetAllByPagination(int pageIndex, int pageSize)
        {
            Paged<Entrepreneur> pagedResult = null;

            List<Entrepreneur> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd(
                "dbo.Entrepreneurs_SelectandPaginate",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Entrepreneur model = new Entrepreneur();
                    int index = 0;
                    model.Id = reader.GetSafeInt32(index++);
                    model.FirstName = reader.GetSafeString(index++);
                    model.LastName = reader.GetSafeString(index++);
                    model.Email = reader.GetSafeString(index++);
                    model.UserId = reader.GetSafeInt32(index++);
                    model.IndustryType = new IndustryType();
                    model.IndustryType.Id = reader.GetSafeInt32(index++);
                    model.IndustryType.Name = reader.GetSafeString(index++);
                    model.CompanyStatus = new CompanyStatus();
                    model.CompanyStatus.Id = reader.GetSafeInt32(index++);
                    model.CompanyStatus.Name = reader.GetSafeString(index++);
                    model.HasSecurityClearance = reader.GetSafeBool(index++);
                    model.HasInsurance = reader.GetSafeBool(index++);
                    model.HasBonds = reader.GetSafeBool(index++);
                    model.SpecializedEquipment = reader.GetSafeString(index++);
                    model.ImageUrl = reader.GetSafeString(index++);
                    totalCount = reader.GetSafeInt32(index++);

                    if (result == null)
                    {
                        result = new List<Entrepreneur>();
                    }

                    result.Add(model);
                }
            );
            if (result != null)
            {
                pagedResult = new Paged<Entrepreneur>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<Entrepreneur> SearchPagination(int pageIndex, int pageSize, string query)
        {
            Paged<Entrepreneur> pagedResult = null;

            List<Entrepreneur> result = null;

            int totalCount = 0;

            _dataProvider.ExecuteCmd(
                "dbo.Entrepreneurs_Search",

                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@PageIndex", pageIndex);
                    parameterCollection.AddWithValue("@PageSize", pageSize);
                    parameterCollection.AddWithValue("@Query", query); 
                },
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    Entrepreneur model = new Entrepreneur();
                    int index = 0;
                    model.Id = reader.GetSafeInt32(index++);
                    model.FirstName = reader.GetSafeString(index++);
                    model.LastName = reader.GetSafeString(index++);
                    model.Email = reader.GetSafeString(index++);
                    model.UserId = reader.GetSafeInt32(index++);
                    model.IndustryType = new IndustryType();
                    model.IndustryType.Id = reader.GetSafeInt32(index++);
                    model.IndustryType.Name = reader.GetSafeString(index++);
                    model.CompanyStatus = new CompanyStatus();
                    model.CompanyStatus.Id = reader.GetSafeInt32(index++);
                    model.CompanyStatus.Name = reader.GetSafeString(index++);
                    model.HasSecurityClearance = reader.GetSafeBool(index++);
                    model.HasInsurance = reader.GetSafeBool(index++);
                    model.HasBonds = reader.GetSafeBool(index++);
                    model.SpecializedEquipment = reader.GetSafeString(index++);
                    model.ImageUrl = reader.GetSafeString(index++);
                    totalCount = reader.GetSafeInt32(index++);

                    if (result == null)
                    {
                        result = new List<Entrepreneur>();
                    }

                    result.Add(model);
                }

            );
            if (result != null)
            {
                pagedResult = new Paged<Entrepreneur>(result, pageIndex, pageSize, totalCount);
            }
            return pagedResult;
        }

        public EntrepreneursTypes GetAllOptions()
        {
            EntrepreneursTypes data = new EntrepreneursTypes();
            _dataProvider.ExecuteCmd("dbo.EntrepreneursTypes_SelectAll", inputParamMapper: null, singleRecordMapper:
                delegate (IDataReader reader, short set)
                {
                    switch (set)
                    {
                        case 0:
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
                    switch (set)
                    {
                        case 1:
                            CompanyStatus model = new CompanyStatus();
                            int startingIndex = 0;
                            model.Id = reader.GetSafeInt32(startingIndex++);
                            model.Name = reader.GetSafeString(startingIndex++);
                            if (data.CompanyStatuses == null)
                            {
                                data.CompanyStatuses = new List<CompanyStatus>();
                            }
                            data.CompanyStatuses.Add(model);
                            break;
                    }
                });
            return data;
        }
    }
}
