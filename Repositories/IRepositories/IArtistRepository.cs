using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IArtistRepository : IGenericRepository<Artist>
    {
        Task<Artist> GetByArtistName(string artistName);
        Task<IEnumerable<Artist>> GetListByArtistName(string artistName);
    }
}
