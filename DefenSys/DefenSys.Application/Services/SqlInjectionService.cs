using DefenSys.Application.Contracts;
using DefenSys.Core.DTOs;
using System.Net;

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
        var maliciousUrl = url + "'";
        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.GetAsync(maliciousUrl);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                return new ScanResultDto
                {
                    IsVulnerable = true,
                    Message = "The server returned a 500 Internal Server Error, which strongly indicates an unhandled SQL error. The target is likely vulnerable.",
                    TestedUrl = maliciousUrl
                };
            }

            var content = await response.Content.ReadAsStringAsync();
            if (content.Contains("You have an error in your SQL syntax", StringComparison.OrdinalIgnoreCase) ||
                content.Contains("Unclosed quotation mark", StringComparison.OrdinalIgnoreCase))
            {
                return new ScanResultDto
                {
                    IsVulnerable = true,
                    Message = "The server's response included a common SQL error message. The target is vulnerable.",
                    TestedUrl = maliciousUrl
                };
            }

            return new ScanResultDto
            {
                IsVulnerable = false,
                Message = "The server responded without any obvious SQL errors. The target does not appear to be vulnerable to this basic test.",
                TestedUrl = maliciousUrl
            };
        }
        catch (HttpRequestException ex)
        {
            return new ScanResultDto
            {
                IsVulnerable = false,
                Message = $"A network error occurred: {ex.Message}",
                TestedUrl = maliciousUrl
            };
        }
    }
}