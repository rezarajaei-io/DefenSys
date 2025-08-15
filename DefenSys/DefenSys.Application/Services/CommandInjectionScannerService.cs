using DefenSys.Application.Contracts;
using DefenSys.Core.DTOs;
using System.Web;

namespace DefenSys.Application.Services;

public class CommandInjectionScannerService : ICommandInjectionScannerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string Beacon = "d3f3nsys-b34c0n";

    private readonly string[] _payloads =
    {
        $";echo {Beacon}", // For Linux/Unix
        $"&echo {Beacon}",  // For Windows
        $"|echo {Beacon}"   // For both (piping)
    };

    public CommandInjectionScannerService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ScanResultDto> ScanAsync(string url)
    {
        var uri = new Uri(url);
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        var client = _httpClientFactory.CreateClient();

        if (queryParams.Count == 0)
        {
            return new ScanResultDto
            {
                IsVulnerable = false,
                Message = "No query parameters found to test for Command Injection."
            };
        }

        foreach (var key in queryParams.AllKeys)
        {
            if (key == null) continue;
            foreach (var payload in _payloads)
            {
                var tempParams = HttpUtility.ParseQueryString(uri.Query);
                var originalValue = queryParams[key];
                tempParams[key] = originalValue + payload;

                var uriBuilder = new UriBuilder(uri.GetLeftPart(UriPartial.Path))
                {
                    Query = tempParams.ToString()?.Replace("+", "%20")
                };
                var maliciousUrl = uriBuilder.ToString();

                try
                {
                    var response = await client.GetAsync(maliciousUrl);
                    var content = await response.Content.ReadAsStringAsync();

                    if (content.Contains(Beacon, StringComparison.OrdinalIgnoreCase))
                    {
                        return new ScanResultDto
                        {
                            IsVulnerable = true,
                            Message = $"Potential Command Injection vulnerability found. A unique beacon was echoed back. Payload: '{payload}', Parameter: '{key}'",
                            TestedUrl = maliciousUrl
                        };
                    }
                }
                catch (HttpRequestException) { /* Continue to next payload */ }
            }
        }

        return new ScanResultDto
        {
            IsVulnerable = false,
            Message = "No Command Injection vulnerabilities found with the beacon test.",
            TestedUrl = url
        };
    }
}