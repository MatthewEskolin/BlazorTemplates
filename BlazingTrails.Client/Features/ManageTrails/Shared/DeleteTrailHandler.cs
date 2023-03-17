using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;
using System.Net.Http.Json;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using JetBrains.Annotations;

namespace BlazingTrails.Client.Features.ManageTrails.Shared;

[UsedImplicitly]
public class DeleteTrailHandler: IRequestHandler<DeleteTrailRequest, DeleteTrailRequest.Response?>
{
    private readonly HttpClient _httpClient;

    public DeleteTrailHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DeleteTrailRequest.Response?> Handle(DeleteTrailRequest request, CancellationToken cancellationToken)
    {
        try
        {

            var response = await _httpClient.DeleteAsync(DeleteTrailRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString()), cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return new DeleteTrailRequest.Response(true);
            }
            else
            {
                return new DeleteTrailRequest.Response(false);
            }

        }
        catch (HttpRequestException)
        {
            return default!;
        }
    }
}

