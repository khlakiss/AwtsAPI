using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.HomeRepo;
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
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository _homeRepo;

        public HomeController(IHomeRepository homeRepo)
        {
            _homeRepo = homeRepo;
        }

        [HttpGet]
        [Route("Web/GetHome")]
        public async Task<IActionResult> GetHome()
        {
            try
            {
                var result = await _homeRepo.GetHomeContent();
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
        [Route("CMS/GetHome")]
        public async Task<IActionResult> GetAllHome([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<HomeContentDTO> items = await _homeRepo.GetAllHomeContents(PageNumber, PageSize);
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
        [Route("CMS/GetHomeById")]
        public async Task<IActionResult> GetHomeById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _homeRepo.GetHomeContentById(Id);
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

        [HttpPost("CMS/AddHome")]
        public async Task<IActionResult> AddHome([FromForm] HomeContentDTO request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.TitleEN))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Title Not Found!");
                else if (String.IsNullOrEmpty(request.TitleAR))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Title Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionEN))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Description Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionAR))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Description Not Found!");
                else if (String.IsNullOrEmpty(request.Video))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Video Not Found!");
                else
                {
                    var result = await _homeRepo.AddAsync(request);
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

        [HttpPost("CMS/EditHome")]
        public async Task<IActionResult> EditHome([FromForm] HomeContentDTO request)
        {
            try
            {
                if (request.Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else if (String.IsNullOrEmpty(request.TitleEN))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Title Not Found!");
                else if (String.IsNullOrEmpty(request.TitleAR))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Title Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionEN))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Description Not Found!");
                else if (String.IsNullOrEmpty(request.DescriptionAR))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Description Not Found!");
                else if (String.IsNullOrEmpty(request.Video))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Video Not Found!");
                else
                {
                    var result = await _homeRepo.UpdateAsync(request);
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

        [HttpPost("CMS/DeleteHome")]
        public async Task<IActionResult> DeleteHome([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _homeRepo.DeleteAsync(Id);
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
