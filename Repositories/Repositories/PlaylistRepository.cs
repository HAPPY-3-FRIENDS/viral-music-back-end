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
    public class PlaylistRepository : GenericRepository<Playlist>, IPlaylistRepository
    {
        public Task<Playlist> GetByUsernameByName(string username, string name)
            => PlaylistDAO.Instance.GetByUsernameByName(username, name);
        public Task<List<Playlist>> GetListByUsername(string username)
            => PlaylistDAO.Instance.GetListByUsername(username);

        public Task<List<Playlist>> GetListByUsernameByName(string username, string name)
            => PlaylistDAO.Instance.GetListByUsernameByName(username, name);
    }
}
