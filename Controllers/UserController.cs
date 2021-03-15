using AWTS.BLL.Models;
using AWTS.BLL.Paging;
using AWTS.BLL.Repositories.UserRepo;
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
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet]
        [Route("CMS/LogIn")]
        public async Task<IActionResult> LogIn([FromQuery(Name = "userName")] string Username, [FromQuery(Name = "password")] string Password)
        {
            try
            {
                if (String.IsNullOrEmpty(Username))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Username not found!");
                else if (String.IsNullOrEmpty(Password))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Passworde not found!");
                else
                {
                    var result = await _userRepo.LogIn(Username,Password);
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

        [HttpGet]
        [Route("CMS/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery(Name = "pageNumber")] int PageNumber, [FromQuery(Name = "pageSize")] int PageSize)
        {
            try
            {
                if (PageNumber <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page number not found!");
                else if (PageSize <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Page size not found!");
                else
                {
                    PagedList<UserDTO> items = await _userRepo.GetCMSUsers(PageNumber, PageSize);
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
        [Route("CMS/GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery(Name = "id")] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _userRepo.GetUserById(Id);
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

        [HttpPost("CMS/AddUser")]
        public async Task<IActionResult> AddUser([FromForm] UserDTO request)
        {
            try
            {
                if (String.IsNullOrEmpty(request.FirstName))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "FirstName Not Found!");
                else if (String.IsNullOrEmpty(request.LastName))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "LastName Not Found!");
                else if (String.IsNullOrEmpty(request.Username))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Username Not Found!");
                else if (String.IsNullOrEmpty(request.Password))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Password Not Found!");
                else
                {
                    var result = await _userRepo.AddAsync(request);
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

        [HttpPost("CMS/EditUser")]
        public async Task<IActionResult> EditUser([FromForm] UserDTO request)
        {
            try
            {
                if (request.Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else if (String.IsNullOrEmpty(request.FirstName))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "FirstName Not Found!");
                else if (String.IsNullOrEmpty(request.LastName))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "LastName Not Found!");
                else if (String.IsNullOrEmpty(request.Username))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Username Not Found!");
                else if (String.IsNullOrEmpty(request.Password))
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Password Not Found!");
                else
                {
                    var result = await _userRepo.UpdateAsync(request);
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

        [HttpPost("CMS/DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromForm] long Id)
        {
            try
            {
                if (Id <= 0)
                    return StatusCode((int)System.Net.HttpStatusCode.BadRequest, "Id not found!");
                else
                {
                    var result = await _userRepo.DeleteAsync(Id);
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
