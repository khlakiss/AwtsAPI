using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.ServiceRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwtsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepo;

        public ServiceController(IServiceRepository serviceRepo)
        {
            _serviceRepo = serviceRepo;
        }

        [HttpGet]
        [Route("Web/GetServices")]
        public async Task<IActionResult> GetServices()
        {
            try
            {
                var result = await _serviceRepo.GetAllServices();
                if (result != null)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("CMS/GetServices")]
        public async Task<IActionResult> GetALLServices([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<ServiceDTO> items = await _serviceRepo.GetCMSServices(PageNumber, PageSize);
                    if (items != null)
                    {
                        var result = new
                        {
                            items.TotalCount,
                            items.PageSize,
                            items.CurrentPage,
                            items.TotalPages,
                            items.HasNext,
                            items.HasPrevious,
                            items
                        };
                        return Ok(result);
                    }
                    else
                        return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("CMS/GetServiceById")]
        public async Task<IActionResult> GetServiceById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _serviceRepo.GetServiceById(Id);
                    if (result != null)
                        return Ok(result);
                    else
                        return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("CMS/AddService")]
        public async Task<IActionResult> AddService([FromForm] ServiceDTO request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.TitleEn))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Title Not Found!");
                else if (String.IsNullOrEmpty(request.TitleAr))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Title Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionEn))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Description Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionAr))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Description Not Found!");
                else
                {
                    var result = await _serviceRepo.AddAsync(request);
                    if (result != null)
                        return Ok(result);
                    else
                        return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("CMS/EditService")]
        public async Task<IActionResult> EditService([FromForm] ServiceDTO request)
        {
            try
            {
                if (request.Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else if (String.IsNullOrEmpty(request.TitleEn))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Title Not Found!");
                else if (String.IsNullOrEmpty(request.TitleAr))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Title Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionEn))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Description Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionAr))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Description Not Found!");
                else
                {
                    var result = await _serviceRepo.UpdateAsync(request);
                    if (result != null)
                        return Ok(result);
                    else
                        return NoContent();
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("CMS/DeleteService")]
        public async Task<IActionResult> DeleteService([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _serviceRepo.DeleteAsync(Id);
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
