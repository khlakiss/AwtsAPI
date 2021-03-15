using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.EmailRepo;
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
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRepo;

        public EmailController(IEmailRepository emailRepo)
        {
            _emailRepo = emailRepo;
        }

        [HttpGet]
        [Route("CMS/GetEmails")]
        public async Task<IActionResult> GetEmails([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<EmailDTO> items = await _emailRepo.GetAllEmails(PageNumber, PageSize);
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
        [Route("CMS/GetEmailById")]
        public async Task<IActionResult> GetEmailById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _emailRepo.GetEmailById(Id);
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

        [HttpPost("CMS/AddEmail")]
        public async Task<IActionResult> AddEmail([FromForm] EmailDTO request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.Email))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Email Not Found!");
                else if (String.IsNullOrEmpty(request.Name))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Name Title Not Found!");
                else if (String.IsNullOrEmpty(request.Message))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Message Title Not Found!");
                else
                {
                    var result = await _emailRepo.AddAsync(request);
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

        [HttpPost("CMS/EditEmail")]
        public async Task<IActionResult> EditEmail([FromForm] EmailDTO request)
        {
            try
            {
                if (request.Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else if (String.IsNullOrEmpty(request.Email))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Email Not Found!");
                else if (String.IsNullOrEmpty(request.Name))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Name Title Not Found!");
                else if (String.IsNullOrEmpty(request.Message))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Message Title Not Found!");
                else
                {
                    var result = await _emailRepo.UpdateAsync(request);
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

        [HttpPost("CMS/DeleteEmail")]
        public async Task<IActionResult> DeleteEmail([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _emailRepo.DeleteAsync(Id);
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
