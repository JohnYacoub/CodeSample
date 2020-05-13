using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Lynwood.Models;
using Lynwood.Models.AppSettings;
using Lynwood.Models.Domain;
using Lynwood.Models.Requests.Files;
using Lynwood.Services;
using Lynwood.Services.Interfaces;
using Lynwood.Web.Controllers;
using Lynwood.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;

using System.Threading.Tasks;


namespace Lynwood.Web.Api.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesApiController : BaseApiController
    {
        private IFileService _fileService = null;

        private IAuthenticationService<int> _authService = null;

        public FilesApiController(IOptions<AppKeys> appKeys, IAuthenticationService<int> authService, IFileService fileService, ILogger<Controller> logger) : base(logger)
        {
            _fileService = fileService;
            _authService = authService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ItemsResponse<string>>> AddFile(IFormFile[] file)
        {
            
            try
            {
                List<string> urls = null;
                int userId = _authService.GetCurrentUserId();
                if (file[0] == null)
                {
                    return NotFound404(new ErrorResponse("No file Submitted"));
                }
                else
                {
                   
                    foreach (var item in file)
                    {
                        if (urls == null)
                        {
                            urls = new List<string>();
                        }
                        string url = await _fileService.UploadFile(item, userId);
                        urls.Add(url);
                    }

                    ItemsResponse<string> resp = new ItemsResponse<string>();
                    resp.Items = urls;
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
        public ActionResult<SuccessResponse> DeleteById(int id)
        {
            try
            {
                _fileService.Delete(id);
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
        public ActionResult<SuccessResponse> Update(int id, FileUpdateRequest model)
        {
            try
            {
                if (id == model.Id)
                {
                    _fileService.Update(model, _authService.GetCurrentUserId());
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

        [HttpGet]
        public ActionResult<ItemResponse<FileUpload>> GetAll()
        {
            try
            {
                List<FileUpload> list = _fileService.Get();
                if (list == null)
                {
                    return NotFound404(new ErrorResponse("No items found"));
                }
                else
                {
                    ItemsResponse<FileUpload> resp = new ItemsResponse<FileUpload>();
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

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<FileUpload>> GetById(int id)
        {
            try
            {
                FileUpload file = _fileService.Get(id);
                if (file == null)
                {
                    return NotFound404(new ErrorResponse("No item Found"));
                }
                else
                {
                    ItemResponse<FileUpload> resp = new ItemResponse<FileUpload>();
                    resp.Item = file;
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
        public ActionResult<ItemResponse<int>> Add(FileAddRequest model)
        {
            try
            {
                int id = _fileService.Add(model, _authService.GetCurrentUserId());
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
        public ActionResult<ItemsResponse<FileUpload>> GetPaginated(int pageIndex, int pageSize)
        {
            try
            {
                Paged<FileUpload> pagedList = _fileService.GetPaginated(pageIndex, pageSize);
                if (pagedList == null)
                {
                    return NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<FileUpload>> response = new ItemResponse<Paged<FileUpload>>();
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
        public ActionResult<ItemsResponse<FileUpload>> SearchPaginated(int pageIndex, int pageSize, string query)
        {
            try
            {
                Paged<FileUpload> pagedList = _fileService.SearchPaginated(pageIndex, pageSize, query);
                if (pagedList == null)
                {
                    return NotFound404(new ErrorResponse("Records Not Found"));
                }
                else
                {
                    ItemResponse<Paged<FileUpload>> response = new ItemResponse<Paged<FileUpload>>();
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

    }
}


