using DefenSys.Core.DTOs;

namespace DefenSys.Application.Contracts
{
    /// <summary>
    /// Defines the contract for the Cross-Site Scripting (XSS) scanning service.
    /// </summary>
    public interface IXssScannerService
    {
        /// <summary>
        /// Asynchronously performs a basic Cross-Site Scripting (XSS) scan on a given URL.
        /// </summary>
        /// <param name="url">The target URL to scan.</param>
        /// <returns>A ScanResult object containing the outcome.</returns>
        Task<ScanResultDto> ScanAsync(string url);
    }
}
