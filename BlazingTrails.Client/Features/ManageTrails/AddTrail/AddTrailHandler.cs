using System.Net.Http.Json;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using JetBrains.Annotations;
using MediatR;

namespace BlazingTrails.Client.Features.ManageTrails.AddTrail;

[UsedImplicitly]
public class AddTrailHandler : IRequestHandler<AddTrailRequest, AddTrailRequest.Response>
{
    private readonly HttpClient _httpClient;

    public AddTrailHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AddTrailRequest.Response> Handle(AddTrailRequest request, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(AddTrailRequest.RouteTemplate, request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var trailId = await response.Content.ReadFromJsonAsync<int>(cancellationToken: cancellationToken);
            return new AddTrailRequest.Response(trailId);
        }
        else
        {
            //why negative 1?
            return new AddTrailRequest.Response(-1);
        }
    }
}
