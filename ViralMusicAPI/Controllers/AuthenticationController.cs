using BusinessObjects.DataTranferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using System.Net;
using System.Threading.Tasks;
using ViralMusicAPI.Handler;

namespace ViralMusicAPI.Controllers
{
    /// <summary>
    /// Authentication API
    /// </summary>
    [Route("/api/auth")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationController(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        /// <summary>
        /// Authentication a User.
        /// </summary>
        /// 
        /// <param name="userDTO">
        /// User's username and password that need to be authenticated.
        /// </param>
        /// 
        /// <returns>Access Token.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return Access Token.
        /// - Sample request: 
        /// 
        ///       POST /api/auth
        ///     
        ///       {
        ///             "username": "tienhuynh-tn",
        ///             "password": "123"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal server error</response>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<string>), 200)]
        [HttpPost]
        public async Task<ActionResult<ResponseDTO<string>>> Authentication([FromBody] UserDTO userDTO)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Authentication user successfully!",
                HttpStatusCode.OK,
                await _authenticationRepository.Authentication(userDTO.Username, userDTO.Password)));
        }
    }
}
