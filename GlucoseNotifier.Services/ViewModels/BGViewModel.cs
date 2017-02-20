using System;

namespace GlucoseNotifier.Services.ViewModels
{
    public class BGViewModel
    {
        public DateTime SugarTime { get; set; }
        public int CurrentSugar { get; set; }
        public int Trend { get; set; }
    }
}
