using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lynwood.Models;
using Lynwood.Models.Domain;
using Lynwood.Models.Domain.Business;
using Lynwood.Models.Requests.BusinessVentures;
using Lynwood.Services;
using Lynwood.Web.Controllers;
using Lynwood.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Lynwood.Web.Api.Controllers
{
    [Route("api/businessventures")]
    [ApiController]
    public class BusinessVenturesApiController : BaseApiController
    {
        private IBusinessVenturesServices _businessVenturesService;
        private IAuthenticationService<int> _authService;
        private IAddressService _addressService;

        public BusinessVenturesApiController(IAuthenticationService<int> authService, IAddressService addressService, IBusinessVenturesServices businessVenturesService, ILogger<BusinessVenturesApiController> logger) : base(logger)
        {
            _businessVenturesService = businessVenturesService;
            _authService = authService;
            _addressService = addressService;
        }

        #region CRUD
        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(BusinessVenturesAddRequest model)
        {
            try
            {
                int id = _businessVenturesService.Add(model);
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

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(int id, BusinessVenturesUpdateRequest model)
        {
            try
            {
                if(id == model.Id)
                {
                    _businessVenturesService.Update(model);
                    return Ok200(new SuccessResponse());
                }
                else
                {
                    return NotFound404(new ErrorResponse("Bad Request: Body Id doesn't match"));
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Business>> GetById(int id)
        {
            try
            {
                ItemResponse<Business> resp = null;
                Business model = _businessVenturesService.Get(id);
                if (model == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    Address address = _addressService.GetByBusinessId(id);
                    if (address != null)
                    {
                        model.Address = address;
                    }
                    resp = new ItemResponse<Business>();
                    resp.Item = model;
                    return Ok200(resp);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }
        
        [HttpGet("current")]
        public ActionResult<ItemResponse<Business>> GetByUserId()
        {
            try
            {
                ItemResponse<Business> resp = null;
                int userId = _authService.GetCurrentUserId();
                Business model = _businessVenturesService.GetByUserId(userId);
                if (model == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    Address address = _addressService.GetByBusinessUserId(userId);
                    if (address != null)
                    {
                        model.Address = address;
                    }
                    resp = new ItemResponse<Business>();
                    resp.Item = model;
                    return Ok200(resp);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }

        [HttpGet]
        public ActionResult<ItemsResponse<Business>> SelectAll()
        {
            try
            {
                List<Business> list = _businessVenturesService.Get();
                if (list == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    ItemsResponse<Business> resp = new ItemsResponse<Business>();
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

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {
            try
            {
                _businessVenturesService.Delete(id);
                return Ok200(new SuccessResponse());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return StatusCode(500, new ErrorResponse(ex.Message));
            }
        }
        #endregion

        #region Pagination/Search
        [HttpGet("paginate")]
        public ActionResult<ItemResponse<Paged<Business>>> GetPaged(int pageIndex, int pageSize)
        {
            try
            {
                Paged<Business> pagedList = null;
                pagedList = _businessVenturesService.GetAllByPagination(pageIndex, pageSize);
                if (pagedList == null)
                {
                    return StatusCode(404, new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Business>> response = new ItemResponse<Paged<Business>>();
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
        public ActionResult<ItemResponse<Paged<Business>>> GetSearch(int pageIndex, int pageSize, string query)
        {
            try
            {
                Paged<Business> pagedList = null;
                pagedList = _businessVenturesService.SearchPagination(pageIndex, pageSize, query);
                if (pagedList == null)
                {
                    return StatusCode(404, new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<Business>> response = new ItemResponse<Paged<Business>>();
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
        #endregion

        #region Business/Status/Industry Types
        [HttpGet("businesstype")]
        public ActionResult<ItemsResponse<BusinessType>> GetBusinessTypes()
        {
            try
            {
                List<BusinessType> list = _businessVenturesService.GetBusinessTypes();
                if (list == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    ItemsResponse<BusinessType> resp = new ItemsResponse<BusinessType>();
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

        [HttpGet("status")]
        public ActionResult<ItemsResponse<Status>> GetBusinessesStatus()
        {
            try
            {
                List<Status> list = _businessVenturesService.GetBusinessesStatus();
                if (list == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    ItemsResponse<Status> resp = new ItemsResponse<Status>();
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

        [HttpGet("industrytype")]
        public ActionResult<ItemsResponse<IndustryType>> GetIndustryType()
        {
            try
            {
                List<IndustryType> list = _businessVenturesService.GetIndustryType();
                if (list == null)
                {
                    return StatusCode(404, new ErrorResponse("Item Not Found"));
                }
                else
                {
                    ItemsResponse<IndustryType> resp = new ItemsResponse<IndustryType>();
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
        #endregion

        #region Year Count
        [HttpGet("monthly/count/{year:int}")]
        public ActionResult<ItemsResponse<BusinessTypesTotalCountByYear>> GetTotalBusinessCount(int year)
        {
            ItemsResponse<BusinessTypesTotalCountByYear> resp = null;
            List<BusinessTypesTotalCountByYear> model = _businessVenturesService.GetBusinessTotalCountsByYear(year);
            try
            {
                if (model == null)
                {
                    return NotFound404(new ErrorResponse("Items Not Found"));
                }
                else
                {
                    resp = new ItemsResponse<BusinessTypesTotalCountByYear>
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
        #endregion
    }
}
