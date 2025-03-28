using Application.Interfaces;
using Domain.Common.Entities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
        public async Task<bool> CheckIfStudentExists(string username)
        {
            bool childExists = await context.Students
                                    .AnyAsync(p => p.Username == username);

            return childExists;
        }
        public async Task<Student> GetStudentByUsername(string username)
        {
            Student childExists = await context.Students
                                    .FirstOrDefaultAsync(p => p.Username == username);

            return childExists;
        }
        public async Task<BaseUser> GetBaseUserByEmail(string email)
        {
            BaseUser user = await context.Parents.FirstOrDefaultAsync(p => p.Email == email) as BaseUser ??
                             await context.Tutors.FirstOrDefaultAsync(t => t.Email == email) as BaseUser ??
                             await context.Students.FirstOrDefaultAsync(s => s.Email == email) as BaseUser;
            return user;
        }
        public async Task<T> GetUserByEmail<T>(string email) where T : BaseUser
        {
            if (typeof(T) == typeof(Parent))
            {
                return await context.Parents.FirstOrDefaultAsync(p => p.Email == email) as T;
            }
            else if (typeof(T) == typeof(Tutor))
            {
                return await context.Tutors.FirstOrDefaultAsync(t => t.Email == email) as T;
            }
            else if (typeof(T) == typeof(Admin))
            {
                return await context.Admins.FirstOrDefaultAsync(a => a.Email == email) as T;
            }
            else return null;
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
