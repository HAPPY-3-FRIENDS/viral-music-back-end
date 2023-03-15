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
using Microsoft.AspNetCore.Authorization;
using System.Net;
using ViralMusicAPI.Handler;
using System.ComponentModel.DataAnnotations;
using ViralMusicAPI.Exceptions;
using BusinessObjects.Exceptions;
using System;

namespace ViralMusicAPI.Controllers
{
    [Route("api/playlists")]
    [ApiController]
    public class PlaylistsController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IMapper _mapper;

        public PlaylistsController(IPlaylistRepository playlistRepository, IMapper mapper)
        {
            _playlistRepository = playlistRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all playlist of an account.
        /// </summary>
        /// 
        /// <returns>A list of all playlist of an account.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all playlist of an account.
        /// - Sample request: 
        /// 
        ///       GET /api/playlists/list/admin
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of playlist Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<PlaylistDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("list/{username}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<List<PlaylistDTO>>>> GetPlaylistsByUsername(string username)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find playlists successfully!",
                HttpStatusCode.OK,
                _mapper.Map<IEnumerable<PlaylistDTO>>(await _playlistRepository.GetListByUsername(username))));
        }

        /// <summary>
        /// Get a list of all playlist of an account and keyword.
        /// </summary>
        /// 
        /// <returns>A list of all playlist of an account and keyword.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all playlist of an account and keyword.
        /// - Sample request: 
        /// 
        ///       GET /api/playlists/list/username/keyword
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of playlist Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<PlaylistDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("list/{username}/{keyword}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<List<GenreDTO>>>> GetPlaylistsByUsernameAndKeyword(string username, string keyword)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find playlists successfully!",
                HttpStatusCode.OK,
                _mapper.Map<IEnumerable<PlaylistDTO>>(await _playlistRepository.GetListByUsernameByName(username, keyword))));
        }

        /// <summary>
        /// Get a playlist of an account and keyword.
        /// </summary>
        /// 
        /// <returns>A playlist of an account and keyword.</returns>
        /// <remarks>
        /// Description:
        /// - Return a playlist of an account and keyword.
        /// - Sample request: 
        /// 
        ///       GET /api/playlists/username/keyword
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of playlist Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<PlaylistDTO>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet("{username}/{keyword}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<List<GenreDTO>>>> GetPlaylistByUsernameAndCorrectName(string username, string keyword)
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find playlist successfully!",
                HttpStatusCode.OK,
                _mapper.Map<PlaylistDTO>(await _playlistRepository.GetByUsernameByName(username, keyword))));
        }

        /// <summary>
        /// Update an existing Playlist.
        /// </summary>
        /// 
        /// <param name="id">
        /// Playlist's id which is needed for updating.
        /// </param>
        /// 
        /// <param name="Playlist">
        /// Playlist object that needs to be updated.
        /// </param>
        /// 
        /// <returns>An update existing Playlist.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return an update existing Playlist.
        /// - Sample request: 
        /// 
        ///       PUT /api/playlists/username/1
        ///     
        /// - Sample request body: 
        ///     
        ///       {
        ///             "name": "Nhạc chữa lành",
        ///             "image": "Image ne",
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Playlist not found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<PlaylistDTO>), 200)]
        [Produces("application/json")]
        [HttpPut("{username}/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<PlaylistDTO>>> PutPlaylist(string username, int id, [Required][FromBody] PlaylistDTO playlistDTO)
        {
            if (await PlaylistExists(id) == false)
                throw new NotFoundException("Playlist with id '" + id + "' does not exist!");

            Playlist updatePlaylist = await _playlistRepository.GetByIdAsync(id);
            if (!updatePlaylist.Owner.Equals(username))
                throw new NotFoundException("Playlist with id '" + id + "' does not exist or you don't have permission!");

            updatePlaylist.Name = playlistDTO.Name;
            updatePlaylist.Image = playlistDTO.Image;

            await _playlistRepository.UpdateAsync(updatePlaylist);

            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Update artist successfully!",
                HttpStatusCode.OK,
                _mapper.Map<PlaylistDTO>(updatePlaylist)));
        }

        /// <summary>
        /// Create a new Playlist.
        /// </summary>
        /// 
        /// <param name="Playlist">
        /// Playlist object that needs to be created.
        /// </param>
        /// 
        /// <returns>A new Playlist.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a new Playlist.
        /// - Sample request: 
        /// 
        ///       POST /api/playlists/username
        ///     
        ///       {
        ///             "name": "Nhạc chữa lành",
        ///             "image": "Image ne",
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="201">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<PlaylistDTO>), 201)]
        [Produces("application/json")]
        [HttpPost("{username}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<PlaylistDTO>>> PostPlaylist(string username, [Required][FromBody] PlaylistDTO playlistDTO)
        {
            if (await PlaylistExists(username, playlistDTO.Name) == true)
                throw new BadRequestException("Playlist with name '" + playlistDTO.Name + "' is existed!");

            playlistDTO.Owner = username;
            await _playlistRepository.AddAsync(_mapper.Map<Playlist>(playlistDTO));
            Playlist createPlaylist = await _playlistRepository.GetByUsernameByName(username, playlistDTO.Name);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create artist successfully!",
                HttpStatusCode.Created,
                _mapper.Map<PlaylistDTO>(createPlaylist)));
        }

        /// <summary>
        /// Delete a specific Playlist.
        /// </summary>
        /// 
        /// <param name="id">
        /// Playlist's id which is needed for deleting a Playlist.
        /// </param>
        /// 
        /// <returns>Delete action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return delete action status.
        /// - Sample request: 
        /// 
        ///       DELETE /api/playlists/username/1
        ///       
        /// </remarks>
        /// 
        /// <response code="204">Delete Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Playlist not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{username}/{id}")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(ResponseDTO<>), 200)]
        public async Task<ActionResult<ResponseDTO<String>>> DeletePlaylist(string username, int id)
        {
            if (await PlaylistExists(id) == false)
                throw new BadRequestException("Playlist with id '" + id + "' is not existed!");

            Playlist playlist = await _playlistRepository.GetByIdAsync(id);
            if (!playlist.Owner.Equals(username))
                throw new BadRequestException("Playlist with id '" + id + "' is not existed or you don't have permission!");

            await _playlistRepository.DeleteAsync(playlist);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create playlist successfully!",
                HttpStatusCode.OK,
                ""
                ));
        }

        /// <summary>
        /// Count Playlist.
        /// </summary>
        /// 
        /// <returns>Quantity of Playlist by String.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return quantity of Playlist by String.
        /// - Sample request: 
        /// 
        ///       GET /api/playlists/username/count
        ///       
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Playlist not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("{username}/count")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(ResponseDTO<int>), 200)]
        public async Task<ActionResult<ResponseDTO<int>>> CountPlaylists(string username)
        {
            List<Playlist> list = await _playlistRepository.GetListByUsername(username);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Count Playlists successfully!",
                HttpStatusCode.OK,
                list.Count
                ));
        }

        private async Task<bool> PlaylistExists(int id)
        {
            if (await _playlistRepository.GetByIdAsync(id) == null)
                return false;
            return true;
        }

        private async Task<bool> PlaylistExists(string username, string name)
        {
            if (await _playlistRepository.GetByUsernameByName(username, name) == null)
                return false;
            return true;
        }
    }
}