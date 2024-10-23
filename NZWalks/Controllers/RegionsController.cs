using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.CustomActionFilters;
using NZWalks.Data;
using NZWalks.Models;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [Route("api/regions")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        // Gets all items from regions database
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var Regions = await regionRepository.GetAllAsync(filterOn, filterQuery, sortBy,isAscending?? true, pageNumber,pageSize);
            var regionsDto = new List<RegionDto>();
            foreach (var region in Regions)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,

                });
            }
            return Ok(regionsDto);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            // essentially search through our databse to find region matching with id 
            var regions = await regionRepository.GetByIdAsync(id);
            // see whetheri it exists or not
            if (regions == null) { return NotFound(); }
            // map  dto to model
            var regionsDto = new RegionDto
            {
                Id = regions.Id,
                Name = regions.Name,
                Code = regions.Code,
                RegionImageUrl = regions.RegionImageUrl,
            };
            return Ok(regionsDto);

        }
        // creating a region so we are just gonna put frombody because we are creating body
        // of region and we are gonna include our dto file in it
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
           

                var RegionModel = new Region
                {
                    Name = addRegionRequestDto.Name,
                    Code = addRegionRequestDto.Code,
                    RegionImageUrl = addRegionRequestDto.RegionImageUrl,
                };

                RegionModel = await regionRepository.CreateAsync(RegionModel);


                var regionModelToDto = new RegionDto
                {
                    Id = RegionModel.Id,
                    Code = RegionModel.Code,
                    Name = RegionModel.Name,
                    RegionImageUrl = RegionModel.RegionImageUrl,

                };

                return CreatedAtAction(nameof(GetRegionById), new { id = RegionModel.Id }, regionModelToDto);
            
            
        }

        [HttpPut("{id:Guid}")]
        [ValidateModel]
        public async  Task<IActionResult> EditRegion(Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            
                var regionDomainModel = new Region { Code = updateRegionDto.Code, Name = updateRegionDto.Name, RegionImageUrl = updateRegionDto.RegionImageUrl };


                regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null) { return NotFound(); }



                var regionModelToDto = new RegionDto
                {
                    Id = regionDomainModel.Id,
                    Code = regionDomainModel.Code,
                    Name = regionDomainModel.Name,
                    RegionImageUrl = regionDomainModel.RegionImageUrl,

                };

                return Ok(regionModelToDto);
          
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion(Guid id) { 
        
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null) { return NotFound(); };
      

            var regionModelToDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,

            };
            return Ok(regionModelToDto);

        }


    }
}