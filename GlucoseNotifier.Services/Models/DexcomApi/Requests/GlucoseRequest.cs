namespace GlucoseNotifier.Services.Models.DexcomApi.Requests
{
    /// <summary>
    /// Glucose request model
    /// Assigned property will provide the latest BG
    /// </summary>
    public class GlucoseRequest
    {
        public string SessionId { get; set; }
        public int Miuntes { get; set; } = 1440;
        public int MaxCount { get; set; } = 10;
    }
}
