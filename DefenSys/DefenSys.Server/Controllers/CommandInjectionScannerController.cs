// DefenSys.Api/Controllers/CommandInjectionScannerController.cs

using DefenSys.Application.Contracts;
using DefenSys.Core;
using DefenSys.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DefenSys.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommandInjectionScannerController : ControllerBase
{
    private readonly ICommandInjectionScannerService _scannerService;

    public CommandInjectionScannerController(ICommandInjectionScannerService scannerService)
    {
        _scannerService = scannerService;
    }

    /// <summary>
    /// Performs a basic Command Injection scan on the provided URL.
    /// </summary>
    [HttpPost("scan")] // Endpoint: POST /api/commandinjectionscanner/scan
    public async Task<IActionResult> ScanForCommandInjection([FromBody] ScanRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Url) || !Uri.IsWellFormedUriString(request.Url, UriKind.Absolute))
        {
            return BadRequest(new ScanResultDto    
            {
                IsVulnerable = false,
                Message = "Invalid or empty URL provided."
            });
        }

        var result = await _scannerService.ScanAsync(request.Url);

        return Ok(result);
    }
}