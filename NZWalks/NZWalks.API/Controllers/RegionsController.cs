using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("Regions")]  //this is the end point for region controller
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
       public async Task<IActionResult> GetAllRegions()
        {
            var regions = await regionRepository.GetAllAsync();

            //return DTO region
            /*var regionsDTO = new List<Models.DTO.Region>();
             regions.ToList().ForEach(region =>
             {
                 var regionDTO = new Models.DTO.Region()
                  {
                      Id = region.Id,
                      Code = region.Code,
                      Name = region.Name,
                      Area = region.Area,
                      Lat = region.Lat,
                      Long = region.Long,
                      Population = region.Population,
                  };
                  regionsDTO.Add(regionDTO);
              }); */
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")] //restrict only taking in guid
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);

            if(region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest request) 
        {
            //request to domain model
            var region = new Models.Domain.Region()
            {
                Code = request.Code,
                Area = request.Area,
                Lat = request.Lat,
                Long = request.Long,
                Name = request.Name,
                Population = request.Population
            };
            //pass details to repository
            region = await regionRepository.AddAsync(region);

            //convert back to dto
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetAllRegions), new { id = regionDTO.Id }, regionDTO);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //get the region from DB
            var region = await regionRepository.DeleteAsync(id);
            //if null Notfounf
            if(region == null)
            {
                return NotFound();
            }
            //convert response back to DTO
            var response = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            //return ok response
            return Ok(response);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateRegionRequest request)
        {
            //convert DTO to domain model
            var region = new Models.Domain.Region
            {
                Code = request.Code,
                Area = request.Area,
                Lat = request.Lat,
                Long = request.Long,
                Name = request.Name,
                Population = request.Population
            };
            //update region using repository
            region = await regionRepository.UpdateAsync(id, region);
            //if null then notfound
            if(region == null)
            {
                return NotFound();
            }
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            //return ok reponse
            return Ok(regionDTO);
        }

    }

 
}
