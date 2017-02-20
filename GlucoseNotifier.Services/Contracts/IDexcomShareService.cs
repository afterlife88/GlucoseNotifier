using System.Collections.Generic;
using System.Threading.Tasks;
using GlucoseNotifier.Services.Models.DexcomApi.Requests;
using GlucoseNotifier.Services.Models.DexcomApi.Responses;

namespace GlucoseNotifier.Services.Contracts
{
    public interface IDexcomShareService
    {
        Task<List<BGResponse>> GetLatestBg(GlucoseRequest request);
        Task<string> Authorize(LoginRequest request);
    }
}
