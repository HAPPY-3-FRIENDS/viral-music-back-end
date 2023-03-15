using BusinessObjects.Models;
using DataAccessObjects;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repositories
{
    public class ArtistRepository : GenericRepository<Artist>, IArtistRepository
    {
        public Task<Artist> GetByArtistName(string artistName)
            => ArtistDAO.Instance.GetByArtistName(artistName);

        public Task<IEnumerable<Artist>> GetListByArtistName(string artistName)
            => ArtistDAO.Instance.GetListByArtistName(artistName);
    }
}
