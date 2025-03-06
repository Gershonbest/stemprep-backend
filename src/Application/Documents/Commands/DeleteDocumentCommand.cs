using Application.Common.Models;
using MediatR;
using Application.Interfaces;
using Application.Auth;
using System.Text.Json.Serialization;

namespace Application.Documents.Commands
{
    public class DeleteDocumentCommand : IRequest<Result>
    {
        public Guid DocumentGuid { get; set; }

        [JsonIgnore]
        public Guid UserGuid { get; set; }
    }

    public class DeleteDocumentCommandHandler(IApplicationDbContext context, ICloudinaryService cloudinaryService) : IRequestHandler<DeleteDocumentCommand, Result>
    {
        public async Task<Result> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            var user = await new AuthHelper(context).GetUserByGuid(request.UserGuid);
            if (user == null)
            {
                return Result.Failure("User does not exist");
            }

            var document = context.Documents.Where(x => x.Guid == request.DocumentGuid && x.UserGuid == request.UserGuid).FirstOrDefault();

            if (document == null)
            {
                return Result.Failure("Document not found");
            }

            context.Documents.Remove(document);
            var result = await cloudinaryService.DeleteFileAsync(document.CloudinaryUrl);

            if (result.Succeeded)
            {
                await context.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }

}

