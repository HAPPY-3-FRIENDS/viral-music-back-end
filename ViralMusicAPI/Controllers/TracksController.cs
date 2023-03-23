using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Repositories.IRepositories;
using AutoMapper;
using BusinessObjects.DataTranferObjects;
using Microsoft.AspNetCore.Http;
using System.Net;
using ViralMusicAPI.Handler;
using ViralMusicAPI.Exceptions;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using System;
using BusinessObjects.Exceptions;

namespace ViralMusicAPI.Controllers
{
    [Route("api/tracks")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IMapper _mapper;

        public TracksController(ITrackRepository trackRepository, IMapper mapper)
        {
            _trackRepository = trackRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all tracks.
        /// </summary>
        /// 
        /// <returns>A list of all tracks.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all tracks.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of tracks Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResponseDTO<List<TrackDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        
        public async Task<ActionResult<ResponseDTO<IEnumerable<Track>>>> GetTracks()
        {
            /*List<Expression<Func<Track, object>>> include = new List<Expression<Func<Track, object>>>
            {
                t => t.TrackArtists,
                t => t.TrackGenres,
                t => t.TrackArtists.Select(x => x.Artist),
                t => t.TrackGenres.Select(x => x.Genre)
            };*/
            List<string> includes = new List<string> { "TrackArtists.Artist", "TrackGenres.Genre" };
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find tracks successfully!",
                HttpStatusCode.OK,
                await _trackRepository.GetAllIncludeAsync(includes)));
        }

        /// <summary>
        /// Get a specific Track by id.
        /// </summary>
        /// 
        /// <param name="id">
        /// Track's id which is needed for finding.
        /// </param>
        /// 
        /// <returns>A specific Track by id.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a specific Track by id.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks/1
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Track not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(ResponseDTO<TrackDTO>), 200)]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseDTO<TrackDTO>>> GetTrack(int id)
        {
            Track track = await _trackRepository.GetByIdAsync(id);
            if (track == null) throw new NotFoundException("Track is Not Found with id: " + id);
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find track successfully!",
                HttpStatusCode.OK,
                _mapper.Map<TrackDTO>(track)));
        }

        /// <summary>
        /// Get a specific Track by id.
        /// </summary>
        /// 
        /// <param name="id">
        /// Track's id which is needed for finding.
        /// </param>
        /// 
        /// <returns>A specific Track by id.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a specific Track by id.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks/name/BaiGiDo
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Track not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(ResponseDTO<IEnumerable<TrackGetByGenreDTO>>), 200)]
        [Produces("application/json")]
        //[Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("name/{name}")]
        public async Task<ActionResult<ResponseDTO<IEnumerable<TrackGetByGenreDTO>>>> GetTracksListByName(string name)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find tracks successfully!",
                HttpStatusCode.OK,
                await _trackRepository.GetTracksListByName(name, _mapper)));
        }

        /// <summary>
        /// Update an existing Track.
        /// </summary>
        /// 
        /// <param name="id">
        /// Track's id which is needed for updating.
        /// </param>
        /// 
        /// <param name="Track">
        /// Track object that needs to be updated.
        /// </param>
        /// 
        /// <returns>An update existing Track.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return an update existing track.
        /// - Sample request: 
        /// 
        ///       PUT /api/tracks/1
        ///     
        /// - Sample request body: 
        ///     
        ///       {
        ///             "title": "Pop",
        ///             "image": "link-image",
        ///             "source": "link-source"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Track not found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<TrackDTO>), 200)]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("{id}")]
        public async Task<ActionResult<ResponseDTO<TrackDTO>>> PutTrack(int id, [Required][FromBody] TrackDTO trackDTO)
        {
            if (await TrackExists(id) == false)
                throw new NotFoundException("Track with id '" + id + "' does not exist!");

            Track updateTrack = await _trackRepository.GetByIdAsync(id);
            updateTrack.Title = trackDTO.Title;
            updateTrack.Image = trackDTO.Image;
            updateTrack.Source = trackDTO.Source;
            await _trackRepository.UpdateAsync(updateTrack);

            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Update track successfully!",
                HttpStatusCode.OK,
                _mapper.Map<TrackDTO>(updateTrack)));
        }

        /// <summary>
        /// Create a new Track.
        /// </summary>
        /// 
        /// <param name="track">
        /// Track object that needs to be created.
        /// </param>
        /// 
        /// <returns>A new Track.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a new Track.
        /// - Sample request: 
        /// 
        ///       POST /api/tracks
        ///     
        ///       {
        ///             "title": "Pop",
        ///             "image": "link-image",
        ///             "source": "link-source"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="201">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<TrackDTO>), 201)]
        [Produces("application/json")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<TrackDTO>>> PostTrack([Required][FromBody] TrackDTO trackDTO)
        {
            DateTime time = DateTime.Now;
            trackDTO.CreatedDate = time;
            await _trackRepository.AddAsync(_mapper.Map<Track>(trackDTO));

            Track track = await _trackRepository.GetByTrackDateTime(time);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create track successfully!",
                HttpStatusCode.Created,
                _mapper.Map<TrackDTO>(track)));
        }

        /// <summary>
        /// Delete a specific Track.
        /// </summary>
        /// 
        /// <param name="id">
        /// Track's id which is needed for deleting a Track.
        /// </param>
        /// 
        /// <returns>Delete action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return delete action status.
        /// - Sample request: 
        /// 
        ///       DELETE /api/tracks/1
        ///       
        /// </remarks>
        /// 
        /// <response code="204">Delete Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Track not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(ResponseDTO<>), 200)]
        public async Task<ActionResult<ResponseDTO<String>>> DeleteTrack(int id)
        {
            if (await TrackExists(id) == false)
                throw new BadRequestException("Track with id '" + id + "' is not existed!");

            Track track = await _trackRepository.GetByIdAsync(id);
            await _trackRepository.DeleteAsync(track);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create track successfully!",
                HttpStatusCode.OK,
                ""
                ));
        }

        /// <summary>
        /// Count Track.
        /// </summary>
        /// 
        /// <returns>Quantity of Track by String.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return quantity of track by String.
        /// - Sample request: 
        /// 
        ///       GET /api/tracks/count
        ///       
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Track not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("count")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(ResponseDTO<int>), 200)]
        public async Task<ActionResult<ResponseDTO<int>>> CountGenres()
        {
            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Count Track successfully!",
                HttpStatusCode.OK,
                await _trackRepository.CountAsync()
                ));
        }

        private async Task<bool> TrackExists(int id)
        {
            if (await _trackRepository.GetByIdAsync(id) == null)
                return false;
            return true;
        }
    }
}