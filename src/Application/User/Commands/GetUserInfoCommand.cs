using Application.Auth;
using Application.Common.Models;
using Application.Dto;
using Application.Interfaces;
using AutoMapper;
using MediatR;
using System.Text.Json.Serialization;

namespace Application.Users.Commands
{
    public class GetUserInfoCommand : IRequest<Result>
    {
        [JsonIgnore]
        public Guid UserGuid { get; set; }
    }

    public class GetUserInfoCommandHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetUserInfoCommand, Result>
    {
        public async Task<Result> Handle(GetUserInfoCommand request, CancellationToken cancellationToken)
        {
            var user = await new AuthHelper(context).GetUserByGuid(request.UserGuid);

            if (user == null)
                return Result.Failure("User not found");

            var userDto = mapper.Map<UserDto>(user);

            return Result.Success(userDto);
        }
    }
}
