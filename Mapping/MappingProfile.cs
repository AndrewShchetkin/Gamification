using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Gamification.Models;
using Gamification.Models.DTO.Map;
using Gamification.Models.DTO.Team;

namespace Gamification.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UsersInTeam>();
            CreateMap<Team, TeamDto>();
            CreateMap<Map, MapDto>();
        }
    }
}
