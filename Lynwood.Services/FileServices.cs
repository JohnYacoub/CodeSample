
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Lynwood.Data;
using Lynwood.Data.Providers;
using Lynwood.Models;
using Lynwood.Models.AppSettings;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests.Files;
using Lynwood.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lynwood.Services
{
    public class FileService : IFileService
    {
        private IDataProvider _dataProvider;

        private AWSCredential _aWSCredential;

        private static IAmazonS3 s3Client;

        public FileService(IOptions<AWSCredential> aWSCredential, IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _aWSCredential = aWSCredential.Value;
        }

        public async Task<string> UploadFile(IFormFile file, int id)
        {
            TransferUtility fileTransferUtility = null;
            BasicAWSCredentials credentials = null;
            string bucketName = _aWSCredential.BucketName;
            string filePath = Path.GetTempFileName();
            string keyName = bucketName + Guid.NewGuid() + "_" + file.FileName;

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                credentials = new BasicAWSCredentials(_aWSCredential.AccessKey, _aWSCredential.Secret);
                s3Client = new AmazonS3Client(credentials, RegionEndpoint.USWest2);
                file.CopyTo(stream);
                fileTransferUtility = new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(stream, bucketName, keyName);
            }
            // This below section inserts file information to sql database. 
            FileAddRequest model = new FileAddRequest()
            {
                Name = file.FileName,
                Url = _aWSCredential.Domain + keyName,
                FileType = 1,
            };

            Add(model, id);

            return model.Url;
        }

        public int Add(FileAddRequest model, int creadtedBy)
        {
            int id = 0;

            _dataProvider.ExecuteNonQuery("dbo.Files_Insert", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                SqlParameter parm = new SqlParameter();
                parm.ParameterName = "@Id";
                parm.SqlDbType = SqlDbType.Int;
                parm.Direction = ParameterDirection.Output;
                parms.Add(parm);

                parms.AddWithValue("@Name", model.Name);
                parms.AddWithValue("@Url", model.Url);
                parms.AddWithValue("@FileType", model.FileType);
                parms.AddWithValue("@CreatedBy", creadtedBy);
            },
            returnParameters: delegate (SqlParameterCollection parms)
            {
                Int32.TryParse(parms["@Id"].Value.ToString(), out id);
            });

            return id;
        }

        public void Update(FileUpdateRequest model, int creadtedBy)
        {
            _dataProvider.ExecuteNonQuery("dbo.Files_Update", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", model.Id);
                parms.AddWithValue("@Name", model.Name);
                parms.AddWithValue("@Url", model.Url);
                parms.AddWithValue("@FileType", model.FileType);
                parms.AddWithValue("@CreatedBy", creadtedBy);
            });

        }

        public FileUpload Get(int id)
        {
            FileUpload model = null;

            _dataProvider.ExecuteCmd("dbo.Files_SelectById", 
                inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = FileMapper(reader);
            });

            return model;
        }

        public List<FileUpload> Get()
        {
            List<FileUpload> list = null;

            _dataProvider.ExecuteCmd("dbo.Files_SelectAll", 
                inputParamMapper: null, 
                singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FileUpload model = FileMapper(reader);
                if (list == null)
                {
                    list = new List<FileUpload>();
                }
                list.Add(model);
            });

            return list;
        }

        public Paged<FileUpload> GetPaginated(int pageIndex, int pageSize)
        {
            List<FileUpload> result = null;
            Paged<FileUpload> pagedResult = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd("dbo.Files_Select_Paginated", inputParamMapper: delegate(SqlParameterCollection parameterCollection) 
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);
            }, 
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FileUpload model = FileMapper(reader);
                if (totalCount <= 0)
                {
                    totalCount = reader.GetSafeInt32(7);
                }
                if (result == null)
                {
                    result = new List<FileUpload>();
                }
                result.Add(model);
            });
            if (result != null)
            {
                pagedResult = new Paged<FileUpload>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public Paged<FileUpload> SearchPaginated(int pageIndex, int pageSize, string query)
        {
            List<FileUpload> result = null;
            Paged<FileUpload> pagedResult = null;
            int totalCount = 0;
            _dataProvider.ExecuteCmd("dbo.Files_Search_Paginated", inputParamMapper: delegate (SqlParameterCollection parameterCollection)
            {
                parameterCollection.AddWithValue("@PageIndex", pageIndex);
                parameterCollection.AddWithValue("@PageSize", pageSize);
                parameterCollection.AddWithValue("@Query", query);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                FileUpload model = FileMapper(reader);
                if (totalCount <= 0)
                {
                    totalCount = reader.GetSafeInt32(7);
                }
                if (result == null)
                {
                    result = new List<FileUpload>();
                }
                result.Add(model);
            });
            if (result != null)
            {
                pagedResult = new Paged<FileUpload>(result, pageIndex, pageSize, totalCount);
            }

            return pagedResult;
        }

        public void Delete(int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.Files_DeleteById", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            });
        }

        private static FileUpload FileMapper(IDataReader reader)
        {
            FileUpload model = new FileUpload();
            int startingIndex = 0;

            model.Id = reader.GetSafeInt32(startingIndex++);
            model.Name = reader.GetSafeString(startingIndex++);
            model.Url = reader.GetSafeString(startingIndex++);
            model.FileType = reader.GetSafeInt32(startingIndex++);
            model.DateCreated = reader.GetSafeDateTime(startingIndex++);
            model.DateModified = reader.GetSafeDateTime(startingIndex++);

            return model;
        }      
    }
}
