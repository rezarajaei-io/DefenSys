using DefenSys.Core.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DefenSys.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class SqlInjectionController : ControllerBase
    {
        // We use IHttpClientFactory for efficient management of HttpClient instances.
        private readonly IHttpClientFactory _httpClientFactory;

        public SqlInjectionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Performs a basic SQL Injection scan on the provided URL.
        /// </summary>
        /// <param name="request">The scan request containing the target URL.</param>
        /// <returns>The result of the scan.</returns>
        [HttpPost("scan")] // The endpoint will be: POST /api/sqlinjection/scan
        public async Task<IActionResult> ScanForSqlInjection([FromBody] ScanRequestDto request)
        {
            if (string.IsNullOrEmpty(request.Url) || !Uri.IsWellFormedUriString(request.Url, UriKind.Absolute))
            {
                return BadRequest(new ScanResultDto
                {
                    IsVulnerable = false,
                    Message = "Invalid or empty URL provided."
                });
            }

            // Append a single quote payload to the URL to try to trigger an SQL error.
            var maliciousUrl = request.Url + "'";
            var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync(maliciousUrl);

                // A 500 Internal Server Error is a strong indicator of a vulnerability,
                // as our injected quote likely broke the backend SQL query.
                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    return Ok(new ScanResultDto
                    {
                        IsVulnerable = true,
                        Message = "The server returned a 500 Internal Server Error, which strongly indicates an unhandled SQL error. The target is likely vulnerable.",
                        TestedUrl = maliciousUrl
                    });
                }

                // Also, check the response body for common SQL error messages.
                var content = await response.Content.ReadAsStringAsync();
                if (content.Contains("You have an error in your SQL syntax", StringComparison.OrdinalIgnoreCase) ||
                    content.Contains("Unclosed quotation mark", StringComparison.OrdinalIgnoreCase))
                {
                    return Ok(new ScanResultDto
                    {
                        IsVulnerable = true,
                        Message = "The server's response included a common SQL error message. The target is vulnerable.",
                        TestedUrl = maliciousUrl
                    });
                }

                // If no specific error is found, we assume it's not vulnerable to this basic test.
                return Ok(new ScanResultDto
                {
                    IsVulnerable = false,
                    Message = "The server responded without any obvious SQL errors. The target does not appear to be vulnerable to this basic test.",
                    TestedUrl = maliciousUrl
                });
            }
            catch (HttpRequestException ex)
            {
                // This catches network-level errors (e.g., the server is down or the URL is invalid).
                return StatusCode(500, new ScanResultDto
                {
                    IsVulnerable = false,
                    Message = $"A network error occurred: {ex.Message}"
                });
            }
        }
    }
}
