using BusinessObjects.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class TrackDAO : GenericDAO<Track>
    {
        private static TrackDAO instance = null;
        private static readonly object instanceLock = new object();

        public static TrackDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TrackDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<Track> GetByTrackDateTime(DateTime dateTime)
        {
            Track track = null;
            try
            {
                var dbContext = new ViralMusicContext();
                /*track = await dbContext.Tracks.LastAsync;*/
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return track;
        }
    }
}
