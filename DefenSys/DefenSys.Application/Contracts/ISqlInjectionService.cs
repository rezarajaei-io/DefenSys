using DefenSys.Core.DTOs;

namespace DefenSys.Application.Contracts
{
    /// <summary>
    /// Defines the contract for the SQL Injection scanning service.
    /// </summary>
    public interface ISqlInjectionService
    {
        /// <summary>
        /// Asynchronously performs a basic SQL Injection scan on a given URL.
        /// </summary>
        /// <param name="url">The target URL to scan.</param>
        /// <returns>A ScanResult object containing the outcome.</returns>
        Task<ScanResultDto> ScanAsync(string url);
    }
}
