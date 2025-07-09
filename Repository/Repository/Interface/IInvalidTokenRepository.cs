using Repository.Models.Entities;

namespace Repository.Repository.Interface
{
    public interface IInvalidTokenRepository
    {
        Task<InvalidatedToken> GetByIdAsync(string id);
        Task CreateAsync(InvalidatedToken token);
    }
}
