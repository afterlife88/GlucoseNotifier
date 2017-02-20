using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GlucoseNotifier.Services.Contracts;
using GlucoseNotifier.Services.Exceptions;
using GlucoseNotifier.Services.Models.DexcomApi.Requests;
using GlucoseNotifier.Services.Models.DexcomApi.Responses;
using Newtonsoft.Json;

namespace GlucoseNotifier.Services.Implementation
{
    public class DexcomShareService : IDexcomShareService
    {
        private string _dexcomUsApiEndpoint = "https://share1.dexcom.com";
        private string _authorizeEndPoint = "/ShareWebServices/Services/General/LoginPublisherAccountByName";
        private string _glucoseEndPoint = "/ShareWebServices/Services/Publisher/ReadPublisherLatestGlucoseValues";

        public async Task<string> Authorize(LoginRequest request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_dexcomUsApiEndpoint);
                string jsonObj = JsonConvert.SerializeObject(request);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Dexcom Share / 3.0.2.11 CFNetwork / 711.2.23 Darwin / 14.0.0");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                HttpContent content = new StringContent(jsonObj, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(_authorizeEndPoint, content);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var sessionId = await response.Content.ReadAsStringAsync();
                        return sessionId;
                    case HttpStatusCode.InternalServerError:
                        var message = await response.Content.ReadAsStringAsync();
                        throw new DexcomShareServiceException(
                            JsonConvert.DeserializeObject<FailedAuthorizeResponse>(message).Message);
                    default:
                        return null;
                }
            }
        }

        public async Task<List<BGResponse>> GetLatestBg(GlucoseRequest request)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_dexcomUsApiEndpoint);
                var requestUrl =
                    $"{_glucoseEndPoint}?sessionID={request.SessionId.Replace("\"", "")}&minutes={request.Miuntes}&maxCount={request.MaxCount}";

                client.DefaultRequestHeaders.UserAgent.ParseAdd("Dexcom Share / 3.0.2.11 CFNetwork / 711.2.23 Darwin / 14.0.0");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                var response = await client.PostAsync(requestUrl, null);
                if (!response.IsSuccessStatusCode)
                    return null;
                var res = await response.Content.ReadAsStringAsync();
                var cleanStr = res.Replace("\\", "");
                var listOfBGs = JsonConvert.DeserializeObject<List<BGResponse>>(cleanStr);
                return listOfBGs;
            }
        }
    }
}
