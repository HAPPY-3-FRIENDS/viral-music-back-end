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

namespace ViralMusicAPI.Controllers
{
    /// <summary>
    /// Track Artist API
    /// </summary>
    [Route("api/tracks-artists")]
    [ApiController]
    public class TrackArtistController : ControllerBase
    {
        private readonly ITrackInPlaylistRepository _trackInPlaylistRepository;
        private readonly ITrackArtistRepository _trackArtistRepository;
        private readonly IMapper _mapper;

        public TrackArtistController(ITrackInPlaylistRepository trackInPlaylistRepository, IMapper mapper, ITrackArtistRepository trackArtistRepository)
        {
            _trackInPlaylistRepository = trackInPlaylistRepository;
            _mapper = mapper;
            _trackArtistRepository = trackArtistRepository;
        }

        /// <summary>
        /// Get a list of all tracks of an artist by artistId.
        /// </summary>
        /// 
        /// <param name="artistId">
        /// ArtistId that needed
        /// </param>
        /// 
        /// <returns>A list of all tracks of an artist by artistId.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all tracks of an artist by artistId.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks-artists/artists/{artistId}
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of all tracks of an artist by artistId Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<TrackArtistDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("/artists/{artistId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TrackArtistDTO>>>> GetAllTracksByArtistId(int artistId)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
            "Find a list of all tracks of an artist by artistId successfully!",
            HttpStatusCode.OK,
            _mapper.Map<IEnumerable<TrackArtistDTO>>(await _trackArtistRepository.GetAllTracksOfArtistByArtistIdAsync(artistId))));
        }

        /// <summary>
        /// Get a list of all artists of a track by trackId.
        /// </summary>
        /// 
        /// <param name="trackId">
        /// TrackId that needed
        /// </param>
        /// 
        /// <returns>A list of all artists of a track by trackId.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all artists of a track by trackId.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks-artists/tracks/{trackId}
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of all artists of a track by trackId Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<TrackArtistDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("/tracks/{trackId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TrackArtistDTO>>>> GetAllArtistsByTrackId(int trackId)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
            "Find a of all artists of a track by trackId successfully!",
            HttpStatusCode.OK,
            _mapper.Map<IEnumerable<TrackArtistDTO>>(await _trackArtistRepository.GetAllArtistOfTrackByTrackIdAsync(trackId))));
        }
    }
}
