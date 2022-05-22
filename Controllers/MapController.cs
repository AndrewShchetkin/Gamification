using Gamification.Data.Interfaces;
using Gamification.Hubs;
using Gamification.Hubs.Clients;
using Gamification.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;

namespace Gamification.Controllers
{
    [Route("api/map")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapRepository _mapRepository;
        private readonly IHubContext<MapHub, IMapClient> _mapHub;
        
        public MapController(IMapRepository mapRepository, IHubContext<MapHub, IMapClient> mapHub)
        {            
            _mapRepository = mapRepository;
            _mapHub = mapHub;
        }

        // POST: MapController/Create
        [HttpPost]
        [Route("save-map")]        
        public async Task<ActionResult> SaveMapData([FromBody] Map cells, CancellationToken token)
        {            
            await _mapRepository.SaveMapAsync(cells, token);

            return Ok();
        }

        [HttpGet]
        [Route("load-map")]
        public async Task<ActionResult<Map>> LoadMapData(CancellationToken token)
        {
            var map = await _mapRepository.LoadMapAsync(token);            

            return Ok(map);
        }

        [HttpPut]
        [Route("update-cell")]
        public async Task<ActionResult> UpdateCell([FromBody] Cell cell, CancellationToken token)
        {
            if (await _mapRepository.UpdateCell(cell, token))
            {
                await _mapHub.Clients.All.UpdateCell(cell);
            };

            return Ok();
        }


    }
}
