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
using BusinessObjects.Exceptions;
using System.ComponentModel.DataAnnotations;

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
        /// - Sample request: 
        /// 
        ///       GET /api/users
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="404">List of users Not Found</response>
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

        /// <summary>
        /// Get a specific User by username.
        /// </summary>
        /// 
        /// <param name="username">
        /// User's username which is needed for finding a user.
        /// </param>
        /// 
        /// <returns>A specific User by username.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a specific User by username.
        /// - Sample request: 
        /// 
        ///       GET /api/users/tienhuynh-tn
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(ResponseDTO<UserDTO>), 200)]
        [Produces("application/json")]
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

        /// <summary>
        /// Create a new User.
        /// </summary>
        /// 
        /// <param name="userDTO">
        /// UserDTO object that needs to be created.
        /// </param>
        /// 
        /// <returns>A new User.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a new User.
        /// - Sample request: 
        /// 
        ///       POST /api/users
        ///     
        ///       {
        ///             "username": "tienhuynh-tn",
        ///             "password": "123",
        ///             "fullname": "Huỳnh Lê Thủy Tiên",
        ///             "avatar": null,
        ///             "roleName": "User"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="201">Successfully</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<UserDTO>), 201)]
        [Produces("application/json")]
        [HttpPost]
        public async Task<ActionResult<ResponseDTO<UserDTO>>> PostUser([Required][FromBody] UserDTO userDTO)
        {
            if (await UserExists(userDTO.Username) == true)
                throw new BadRequestException("User with username '" + userDTO.Username + "' is existed!");

            await _userRepository.AddUserAsync(mapper.Map<User>(userDTO));

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create user successfully!",
                HttpStatusCode.Created,
                userDTO
                ));
        }

        /// <summary>
        /// Update an existing User.
        /// </summary>
        /// 
        /// <param name="username">
        /// User's username which is needed for updating a User.
        /// </param>
        /// 
        /// <param name="userDTO">
        /// UserDTO object that needs to be updated.
        /// </param>
        /// 
        /// <returns>An update existing User.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return an update existing user.
        /// - Sample request: 
        /// 
        ///       PUT /api/user/tienhuynh-tn
        ///     
        /// - Sample request body: 
        ///     
        ///       {
        ///             "password": "123",
        ///             "fullname": "Huỳnh Lê Thủy Tiên",
        ///             "avatar": null,
        ///             "roleName": "User"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<UserDTO>), 200)]
        [Produces("application/json")]
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUser(string username, [Required][FromBody] UserDTO userDTO)
        {
            if (await UserExists(username) == false)
                throw new NotFoundException("User with username '" + username + "' does not exist!");

            userDTO.Username = username;
            await _userRepository.UpdateUserAsync(mapper.Map<User>(userDTO));

            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Update user successfully!",
                HttpStatusCode.OK,
                mapper.Map<UserDTO>(userDTO)));
        }

        /// <summary>
        /// Delete a specific User.
        /// </summary>
        /// 
        /// <param name="username">
        /// User's username which is needed for deleting a User.
        /// </param>
        /// 
        /// <returns>Delete action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return delete action status.
        /// - Sample request: 
        /// 
        ///       DELETE /api/users/tienhuynh-tn
        ///       
        /// </remarks>
        /// 
        /// <response code="204">Delete Successfully</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Produces("application/json")]
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            User deleteUser = await _userRepository.GetByUsername(username);
            if (deleteUser == null)
            {
                throw new NotFoundException("User with username '" + username + "' does not exist!");
            }

            await _userRepository.DeleteAsync(deleteUser);

            return NoContent();
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

        private async Task<bool> UserExists(string username)
        {
            if (await _userRepository.GetByUsername(username) == null)
                return false;
            return true;
        }
    }
}