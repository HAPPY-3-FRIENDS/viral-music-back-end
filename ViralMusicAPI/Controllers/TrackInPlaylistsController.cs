using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BusinessObjects.Models;
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
    /// Track In Playlist API
    /// </summary>
    [Route("api/track-in-playlist")]
    [ApiController]
    public class TrackInPlaylistsController : ControllerBase
    {
        private readonly ITrackInPlaylistRepository _trackInPlaylistRepository;
        private readonly IMapper _mapper;

        public TrackInPlaylistsController(ITrackInPlaylistRepository trackInPlaylistRepository, IMapper mapper)
        {
            _trackInPlaylistRepository = trackInPlaylistRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all tracks in a playlist.
        /// </summary>
        /// 
        /// <returns>A list of all tracks in a playlist.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all tracks in a playlist.
        /// - Sample request: 
        /// 
        ///       GET /api/track-in-playlist/{playListId}
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of all tracks in a playlist Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<TrackInPlaylistDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TrackInPlaylistDTO>>>> GetAllTracksInPlaylistByPlaylistId(int playListId)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
            "Find a list of all tracks in a playlist successfully!",
            HttpStatusCode.OK,
            _mapper.Map<IEnumerable<TrackInPlaylistDTO>>(await _trackInPlaylistRepository.GetAllTracksInPlaylistByPlaylistIdAsync(playListId))));
        }

        /// <summary>
        /// Add a track to a playlist by trackId and playlistId
        /// </summary>
        /// 
        /// <param name="trackInPlaylistDTO">
        /// TrackInPlaylistDTO object that needs to be created.
        /// </param>
        /// 
        /// <returns>Create action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return create action status.
        /// - Sample request: 
        /// 
        ///       POST /api/track-in-playlist
        ///     
        ///       {
        ///             "playlistId": 1,
        ///             "trackId": 1
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="201">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Resoure Not Found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<UserDTO>), 201)]
        [Produces("application/json")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<TrackInPlaylist>> AddATrackToAPlaylist([FromBody] TrackInPlaylistDTO trackInPlaylistDTO)
        {
            await _trackInPlaylistRepository.AddATrackToAPlaylist(trackInPlaylistDTO.PlaylistId, trackInPlaylistDTO.TrackId);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
            "Add a track to a playlist by trackId and playlistId successfully!",
            HttpStatusCode.Created,
            ""));
        }

        /// <summary>
        /// Delete a track in a playlist.
        /// </summary>
        /// 
        /// <param name="trackInPlaylistDTO">
        /// TrackInPlaylistDTO object that needs to be deleted.
        /// </param>
        /// 
        /// <returns>Delete action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return delete action status.
        /// - Sample request: 
        /// 
        ///       DELETE /api/track-in-playlist
        ///     
        ///       {
        ///             "playlistId": 1,
        ///             "trackId": 1
        ///       }
        ///       
        /// </remarks>
        /// 
        /// <response code="204">Delete Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpDelete]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteTrackInPlaylist(TrackInPlaylistDTO trackInPlaylistDTO)
        {
            await _trackInPlaylistRepository.DeleteATrackInAPlaylist(trackInPlaylistDTO.PlaylistId, trackInPlaylistDTO.TrackId);

            return NoContent();
        }
    }
}
