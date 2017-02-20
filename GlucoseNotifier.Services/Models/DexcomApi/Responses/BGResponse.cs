using System;

namespace GlucoseNotifier.Services.Models.DexcomApi.Responses
{
    public class BGResponse
    {
        public DateTime DT { get; set; }
        public DateTime ST { get; set; }
        public DateTime WT { get; set; }
        public int Trend { get; set; }
        public int Value { get; set; }
    }
}
