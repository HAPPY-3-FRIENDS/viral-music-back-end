using BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IPlaylistRepository : IGenericRepository<Playlist>
    {
        Task<List<Playlist>> GetListByUsername(string username);
        Task<Playlist> GetByUsernameByName(string username, string name);
        Task<List<Playlist>> GetListByUsernameByName(string username, string name);
    }
}
