using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lywnood.Models;
using Lywnood.Models.Domain;
using Lywnood.Models.Domain.Users;
using Lywnood.Models.Requests.Users;
using Lywnood.Services;
using Lywnood.Services.Interfaces;
using Lywnood.Web.Controllers;
using Lywnood.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Lywnood.Web.Api.Controllers
{
    [Route("api/users/profiles")]
    [ApiController]
    public class UserProfileApiController : BaseApiController
    {
        private IUserProfileService _profileService = null;
        private IAuthenticationService<int> _authService = null;
        public UserProfileApiController(IAuthenticationService<int> authService, IUserProfileService profileService, ILogger<UserProfileApiController> logger) : base(logger)
        {
            _profileService = profileService;
            _authService = authService;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            try
            {
                _profileService.Delete(id);
                SuccessResponse resp = new SuccessResponse();
                return Ok200(resp);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(int id, UserProfileUpdateRequest model)
        {
            try
            {
                if (id == model.Id)
                {
                    _profileService.Update(model);
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

        //[HttpGet]
        //public ActionResult<ItemsResponse<UserProfile>> GetAllPagination()
        //{
        //    try
        //    {
        //        List<UserProfile> profileList = _profileService.Get();
        //        if (profileList == null)
        //        {
        //            return NotFound404(new ErrorResponse("No items found"));
        //        }
        //        else
        //        {
        //            ItemsResponse<UserProfile> resp = new ItemsResponse<UserProfile>();
        //            resp.Items = profileList;
        //            return Ok200(resp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex.ToString());
        //        return StatusCode(500, new ErrorResponse(ex.Message));
        //    }
        //}

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<UserProfile>> Get(int id)
        {
            try
            {
                UserProfile profile = _profileService.GetById(id);
                if (profile == null)
                {
                    return NotFound404(new ErrorResponse("No item found"));
                }
                else
                {
                    ItemResponse<UserProfile> resp = new ItemResponse<UserProfile>();
                    resp.Item = profile;
                    return Ok200(resp);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("user/{userId:int}")]
        public ActionResult<ItemResponse<UserProfile>> GetByUserId(int userId)
        {
            try
            {
                UserProfile profile = _profileService.GetByUserId(userId);
                if (profile == null)
                {
                    return NotFound404(new ErrorResponse("No item found"));
                }
                else
                {
                    ItemResponse<UserProfile> resp = new ItemResponse<UserProfile>();
                    resp.Item = profile;
                    return Ok200(resp);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> AddProfile(UserProfileAddRequest model)
        {
            try
            {
                int id = _profileService.AddProfile(model, _authService.GetCurrentUserId());
                ItemResponse<int> resp = new ItemResponse<int>();
                resp.Item = id;
                return Created201(resp);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }


        }

        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<UserProfile>>> GetPaged(int pageIndex, int pageSize)
        {
            try
            {
                Paged<UserProfile> pagedList = null;
                pagedList = _profileService.GetAllPagination(pageIndex, pageSize);

                if (pagedList == null)
                {
                    return NotFound404(new ErrorResponse("NO items found"));
                }
                else
                {
                    ItemResponse<Paged<UserProfile>> response = new ItemResponse<Paged<UserProfile>>();
                    response.Item = pagedList;
                    return Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<UserProfile>>> GetSearchPaged(int pageIndex, int pageSize, string query)
        {
            try
            {
                Paged<UserProfile> pagedList = null;
                pagedList = _profileService.SearchPagination(pageIndex, pageSize, query);

                if (pagedList == null)
                {
                    return NotFound404(new ErrorResponse("NO items found"));
                }
                else
                {
                    ItemResponse<Paged<UserProfile>> response = new ItemResponse<Paged<UserProfile>>();
                    response.Item = pagedList;
                    return Ok200(response);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("raceEthnicity")]
        public ActionResult<ItemResponse<RaceEthnicity>> GetRaceEthnicitys()
        {
            try
            {
                List<RaceEthnicity> list = _profileService.GetRaceEthnicitys();
                if (list == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    ItemsResponse<RaceEthnicity> resp = new ItemsResponse<RaceEthnicity>();
                    resp.Items = list;
                    return Ok200(resp);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("educationLevel")]
        public ActionResult<ItemsResponse<EducationLevel>> GetEducationLevels()
        {
            try
            {
                List<EducationLevel> list = _profileService.GetEducationLevels();
                if(list == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    ItemsResponse<EducationLevel> resp = new ItemsResponse<EducationLevel>();
                    resp.Items = list;
                    return Ok200(resp);
                }
            }
            catch(Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("educationLevel/count/{year:int}")]
        public ActionResult<ItemsResponse<EducationLevelTotalByYear>> GetEducationLevelTotal(int year)
        {
            List<EducationLevelTotalByYear> list = _profileService.GetEducationLevelTotalByYears(year);
            if(list == null)
            {
                return StatusCode(404, new ErrorResponse("Item Not Found"));
            }
            else
            {
                ItemsResponse<EducationLevelTotalByYear> resp = new ItemsResponse<EducationLevelTotalByYear>();
                resp.Items = list;
                return Ok200(resp);
            }
        }

        [HttpGet("raceEthnicity/count/{year:int}")]
        public ActionResult<ItemsResponse<RaceEthnicity>> GetRaceEthnicityTotal(int year)
        {
            List<RaceEthnicityTotalByYear> list = _profileService.GetRaceEthnicityTotalByYears(year);
            if(list == null)
            {
                return StatusCode(404, new ErrorResponse("Item Not Found"));
            }
            else
            {
                ItemsResponse<RaceEthnicityTotalByYear> resp = new ItemsResponse<RaceEthnicityTotalByYear>();
                resp.Items = list;
                return Ok200(resp); 
            }

        }
    }
}