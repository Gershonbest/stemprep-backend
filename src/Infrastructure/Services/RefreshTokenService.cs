using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Services
{
    public class RefreshTokenService(ApplicationDbContext context) : IRefreshTokenService
    {
        public async Task AddRefreshTokenAsync<T>(RefreshToken token)
        {
            RefreshToken existingToken = null;
            if (typeof(T) == typeof(Parent))
            {
                existingToken = await context.RefreshTokens.FirstOrDefaultAsync(rf => rf.ParentId == token.ParentId);
            }
            else if (typeof(T) == typeof(Tutor))
            {
                existingToken = await context.RefreshTokens.FirstOrDefaultAsync(rf => rf.TutorId == token.TutorId);
            }
            else if (typeof(T) == typeof(Admin))
            {
                existingToken = await context.RefreshTokens.FirstOrDefaultAsync(rf => rf.AdminId == token.AdminId);
            }

            if(existingToken != null)
            {
                context.RefreshTokens.Remove(existingToken);
            }

            token.CreatedAt = DateTime.UtcNow;
            context.RefreshTokens.Add(token);
            await context.SaveChangesAsync(CancellationToken.None);
        }

    }
}
