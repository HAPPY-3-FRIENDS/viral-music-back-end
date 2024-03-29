﻿using AutoMapper;
using BusinessObjects.DataTranferObjects;
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
    public class TrackRepository : GenericRepository<Track>, ITrackRepository
    {
        public async Task<Track> GetByTrackDateTime(DateTime trackDateTime)
            => await TrackDAO.Instance.GetByTrackDateTime(trackDateTime);

        public async Task<IEnumerable<TrackGetByGenreDTO>> GetTracksListByName(string name, IMapper mapper)
            => await TrackDAO.Instance.GetTracksListByName(name, mapper);
    }
}
