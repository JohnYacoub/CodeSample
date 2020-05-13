using Lynwood.Data;
using Lynwood.Data.Providers;
using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Users;
using Lynwood.Models.Requests.Users;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lynwood.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;

        public UserService(IAuthenticationService<int> authSerice, IDataProvider dataProvider)
        {
            _authenticationService = authSerice;
            _dataProvider = dataProvider;
        }

        public async Task<bool> LogInAsync(string email, string password)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(email, password);

            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public async Task<bool> LogInTest(string email, string password, int id, string[] roles = null)
        {
            bool isSuccessful = false;
            var testRoles = new[] { "User", "Super", "Content Manager" };

            var allRoles = roles == null ? testRoles : testRoles.Concat(roles);

            IUserAuthData response = new UserBase
            {
                Id = id
                ,
                Name = email
                ,
                Roles = allRoles
                ,
                TenantId = "Acme Corp UId"
            };

            Claim fullName = new Claim("CustomClaim", "Lynwood Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });

            return isSuccessful;
        }

        public int Create(object userModel)
        {
            //make sure the password column can hold long enough string. put it to 100 to be safe

            int userId = 0;
            string password = "Get from user model when you have a concreate class";
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, "");

            //DB provider call to create user and get us a user id

            //be sure to store both salt and passwordHash
            //DO NOT STORE the original password value that the user passed us

            return userId;
        }

        /// <summary>
        /// Gets the Data call to get a give user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        private IUserAuthData Get(string email, string password)
        {
            //make sure the password column can hold long enough string. put it to 100 to be safe
            string passwordFromDb = "";
            UserBase user = null;

            //get user object from db;

            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);

            return user;
        }

        public User GetWithRolesById(int id)
        {
            User model = null;
            List<string> roles = null;

            _dataProvider.ExecuteCmd("dbo.Users_SelectById_Roles",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Id", id);
                },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                switch (set)
                {
                    case 0:
                        model = UserMapper(reader);
                        break;
                    case 1:
                        int index = 0;
                        string role = reader.GetSafeString(index++);
                        if (roles == null)
                        {
                            roles = new List<string>();
                        }
                        roles.Add(role);
                        break;
                }
            });
            model.Roles = roles;
            return model;
        }

        public UserAuth GetWithRolesByEmail(string email)
        {
            UserAuth model = null;
            List<string> roles = null;

            _dataProvider.ExecuteCmd("dbo.Users_SelectByEmail_Roles",
                inputParamMapper: delegate (SqlParameterCollection parameterCollection)
                {
                    parameterCollection.AddWithValue("@Email", email);
                },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                switch (set)
                {
                    case 0:
                        model = UserAuthMapper(reader);
                        break;
                    case 1:
                        int index = 0;
                        string role = reader.GetSafeString(index++);
                        if (roles == null)
                        {
                            roles = new List<string>();
                        }
                        roles.Add(role);
                        break;
                }
            });
            model.Roles = roles;
            return model;
        }

        #region UserService

        public void AddUserRoleId(int id, int userTypeId)
        {
            _dataProvider.ExecuteNonQuery("dbo.UserRoles_Insert", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@UserId", id);
                parms.AddWithValue("@UserTypeId", userTypeId);
            });
        }

        public int Add(UserAddRequest model)
        {
            int id = 0;
            string hashPassword = BCrypt.BCryptHelper.HashPassword(model.PasswordHash, BCrypt.BCryptHelper.GenerateSalt());

            _dataProvider.ExecuteNonQuery("dbo.Users_Insert", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                SqlParameter parm = new SqlParameter();
                parm.ParameterName = "@Id";
                parm.SqlDbType = SqlDbType.Int;
                parm.Direction = ParameterDirection.Output;
                parms.Add(parm);
                parms.AddWithValue("@email", model.Email);
                parms.AddWithValue("@passwordHash", hashPassword);
            },
            returnParameters: delegate (SqlParameterCollection parms)
            {
                Int32.TryParse(parms["@Id"].Value.ToString(), out id);
            });

            return id;
        }

        public void Update(UserUpdateRequest model)
        {
            _dataProvider.ExecuteNonQuery("dbo.User_Update", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@id", model.Id);
                parms.AddWithValue("@email", model.Email);
                parms.AddWithValue("@passwordHash", model.PasswordHash);
                parms.AddWithValue("@isConfirmed", false);
                parms.AddWithValue("@status", model.Status);
            });

        }

        public void UpdateStatus(int id)
        {
            _dataProvider.ExecuteNonQuery("dbo.Users_ToggleActiveStatus", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", id);
            });

        }

        public void Confirm(int userId)
        {
            _dataProvider.ExecuteNonQuery("dbo.Users_Confirm", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Id", userId);
            });
        }

        public User Get(int id)
        {
            User model = null;

            _dataProvider.ExecuteCmd("dbo.User_SelectById", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = UserMapper(reader);
            });

            return model;
        }

        public int GetByToken(string token)
        {
            int userId = 0;

            _dataProvider.ExecuteCmd("dbo.Tokens_SelectByToken", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Token", token);
            }, singleRecordMapper: delegate (IDataReader reader, short set) 
            {
                userId = reader.GetSafeInt32(0);
            });
          
            return userId;
        }

        public UserDetails Get(string email)
        {
            UserDetails model = null;

            _dataProvider.ExecuteCmd("dbo.User_DetailsByEmail", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@email", email);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = new UserDetails();
                int startingIndex = 0;

                model.FirstName = reader.GetSafeString(startingIndex++);
                model.LastName = reader.GetSafeString(startingIndex++);
                model.AvatarUrl = reader.GetSafeString(startingIndex++);
            });

            return model;
        }

        public List<User> Get()
        {
            List<User> list = null;

            _dataProvider.ExecuteCmd("dbo.Users_SelectAll", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                User model = UserMapper(reader);
                if (list == null)
                {
                    list = new List<User>();
                }
                list.Add(model);
            });

            return list;
        }

        public AdminTotals GetAdminTotals()
        {
            AdminTotals model = null;

            _dataProvider.ExecuteCmd("dbo.Admin_SelectDashboardTotals", inputParamMapper: null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;
                model = new AdminTotals();

                model.SurveysCompletedTotal = reader.GetSafeInt32(index++);
                model.UsersTotal = reader.GetSafeInt32(index++);
                model.ResourcesTotal = reader.GetSafeInt32(index++);

            });

            return model;
        }

        public List<UsersTotalByMonth> GetUsersTotalByMonths(int year)
        {
            List<UsersTotalByMonth> list = null;

            _dataProvider.ExecuteCmd("dbo.Users_CountByMonth",
                inputParamMapper: delegate (SqlParameterCollection parms)
                {
                    parms.AddWithValue("@Year", year);
                }, singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    UsersTotalByMonth model = new UsersTotalByMonth();
                    int startingIndex = 0;

                    model.Month = reader.GetSafeInt32(startingIndex++);
                    model.Count = reader.GetSafeInt32(startingIndex++);

                    if (list == null)
                    {
                        list = new List<UsersTotalByMonth>();
                    }
                    list.Add(model);
                });
            return list;
        }

        public UserAuth GetByEmail(string email)
        {
            UserAuth model = null;
            
            _dataProvider.ExecuteCmd("dbo.Users_SelectByEmail", inputParamMapper: delegate (SqlParameterCollection parms)
            {
                parms.AddWithValue("@Email", email);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                model = new UserAuth();
                int startingIndex = 0;
                model.Id = reader.GetSafeInt32(startingIndex++);
                model.Email = reader.GetSafeString(startingIndex++);
                model.PasswordHash = reader.GetSafeString(startingIndex++);
                model.IsConfirmed = reader.GetSafeBool(startingIndex++);
                model.Status = reader.GetSafeInt32(startingIndex++);
            });
            return model;

        }

        public async Task<bool> AuthByEmail(string email, string password)
        {
            bool isSuccessful = false;
            UserAuth user = GetWithRolesByEmail(email);
            if (user != null)
            {
                bool isAuthenticated = BCrypt.BCryptHelper.CheckPassword(password, user.PasswordHash);

                if (isAuthenticated)
                {
                    bool isAuthorized = await LogIn(user.Email, user.Id, user.Roles);
                    if (isAuthorized)
                    {
                        isSuccessful = true;
                    }
                }
            }
            return isSuccessful;
        }

        public async Task<bool> LogIn(string email, int id, IEnumerable<string> roles = null)
        {
            bool isSuccessful = false;

            IUserAuthData response = new UserBase
            {
                Id = id
                ,
                Name = email
                ,
                Roles = roles
                ,
                TenantId = "Lynwood Resource Center"
            };

            Claim fullName = new Claim("CustomClaim", "Lynwood Bootcamp");
            await _authenticationService.LogInAsync(response, new Claim[] { fullName });
            isSuccessful = true;

            return isSuccessful;
        }

        private static User UserMapper(IDataReader reader)
        {
            User model = new User();
            int startingIndex = 0;

            model.Id = reader.GetSafeInt32(startingIndex++);
            model.Email = reader.GetSafeString(startingIndex++);
            model.IsConfirmed = reader.GetSafeBool(startingIndex++);
            model.Status = reader.GetSafeInt32(startingIndex++);

            return model;
        }

        private static UserAuth UserAuthMapper(IDataReader reader)
        {
            UserAuth model = new UserAuth();
            int startingIndex = 0;

            model.Id = reader.GetSafeInt32(startingIndex++);
            model.Email = reader.GetSafeString(startingIndex++);
            model.PasswordHash = reader.GetSafeString(startingIndex++);
            model.IsConfirmed = reader.GetSafeBool(startingIndex++);
            model.Status = reader.GetSafeInt32(startingIndex++);

            return model;
        }
        #endregion
    }
}