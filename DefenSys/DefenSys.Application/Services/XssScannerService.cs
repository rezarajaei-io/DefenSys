// DefenSys.Application/Services/XssScannerService.cs

using DefenSys.Application.Contracts;
using DefenSys.Core;
using DefenSys.Core.DTOs;
using System.Web; // Needed for HttpUtility

namespace DefenSys.Application.Services;

public class XssScannerService : IXssScannerService
{
    private readonly IHttpClientFactory _httpClientFactory;

    // A simple, unique payload to test for reflection.
    private const string XssPayload = "<d3f3nSys-xss-test>";

    public XssScannerService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ScanResultDto> ScanAsync(string url)
    {
        var uri = new Uri(url);
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        var client = _httpClientFactory.CreateClient();

        // If there are no parameters in the URL, we can't test it this way.
        if (queryParams.Count == 0)
        {
            return new ScanResultDto
            {
                IsVulnerable = false,
                Message = "No query parameters found in the URL to test for reflected XSS.",
                TestedUrl = url
            };
        }

        // Test each parameter by injecting the payload.
        foreach (var key in queryParams.AllKeys)
        {
            var originalValue = queryParams[key];
            var tempParams = HttpUtility.ParseQueryString(uri.Query);
            tempParams[key] = XssPayload;

            // Rebuild the URL with the malicious payload.
            var uriBuilder = new UriBuilder(url) { Query = tempParams.ToString() };
            var maliciousUrl = uriBuilder.ToString();

            try
            {
                var response = await client.GetAsync(maliciousUrl);
                var content = await response.Content.ReadAsStringAsync();

                // If our exact payload is reflected in the response body, it's vulnerable.
                if (content.Contains(XssPayload, StringComparison.OrdinalIgnoreCase))
                {
                    return new ScanResultDto
                    {
                        IsVulnerable = true,
                        Message = $"Potential XSS vulnerability found. The payload was reflected in the response. Parameter: '{key}'",
                        TestedUrl = maliciousUrl
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                // If a network error occurs, we report it and stop.
                return new ScanResultDto
                {
                    IsVulnerable = false,
                    Message = $"A network error occurred while testing parameter '{key}': {ex.Message}",
                    TestedUrl = maliciousUrl
                };
            }
        }

        // If we've tested all parameters and found nothing, it's likely safe.
        return new ScanResultDto
        {
            IsVulnerable = false,
            Message = "No reflected XSS vulnerabilities found with this basic test.",
            TestedUrl = url
        };
    }
}