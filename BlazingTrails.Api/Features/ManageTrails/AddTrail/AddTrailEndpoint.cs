using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using Microsoft.AspNetCore.Mvc;

namespace BlazingTrails.Api.Features.ManageTrails.AddTrail;

public class AddTrailEndpoint : EndpointBaseAsync.WithRequest<AddTrailRequest>.WithActionResult<int>
{
    private readonly BlazingTrailsContext _database;

    public AddTrailEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [HttpPost(AddTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<int>> HandleAsync(AddTrailRequest request, CancellationToken cancellationToken = default)
    {
        var trail = new Trail
        {
            Name = request.Trail.Name,
            Description = request.Trail.Description,
            Location = request.Trail.Location,
            TimeInMinutes = request.Trail.TimeInMinutes,
            Length = request.Trail.Length
        };

        await _database.Trails.AddAsync(trail, cancellationToken);

        var routeInstructions = request.Trail.Waypoints.Select(x => new TrailDto.WaypointDto(x.Latitude, x.Longitude));

        await _database.AddRangeAsync(routeInstructions, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);

        return Ok(trail.Id);
    }
}
