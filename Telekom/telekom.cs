using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Windows.Storage;
using Windows.UI.Popups;

namespace Telekom
{
    public class Telekom
    {
        public ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        public Guid deviceId = Windows.Storage.Streams.DataReader.FromBuffer(Windows.System.Profile.SystemIdentification.GetSystemIdForPublisher().Id).ReadGuid();
        private string nonce = "";
        public string accessToken = "";
        public string refreshToken = "";
        public string productId = "";
        public string productLabel = "";
        public string productName = "";
        public string fullName = "";
        public double maxGB, remainingGB = 0;
        public long serviceId = 0;
        public string lastError = "";
        public string lastCode = "";
        internal static HttpClient httpClient = new HttpClient();

        public async Task ShowError()
        {
            var messageDialog = new MessageDialog(App.TLKM.lastError + "\n" + App.TLKM.lastCode);
            messageDialog.Commands.Add(new UICommand("OK"));
            messageDialog.DefaultCommandIndex = 0;
            messageDialog.CancelCommandIndex = 0;

            await messageDialog.ShowAsync();
        }

        public async Task<bool> Dashboard()
        {
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/dashboard/product/" + productId + "?enableFreeUnit=true&priority=primary&profileId=MSISDN_" + serviceId + "&serviceOnboarding=false&serviceOutageEnabled=false&showTotalCreditBalance=true&showUnlimited=true"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                request.Headers.TryAddWithoutValidation("X-Client-Version", "18.8.2 (887) 2-78c3ec0 (HEAD)");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                var response = await httpClient.SendAsync(request);
                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - dashboard] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                productName = json.campaignPlanDetail.name;
                maxGB = json.consumption.max.value;
                remainingGB = json.consumption.remaining.value;
                Debug.WriteLine("[tlkm_main - dashboard] " + productName + " - max: " + maxGB + "GB remaining: " + remainingGB + "GB");

                return true;
            }
        }

        public async Task<bool> Regen_token()
        {

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://t-app.telekom.sk/token/"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "7d06dd59-687c-454e-ade8-3520ff79a00d");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "188C1694-71FC-4CA6-B013-579755690106");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                request.Content = new StringContent("{\"genCenToken\":false,\"refreshToken\":\"" + refreshToken + "\",\"deviceId\":\"" + deviceId + "\"}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);

                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - regen_token] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                Debug.WriteLine("[tlkm_main - regen_token] saving accessToken");
                accessToken = json.accessToken;
                App.TLKM.localSettings.Values["accessToken"] = accessToken;

                Debug.WriteLine("[tlkm_main - regen_token] saving refreshToken");
                refreshToken = json.refreshToken;
                App.TLKM.localSettings.Values["refreshToken"] = refreshToken;

                return true;
            }
        }

        public async Task<bool> Login()
        {
            //this function is not invoked until the variables are not populated
            //this should not happen in any other order.

            Debug.WriteLine("[tlkm_main] login for " + serviceId);

            using (var request = new HttpRequestMessage(new HttpMethod("GET"), "https://t-app.telekom.sk/profiles/?deviceId=" + deviceId + "&devicesWithEMI=false&genCenToken=true&hybridEnabled=true&loyaltyEnabled=false&sub=MSISDN_" + serviceId + "&subscriptionServiceEnabled=false"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                var response = await httpClient.SendAsync(request);

                var semiParsedJson = response.Content.ReadAsStringAsync().Result;

                if (semiParsedJson.StartsWith("["))
                    semiParsedJson = response.Content.ReadAsStringAsync().Result.Remove(0, 2).Remove(response.Content.ReadAsStringAsync().Result.Length - 3, 1);

                dynamic json = JObject.Parse(semiParsedJson);
                JObject jobj = JObject.Parse(semiParsedJson);

                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - login] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                var product = from p in jobj["manageableAssets"] select (string)p["id"];
                foreach (var item in product)
                {
                    Debug.WriteLine("[tlkm_main - login] productId: " + item);
                    productId = item;
                }

                var label = from p in jobj["manageableAssets"] select (string)p["label"];
                foreach (var item in label)
                {
                    Debug.WriteLine("[tlkm_main - login] productLabel: " + item);
                    productLabel = item;
                }

                fullName = json.individual.givenName + " " + json.individual.familyName;
                Debug.WriteLine("[tlkm_main - login] fullName: " + fullName);

                return true;
            }
        }

        public Task<bool> Pin(long parsedNumber)
        {
            Debug.WriteLine("[tlkm_main] PIN for " + parsedNumber);

            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://t-app.telekom.sk/pin/"))
            {
                request.Headers.TryAddWithoutValidation("Authorization", "7d06dd59-687c-454e-ade8-3520ff79a00d");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                request.Content = new StringContent("{\"serviceId\":\"+" + parsedNumber + "\",\"serviceType\":\"phoneNumber\",\"device\":{\"os\":\"ios\"},\"context\":\"login\"}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = httpClient.SendAsync(request);

                dynamic json = JObject.Parse(response.Result.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - pin] " + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return Task.FromResult(false);
                }
                string jsonNonce = json.nonce;
                nonce = jsonNonce;
                serviceId = parsedNumber;

                return Task.FromResult(true);
            }
        }

        public async Task<bool> Verif(long PIN)
        {
            Debug.WriteLine("[tlkm_main] PIN verif input " + PIN);

            using (var request = new HttpRequestMessage(new HttpMethod("PUT"), "https://t-app.telekom.sk/pin/"))
            {
                request.Headers.TryAddWithoutValidation("Accept", "*/*");
                request.Headers.TryAddWithoutValidation("Authorization", "7d06dd59-687c-454e-ade8-3520ff79a00d");
                request.Headers.TryAddWithoutValidation("X-Request-Session-Id", "FC4DF625-01D0-4ACC-A8E5-5260A3F9AC7F");
                request.Headers.TryAddWithoutValidation("X-Request-Tracking-Id", "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

                request.Content = new StringContent("{\"device\":{\"id\":\"" + deviceId + "\",\"os\":\"ios\"},\"enableProfilePin\":false,\"serviceId\":\"+" + serviceId + "\",\"serviceType\":\"phoneNumber\",\"context\":\"login\",\"PIN\":\"" + PIN + "\",\"nonce\":\"" + nonce + "\"}");
                request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                var response = await httpClient.SendAsync(request);

                dynamic json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
                if (json.errorType != null)
                {
                    string errorMessage = "[tlkm_main - verif]" + json.message + " " + json.code;
                    Debug.WriteLine(errorMessage);
                    lastCode = json.code;
                    lastError = json.message;
                    return false;
                }

                Debug.WriteLine("[tlkm_main - verif] saving deviceId");
                App.TLKM.localSettings.Values["deviceId"] = deviceId;

                Debug.WriteLine("[tlkm_main - verif] saving accessToken");
                accessToken = json.accessToken;
                App.TLKM.localSettings.Values["accessToken"] = accessToken;

                Debug.WriteLine("[tlkm_main - verif] saving refreshToken");
                refreshToken = json.refreshToken;
                App.TLKM.localSettings.Values["refreshToken"] = refreshToken;

                Debug.WriteLine("[tlkm_main - verif] saving serviceId");
                App.TLKM.localSettings.Values["serviceId"] = serviceId;

                Debug.WriteLine("[tlkm_main - verif] saving hasAccount");
                App.TLKM.localSettings.Values["hasAccount"] = "yes";


                return true;
            }
        }
    }
}
