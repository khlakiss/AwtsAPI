using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.WhatWeDoRepo;
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
    public class WhatWeDoController : ControllerBase
    {
        private readonly IWhatWeDoRepository _whatWeDoRepo;

        public WhatWeDoController(IWhatWeDoRepository whatWeDoRepo)
        {
            _whatWeDoRepo = whatWeDoRepo;
        }

        [HttpGet]
        [Route("Web/GetWhatWeDo")]
        public async Task<IActionResult> GetWhatWeDo()
        {
            try
            {
                var result = await _whatWeDoRepo.GetWhatWeDoContent();
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
        [Route("CMS/GetWhatWeDo")]
        public async Task<IActionResult> GetAllWhatWeDo([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<WhatWeDoContentDTO> items = await _whatWeDoRepo.GetAllWhatWeDo(PageNumber, PageSize);
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
        [Route("CMS/GetWhatWeDoById")]
        public async Task<IActionResult> GetAboutUsById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _whatWeDoRepo.GetWhatWeDoById(Id);
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

        [HttpPost("CMS/AddWhatWeDo")]
        public async Task<IActionResult> AddWhatWeDo([FromForm] WhatWeDoContentDTO request)
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
                    var result = await _whatWeDoRepo.AddAsync(request);
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

        [HttpPost("CMS/EditWhatWeDo")]
        public async Task<IActionResult> EditWhatWeDo([FromForm] WhatWeDoContentDTO request)
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
                    var result = await _whatWeDoRepo.UpdateAsync(request);
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

        [HttpPost("CMS/DeleteWhatWeDo")]
        public async Task<IActionResult> DeleteWhatWeDo([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _whatWeDoRepo.DeleteAsync(Id);
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
