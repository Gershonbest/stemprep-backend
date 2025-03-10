//using Application.Common.Models;
//using Application.Extensions;
//using Domain.Entities;
//using MediatR;

//namespace Application.Auth.Commands
//{
//    public class ResetPasswordCommand : IRequest<Result>
//    {
//        public string Email { get; set; }
//        public string NewPassword { get; set; }
//        public string ConfirmPassword { get; set; }
//    }

//    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
//    {
//        private readonly UserManager<Student> _userManager;
//        private readonly IMediator _mediator;

//        public ResetPasswordCommandHandler(UserManager<Student> userManager, IMediator mediator)
//        {
//            _userManager = userManager;
//            _mediator = mediator;
//        }

//        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
//        {
//            Student? user = await _userManager.FindByEmailAsync(request.Email);
//            if (user == null)
//            {
//                return Result.Failure<ResetPasswordCommand>("Invalid email.");
//            }

//            // Validate password using custom validator
//            await request.ValidateAsync(new PasswordValidator(), cancellationToken);

//            // Hash the new password
//            user.Password = _userManager.PasswordHasher.HashPassword(user, request.NewPassword);

//            // Update user with the new password
//            IdentityResult result = await _userManager.UpdateAsync(user);
//            if (!result.Succeeded)
//            {
//                return Result.Failure("Failed to reset the password.");
//            }

//            // Success response
//            return Result.Success("Password reset successfully.");
//        }
//    }
//}

