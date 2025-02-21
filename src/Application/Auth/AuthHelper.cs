using Application.Interfaces;
using Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Application.Auth
{
    internal class AuthHelper(IApplicationDbContext context)
    {
        public async Task<bool> CheckIfUserExists(string email)
        {
            bool userExists = await context.Parents.AnyAsync(p => p.Email == email) ||
                              await context.Tutors.AnyAsync(t => t.Email == email) ||
                              await context.Admins.AnyAsync(a => a.Email == email);
            return userExists;
        }
        public async Task<bool> CheckIfChildExists(string username, Guid parentGuid)
        {
            bool childExists = await context.Parents
                                    .Include(x=>x.Children)
                                    .AnyAsync(p => p.Guid == parentGuid && p.Children.Any(x=> x.Username == username));

            return childExists;
        }
        public async Task<BaseUser> GetUserByEmail(string email)
        {
            BaseUser user = await context.Parents.FirstOrDefaultAsync(p => p.Email == email) as BaseUser ??
                             await context.Tutors.FirstOrDefaultAsync(t => t.Email == email) as BaseUser ??
                             await context.Students.FirstOrDefaultAsync(s => s.Email == email) as BaseUser;
            return user;
        }
        public async Task<BaseUser> GetUserByGuid(Guid userGuid)
        {
            BaseUser user = await context.Parents.FirstOrDefaultAsync(p => p.Guid == userGuid) as BaseUser ??
                             await context.Tutors.FirstOrDefaultAsync(t => t.Guid == userGuid) as BaseUser ??
                             await context.Students.FirstOrDefaultAsync(s => s.Guid == userGuid) as BaseUser;
            return user;
        }
    }
}
