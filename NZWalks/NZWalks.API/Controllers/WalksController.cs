﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("Walks")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walk = await walkRepository.GetAlllWalks();

            var walkDTO = mapper.Map<List<Models.DTO.Walk>>(walk);

            return Ok(walkDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetAllWalksAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //get walk domain object from DB
            var walk = await walkRepository.GetWalkAsync(id);

            //convert domain object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);


            return Ok(walkDTO);
        }


        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest request)
        {
            //Validate the imcoming request
            if(!(await ValidateAddWalkAsync(request)))
            {
                return BadRequest(ModelState);
            }

            //convert dto to domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = request.Length,
                Name = request.Name,
                RegionId = request.RegionId,
                WalkDifficultyId = request.WalkDifficultyId
            };

            walkDomain = await walkRepository.AddWalk(walkDomain);

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return CreatedAtAction(nameof(GetAllWalksAsync), new {id = walkDTO.Id}, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> UpdatewWalkAsync([FromRoute]Guid id, [FromBody]Models.DTO.UpdateWalkRequest request)
        {
            //Convert DTO to domain object
            var walkDomain = new Models.Domain.Walk
            {
                Length = request.Length,
                Name = request.Name,
                RegionId = request.RegionId,
                WalkDifficultyId = request.WalkDifficultyId
            };

            //Pass details to repository
           walkDomain = await walkRepository.UpdateWalk(id, walkDomain);
            //Handle null
            if(walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };
            //return response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]

        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
           var walkDomain = await walkRepository.DeleteWalk(id);
            if(walkDomain == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }


        #region Private Method
        private async Task<bool> ValidateAddWalkAsync(AddWalkRequest request)
        {
           /* if(request == null)
            {
                ModelState.AddModelError(nameof(request),
                    $"{nameof(request)} cannot be less than or equal to zero");
                return false; 
            }
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                ModelState.AddModelError(nameof(request.Name),
                    $"{nameof(request.Name)} is required");
            } */ 

            var region = await regionRepository.GetAsync(request.RegionId);
            if(region == null)
            {
                ModelState.AddModelError(nameof(request.RegionId),
                    $"{nameof(request.RegionId)} is invalid");
            }
            var walkDifficulity = await walkDifficultyRepository.GetAsync(request.WalkDifficultyId);
            if(walkDifficulity == null)
            {
                ModelState.AddModelError(nameof(request.WalkDifficultyId),
                    $"{nameof(request.WalkDifficultyId)} is invalid");
            }

            if(ModelState.ErrorCount > 0)
            {
                return false; 
            }

            return true; 
        }
        #endregion
    }
}
