using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using BlazingTrails.Shared.Features.ManageTrails;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace BlazingTrails.Api.Features.ManageTrails.Shared;

public class UploadTrailImageEndpoint : EndpointBaseAsync.WithRequest<int>.WithActionResult<string>
{
    private readonly BlazingTrailsContext _database;

    public UploadTrailImageEndpoint(BlazingTrailsContext database)
    {
        _database = database;
    }

    [HttpPost(UploadTrailImageRequest.RouteTemplate)]
    public override async Task<ActionResult<string>> HandleAsync([FromRoute] int trailId, CancellationToken cancellationToken = default)
    {
        var trail = await _database.Trails.SingleOrDefaultAsync(x => x.Id == trailId, cancellationToken);
        if (trail is null)
        {
            return BadRequest("Trail does not exist.");
        }

        var file = Request.Form.Files[0];
        if (file.Length == 0)
        {
            return BadRequest("No image found.");
        }

        var filename = $"{Guid.NewGuid()}.jpg";
        var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), "Images", filename);

        var resizeOptions = new ResizeOptions
        {
            Mode = ResizeMode.Pad,
            Size = new Size(640, 426)
        };

        using var image = await Image.LoadAsync(file.OpenReadStream(), cancellationToken);
        image.Mutate(x => x.Resize(resizeOptions));
        await image.SaveAsJpegAsync(saveLocation, cancellationToken: cancellationToken);

        if (!string.IsNullOrWhiteSpace(trail.Image))
        {
            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", trail.Image));
        }

        trail.Image = filename;
        await _database.SaveChangesAsync(cancellationToken);

        return Ok(trail.Image);
    }
}



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
        var trail = await _database.Trails.Include(x => x.Route).SingleOrDefaultAsync(x => x.Id == request.Trail.Id, cancellationToken: cancellationToken);

        if (trail is null)
        {
            return BadRequest("Trail could not be found.");
        }

        trail.Name = request.Trail.Name;
        trail.Description = request.Trail.Description;
        trail.Location = request.Trail.Location;
        trail.TimeInMinutes = request.Trail.TimeInMinutes;
        trail.Length = request.Trail.Length;
        trail.Route = request.Trail.Route.Select(ri => new RouteInstruction
        {
            Stage = ri.Stage,
            Description = ri.Description,
            Trail = trail
        }).ToList();

        if (request.Trail.ImageAction == ImageAction.Remove)
        {
            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "Images", trail.Image!));
            trail.Image = null;
        }

        await _database.SaveChangesAsync(cancellationToken);

        return Ok(true);
    }
}
