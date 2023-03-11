using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazingTrails.Shared.Features.ManageTrails.Shared;


public record UploadTrailImageRequest(int TrailId, IBrowserFile File) : IRequest<UploadTrailImageRequest.Response>
{
    public const string RouteTemplate = "/api/trails/{trailId}/images";

    [UsedImplicitly]
    //yay!, green underline is gone! 
    public record Response(string ImageName);

}
