using DefenSys.Application.Contracts;
using DefenSys.Core.DTOs;
using System.Web;

namespace DefenSys.Application.Services;
/// <summary>
/// Implements the logic for performing SQL Injection scans.
/// </summary>
public class SqlInjectionService : ISqlInjectionService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SqlInjectionService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ScanResultDto> ScanAsync(string url)
    {
        var uri = new Uri(url);
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        var client = _httpClientFactory.CreateClient();
        const string payload = "'"; // Our simple SQLi payload

        if (queryParams.Count == 0)
        {
            return new ScanResultDto
            {
                IsVulnerable = false,
                Message = "No query parameters found in the URL to test for SQL Injection."
            };
        }

        foreach (var key in queryParams.AllKeys)
        {
            if (key == null) continue;
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

                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError ||
                    content.Contains("You have an error in your SQL syntax", StringComparison.OrdinalIgnoreCase) ||
                    content.Contains("Unclosed quotation mark", StringComparison.OrdinalIgnoreCase))
                {
                    return new ScanResultDto
                    {
                        IsVulnerable = true,
                        Message = $"Potential SQL Injection vulnerability found. Parameter: '{key}'",
                        TestedUrl = maliciousUrl
                    };
                }
            }
            catch (HttpRequestException) { /* Continue to next parameter */ }
        }

        return new ScanResultDto
        {
            IsVulnerable = false,
            Message = "The server responded without any obvious SQL errors.",
            TestedUrl = url
        };
    }
}