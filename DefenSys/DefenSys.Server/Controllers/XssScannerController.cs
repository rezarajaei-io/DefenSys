// DefenSys.Api/Controllers/XssScannerController.cs

using DefenSys.Application.Contracts;
using DefenSys.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DefenSys.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class XssScannerController : ControllerBase
{
    // The controller depends on the interface from the Application layer.
    private readonly IXssScannerService _xssScannerService;

    public XssScannerController(IXssScannerService xssScannerService)
    {
        _xssScannerService = xssScannerService;
    }

    /// <summary>
    /// Performs a basic Cross-Site Scripting (XSS) scan on the provided URL.
    /// </summary>
    /// <param name="request">The scan request containing the target URL.</param>
    /// <returns>The result of the scan.</returns>
    [HttpPost("scan")] // The endpoint will be: POST /api/xssscanner/scan
    public async Task<IActionResult> ScanForXss([FromBody] ScanRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Url) || !Uri.IsWellFormedUriString(request.Url, UriKind.Absolute))
        {
            return BadRequest(new ScanResultDto
            {
                IsVulnerable = false,
                Message = "Invalid or empty URL provided."
            });
        }

        // The controller's only job is to delegate the work to the service.
        var result = await _xssScannerService.ScanAsync(request.Url);

        // And return the result from the service.
        return Ok(result);
    }
}