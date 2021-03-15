using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.OurMissionRepo;
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
    public class OurMissionController : ControllerBase
    {
        private readonly IOurMissionRepository _ourMissionRepo;

        public OurMissionController(IOurMissionRepository ourMissionRepo)
        {
            _ourMissionRepo = ourMissionRepo;
        }

        [HttpGet]
        [Route("Web/GetOurMission")]
        public async Task<IActionResult> GetOurMission()
        {
            try
            {
                var result = await _ourMissionRepo.GetOurMissionContent();
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
        [Route("CMS/GetOurMission")]
        public async Task<IActionResult> GetAllOurMission([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<OurMissionContentDTO> items = await _ourMissionRepo.GetAllOurMission(PageNumber, PageSize);
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
        [Route("CMS/GetOurMissionById")]
        public async Task<IActionResult> GetOurMissionById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _ourMissionRepo.GetOurMissionById(Id);
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

        [HttpPost("CMS/AddOurMission")]
        public async Task<IActionResult> AddOurMission([FromForm] OurMissionContentDTO request)
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
                    var result = await _ourMissionRepo.AddAsync(request);
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

        [HttpPost("CMS/EditOurMission")]
        public async Task<IActionResult> EditOurMission([FromForm] OurMissionContentDTO request)
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
                    var result = await _ourMissionRepo.UpdateAsync(request);
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

        [HttpPost("CMS/DeleteOurMission")]
        public async Task<IActionResult> DeleteOurMission([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _ourMissionRepo.DeleteAsync(Id);
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
