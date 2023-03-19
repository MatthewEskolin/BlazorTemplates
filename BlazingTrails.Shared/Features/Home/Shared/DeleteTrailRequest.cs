using MediatR;

namespace BlazingTrails.Shared.Features.Home.Shared;

public record DeleteTrailRequest(DeleteTrailDto DeleteTrail) : IRequest<DeleteTrailRequest.Response>
{

    public const string RouteTemplate = "/api/trails/{trailId}";

    public record Response(bool Success);
}

public record DeleteTrailDto(int TrailId);
