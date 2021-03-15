using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.ContactRepo;
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
    public class ContactController : ControllerBase
    {
        private readonly IContactRepo _contactRepo;

        public ContactController(IContactRepo contactRepo)
        {
            _contactRepo = contactRepo;
        }

        [HttpGet]
        [Route("Web/GetContacts")]
        public async Task<IActionResult> GetContacts()
        {
            try
            {
                var result = await _contactRepo.GetAllContacts();
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
        [Route("CMS/GetContacts")]
        public async Task<IActionResult> GetAllContacts([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<ContactUsDTO> items = await _contactRepo.GetCMSContacts(PageNumber, PageSize);
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
        [Route("CMS/GetContactById")]
        public async Task<IActionResult> GetContactById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _contactRepo.GetContactById(Id);
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

        [HttpPost("CMS/AddContact")]
        public async Task<IActionResult> AddContact([FromForm] ContactUsDTO request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.NameEn))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Name Not Found!");
                else if (String.IsNullOrEmpty(request.NameAr))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Name Not Found!");
                else if (request.Latitude <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Latitude Not Found!");
                else if (request.Longitude <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Latitude Not Found!");
                else if (String.IsNullOrEmpty(request.Telephone))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Telephone Not Found!");
                else if (String.IsNullOrEmpty(request.Fax))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Fax Not Found!");
                else if (String.IsNullOrEmpty(request.TollFree))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "TollFree Not Found!");
                else if (String.IsNullOrEmpty(request.Mobile))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Mobile Not Found!");
                else if (String.IsNullOrEmpty(request.Email))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Email Not Found!");
                else if (String.IsNullOrEmpty(request.PoBox))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "PoBox Not Found!");
                else
                {
                    var result = await _contactRepo.AddAsync(request);
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

        [HttpPost("CMS/EditContact")]
        public async Task<IActionResult> EditContact([FromForm] ContactUsDTO request)
        {
            try
            {
                if (request.Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                if (String.IsNullOrEmpty(request.NameEn))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "English Name Not Found!");
                else if (String.IsNullOrEmpty(request.NameAr))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Arabic Name Not Found!");
                else if (request.Latitude <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Latitude Not Found!");
                else if (request.Longitude <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Latitude Not Found!");
                else if (String.IsNullOrEmpty(request.Telephone))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Telephone Not Found!");
                else if (String.IsNullOrEmpty(request.Fax))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Fax Not Found!");
                else if (String.IsNullOrEmpty(request.TollFree))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "TollFree Not Found!");
                else if (String.IsNullOrEmpty(request.Mobile))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Mobile Not Found!");
                else if (String.IsNullOrEmpty(request.Email))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Email Not Found!");
                else if (String.IsNullOrEmpty(request.PoBox))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "PoBox Not Found!");
                else
                {
                    var result = await _contactRepo.UpdateAsync(request);
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

        [HttpPost("CMS/DeleteContact")]
        public async Task<IActionResult> DeleteContact([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _contactRepo.DeleteAsync(Id);
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
