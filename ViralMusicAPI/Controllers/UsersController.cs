using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
using Repositories.IRepositories;
using AutoMapper;
using System;
using BusinessObjects.DataTranferObjects;
using ViralMusicAPI.Handler;
using System.Net;
using Microsoft.AspNetCore.Http;
using ViralMusicAPI.Exceptions;
using System.Linq.Expressions;

namespace ViralMusicAPI.Controllers
{
    /// <summary>
    /// User API
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        /// 
        /// <returns>A list of all users.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all users.
        /// - Sample request: GET /api/users
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="404">List of users is Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<UserDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet]
        public async Task<ActionResult<ResponseDTO<List<UserDTO>>>> GetUsers()
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find user successfully!",
                HttpStatusCode.OK,
                mapper.Map<IEnumerable<UserDTO>>(await _userRepository.GetAllIncludeAsync(new List<Expression<Func<User, object>>>
                {
                    u => u.Role
                })))) ;
        }

        [HttpGet("count", Name = "CountUsers")]
        public async Task<ActionResult<int>> CountUsers()
        {
            try
            {
                var res = await _userRepository.CountUsers();
                return res;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // GET: api/users/pequan
        [HttpGet("{username}")]
        public async Task<ActionResult<ResponseDTO<UserDTO>>> GetUser(string username)
        {
            User user = await _userRepository.GetByUsername(username);
            if (user == null) throw new NotFoundException("User is Not Found with username: " + username);
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find user successfully!",
                HttpStatusCode.OK,
                mapper.Map<UserDTO>(user)));
        }

        // PUT: api/users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] User user)
        {
            if (await UserExists(user.Username) == false)
            {
                return BadRequest("User doesn't exist!");
            }

            try
            {
                await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] User user)
        {
            if (await UserExists(user.Username) == true)
            {
                return BadRequest("User has existed!");
            }

            try
            {
                user = await _userRepository.InitUser(user);
                await _userRepository.AddAsync(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            User deleteUser = await _userRepository.GetByUsername(username);
            if (deleteUser == null)
            {
                return BadRequest("User doesn't exist!");
            }

            try
            {
                await _userRepository.DeleteAsync(deleteUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return NoContent();
        }

        private async Task<bool> UserExists(string username)
        {
            if (await _userRepository.GetByUsername(username) == null)
                return false;
            return true;
        }
    }
}