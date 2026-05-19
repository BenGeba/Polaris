using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Polaris.Api.Client;

namespace Polaris.Infrastructure.Factories;

public class ImmichClientFactory(IHttpClientFactory httpClientFactory)
{
    public ImmichApiClient Create(string serverUrl, string apiKey)
    {
        var baseUrl = NormalizeImmichApiUrl(serverUrl);
        
        var client = httpClientFactory.CreateClient();

        client.BaseAddress = new Uri(baseUrl);
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Remove("x-api-key");
        client.DefaultRequestHeaders.Add("x-api-key", apiKey);

        var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: client)
        {
            BaseUrl =  baseUrl
        };
        
        return new ImmichApiClient(adapter);
    }
    
    private static string NormalizeImmichApiUrl(string serverUrl)
    {
        var url = serverUrl.Trim().TrimEnd('/');

        return url.EndsWith("/api", StringComparison.OrdinalIgnoreCase)
            ? url
            : $"{url}/api";
    }
}