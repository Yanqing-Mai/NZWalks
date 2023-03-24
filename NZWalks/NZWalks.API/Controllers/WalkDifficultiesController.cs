using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper; 
        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository; 
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficulties()
        {
           var walkDifficulity = await walkDifficultyRepository.GetAllAsync();
            var walkDifficulityDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulity);
            return Ok(walkDifficulityDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if(walkDifficulty == null)
            {
                return NotFound();
            }
            //Covert domain to DTO
            var walkDifficulityDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficulityDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDiddiculty(AddWalkDifficultyRequest request)
        {
            var walkDifficulityDomain = new Models.Domain.WalkDifficulty
            {
                Code = request.Code
            };

            walkDifficulityDomain = await walkDifficultyRepository.AddWalkDifficultyasync(walkDifficulityDomain);

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulityDomain);

            return CreatedAtAction(nameof(GetWalkDifficultyById), 
                new {id = walkDifficultyDTO.Id}, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficulity(Guid id, UpdateWalkDifficulityRequest request)
        {
            var walkDifficulityDomain = new Models.Domain.WalkDifficulty
            {
                Code = request.Code
            };

            await walkDifficultyRepository.UpdateWalkDifficulty(id, walkDifficulityDomain);

            if(walkDifficulityDomain == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulityDomain);

            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficulity(Guid id)
        {
            var walkDifficulityDomain = await walkDifficultyRepository.DeleteAsync(id);

            if(walkDifficulityDomain == null)
            {
                return NotFound(); 
            }

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulityDomain);

            return Ok(walkDifficultyDTO); 
        }
    }
}
