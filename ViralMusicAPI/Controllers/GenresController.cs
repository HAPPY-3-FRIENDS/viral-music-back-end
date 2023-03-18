using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Repositories.IRepositories;
using BusinessObjects.DataTranferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using ViralMusicAPI.Handler;
using ViralMusicAPI.Exceptions;
using System.ComponentModel.DataAnnotations;
using BusinessObjects.Exceptions;
using System;
using AutoMapper;
using System.Linq.Expressions;

namespace ViralMusicAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenresController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a list of all genres.
        /// </summary>
        /// 
        /// <returns>A list of all genres.</returns>
        /// <remarks>
        /// Description:
        /// - Return a list of all genres.
        /// - Sample request: 
        /// 
        ///       GET /api/genres
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">List of genre Not Found</response>
        /// <response code="500">Internal Server Error</response>
        [ProducesResponseType(typeof(ResponseDTO<List<GenreDTO>>), StatusCodes.Status200OK)]
        [Produces("application/json")]
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin,User")]
        public async Task<ActionResult<ResponseDTO<List<GenreDTO>>>> GetGenres()
        {
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find genres successfully!",
                HttpStatusCode.OK,
                _mapper.Map<IEnumerable<GenreDTO>>(await _genreRepository.GetAllAsync())));
        }

        /// <summary>
        /// Get a specific Genre by id.
        /// </summary>
        /// 
        /// <param name="id">
        /// Genre's id which is needed for finding.
        /// </param>
        /// 
        /// <returns>A specific Genre by id.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a specific Genre by id.
        /// - Sample request: 
        /// 
        ///       GET /api/genres/1
        /// 
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Genre not found</response>
        /// <response code="500">Internal server error</response>
        [ProducesResponseType(typeof(ResponseDTO<GenreDTO>), 200)]
        [Produces("application/json")]
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO<GenreDTO>>> GetGenre(int id)
        {
            Genre genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null) throw new NotFoundException("Genre is Not Found with id: " + id);
            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Find genre successfully!",
                HttpStatusCode.OK,
                _mapper.Map<GenreDTO>(genre)));
        }

        /// <summary>
        /// Update an existing Genre.
        /// </summary>
        /// 
        /// <param name="id">
        /// Genre's id which is needed for updating.
        /// </param>
        /// 
        /// <param name="Genre">
        /// Genre object that needs to be updated.
        /// </param>
        /// 
        /// <returns>An update existing Genre.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return an update existing genre.
        /// - Sample request: 
        /// 
        ///       PUT /api/genres/1
        ///     
        /// - Sample request body: 
        ///     
        ///       {
        ///             "name": "Pop"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Genre not found</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<GenreDTO>), 200)]
        [Produces("application/json")]
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO<GenreDTO>>> PutGenre(int id, [Required][FromBody] GenreDTO genreDTO)
        {
            if (await GenreExists(id) == false)
                throw new NotFoundException("Genre with id '" + id + "' does not exist!");

            Genre updateGenre = await _genreRepository.GetByIdAsync(id);
            updateGenre.Name = genreDTO.Name;
            await _genreRepository.UpdateAsync(updateGenre);

            return StatusCode((int)HttpStatusCode.OK, ResponseBuilderHandler.generateResponse(
                "Update genre successfully!",
                HttpStatusCode.OK,
                _mapper.Map<GenreDTO>(updateGenre)));
        }

        /// <summary>
        /// Create a new Genre.
        /// </summary>
        /// 
        /// <param name="genre">
        /// Genre object that needs to be created.
        /// </param>
        /// 
        /// <returns>A new Genre.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return a new Genre.
        /// - Sample request: 
        /// 
        ///       POST /api/genres
        ///     
        ///       {
        ///             "name": "C-Pop"
        ///       }
        ///     
        /// </remarks>
        /// 
        /// <response code="201">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal server error</response>
        [Consumes("application/json")]
        [ProducesResponseType(typeof(ResponseDTO<GenreDTO>), 201)]
        [Produces("application/json")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        public async Task<ActionResult<ResponseDTO<GenreDTO>>> PostGenre([Required][FromBody] GenreDTO genreDTO)
        {
            if (await GenreExists(genreDTO.Name) == true)
                throw new BadRequestException("Genre with name '" + genreDTO.Name + "' is existed!");

            await _genreRepository.AddAsync(_mapper.Map<Genre>(genreDTO));
            Genre createGenre = await _genreRepository.GetByGenreName(genreDTO.Name);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create genre successfully!",
                HttpStatusCode.Created,
                _mapper.Map<GenreDTO>(createGenre)));
        }

        /// <summary>
        /// Delete a specific Genre.
        /// </summary>
        /// 
        /// <param name="id">
        /// Genre's id which is needed for deleting a Genre.
        /// </param>
        /// 
        /// <returns>Delete action status.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return delete action status.
        /// - Sample request: 
        /// 
        ///       DELETE /api/genres/1
        ///       
        /// </remarks>
        /// 
        /// <response code="204">Delete Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Genre not found</response>
        /// <response code="500">Internal server error</response>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseDTO<>), 200)]
        public async Task<ActionResult<ResponseDTO<String>>> DeleteGenre(int id)
        {
            if (await GenreExists(id) == false)
                throw new BadRequestException("Genre with id '" + id + "' is not existed!");
            
            Genre genre = await _genreRepository.GetByIdAsync(id);
            await _genreRepository.DeleteAsync(genre);

            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Create genre successfully!",
                HttpStatusCode.OK,
                ""
                ));
        }

        /// <summary>
        /// Count Genre.
        /// </summary>
        /// 
        /// <returns>Quantity of Genres by String.</returns>
        /// 
        /// <remarks>
        /// Description: 
        /// - Return quantity of genres by String.
        /// - Sample request: 
        /// 
        ///       GET /api/genres/count
        ///       
        /// </remarks>
        /// 
        /// <response code="200">Successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Genre not found</response>
        /// <response code="500">Internal server error</response>
        [HttpGet("count")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
        [ProducesResponseType(typeof(ResponseDTO<int>), 200)]
        public async Task<ActionResult<ResponseDTO<int>>> CountGenres()
        {
            return StatusCode((int)HttpStatusCode.Created, ResponseBuilderHandler.generateResponse(
                "Count Genre successfully!",
                HttpStatusCode.OK,
                await _genreRepository.CountAsync()
                ));
        }

        private async Task<bool> GenreExists(int id)
        {
            if (await _genreRepository.GetByIdAsync(id) == null)
                return false;
            return true;
        }

        private async Task<bool> GenreExists(string genreName)
        {
            if (await _genreRepository.GetByGenreName(genreName) == null)
                return false;
            return true;
        }
    }
}