using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;
using System.Net.Http.Json;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;
using JetBrains.Annotations;
using System.Net.Http;
using BlazingTrails.Shared.Features.Home.Shared;

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
            var route = DeleteTrailRequest.RouteTemplate.Replace("{trailId}", request.DeleteTrail.TrailId.ToString());

            HttpRequestMessage httpRequest = new HttpRequestMessage
            {
                Content = JsonContent.Create(request),
                Method = HttpMethod.Delete,
                RequestUri = new Uri(route, UriKind.Relative)
            };

            var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

            return response.IsSuccessStatusCode ? 
                new DeleteTrailRequest.Response(true) : 
                new DeleteTrailRequest.Response(false);
        }
        catch (HttpRequestException) 
        {
            return default!;
        }
    }
}

