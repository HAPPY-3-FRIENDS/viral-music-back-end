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
using ViralMusicAPI.Exceptions;
using System.ComponentModel.DataAnnotations;
using BusinessObjects.Exceptions;
using System;

namespace ViralMusicAPI.Controllers
{
    [Route("api/artists")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistsController(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all artists.
        /// </summary>
        /// 
        /// <returns>A list of all artists.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all artists.
        /// - Sample request: 
        /// 
        ///       GET /api/artists
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Return a list of all artists Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">List of artists Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<ArtistDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<List<ArtistDTO>>>> GetArtists()
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find list of all artists successfully!",
                HttpStatusCode.OK,
                _mapper.Map<IEnumerable<ArtistDTO>>(await _artistRepository.GetAllAsync())));
        }

        /// <summary>
        /// Get a specific Artist by id.
        /// </summary>
        /// 
        /// <param name="id">
        /// Artist's id which is needed for finding.
        /// </param>
        /// 
        /// <returns>A specific Artist by id.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a specific Artist by id.
        /// - Sample request: 
        /// 
        ///       GET /api/artists/id/1
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Return a specific Artist by id Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">Artist not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(ResponseDTO<ArtistDTO>), 200)]
        [Produces("application/json")]
        [HttpGet("id/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<ArtistDTO>>> GetArtistById(int id)
        {
            Artist artist = await _artistRepository.GetByIdAsync(id);
            if (artist == null) throw new NotFoundException("Artist is Not Found with id: " + id);
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find artist by Id successfully!",
                HttpStatusCode.OK,
                _mapper.Map<ArtistDTO>(artist)));
        }

        /// <summary>
        /// Get a specific Artist by name.
        /// </summary>
        /// 
        /// <param name="name">
        /// Artist's name which is needed for finding.
        /// </param>
        /// 
        /// <returns>A specific Artist by name.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a specific Artist by name.
        /// - Sample request: 
        /// 
        ///       GET /api/artists/name/tlinh
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">Artist not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(ResponseDTO<ArtistDTO>), 200)]
        [Produces("application/json")]
        [HttpGet("name/{name}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<ResponseDTO<ArtistDTO>>> GetArtistByName(string name)
        {
            Artist artist = await _artistRepository.GetByArtistName(name);
            if (artist == null) throw new NotFoundException("Artist is Not Found with name: " + name);
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find artist by name successfully!",
                HttpStatusCode.OK,
                _mapper.Map<ArtistDTO>(artist)));
        }

        /// <summary>
        /// Get a List Artist by name.
        /// </summary>
        /// 
        /// <param name="name">
        /// Artist's name which is needed for finding.
        /// </param>
        /// 
        /// <returns>A List of Artist by name.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a list of Artist by name.
        /// - Sample request: 
        /// 
        ///       GET /api/artists/list/tlinh
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">Artist not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(ResponseDTO<IEnumerable<ArtistDTO>>), 200)]
        [Produces("application/json")]
        [HttpGet("list/{name}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<IEnumerable<ArtistDTO>>> GetArtistList(string name)
        {
            IEnumerable<Artist> artists = await _artistRepository.GetListByArtistName(name);
            if (artists == null) throw new NotFoundException("Artists is Not Found with name: " + name);
            List<ArtistDTO> artistDTOs = new List<ArtistDTO>();
            foreach (var artist in artists)
            {
                var artistDTO = _mapper.Map<ArtistDTO>(artist);
                artistDTOs.Add(artistDTO);
            }

            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find artists successfully!",
                HttpStatusCode.OK,
                artistDTOs));
        }

        /// <summary>
        /// Update an existing Artist.
        /// </summary>
        /// 
        /// <param name="id">
        /// Artist's id which is needed for updating.
        /// </param>
        /// 
        /// <param name="Artist">
        /// Artist object that needs to be updated.
        /// </param>
        /// 
        /// <returns>An update existing Artist.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return an update existing Artist.
        /// - Sample request: 
        /// 
        ///       PUT /api/artist/1
        ///     
        /// - Sample request body: 
        ///     
        ///       {
        ///             "name": "tlinh",
        ///             "profile": "Profile ne",
        ///             "avatar": "Avatar ne"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">Artist not found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<ArtistDTO>), 200)]
        [Produces("application/json")]
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO<ArtistDTO>>> PutArtist(int id, [Required][FromBody] ArtistDTO artistDTO)
        {
            if (await ArtistExists(id) == false)
                throw new NotFoundException("Artist with id '" + id + "' does not exist!");

            Artist updateArtist = await _artistRepository.GetByIdAsync(id);
            updateArtist.Avatar = artistDTO.Avatar;
            updateArtist.Profile = artistDTO.Profile;
            updateArtist.Name = artistDTO.Name;
            await _artistRepository.UpdateAsync(updateArtist);

            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Update artist successfully!",
                HttpStatusCode.OK,
                _mapper.Map<ArtistDTO>(updateArtist)));
        }

        /// <summary>
        /// Create a new Artist.
        /// </summary>
        /// 
        /// <param name="Artist">
        /// Artist object that needs to be created.
        /// </param>
        /// 
        /// <returns>A new Artist.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a new Artist.
        /// - Sample request: 
        /// 
        ///       POST /api/artist
        ///     
        ///       {
        ///             "name": "tlinh",
        ///             "profile": "Profile ne",
        ///             "avatar": "Avatar ne"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="201">Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<ArtistDTO>), 201)]
        [Produces("application/json")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO<ArtistDTO>>> PostArtist([Required][FromBody] ArtistDTO artistDTO)
        {
            if (await ArtistExists(artistDTO.Name) == true)
                throw new BadRequestException("Artist with name '" + artistDTO.Name + "' is existed!");

            await _artistRepository.AddAsync(_mapper.Map<Artist>(artistDTO));
            Artist createArtist = await _artistRepository.GetByArtistName(artistDTO.Name);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create artist successfully!",
                HttpStatusCode.Created,
                _mapper.Map<ArtistDTO>(createArtist)));
        }

        /// <summary>
        /// Delete a specific Artist.
        /// </summary>
        /// 
        /// <param name="id">
        /// Artist's id which is needed for deleting a Artist.
        /// </param>
        /// 
        /// <returns>Delete action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return delete action status.
        /// - Sample request: 
        /// 
        ///       DELETE /api/artists/1
        ///       
        /// </remarks>
        /// 
        /// <response code="204">Delete Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">Artist not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseDTO<>), 200)]
        public async Task<ActionResult<ResponseDTO<String>>> DeleteArtist(int id)
        {
            if (await ArtistExists(id) == false)
                throw new BadRequestException("Artist with id '" + id + "' is not existed!");

            Artist artist = await _artistRepository.GetByIdAsync(id);
            await _artistRepository.DeleteAsync(artist);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create artist successfully!",
                HttpStatusCode.OK,
                ""
                ));
        }

        /// <summary>
        /// Count Artist.
        /// </summary>
        /// 
        /// <returns>Quantity of Artist by String.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return quantity of Artist by String.
        /// - Sample request: 
        /// 
        ///       GET /api/artists/count
        ///       
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthenticated</response>
        /// <response code="403">Unauthorized</response>
        /// <response code="404">Artist not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("count")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(ResponseDTO<int>), 200)]
        public async Task<ActionResult<ResponseDTO<int>>> CountArtists()
        {
            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Count Artists successfully!",
                HttpStatusCode.OK,
                await _artistRepository.CountAsync()
                ));
        }

        private async Task<bool> ArtistExists(int id)
        {
            if (await _artistRepository.GetByIdAsync(id) == null)
                return false;
            return true;
        }

        private async Task<bool> ArtistExists(string name)
        {
            if (await _artistRepository.GetByArtistName(name) == null)
                return false;
            return true;
        }
    }
}