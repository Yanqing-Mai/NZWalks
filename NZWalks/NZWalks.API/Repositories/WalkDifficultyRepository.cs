using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext; 
        }

        public async Task<WalkDifficulty> AddWalkDifficultyasync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulity = await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if(existingWalkDifficulity != null)
            {
                nZWalksDbContext.WalkDifficulty.Remove(existingWalkDifficulity);
                await nZWalksDbContext.SaveChangesAsync();
                return existingWalkDifficulity;
            }

            return null; 
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateWalkDifficulty(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulity = await nZWalksDbContext.WalkDifficulty.FindAsync(id);
            if(existingWalkDifficulity == null)
            {
                return null; 
            }
            existingWalkDifficulity.Code = walkDifficulty.Code;
            await nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulity;
        }
    }
}
