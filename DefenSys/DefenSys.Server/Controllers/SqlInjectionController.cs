using DefenSys.Application.Contracts;
using DefenSys.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DefenSys.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SqlInjectionController : ControllerBase
    {
        // The controller depends on the interface, not the concrete service class.
        private readonly ISqlInjectionService _sqlInjectionService;

        public SqlInjectionController(ISqlInjectionService sqlInjectionService)
        {
            _sqlInjectionService = sqlInjectionService;
        }

        /// <summary>
        /// Performs a basic SQL Injection scan on the provided URL.
        /// </summary>
        [HttpPost("scan")] // Endpoint: POST /api/sqlinjection/scan
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

            // The controller's only job is to delegate the work to the service.
            var result = await _sqlInjectionService.ScanAsync(request.Url);

            // And return the result from the service.
            return Ok(result);
        }
    }
}