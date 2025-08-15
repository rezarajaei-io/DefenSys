using DefenSys.Core.DTOs;

namespace DefenSys.Application.Contracts
{
    /// <summary>
    /// Defines the contract for the Command Injection scanning service.
    /// </summary>
    public interface ICommandInjectionScannerService
    {
        /// <summary>
        /// Asynchronously performs a basic Command Injection scan on a given URL.
        /// </summary>
        /// <param name="url">The target URL to scan.</param>
        /// <returns>A ScanResult object containing the outcome.</returns>
        Task<ScanResultDto> ScanAsync(string url);
    }
}
