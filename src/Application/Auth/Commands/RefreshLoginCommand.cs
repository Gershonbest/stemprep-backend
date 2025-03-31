//using Application.Common.Helpers;
//using Application.Common.Models;
//using Application.Common;
//using Application.Interfaces;
//using Domain.Common.Entities;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using StackExchange.Redis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Application.Common;
//using Application.Common.Helpers;
//using Application.Common.Models;
//using Application.Interfaces;
//using MediatR;
//using StackExchange.Redis;
//using Microsoft.AspNetCore.Http;
//using Domain.Common.Entities;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Cryptography;


//namespace Application.Auth.Commands
//{
    
//    public class RefreshLoginCommand<TUser> : IRequest<Result>
//        where TUser : BaseUser
//    {
//        public string RefreshToken { get; set; }
//    }

//    public class RefreshLoginCommandHandler<TUser>(IApplicationDbContext context,
//                                      ITokenGenerator generateToken,
//                                      IEmailService emailService,
//                                      IConnectionMultiplexer redis,
//                                      IHttpContextAccessor httpContextAccessor,
//                                      ISecretHasherService secretHasherService,
//                                      IRefreshTokenService refreshTokenService) : IRequestHandler<RefreshLoginCommand<TUser>, Result>
//        where TUser : BaseUser
//    {
//        private readonly IDatabase _redisDb = redis.GetDatabase();

//        public async Task<Result> Handle(RefreshLoginCommand<TUser> request, CancellationToken cancellationToken)
//        {
//            TUser user = await new AuthHelper(context).GetUserByEmail<TUser>(request.Email);

//            if (user == null)
//            {
//                return Result.Failure<RefreshLoginCommand<TUser>>("Invalid Email or Password");
//            }

//            var userType = await new AuthHelper(context).GetUserTypeByEmail(request.Email);

//            if (userType != typeof(TUser))
//            {
//                return Result.Failure<RefreshLoginCommand<TUser>>($"User is registered as a {userType.Name}");
//            }

//            if (string.IsNullOrEmpty(refreshRequest.RefreshToken))
//            {
//                return BadRequest("Refresh token is required.");
//            }

//            // Retrieve the refresh token from the database including its associated user.
//            var tokenEntity = await _dbContext.RefreshTokens
//                .Include(rt => rt.BaseUser)
//                .SingleOrDefaultAsync(rt => rt.Token == refreshRequest.RefreshToken);

//            if (tokenEntity == null)
//            {
//                return Unauthorized("Invalid refresh token.");
//            }

//            // Validate that the token is still valid.
//            if (tokenEntity.Expires < DateTime.UtcNow || tokenEntity.IsRevoked)
//            {
//                return Unauthorized("Refresh token is expired or revoked.");
//            }

//            // Revoke the old token to prevent reuse.
//            tokenEntity.IsRevoked = true;
//            tokenEntity.RevokedAt = DateTime.UtcNow;

//            // Generate a new access token. (Replace this with your own token generation logic.)
//            var newAccessToken = GenerateAccessToken(tokenEntity.BaseUser);

//            // Generate a new refresh token.
//            var newRefreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
//            var newRefreshToken = new RefreshToken
//            {
//                Token = newRefreshTokenValue,
//                Expires = DateTime.UtcNow.AddDays(7), // For example, valid for 7 days.
//                CreatedAt = DateTime.UtcNow,
//                IsRevoked = false,
//                UserId = tokenEntity.BaseUserId
//            };

//            // Add the new refresh token and update the database.
//            _dbContext.RefreshTokens.Add(newRefreshToken);
//            await _dbContext.SaveChangesAsync();

//            // Return the new tokens.
//            return Ok(new
//            {
//                AccessToken = newAccessToken,
//                RefreshToken = newRefreshTokenValue
//            });
//            var tokens = generateToken.GenerateTokens(user.FirstName, user.Email!, user.UserType.ToString(), user.Guid);

//            CookieHelper.SetTokensInCookies(httpContextAccessor, tokens.AccessToken, tokens.RefreshToken);

//            await refreshTokenService.AddRefreshTokenAsync<TUser>(new RefreshToken
//            {
//                Token = tokens.RefreshToken,
//                Expires = DateTime.Now.AddDays(30)
//            });

//            return Result.Success<RefreshLoginCommand<TUser>>("Successfully logged in", tokens);
//        }
//    }

//}
