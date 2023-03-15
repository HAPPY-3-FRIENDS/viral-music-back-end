using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class PlaylistDAO : GenericDAO<Playlist>
    {
        private static PlaylistDAO instance = null;
        private static readonly object instanceLock = new object();

        public static PlaylistDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new PlaylistDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Playlist>> GetListByUsername(string username)
        {
            List<Playlist> list = new List<Playlist>();
            try
            {
                var dbContext = new ViralMusicContext();
                User user = await UserDAO.Instance.GetByUsername(username);
                if (user == null)
                    return null;

                if (user.RoleId == 2)
                {
                    list = await dbContext.Set<Playlist>().Where(u => u.Owner.Equals(username) || u.Owner.Equals("admin")).ToListAsync();
                } else
                {
                    list = await dbContext.Set<Playlist>().Where(u => u.Owner.Equals(username)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public async Task<Playlist> GetByUsernameByName(string username, string name)
        {
            Playlist playlist = new Playlist();
            try
            {
                var dbContext = new ViralMusicContext();
                playlist = await dbContext.Playlists.FirstOrDefaultAsync(u => u.Name == name);
                
                if (playlist == null) 
                    return null;

                if (playlist.Owner != "admin" && playlist.Owner != username)
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return playlist;
        }

        public async Task<List<Playlist>> GetListByUsernameByName(string username, string name)
        {
            List<Playlist> list = new List<Playlist>();
            try
            {
                var dbContext = new ViralMusicContext();
                User user = await UserDAO.Instance.GetByUsername(username);
                if (user == null)
                    return null;

                if (user.RoleId == 2)
                {
                    list = await dbContext.Set<Playlist>().Where(u => ((u.Owner.Equals(username) || u.Owner.Equals("admin")) && u.Name.Contains(name))).ToListAsync();
                }
                else
                {
                    list = await dbContext.Set<Playlist>().Where(u => u.Owner.Equals(username) && u.Name.Contains(name)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
    }
}
