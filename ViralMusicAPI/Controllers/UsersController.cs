using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Repositories.IRepositories;
using AutoMapper;
using System;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Mapper;

namespace ViralMusicAPI.Controllers
{
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

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                return Ok(mapper.Map<IEnumerable<UserDTO>>(await _userRepository.GetAllAsync()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
        public async Task<ActionResult<User>> GetUser(string username)
        {
            try
            {
                return Ok(mapper.Map<UserDTO>(await _userRepository.GetByUsername(username)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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