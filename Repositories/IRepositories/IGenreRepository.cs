using AutoMapper;
using BusinessObjects.DataTranferObjects;
using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IGenreRepository : IGenericRepository<Genre>
    {
        Task<Genre> GetByGenreName(string genreName);
        Task<IEnumerable<GenreGetAllTrackDTO>> getAllTracksFilterByGenre(IMapper mapper);
    }
}
