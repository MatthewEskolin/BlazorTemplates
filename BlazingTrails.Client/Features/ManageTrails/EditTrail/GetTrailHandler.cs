using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;
using System.Net.Http.Json;
using JetBrains.Annotations;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail;

[UsedImplicitly]
public class GetTrailHandler : IRequestHandler<GetTrailRequest, GetTrailRequest.Response?>
{
    private readonly HttpClient _httpClient;

    public GetTrailHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetTrailRequest.Response?> Handle(GetTrailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<GetTrailRequest.Response>(GetTrailRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString()));
        }
        catch (HttpRequestException)
        {
            return default!;
        }
    }
}

