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
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public async Task<Genre> GetByGenreName(string genreName)
            => await GenreDAO.Instance.GetByGenreName(genreName);
    }
}
