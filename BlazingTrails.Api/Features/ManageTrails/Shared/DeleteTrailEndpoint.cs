using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.Home.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Features.ManageTrails.Shared;

public class DeleteTrailEndpoint : EndpointBaseAsync.WithRequest<DeleteTrailRequest>.WithActionResult<bool>
{
    private readonly BlazingTrailsContext _database;

    public DeleteTrailEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [HttpDelete(DeleteTrailRequest.RouteTemplate)]
    public override async Task<ActionResult<bool>> HandleAsync(DeleteTrailRequest request, CancellationToken cancellationToken = default)
    {
        var trail = await _database.Trails.Include(x => x.Route).SingleOrDefaultAsync(x => x.Id == request.DeleteTrail.TrailId, cancellationToken: cancellationToken);

        if (trail is null)
        {
            return BadRequest("Trail could not be found.");
        }

        //Delete Routes, then delete Trail
        _database.RouteInstructions.RemoveRange(trail.Route);
        _database.Trails.Remove(trail);

        //remove image
        if (trail.Image != null)
        {
            //delete it
            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", trail.Image!));
        }

        await _database.SaveChangesAsync(cancellationToken);

        return Ok(true);
    }
}