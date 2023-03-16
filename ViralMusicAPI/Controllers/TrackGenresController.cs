using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using AutoMapper;
using BusinessObjects.DataTranferObjects;
using System.Net;
using ViralMusicAPI.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BusinessObjects.Models;

namespace ViralMusicAPI.Controllers
{
    /// <summary>
    /// Track Genre API
    /// </summary>
    [Route("api/tracks-genres")]
    [ApiController]
    public class TrackGenresController : ControllerBase
    {
        private readonly ITrackGenreRepository _trackGenreRepository;
        private readonly IMapper _mapper;

        public TrackGenresController(IMapper mapper, ITrackGenreRepository trackGenreRepository)
        {
            _mapper = mapper;
            _trackGenreRepository = trackGenreRepository;
        }

        /// <summary>
        /// Get a list of all tracks of a genre by genreId.
        /// </summary>
        /// 
        /// <param name="genreId">
        /// GenreId that needed
        /// </param>
        /// 
        /// <returns>A list of all tracks of a genre by genreId.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all tracks of a genre by genreId.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks-genres/genres/{genreId}
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of all tracks of a genre by genreId Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<TrackGenreDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("/genres/{genreId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TrackGenreDTO>>>> GetAllTracksByGenreId(int genreId)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
            "Find a list of all tracks of a genre by genreId successfully!",
            HttpStatusCode.OK,
            _mapper.Map<IEnumerable<TrackGenreDTO>>(await _trackGenreRepository.GetAllTracksOfGenreByGenreIdAsync(genreId))));
        }

        /// <summary>
        /// Get a list of all genres of a track by trackId.
        /// </summary>
        /// 
        /// <param name="trackId">
        /// TrackId that needed
        /// </param>
        /// 
        /// <returns>A list of all genres of a track by trackId.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all genres of a track by trackId.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks-genres/track/{trackId}
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of all genres of a track by trackId Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<TrackGenreDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("/track/{trackId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TrackGenreDTO>>>> GetAllGenresByTrackId(int trackId)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
            "Find a of all genres of a track by trackId successfully!",
            HttpStatusCode.OK,
            _mapper.Map<IEnumerable<TrackGenreDTO>>(await _trackGenreRepository.GetAllGenresOfTrackByTrackIdAsync(trackId))));
        }

        /// <summary>
        /// Add list of genreId to a Track by trackId.
        /// </summary>
        /// 
        /// <param name="listGenreIds">
        /// List of genre Ids
        /// </param>
        /// 
        /// <returns>Create action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return create action status.
        /// - Sample request: 
        /// 
        ///       POST /api/tracks-artists/track/{trackId}
        ///     
        /// </remarks>
        /// 
        /// <response code="201">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Resoure Not Found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<TrackGenreDTO>), 201)]
        [Produces("application/json")]
        [HttpPost("/track/{trackId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<TrackGenreDTO>>> AddListGenreToATrack(int trackId, [FromBody] List<int> listGenreIds)
        {
            await _trackGenreRepository.AddListGenreToATrack(trackId, listGenreIds);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
            "Add a list of genre id to a track by trackId successfully!",
            HttpStatusCode.Created,
            ""));
        }
    }
}
