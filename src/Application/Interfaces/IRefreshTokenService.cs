using Domain.Common.Entities;

namespace Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task AddRefreshTokenAsync<T>(RefreshToken token);
    }
}
