using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.AdvertisingRepo;
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
    public class AdvertisingController : ControllerBase
    {
        private readonly IAdvertisingRepository _advertisingRepo;

        public AdvertisingController(IAdvertisingRepository advertisingRepo)
        {
            _advertisingRepo = advertisingRepo;
        }

        [HttpGet]
        [Route("Web/GetAdvertising")]
        public async Task<IActionResult> GetAdvertising()
        {
            try
            {
                var result = await _advertisingRepo.GetAllAdvertisings();
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
        [Route("CMS/GetAdvertising")]
        public async Task<IActionResult> GetAllAdvertising([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<AdvertisingDTO> items = await _advertisingRepo.GetCMSAdvertisings(PageNumber, PageSize);
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
        [Route("CMS/GetAdvertisingById")]
        public async Task<IActionResult> GetAdvertisingById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _advertisingRepo.GetAdvertisingById(Id);
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

        [HttpPost("CMS/AddAdvertising")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddAdvertising([FromForm] AdvertisingDTO request)
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
                    var result = await _advertisingRepo.AddAsync(request, request.File);
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

        [HttpPost("CMS/EditAdvertising")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> EditAdvertising([FromForm] AdvertisingDTO request)
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
                    var result = await _advertisingRepo.UpdateAsync(request, request.File);
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

        [HttpPost("CMS/DeleteAdvertising")]
        public async Task<IActionResult> DeleteAdvertising([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _advertisingRepo.DeleteAsync(Id);
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
