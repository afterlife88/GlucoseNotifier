using System;

namespace GlucoseNotifier.Services.Exceptions
{
    public class DexcomShareServiceException : Exception
    {
        public DexcomShareServiceException(string message) : base(message)
        {

        }
    }
}
