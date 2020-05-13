using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests;
using Lynwood.Models.Requests.Users;
using Lynwood.Services;
using Lynwood.Services.Interfaces;
using Lynwood.Web.Controllers;
using Lynwood.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lynwood.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]

    public class UserApiController : BaseApiController
    {

        private IUserService _userService = null;
        private ITokensService _tokensService;
        private IEmailService _emailService;
        public UserApiController(IUserService userService, ITokensService tokensService, IEmailService emailService, ILogger<UserApiController> logger) : base(logger)
        {
            _userService = userService;
            _tokensService = tokensService;
            _emailService = emailService;
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(int id, UserUpdateRequest model)
        {
            try
            {
                if (id == model.Id)
                {
                    _userService.Update(model);
                    SuccessResponse resp = new SuccessResponse();
                    return Ok200(resp);
                }
                else
                {
                    return NotFound404(new ErrorResponse("Bad Request: Body Id does not match url"));
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpPut("status/{id:int}")]
        public ActionResult<SuccessResponse> Update(int id)
        {
            try
            {
                    _userService.UpdateStatus(id);
                    SuccessResponse resp = new SuccessResponse();
                    return Ok200(resp);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<User>> GetById(int id)
        {
            try
            {
                User user = _userService.Get(id);
                if (user == null)
                {
                    return NotFound404(new ErrorResponse("No item Found"));
                }
                else
                {
                    ItemResponse<User> resp = new ItemResponse<User>();
                    resp.Item = user;
                    return Ok200(resp);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("details")]
        public ActionResult<ItemResponse<UserDetails>> GetDetailsByEmail(string email)
        {
            try
            {
                UserDetails user = _userService.Get(email);
                if (user == null)
                {
                    return NotFound404(new ErrorResponse("No item Found"));
                }
                else
                {
                    ItemResponse<UserDetails> resp = new ItemResponse<UserDetails>();
                    resp.Item = user;
                    return Ok200(resp);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("role")]
        public ActionResult<ItemsResponse<User>> GetWithRoles(string email)
        {
            try
            {
                User model = _userService.GetWithRolesByEmail(email);
                if (model == null)
                {
                    return NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<User> resp = new ItemResponse<User>();
                    resp.Item = model;
                    return Ok200(resp);
                };
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpPost("register"), AllowAnonymous]
        public ActionResult<ItemResponse<int>> Add(UserAddRequest model)
        {
            try
            {
                int id = _userService.Add(model);
                if (id > 0)
                {
                _userService.AddUserRoleId(id, model.UserTypeId);
                    var token = Guid.NewGuid();
                    TokensAddRequest tokenModel = new TokensAddRequest();
                    int tokenId = _tokensService.Insert(tokenModel,1, token, id);
                   _emailService.RegisterEmail(model.Email, token);
                    ItemResponse<int> resp = new ItemResponse<int>();
                    resp.Item = id;
                    return Created201(resp);
                }
                else
                {
                    return Ok200(new SuccessResponse());
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<SuccessResponse>> AuthByEmail(UserLoginRequest model)
        {
            try
            {
                bool isSuccessful = await _userService.AuthByEmail(model.Email, model.Password);
                if (!isSuccessful)
                {
                    return StatusCode(404, new ErrorResponse("Invalid Email or Password"));
                }
                else
                {
                    return Ok200(new SuccessResponse());
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("monthly/count/{year:int}")]
        public ActionResult<ItemsResponse<UsersTotalByMonth>> GetTotalUsersCountByMonth(int year)
        {
            try
            {
                ItemsResponse<UsersTotalByMonth> resp = null;
                List<UsersTotalByMonth> model = _userService.GetUsersTotalByMonths(year);

                if (model == null)
                {
                    return NotFound404(new ErrorResponse("Items Not Found"));
                }
                else
                {
                    resp = new ItemsResponse<UsersTotalByMonth>
                    { Items = model };
                    return Ok200(resp);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));              
            }
        }

        [HttpGet("token/{userToken}"), AllowAnonymous]
        public ActionResult<SuccessResponse> GetByToken(string userToken)
        {
            try
            {
                int userId = _userService.GetByToken(userToken);
                if (userId == 0)
                {
                    return NotFound404(new ErrorResponse("No item Found"));
                }
                else
                {
                    _userService.Confirm(userId);
                    return Ok200(new SuccessResponse());
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        #region Admin tools

        [HttpGet("admin/stats")]
        public ActionResult<ItemResponse<AdminTotals>> GetAdminStats()
        {
            try
            {
                AdminTotals totals = _userService.GetAdminTotals();
                if (totals == null)
                {
                    return NotFound404(new ErrorResponse("No item Found"));
                }
                else
                {
                    ItemResponse<AdminTotals> resp = new ItemResponse<AdminTotals>();
                    resp.Item = totals;
                    return Ok200(resp);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        #endregion
    }
}