using FluentValidation;
using MediatR;

namespace BlazingTrails.Shared.Features.ManageTrails.AddTrail;

public record DeleteTrailRequest(int TrailId) : IRequest<DeleteTrailRequest.Response>
{
    public const string RouteTemplate = "/api/trails/{trailId}";

    public record Response(bool Success);
}

