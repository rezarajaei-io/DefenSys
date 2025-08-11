namespace DefenSys.Core.DTOs
{
    /// <summary>
    /// This class represents the data we expect to receive from the frontend for a scan request.
    /// </summary>
    public class ScanRequestDto
    {
        /// <summary>
        /// The target URL that the user wants to scan.
        /// </summary>
        public required string Url { get; set; }
    }
    /// <summary>
    /// This class represents the result of a scan that we will send back to the frontend.
    /// </summary>
    public class ScanResultDto
    {
        /// <summary>
        /// True if a vulnerability was found, otherwise false.
        /// </summary>
        public bool IsVulnerable { get; set; }

        /// <summary>
        /// A message describing the outcome of the scan.
        /// </summary>
        public required string Message { get; set; }

        /// <summary>
        /// The exact URL that was tested, including the payload.
        /// </summary>
        public string? TestedUrl { get; set; }
    }
}
