using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAlllWalks();

        Task<Walk> GetWalkAsync(Guid id);

        Task<Walk> AddWalk(Walk walk);

        Task<Walk> UpdateWalk(Guid id, Walk walk);

        Task<Walk> DeleteWalk(Guid id);
    }
}
